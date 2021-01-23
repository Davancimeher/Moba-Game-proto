using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterClientSwitcher : MonoBehaviour
{
    public int m_Ping;

    private ExitGames.Client.Photon.Hashtable PingCustomes = new ExitGames.Client.Photon.Hashtable();
    private int LastPingSended;
    private Player NextMaster;

    private void Start()
    {
        PingCustomes.Add("Ping", PhotonNetwork.GetPing());
        PhotonNetwork.LocalPlayer.SetCustomProperties(PingCustomes);
        StartCoroutine(PingCoroutine());
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("OnApplicationPause");
        if (pauseStatus)
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                // Game is in Background | pause | quit

                //PhotonNetwork.BackgroundTimeout = 2f;                                                
                ChangeMasterClientifAvailble();
                PhotonNetwork.SendAllOutgoingCommands();
            }
        }
        else
        {

            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                // do whatever you want . if you not reach time to live player then you are connected and inroom already.
            }
            else
            {
                PhotonNetwork.ReconnectAndRejoin();
            }

            //check if game end ||room destroyed ||timeout
        }
    } // End OnApplication Pause.


    /// <summary>
    /// Changes the master client if availble.
    /// </summary>
    public void ChangeMasterClientifAvailble()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            return;
        }
        if (NextMaster != null)
        {
            PhotonNetwork.SetMasterClient(NextMaster);
        }
        else
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
            InGameManager.IGM.StopTowers();
        }
    }

    public void SetPing()
    {
        if (m_Ping != LastPingSended)
        {
            PingCustomes["Ping"] = m_Ping;
            LastPingSended = m_Ping;
            PhotonNetwork.LocalPlayer.SetCustomProperties(PingCustomes);
        }
        GetBestPlayerWithPing();
    }

    public IEnumerator PingCoroutine()
    {
        while (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            m_Ping = PhotonNetwork.GetPing();
            SetPing();
            Debug.Log("Get Ping");
            yield return new WaitForSeconds(5f);
        }
    }

    public void GetBestPlayerWithPing()
    {
        var players = PhotonNetwork.CurrentRoom.Players;
        int lowestPing = 999;
        foreach (var playerPair in players)
        {
            if(playerPair.Key != PhotonNetwork.MasterClient.ActorNumber)
            {
                if (playerPair.Value.CustomProperties.ContainsKey("Ping"))
                {
                    var ping = (int)playerPair.Value.CustomProperties["Ping"];
                    if (ping < lowestPing)
                    {
                        lowestPing = ping;
                        Debug.Log("Get best player with ping ! ");
                        NextMaster = playerPair.Value;
                    }
                }
            }
        }
    }

}
