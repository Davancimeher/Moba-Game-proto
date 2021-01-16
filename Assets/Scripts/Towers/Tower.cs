using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum TowerState : byte
{
    PASSIVE = 0,
    ATTACKING_LOCAL_PLAYER = 1,
    ATTACKING_ANOTHER_PLAYER = 2,
    ATTACKING_MINION = 3,
    DESTROYED = 4
}
public class Tower : MonoBehaviour
{
    public TowerTarget m_ActualTarget = null;
    public GameObject m_TowerIndicator;
    public PhotonView m_TowerPhotonView;
    public Transform TowerProjectile;
    public GameObject TowerProjectilePrefab = null;
    public int Damage;

    public TowerState m_TowerState = TowerState.PASSIVE;
    private Dictionary<int, TowerTarget> m_MinionTargetsDict = new Dictionary<int, TowerTarget>();
    private Dictionary<int, TowerTarget> m_ChampionTargetsDict = new Dictionary<int, TowerTarget>();
    private Dictionary<int, TowerTarget> m_PriorityTargetsDict = new Dictionary<int, TowerTarget>();

    private List<int> m_ChampionTargetsList = new List<int>();
    public List<int> m_PriorityTargetsList = new List<int>();

    public byte m_TeamIndex;

    public void InitTower()
    {
        if (PhotonNetwork.IsMasterClient)
            StartAttacking();
    }

    private void OnTriggerEnter(Collider other)
    {
        var towerTarget = other.GetComponent<TowerTarget>();
        if (towerTarget == null) return;

        if (towerTarget.teamIndex == m_TeamIndex) return;

        AddTarget(towerTarget);

        if (PhotonNetwork.IsMasterClient)
            m_ActualTarget = GetTarget();
    }
    private void OnTriggerExit(Collider other)
    {
        var towerTarget = other.GetComponent<TowerTarget>();
        if (towerTarget == null) return;

        if (towerTarget.teamIndex == m_TeamIndex) return;

        RemoveTarget(towerTarget);
        RemovePriorityTarget(towerTarget);

        if (PhotonNetwork.IsMasterClient)
        {
            if (m_ActualTarget != null)
                if (m_ActualTarget.ViewID == towerTarget.ViewID)
                {
                    m_ActualTarget = null;
                    UpdateAttackingTarget();
                }
        }
    }
    public void AddTarget(TowerTarget _towerTarget)
    {
        switch (_towerTarget.TowerTargetType)
        {
            case TowerTargetType.CHAMPION:
                if (!m_ChampionTargetsDict.ContainsKey(_towerTarget.ViewID))
                {
                    m_ChampionTargetsDict.Add(_towerTarget.ViewID, _towerTarget);
                }
                if (!m_ChampionTargetsList.Contains(_towerTarget.ViewID))
                {
                    m_ChampionTargetsList.Add(_towerTarget.ViewID);
                }
                break;
            case TowerTargetType.MINION:
                if (!m_MinionTargetsDict.ContainsKey(_towerTarget.ViewID))
                {
                    m_MinionTargetsDict.Add(_towerTarget.ViewID, _towerTarget);
                }
                break;
        }
    }
    public void RemoveTarget(TowerTarget _towerTarget)
    {
        switch (_towerTarget.TowerTargetType)
        {
            case TowerTargetType.CHAMPION:
                if (m_ChampionTargetsDict.ContainsKey(_towerTarget.ViewID))
                {
                    m_ChampionTargetsDict.Remove(_towerTarget.ViewID);
                }
                if (m_ChampionTargetsList.Contains(_towerTarget.ViewID))
                {
                    m_ChampionTargetsList.Remove(_towerTarget.ViewID);
                }
                break;
            case TowerTargetType.MINION:
                if (m_MinionTargetsDict.ContainsKey(_towerTarget.ViewID))
                {
                    m_MinionTargetsDict.Remove(_towerTarget.ViewID);
                }
                break;
        }
    }

    public void AddPriorityTarget(TowerTarget _towerTarget)
    {
        if (!m_PriorityTargetsDict.ContainsKey(_towerTarget.ViewID))
        {
            m_PriorityTargetsDict.Add(_towerTarget.ViewID, _towerTarget);
        }
        if (!m_PriorityTargetsList.Contains(_towerTarget.ViewID))
        {
            m_PriorityTargetsList.Add(_towerTarget.ViewID);
        }
    }
    public void RemovePriorityTarget(TowerTarget _towerTarget)
    {
        if (m_PriorityTargetsDict.ContainsKey(_towerTarget.ViewID))
        {
            m_PriorityTargetsDict.Remove(_towerTarget.ViewID);
        }
        if (m_PriorityTargetsList.Contains(_towerTarget.ViewID))
        {
            m_PriorityTargetsList.Remove(_towerTarget.ViewID);
        }
    }

