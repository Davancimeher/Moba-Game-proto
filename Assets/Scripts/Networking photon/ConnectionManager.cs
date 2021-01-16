using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ConnectionManager : MonoBehaviour, IConnectionCallbacks
{
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    private void Start()
    {
        ConnectToPhotonServer();
    }

    private void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to server ..");
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    private void OnPlayerConnected()
    {
        UIManager.UIM.ChangeStartButtonState(true);
        Debug.Log("Player Connected");
    }

    #region Photon CallBacks Region
    public void OnConnected()
    {

    }

    public void OnConnectedToMaster()
    {
        OnPlayerConnected();
        PlayerState.m_Instance.OverrideState(State.CONNECTED);
        UIManager.UIM.StartPingCouretine();

    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {

    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {

    }

    public void OnDisconnected(DisconnectCause cause)
    {
        PlayerState.m_Instance.OverrideState(State.DISCONECTED);
        Debug.LogError("Disconnected");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {

    }
    #endregion

}
