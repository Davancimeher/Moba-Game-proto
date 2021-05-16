using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChampionScore : MonoBehaviour
{
    public PhotonView PhotonView;   

    public int Damage;

    public int Kills;
    public int Deaths;

    public int Assists;

    public int TotalScore;


    public TextMeshProUGUI KillsText;
    public TextMeshProUGUI DeathsText;
    public TextMeshProUGUI AssistsText;


    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(Kills);
    //        stream.SendNext(Deaths);
    //        stream.SendNext(Assists);
    //    }
    //    //else
    //    //{
    //    //    Kills = (int)stream.ReceiveNext();
    //    //    Deaths = (int)stream.ReceiveNext();
    //    //    Assists = (int)stream.ReceiveNext();
    //    //    UpdateUI();
    //    //}
    //}

    public void UpdateUI()
    {
        if (PhotonView.IsMine)
        {
            KillsText.text = Kills.ToString();
            DeathsText.text = Deaths.ToString();
            AssistsText.text = Assists.ToString();
        }

    }
    public void AddKill()
    {
        if (PhotonView.IsMine)
        {
            Kills++;
            CalculeNewScore();
            UpdateUI();
        }
    }
    public void AddDeath()
    {
        if (PhotonView.IsMine)
        {
            Deaths++;
            CalculeNewScore();
            UpdateUI();
        }
    }
    public void AddAssists()
    {
        if (PhotonView.IsMine)
        {
            Assists++;
            CalculeNewScore();
            UpdateUI();
        }
    }
    public void AddDamage()
    {
        Damage++;
    }
    public void CalculeNewScore()
    {
        TotalScore = Kills * 10 + Deaths * -3 + Assists;
    }
    public void AddDeath(int _killerActor,int _assiterActor)
    {
        AddDeath();
        Debug.LogError("Add Death : killer id : " + _killerActor);

        if (_killerActor >= 0)
        {
            ExecuteAddKillForKiller(_killerActor);
        }
        if(_assiterActor >= 0 && _assiterActor != _killerActor)
        {
            ExecuteAddAssistForAssistor(_assiterActor);
        }
    }
    public void ExecuteAddKillForKiller(int _killerActor)
    {
        
        Debug.LogError("ExecuteAddKillForKiller : killer id : "+ _killerActor);

        if (RoomData.RD.PlayersChampions.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            PhotonView pv = RoomData.RD.PlayersChampions[PhotonNetwork.LocalPlayer.ActorNumber].GetComponent<PhotonView>();
            pv.RPC("RPC_AddKillForKiller", RpcTarget.AllViaServer, _killerActor);
        }
    }

    [PunRPC]
    public void RPC_AddKillForKiller(int _killerActor)
    {
        if (RoomData.RD.PlayersChampions.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            Debug.LogError("RPC_AddKillForKiller");
            ChampionManager cm = RoomData.RD.PlayersChampions[PhotonNetwork.LocalPlayer.ActorNumber].GetComponent<ChampionManager>();

            if(cm.m_MyPhotonView.ViewID == _killerActor)
            {
                cm.m_ChampionScore.AddKill();
                Debug.LogError("RPC_AddKillForKiller 1 : i'm the Killer");
                Debug.Log("add kill");
            }
            else
            {
                Debug.LogError("RPC_AddKillForKiller 1 : i'm not the Killer  My ID : " + PhotonView.ViewID + " killer ID : " + _killerActor);
            }
        }

    }
    public void ExecuteAddAssistForAssistor(int _AssistorActor)
    {

        Debug.LogError("ExecuteAddAssistForAssistor : assistor id : " + _AssistorActor);

        if (RoomData.RD.PlayersChampions.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            PhotonView pv = RoomData.RD.PlayersChampions[PhotonNetwork.LocalPlayer.ActorNumber].GetComponent<PhotonView>();
            pv.RPC("RPC_AddAssistForAssistor", RpcTarget.AllViaServer, _AssistorActor);
        }
    }

    [PunRPC]
    public void RPC_AddAssistForAssistor(int _AssistorActor)
    {   
        if (RoomData.RD.PlayersChampions.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            Debug.LogError("RPC_AddAssistForAssistor");
            ChampionManager cm = RoomData.RD.PlayersChampions[PhotonNetwork.LocalPlayer.ActorNumber].GetComponent<ChampionManager>();

            if (cm.m_MyPhotonView.ViewID == _AssistorActor)
            {
                cm.m_ChampionScore.AddAssists();
                Debug.LogError("RPC_AddAssistForAssistor 1 : i'm the assistor");
                Debug.Log("add assist");
            }
            else
            {
                Debug.LogError("RPC_AddAssistForAssistor 1 : i'm not the assistor  My ID : " + PhotonView.ViewID + " killer ID : " + _AssistorActor);
            }
        }

    }
}
