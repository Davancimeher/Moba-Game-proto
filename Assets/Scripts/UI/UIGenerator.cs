using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGenerator : MonoBehaviour
{
    public static UIGenerator UIG;

    public Dictionary<int, PlayerAvatarNetwork> m_playersReadyUIDictionary = new Dictionary<int, PlayerAvatarNetwork>();
    public Dictionary<int, PlayerAvatarNetwork> m_playersUILoadDictionary = new Dictionary<int, PlayerAvatarNetwork>();
    public Dictionary<int, PlayerTeamsHandler> m_playerTeamsGO = new Dictionary<int, PlayerTeamsHandler>();
    public Dictionary<int, GameObject> m_playerTeamChampions = new Dictionary<int, GameObject>();

    private void Awake()
    {
        #region singleton
        if (UIG == null)
        {
            UIG = this;
        }
        else
        {
            if (UIG != this)
            {
                UIG = this;
            }
        }
        #endregion
    }
    public void RemoveAllPlayersUIObjects()
    {
        RemoveAllPlayersAvatarUIObjects();
        RemoveAllPlayersLoadUIObjects();
        RemoveAllPlayersTeamUIObject();
    }
    public void RemovePlayerUIObject(Player player)
    {
        RemovePlayerAvatarUIObjects(player);
        RemovePlayerLoadUIObjects(player);
        RemoveAllPlayersTeamUIObject();
    }

    #region Ready UI Objects
    public void InitPlayerAvatarUIObjects()
    {
        foreach (var UIObject in m_playersReadyUIDictionary.Values)
        {
            UIObject.ResetReadyUIObject();
        }
    }
    public void AddPlayerAvatarUIObjects(Player player)
    {
        var obj = Instantiate(UIManager.UIM.m_AvatarNetworkPrefab, UIManager.UIM.m_AvatarNetworkPrefabsParent);
        PlayerAvatarNetwork playerUIItulity = obj.GetComponent<PlayerAvatarNetwork>();

        playerUIItulity.Init(player);

        if (!m_playersReadyUIDictionary.ContainsKey(player.ActorNumber))
            m_playersReadyUIDictionary.Add(player.ActorNumber, playerUIItulity);
    }
    public void AddOldPlayersAvatarUIObjects(List<Player> players)
    {
        foreach (var player in players)
        {
            var obj = Instantiate(UIManager.UIM.m_AvatarNetworkPrefab, UIManager.UIM.m_AvatarNetworkPrefabsParent);
            PlayerAvatarNetwork playerUIItulity = obj.GetComponent<PlayerAvatarNetwork>();

            playerUIItulity.Init(player);

            if (!m_playersReadyUIDictionary.ContainsKey(player.ActorNumber))
                m_playersReadyUIDictionary.Add(player.ActorNumber, playerUIItulity);
        }
    }
    private void RemovePlayerAvatarUIObjects(Player player)
    {
        if (m_playersReadyUIDictionary.ContainsKey(player.ActorNumber))
        {
            Destroy(m_playersReadyUIDictionary[player.ActorNumber].gameObject);
            m_playersReadyUIDictionary.Remove(player.ActorNumber);
        }

        InitPlayerAvatarUIObjects();
    }
    private void RemoveAllPlayersAvatarUIObjects()
    {
        foreach (var UiObject in m_playersReadyUIDictionary.Values)
        {
            Destroy(UiObject.gameObject);

        }
        m_playersReadyUIDictionary.Clear();
    }
    #endregion

    #region Load UI Objects

    public void InitPlayerLoadUIObjects()
    {
        foreach (var UIObject in m_playersUILoadDictionary.Values)
        {
            UIObject.ResetLoadUIObject();
        }
    }
    public void AddPlayerLoadUIObjects(Player player)
    {
        var obj = Instantiate(UIManager.UIM.m_PlayerNetworkPrefab, UIManager.UIM.m_PlayerNetworkPrefabsParent);
        PlayerAvatarNetwork playerUIItulity = obj.GetComponent<PlayerAvatarNetwork>();

        playerUIItulity.LoadSceneInit(player);

        if (!m_playersUILoadDictionary.ContainsKey(player.ActorNumber))
            m_playersUILoadDictionary.Add(player.ActorNumber, playerUIItulity);
    }
    private void RemovePlayerLoadUIObjects(Player player)
    {
        if (m_playersUILoadDictionary.ContainsKey(player.ActorNumber))
        {
            Destroy(m_playersUILoadDictionary[player.ActorNumber].gameObject);
            m_playersUILoadDictionary.Remove(player.ActorNumber);
        }

        InitPlayerLoadUIObjects();
    }
    private void RemoveAllPlayersLoadUIObjects()
    {
        foreach (var UiObject in m_playersUILoadDictionary.Values)
        {
            Destroy(UiObject.gameObject);

        }
        m_playersUILoadDictionary.Clear();
    }
    #endregion

    #region Team UI Objects
    public void AddPlayersTeamUIObject(List<Player> players)
    {
        foreach (var player in players)
        {
            GameObject playerTeamUIObj = Instantiate(UIManager.UIM.m_PlayersTeamPrefab, UIManager.UIM.m_TeamLayoutGroup);
            PlayerTeamsHandler playerTeamUI = playerTeamUIObj.GetComponent<PlayerTeamsHandler>();

            playerTeamUI.Init(player);

            if (!m_playerTeamsGO.ContainsKey(player.ActorNumber))
            {
                m_playerTeamsGO.Add(player.ActorNumber, playerTeamUI);
            }
        }
    }
    private void RemoveAllPlayersTeamUIObject()
    {
        foreach (var UiObject in m_playerTeamsGO.Values)
        {
            Destroy(UiObject.gameObject);
        }
        m_playerTeamsGO.Clear();
    }
    #endregion


}