    public TowerTarget GetTarget()
    {
        TowerTarget towerTarget = null;

        if (m_PriorityTargetsDict.Count > 0)
        {
            int index = m_PriorityTargetsList.First();

            towerTarget = m_PriorityTargetsDict[index];
        }
        else if (m_MinionTargetsDict.Count > 0)
        {
            towerTarget = m_MinionTargetsDict.First().Value;
        }
        else if (m_ChampionTargetsDict.Count > 0)
        {
            int index = m_ChampionTargetsList.First();

            towerTarget = m_ChampionTargetsDict[index];
        }
        else
        {
            return null;
        }
        return towerTarget;
    }

    public void StartAttacking()
    {
        StartCoroutine(AttackingCoroutine());
    }
    public void UpdateAttackingTarget()
    {
        if (m_ActualTarget != null) return;
        m_ActualTarget = GetTarget();
    }
    public IEnumerator AttackingCoroutine()
    {
        while (m_TowerState != TowerState.DESTROYED)
        {
            if (m_ActualTarget != null)
            {
                if (m_ActualTarget.HealthManager.Health > 0)
                {
                    ExecuteRPCTowerAttack(m_ActualTarget.ViewID, (byte)m_ActualTarget.TowerTargetType);
                }
                else
                {
                    RemoveTarget(m_ActualTarget);
                    m_ActualTarget = null;
                }
            }
            yield return new WaitForSeconds(2f);
            UpdateAttackingTarget();
        }
    }

    public void ExecuteRPCTowerAttack(int _towerTargetViewId, byte TowerTargetType)
    {
        m_TowerPhotonView.RPC("RPC_TowerAttack", RpcTarget.AllViaServer, _towerTargetViewId, TowerTargetType);
    }

    [PunRPC]
    public void RPC_TowerAttack(int _towerTargetViewId, byte TowerTargetType)
    {
        TowerTarget towerTarget = GetLocalTarget(_towerTargetViewId, TowerTargetType);

        if (towerTarget == null) return;

        GameObject obj = Instantiate(TowerProjectilePrefab, TowerProjectile.position, TowerProjectile.rotation);
        var towerProjectile = obj.GetComponent<TowerProjectile>();

        towerProjectile.m_ActualTowerTarget = towerTarget;
        towerProjectile.Damage = Damage;
    }

    public void ExecuteAddTargetToMaster(int _towerTargetViewId)
    {
        Debug.LogError("ExecuteAddTargetToMaster");
        m_TowerPhotonView.RPC("RPC_AddTargetToMastert", RpcTarget.MasterClient, _towerTargetViewId);
    }

    [PunRPC]
    public void RPC_AddTargetToMastert(int _towerTargetViewId)
    {
        Debug.LogError("RPC_AddTargetToMastert");
        ExecuteRPCAddPriorityTarget(_towerTargetViewId);
    }

    public void ExecuteRPCAddPriorityTarget(int _towerTargetViewId)
    {
        Debug.LogError("ExecuteRPCAddPriorityTarget");

        m_TowerPhotonView.RPC("RPC_AddPriorityTarget", RpcTarget.AllViaServer, _towerTargetViewId);
    }   

    [PunRPC]
    public void RPC_AddPriorityTarget(int _towerTargetViewId)
    {
        Debug.LogError("RPC_AddPriorityTarget");

        TowerTarget towerTarget = m_ChampionTargetsDict[_towerTargetViewId];

        if (towerTarget == null) return;

        AddPriorityTarget(towerTarget);

        if (PhotonNetwork.IsMasterClient)
        {
            m_ActualTarget = GetTarget();
        }
    }

    public TowerTarget GetLocalTarget(int _towerTargetViewId, byte _towerTargetType)
    {
        TowerTarget towerTarget = null;

        TowerTargetType towerTargetType = (TowerTargetType)_towerTargetType;

        switch (towerTargetType)
        {
            case TowerTargetType.CHAMPION:

                if (m_ChampionTargetsDict.ContainsKey(_towerTargetViewId))
                {
                    towerTarget = m_ChampionTargetsDict[_towerTargetViewId];
                }
                break;
            case TowerTargetType.MINION:

                if (m_MinionTargetsDict.ContainsKey(_towerTargetViewId))
                {
                    towerTarget = m_MinionTargetsDict[_towerTargetViewId];
                }
                break;
        }
        return towerTarget;
    }


    //Local Player

    public void EnableIndicator()
    {
        m_TowerIndicator.SetActive(true);
    }
    public void DisableIndicator()
    {
        m_TowerIndicator.SetActive(false);
    }
    public void UpdateIndicatorState()
    {

    }

}
