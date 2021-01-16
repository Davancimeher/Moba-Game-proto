using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthManager : MonoBehaviour, IPunObservable
{
    public int m_Health;
    public int m_MaxHealth;
    public Image m_TowerHealthUI;

    public GameObject m_DamagePrefab;
    public GameObject m_DamageCanvas;
    public Tower m_Tower;
    public GameObject TowerHitbox;
    private void Start()
    {
        if (RoomData.RD.m_PlayersTeams[PhotonNetwork.LocalPlayer.ActorNumber] != m_Tower.m_TeamIndex)
        {
            m_TowerHealthUI.color = Color.red;
            gameObject.tag = GlobalVariables.m_EnemyTag;

            if (TowerHitbox != null)
                TowerHitbox.tag = GlobalVariables.m_EnemyTag;
        }
        else
        {
            m_TowerHealthUI.color = Color.blue;
            gameObject.tag = GlobalVariables.m_TeamateTag;

            if (TowerHitbox != null)
                TowerHitbox.tag = GlobalVariables.m_TeamateTag;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(m_Health);
        }
        else
        {
            UpdateHealthBar((int)stream.ReceiveNext());
        }
    }

    public void UpdateHealthBar(int _health)
    {
        m_Health = _health;
        //percent of health from max health
        var percent = m_Health / (float)m_MaxHealth;
        m_TowerHealthUI.fillAmount = percent;
    }
    public void ShowDamage(int _damage)
    {
        GameObject damageObject = Instantiate(m_DamagePrefab, m_DamageCanvas.transform);
        var damageText = damageObject.GetComponent<TextMeshProUGUI>();
        damageText.text = _damage.ToString();
        Destroy(damageObject, 3f);
    }

    public void ExecuteDamageRPC(PhotonView senderPhotonView, int _damage)
    {
        senderPhotonView.RPC("RPC_ApplyDamageToTower", RpcTarget.MasterClient, _damage);
    }

    [PunRPC]
    public void RPC_ApplyDamageToTower(int _damage)
    {
        TakeDamage(_damage);
    }
    public void TakeDamage(int _damage)
    {
        m_Health -= _damage;
        CheckIfGoToDestroy();
        UpdateHealthBar(m_Health);
        // ShowDamage(_damage);
        //CheckIfGoToDead();
    }
    public void CheckIfGoToDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (m_Health <= 0)
            {
                ExecuteGoToDestroy();
            }
        }
    }
    public void ExecuteGoToDestroy()
    {
        m_Tower.m_TowerPhotonView.RPC("RPC_GoToDestroy", RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void RPC_GoToDestroy()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
