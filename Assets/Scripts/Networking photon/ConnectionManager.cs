using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour, IConnectionCallbacks
{
    public bool InGameScene;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
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
    public void ReconnectAndJoin()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ReconnectAndRejoin();
        }
    }

    private void OnPlayerConnected()
    {
        UIManager.UIM.ChangeStartButtonState(true);
        Debug.Log("Player Connected");

        if (InGameManager.IGM != null)
            if (InGameManager.IGM.m_InGameScene)
            {
                InGameManager.IGM.HideDisconnectionInternetPanel();
            }
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
        if (InGameManager.IGM != null)
            if (InGameManager.IGM.m_InGameScene)
            {
                InGameManager.IGM.ShowDisconnectionInternetPanel();
            }
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {

    }
    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            InGameManager.IGM.m_InGameScene = true;
        }
    }
    #endregion
}