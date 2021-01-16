using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchMakingManager : MonoBehaviour, IMatchmakingCallbacks, ILobbyCallbacks, IInRoomCallbacks
{
    public void JoinClientToLobby()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        SetNickName();
    }

    private static void SetNickName()
    {
        if (string.IsNullOrEmpty(UIManager.UIM.m_PlayersNameInputField.text))
        {
            string _name = PhotonNetwork.LocalPlayer.UserId.Split('-')[0].ToUpper();
            string name = _name.Remove(3, _name.Length - 4);
            string randomNickName = "Guest_" + name;
            UIManager.UIM.m_PlayersNameInputField.text = randomNickName;
            PhotonNetwork.LocalPlayer.NickName = randomNickName;
            UIManager.UIM.m_ProfileName.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = UIManager.UIM.m_PlayersNameInputField.text;
            UIManager.UIM.m_ProfileName.text = PhotonNetwork.LocalPlayer.NickName;
        }

    }

    public void LeaveClientToLobby()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void JoinClientToRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    private RoomOptions CreateRoomOptions()
    {
        ExitGames.Client.Photon.Hashtable Costume = new ExitGames.Client.Photon.Hashtable();
        // Costume.Add("Time", 20);

        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)GlobalVariables.m_MaxPlayersInRoom,
            PlayerTtl = 1,
            EmptyRoomTtl = 5,
            CustomRoomProperties = Costume,
            CleanupCacheOnLeave = true
        };

        return roomOptions;
    }
    public void CreateRoom()
    {
        RoomOptions roomOptions = CreateRoomOptions();
        string roomName = PhotonNetwork.LocalPlayer.UserId.Split('-')[0].ToUpper();

        if (PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default))
        {
            Debug.Log("create room Succefully sent");
        }
        else
        {
            Debug.Log("create room failed to sent");
        }


    }

    #region Subscribe To Callback Region
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    #endregion
    #region ILobbyCallbacks
    public void OnJoinedLobby()
    {
        Debug.Log("Player joined Lobby");
        PlayerState.m_Instance.OverrideState(State.INLOBBY);
        UIManager.UIM.StartMatchMakingTime();
        JoinClientToRandomRoom();
    }

    public void OnLeftLobby()
    {


    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {

    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {

    }
    #endregion

    #region IMatchmakingCallbacks
    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {

    }

    public void OnCreatedRoom()
    {
        Debug.Log("Room Created");
        RoomData.RD.ClearAllData();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {

    }

    public void OnJoinedRoom()
    {

        UIManager.UIM.InitPlayerInLobbyCount(PhotonNetwork.CurrentRoom.PlayerCount, GlobalVariables.m_MaxPlayersInRoom, true);

        var playerList = PhotonNetwork.CurrentRoom.Players.Values.ToList();

        CurrentRoomManager.CRM.AddPlayers(playerList);
        CurrentRoomManager.CRM.AddPlayersObjects(playerList);

    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {

    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        CreateRoom();
    }

    public void OnLeftRoom()
    {
        PlayerState.m_Instance.OverrideState(State.CONNECTED);
        UIManager.UIM.StopMatchMaking();
        UIManager.UIM.InitPlayerInLobbyCount(0, 0, false);
        Debug.Log("Player Left Lobby");

        UIGenerator.UIG.RemoveAllPlayersUIObjects();
        RoomData.RD.ClearAllData();
    }
    #endregion

    #region IInRoomCallbacks
    public void OnMasterClientSwitched(Player newMasterClient)
    {

    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom");
        UIManager.UIM.OverridePlayerInLobbyCount(PhotonNetwork.CurrentRoom.PlayerCount, GlobalVariables.m_MaxPlayersInRoom);
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom");

        UIManager.UIM.OverridePlayerInLobbyCount(PhotonNetwork.CurrentRoom.PlayerCount, GlobalVariables.m_MaxPlayersInRoom);
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {

    }
    #endregion
}
