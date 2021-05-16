using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class HealthManager : MonoBehaviour, IPunObservable
{
    public int Health;
    public SetPlayerInGameInfo m_SetPlayerInGameInfo;
    public int MaxHealth;

    public Transform _DamageCanvas;
    public GameObject _DamagePrefab;
    public GameObject _RegenPrefab;

    public PhotonView m_PhotonView;
    public ChampionManager m_ChampionManager;

    public bool InRegen;
    public bool IsDead;

    public byte teamIndex;

    public int Killer;
    public int LastAssist;

    public List<int> Assistors = new List<int>();

    public ChampionScore _ChampionScore;
    private void Start()
    {
        _ChampionScore = m_ChampionManager.m_ChampionScore;
        Killer = -1;
        LastAssist = -1;
    }

    public int Lastdamage
    {
        get { return Lastdamage; }
        set
        {
            Lastdamage = value;
            ShowDamage(Lastdamage);
            // UIManager.UIM.PanelManaging(myState);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
        }
        else
        {
            UpdateHealthBar((int)stream.ReceiveNext());
        }
    }
    public void UpdateHealthBar(int _health)
    {
        Health = _health;
        //percent of health from max health
        var percent = Health / (float)MaxHealth;
        m_SetPlayerInGameInfo.HealthImage.fillAmount = percent;
    }
    public void TakeDamage(int _damage)
    {
        Health -= _damage;
        UpdateHealthBar(Health);
        ShowDamage(_damage);
        CheckIfGoToDead();
        if (m_ChampionManager.m_SpellManager.m_InRecall)
        {
            m_ChampionManager.ExecuteCancelRecall();
        }
    }

    public void ExecuteDamageRPC(PhotonView senderPhotonView, Player playerDamaged, int _damage, bool IsTower)
    {
        senderPhotonView.RPC("RPC_ApplyDamage", playerDamaged, _damage, IsTower, senderPhotonView.ViewID);
    }

    [PunRPC]
    public void RPC_ApplyDamage(int _damage, bool isTower, int DamageActor)
    {
        RoomData.RD.m_MyChampionManager.m_HealthManager.TakeDamage(_damage);
        RoomData.RD.m_MyChampionManager.m_HealthManager.SetHitter(isTower, DamageActor,_damage);
        RoomData.RD.m_MyChampionManager.m_HealthManager.SetAssister(isTower, DamageActor);


        //if (RoomData.RD.PlayersChampions.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
        //{
        //    HealthManager healthManager = RoomData.RD.PlayersChampions[PhotonNetwork.LocalPlayer.ActorNumber].GetComponent<HealthManager>();

        //    healthManager.TakeDamage(_damage);
        //    healthManager.SetHitter(isTower, DamageActor);
        //}
    }

    public void AddDeath(int KillerActor,List< int> assisterActor)
    {
        _ChampionScore.AddDeath(KillerActor, assisterActor);
    }
    public void SetHitter(bool isTower, int actor,int damage)
    {
        if(Health - damage <= 0)
        {
            if (isTower)
            {
                Killer = -1;
            }
            else
            {
                Killer = actor;
            }
        }
        
    }
    public void SetAssister(bool isTower, int actor)
    {
        if (!isTower)
            if (Killer != actor && !Assistors.Contains(actor))
            {
                Assistors.Add(actor);
            }
    }


    public void ExecuteTowerDamageRPC(PhotonView senderPhotonView, Player playerDamaged, int _damage)
    {
        senderPhotonView.RPC("RPC_ApplyTowerDamage", playerDamaged, _damage);
    }

    [PunRPC]
    public void RPC_ApplyTowerDamage(int _damage)
    {
        if (RoomData.RD.PlayersChampions.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            RoomData.RD.PlayersChampions[PhotonNetwork.LocalPlayer.ActorNumber].GetComponent<HealthManager>().TakeDamage(_damage);
        }
    }
    public void ShowDamage(int _damage)
    {
        GameObject damageObject = Instantiate(_DamagePrefab, _DamageCanvas);
        var damageText = damageObject.GetComponent<TextMeshProUGUI>();
        damageText.text = _damage.ToString();
        Destroy(damageObject, 3f);
    }
    public void ShowRegen(int _regenValue)
    {
        GameObject RegenObject = Instantiate(_RegenPrefab, _DamageCanvas);
        var regentText = RegenObject.GetComponent<TextMeshProUGUI>();
        regentText.text = _regenValue.ToString();
        Destroy(RegenObject, 3f);
    }
    public void CheckIfGoToDead()
    {
        if (m_PhotonView.IsMine)
        {
            if (Health <= 0 && !IsDead)
            {
                AddDeath(Killer, Assistors);
                Debug.Log("dzdz");
                m_ChampionManager.ExecuteSetDead();
                m_ChampionManager.SetDead();
                IsDead = true;
            }
        }
    }
    public void ResetHealth()
    {
        if (m_PhotonView.IsMine)
        {
            IsDead = false;
            Health = MaxHealth;
            UpdateHealthBar(Health);

            Assistors.Clear();
            Killer = -1;
        }
    }
    public IEnumerator RegenerateHealth()
    {
        InRegen = true;

        var tenPercent = (MaxHealth / 100) * 10;

        while (Health < MaxHealth)
        {
            yield return new WaitForSeconds(1f);

            if (Health + tenPercent <= MaxHealth)
            {
                Health += tenPercent;
                UpdateHealthBar(Health);
                ShowRegen(tenPercent);
            }
            else
            {
                ShowRegen(MaxHealth - Health);
                Health = MaxHealth;
                UpdateHealthBar(Health);
            }
            if (Health == MaxHealth)
            {
                InRegen = false;
                StopCoroutine(RegenerateHealth());

            }
        }
    }
    public void StartRegen()
    {
        if (!InRegen && Health < MaxHealth)
            StartCoroutine(RegenerateHealth());
    }
}
