using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SetPlayerInGameInfo : MonoBehaviour, IPunInstantiateMagicCallback
{
    public GameObject m_ChampionCanvas;
    public TextMeshProUGUI m_PlayerName;
    public Image HealthImage;
    public TowerTarget m_TowerTarget;
    public PhotonView m_PhotonView;
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void SetPlayerName(string _playerName)
    {
        m_PlayerName.text = _playerName;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        SetPlayerName(info.Sender.NickName);

        object[] instantiationData = info.photonView.InstantiationData;

        SetTowerTarget(info.Sender.ActorNumber);

        if (RoomData.RD.m_MyTeamPlayers.ContainsKey(info.Sender.ActorNumber))
        {
            ChampionManager championManager = this.gameObject.GetComponent<ChampionManager>();
            championManager.m_HitBox.tag = GlobalVariables.m_TeamateTag;
            HealthImage.color = Color.blue;
        }
        else
        {
            ChampionManager championManager = this.gameObject.GetComponent<ChampionManager>();
            championManager.m_HitBox.tag = GlobalVariables.m_EnemyTag;
            HealthImage.color = Color.red;
        }
        if (!RoomData.RD.PlayersChampions.ContainsKey(info.Sender.ActorNumber))
        {
            RoomData.RD.PlayersChampions.Add(info.Sender.ActorNumber, this.gameObject);
        }

        HealthManager healthManager = this.gameObject.GetComponent<HealthManager>();

        healthManager.MaxHealth = (int)instantiationData[0];

        info.Sender.TagObject = gameObject;
    }

    public void SetTowerTarget(int playerActor)
    {
        m_TowerTarget.teamIndex = RoomData.RD.m_PlayersTeams[playerActor];
        m_TowerTarget.ViewID = m_PhotonView.ViewID;
        m_TowerTarget.HealthManager = this.gameObject.GetComponent<HealthManager>();
    }
}
