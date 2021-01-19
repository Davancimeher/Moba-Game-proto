using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum CurrentRoomState : byte
{
    WAITING = 0,
    READY_COUNTDOWN = 1,
    CHOOSE_COUNTDOWN = 2,
    FREZZ = 3,
    IN_LOADING = 4,
    IN_GAME = 5,
    ENDMATCH = 6
}
public class CurrentRoomManager : MonoBehaviour, IInRoomCallbacks
{

    public static CurrentRoomManager CRM; //SINGLETON

    public CurrentRoomState m_RoomState = CurrentRoomState.WAITING;// ROOM STATE

    #region PUBLIC VARIABLES

    public byte m_MyTeamIndex;
    public byte m_ReadyCountDown;
    public byte m_ChooseHeroCountDown;
    #endregion

    #region PRIVATE VARIABLES
    private AsyncOperation loadOperation = null;
    private bool sceneLoaded = false;
    private bool sceneInLoading = false;
    private bool SceneLoadedSended = false;
    #endregion

    private void Awake()
    {
        #region singleton
        if (CRM == null)
        {
            CRM = this;
        }
        else
        {
            if (CRM != this)
            {
                CRM = this;
            }
        }
        #endregion
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    #region PUBLIC FUNCTIONS
    public void AddPlayer(Player player)
    {
        if (!RoomData.RD.m_playersDictionary.ContainsKey(player.ActorNumber))
        {
            RoomData.RD.m_playersDictionary.Add(player.ActorNumber, player);
        }
    }
    public void AddPlayers(List<Player> players)
    {
        foreach (var player in players)
        {
            if (!RoomData.RD.m_playersDictionary.ContainsKey(player.ActorNumber))
            {
                RoomData.RD.m_playersDictionary.Add(player.ActorNumber, player);
            }
        }
    }
    public void RemovePlayer(Player player)
    {
        if (RoomData.RD.m_playersDictionary.ContainsKey(player.ActorNumber))
            RoomData.RD.m_playersDictionary.Remove(player.ActorNumber);
    }


    public void AddPlayersObjects(List<Player> players)
    {
        foreach (var player in players)
        {
            UIGenerator.UIG.AddPlayerAvatarUIObjects(player);
        }
    }
    public void RemovePlayerObjects(Player player)
    {
        UIGenerator.UIG.RemovePlayerUIObject(player);
    }
    public void AddPlayerObjects(Player player)
    {
        UIGenerator.UIG.AddPlayerAvatarUIObjects(player);
    }
    public void AddPlayersLoadObjects(List<Player> players)
    {
        foreach (var player in players)
        {
            UIGenerator.UIG.AddPlayerLoadUIObjects(player);
        }
    }
    #endregion

    #region PRIVATE FUNCTION
    private Dictionary<int, byte> GetPlayersTeam()
    {
        Dictionary<int, byte> playersTeamDict = new Dictionary<int, byte>();
        byte currentTeamIndex = 1;

        foreach (var player in RoomData.RD.m_playersDictionary.Values)
        {
            if (!playersTeamDict.ContainsKey(player.ActorNumber))
            {
                playersTeamDict.Add(player.ActorNumber, currentTeamIndex);
                currentTeamIndex = ChangeTeamIndex(currentTeamIndex);
            }

        }
        return playersTeamDict;
    }
    private byte ChangeTeamIndex(byte teamIndex)
    {
        if (teamIndex == 1) return 2;
        else return 1;
    }
    private List<string> SerializePlayersTeam(Dictionary<int, byte> _playerTeamsDict)
    {
        List<string> serializedPlayersTeamStrings = new List<string>();

        foreach (var pair in _playerTeamsDict)
        {
            string playerTeam = pair.Key.ToString() + "|" + pair.Value.ToString();
            if (!serializedPlayersTeamStrings.Contains(playerTeam))
            {
                serializedPlayersTeamStrings.Add(playerTeam);
            }
        }
        return serializedPlayersTeamStrings;
    }
    private Dictionary<int, byte> DeserializePlayersTeam(List<object> PlayersTeamStrings)
    {
        Dictionary<int, byte> DeserializedPlayersTeam = new Dictionary<int, byte>();

        foreach (var PlayersTeam in PlayersTeamStrings)
        {
            string PlayersTeamString = PlayersTeam.ToString();
            string[] subs = PlayersTeamString.Split('|');

            int actor = int.Parse(subs[0]);
            byte team = Convert.ToByte(subs[1]);

            if (!DeserializedPlayersTeam.ContainsKey(actor))
            {
                DeserializedPlayersTeam.Add(actor, team);
            }
        }
        return DeserializedPlayersTeam;
    }
    private List<int> GetNonReadyPlayers()
    {
        List<int> nonReadyPlayers = new List<int>();

        foreach (var player in RoomData.RD.m_playersDictionary)
        {
            if (!RoomData.RD.m_PlayersReady.Contains(player.Value.ActorNumber))
            {
                if (!nonReadyPlayers.Contains(player.Value.ActorNumber))
                {
                    nonReadyPlayers.Add(player.Value.ActorNumber);
                }
            }
        }
        return nonReadyPlayers;
    }
    private void SetRandomHeros()
    {
        foreach (var teamIndex in GlobalVariables.m_TeamIndex)
        {
            Dictionary<int, byte> playerTeamHeros = new Dictionary<int, byte>();

            var Playerlist = RoomData.RD.m_PlayersTeams.Where(x => x.Value == teamIndex)
                  .Select(x => x.Key)
                  .ToList();

            List<byte> herosList = GameDataManager.GDM.m_HeroHandlers.Keys.ToList();

            List<int> playersWithNoHero = new List<int>();

            foreach (var player in Playerlist)
            {
                if (RoomData.RD.m_PlayersHero.ContainsKey(player))
                {
                    herosList.Remove(RoomData.RD.m_PlayersHero[player]);
                }
                else
                {
                    playersWithNoHero.Add(player);
                }
            }

            foreach (var player in playersWithNoHero)
            {
                var hero = herosList.PickRandom();
                SendRandomHero(player, hero);
                herosList.Remove(hero);
            }

        }

    }
    #endregion

    #region LISTNER REGION

    private void NetworkingClient_EventReceived(EventData obj)
    {
        NetworkEvent eventType = (NetworkEvent)obj.Code;

        switch (eventType)
        {
            case NetworkEvent.SEND_READY:

                RecieveReady(obj.CustomData);
                break;

            case NetworkEvent.SEND_TEAMS:
                RecieveTeams(obj.CustomData);
                break;

            case NetworkEvent.SEND_ROOM_STATE:
                RecieveUpdateRoomState(obj.CustomData);
                break;

            case NetworkEvent.SEND_READY_COUNTDOWN:
                RecieveUpdateReadyCountDown(obj.CustomData);
                break;
            case NetworkEvent.SEND_LEAVE_ROOM:
                RecieveLeaveRoom(obj.CustomData);
                break;
            case NetworkEvent.SEND_HERO_COUNTDOWN:
                RecieveUpdateHeroCountDown(obj.CustomData);
                break;
            case NetworkEvent.SEND_LOCK_HERO:
                RecieveLockHero(obj.CustomData);
                break;
            case NetworkEvent.SEND_LOAD_GAME_SCENE:
                RecieveLoadGameScene(obj.CustomData);
                break;
            case NetworkEvent.SEND_RANDOM_HERO:
                RecieveRandomHero(obj.CustomData);
                break;
            case NetworkEvent.SEND_SCENE_LOADED:
                RecieveSceneLoaded(obj.CustomData);
                break;
            case NetworkEvent.SEND_ACTIVE_GAME_SCENE:
                RecieveActiveGameScene(obj.CustomData);
                break;
        }
    }
    private void RecieveReady(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            var actorNumber = (int)datas[0];
            if (UIGenerator.UIG.m_playersReadyUIDictionary.ContainsKey(actorNumber))
            {
                Debug.Log("recieving ready from ID : " + (RoomData.RD.m_playersDictionary[actorNumber].NickName));

                UIGenerator.UIG.m_playersReadyUIDictionary[actorNumber].SetPlayerReady();

                if (!RoomData.RD.m_PlayersReady.Contains(actorNumber))
                    RoomData.RD.m_PlayersReady.Add(actorNumber);

                if (PhotonNetwork.IsMasterClient)
                {
                    if (RoomData.RD.m_PlayersReady.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
                    {
                        //send teams 
                        RoomData.RD.m_PlayersTeams = GetPlayersTeam();
                        var SerializedPlayersTeam = SerializePlayersTeam(RoomData.RD.m_PlayersTeams);

                        SendTeams(SerializedPlayersTeam);
                        SendUpdateMatchState(CurrentRoomState.CHOOSE_COUNTDOWN);
                        StopCoroutine(ReadyTimeCoroutine());
                    }
                }

            }
        }

    }
    private void RecieveTeams(object content)
    {
        object[] datas = content as object[];
        var dataList = datas.ToList();

        RoomData.RD.m_PlayersTeams = DeserializePlayersTeam(dataList);

        if (RoomData.RD.m_PlayersTeams.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            m_MyTeamIndex = RoomData.RD.m_PlayersTeams[PhotonNetwork.LocalPlayer.ActorNumber];
        }

        List<int> teamPlayersActor = new List<int>();

        foreach (var player in RoomData.RD.m_PlayersTeams)
        {
            if (player.Value == m_MyTeamIndex)
            {
                teamPlayersActor.Add(player.Key);
            }
        }

        foreach (var actor in teamPlayersActor)
        {
            if (RoomData.RD.m_playersDictionary.ContainsKey(actor))
            {
                if (!RoomData.RD.m_MyTeamPlayers.ContainsKey(actor))
                    RoomData.RD.m_MyTeamPlayers.Add(actor, RoomData.RD.m_playersDictionary[actor]);
            }
        }
        GameDataManager.GDM.SetTeamPlayersName();
    }
    private void RecieveUpdateRoomState(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            m_RoomState = (CurrentRoomState)datas[0];
            Debug.Log("recieving room state update : " + m_RoomState.ToString());

            switch (m_RoomState)
            {
                case CurrentRoomState.WAITING:
                    UIManager.UIM.ResetHerosButtons();

                    break;
                case CurrentRoomState.READY_COUNTDOWN:
                    m_ReadyCountDown = (byte)GlobalVariables.m_ReadyCountDown;
                    PlayerState.m_Instance.OverrideState(State.IN_READY_PANEL);

                    UIManager.UIM.ResetHerosButtons();

                    if (PhotonNetwork.IsMasterClient)
                        StartCoroutine(ReadyTimeCoroutine());
                    break;
                case CurrentRoomState.CHOOSE_COUNTDOWN:
                    UIGenerator.UIG.AddPlayersTeamUIObject(RoomData.RD.m_MyTeamPlayers.Values.ToList());
                    PlayerState.m_Instance.OverrideState(State.IN_HERO_PANEL);
                    m_ChooseHeroCountDown = (byte)GlobalVariables.m_ChooseHeroCountDown;

                    if (PhotonNetwork.IsMasterClient)
                        StartCoroutine(ChooseHeroCoroutine());
                    break;
                case CurrentRoomState.ENDMATCH:
                    break;
            }
        }
    }
    private void RecieveUpdateReadyCountDown(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            m_ReadyCountDown = (byte)datas[0];
            UIManager.UIM.UpdateReadyCountdownUI(m_ReadyCountDown);
        }
    }
    private void RecieveLeaveRoom(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == (int)datas[0])
            {
                PhotonNetwork.LeaveRoom();
            }
        }
    }
    private void RecieveUpdateHeroCountDown(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            m_ChooseHeroCountDown = (byte)datas[0];
            UIManager.UIM.UpdateChooseHeroCountdownUI(m_ChooseHeroCountDown);
        }
    }
    private void RecieveLockHero(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 2)
        {
            var playerActor = (int)datas[0];
            var heroId = (byte)datas[1];

            if (RoomData.RD.m_PlayersHero.ContainsKey(playerActor))
            {
                RoomData.RD.m_PlayersHero[playerActor] = heroId;
            }
            else
            {
                RoomData.RD.m_PlayersHero.Add(playerActor, heroId);
            }

            if (!RoomData.RD.m_MyTeamPlayers.ContainsKey(playerActor)) return;

            //player changed his selected hero
            if (RoomData.RD.m_PlayersTeamHero.ContainsKey(playerActor))
            {
                var oldHero = RoomData.RD.m_PlayersTeamHero[playerActor];

                if (GameDataManager.GDM.m_HeroHandlers.ContainsKey(oldHero))
                {
                    GameDataManager.GDM.m_HeroHandlers[oldHero].UnlockHero();
                }

                var playerUI = UIGenerator.UIG.m_playerTeamsGO[playerActor];

                playerUI.SetHero(heroId);

                RoomData.RD.m_PlayersTeamHero[playerActor] = heroId;
            }
            //first selected Hero
            else
            {
                if (UIGenerator.UIG.m_playerTeamsGO.ContainsKey(playerActor))
                {
                    var playerUI = UIGenerator.UIG.m_playerTeamsGO[playerActor];
                    playerUI.SetHero(heroId);
                }
                RoomData.RD.m_PlayersTeamHero.Add(playerActor, heroId);
            }
            if (playerActor == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                GameDataManager.GDM.SetHeroId(heroId);
            }
            else
            {
                GameDataManager.GDM.SetTeamPlayerHero(playerActor, heroId);
            }

        }
    }
    private void RecieveRandomHero(object content)
    {
        object[] datas = content as object[];

        int actor = (int)datas[0];
        byte hero = (byte)datas[1];

        Debug.Log("Recieve Random Hero to " + RoomData.RD.m_playersDictionary[actor].NickName + " Hero : " + GameDataManager.GDM.m_HerosDict[hero].HeroName);

        if (RoomData.RD.m_PlayersHero.ContainsKey(actor))
        {
            RoomData.RD.m_PlayersHero[actor] = hero;
        }
        else
        {
            RoomData.RD.m_PlayersHero.Add(actor, hero);
        }

        if (actor == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("SendRandomHero to me : " + " Hero : " + GameDataManager.GDM.m_HerosDict[hero].HeroName);

            GameDataManager.GDM.SetSelectedHero(hero);
            GameDataManager.GDM.SetHeroId(hero);
        }
        if (RoomData.RD.m_MyTeamPlayers.ContainsKey(actor))
        {
            if (UIGenerator.UIG.m_playerTeamsGO.ContainsKey(actor))
            {
                var playerUI = UIGenerator.UIG.m_playerTeamsGO[actor];
                playerUI.SetHero(hero);
            }
            RoomData.RD.m_PlayersTeamHero.Add(actor, hero);
        }

    }
    private void RecieveLoadGameScene(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            Debug.Log("RecieveLoadGameScene ");
            if (!sceneInLoading)
                StartCoroutine(LoadSceneCoroutine());
        }
    }
    private void RecieveSceneLoaded(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            var actor = (int)datas[0];
            if (UIGenerator.UIG.m_playersUILoadDictionary.ContainsKey(actor))
            {
                UIGenerator.UIG.m_playersUILoadDictionary[actor].SetSceneLoaded();
                if (!RoomData.RD.m_PlayersSceneReady.Contains(actor))
                {
                    RoomData.RD.m_PlayersSceneReady.Add(actor);
                }
            }

            if (PhotonNetwork.IsMasterClient)
            {
                if (RoomData.RD.m_PlayersSceneReady.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    StartCoroutine(ActivateSceneCoroutine());

                }
            }
        }
    }
    private void RecieveActiveGameScene(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 1)
        {
            Debug.Log("Recieve Active Game Scene ");
            if (loadOperation != null)
            {
                loadOperation.allowSceneActivation = true;
            }
        }
    }
    #endregion

    #region SENDER REGION
    public void SendReady()
    {
        if (RoomData.RD.m_PlayersReady.Contains(PhotonNetwork.LocalPlayer.ActorNumber)) return;

        object[] datas = new object[]
        {
            PhotonNetwork.LocalPlayer.ActorNumber
        };
        Debug.Log("sending ID: " + PhotonNetwork.LocalPlayer.UserId + " Ready !");

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_READY, datas, options, SendOptions.SendReliable);
    }
    private void SendUpdateMatchState(CurrentRoomState currentRoomState)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            object[] datas = new object[]
            {
                 (byte)currentRoomState
            };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                CachingOption = EventCaching.DoNotCache,
                Receivers = ReceiverGroup.All
            };
            PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_ROOM_STATE, datas, options, SendOptions.SendReliable);
        }

    }
    public void SendUpdateReadyCountDown()
    {
        object[] datas = new object[]
        {
            m_ReadyCountDown
        };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_READY_COUNTDOWN, datas, options, SendOptions.SendUnreliable);
    }
    public void SendLeaveRoom(int actor)
    {
        object[] datas = new object[]
        {
            actor
        };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_LEAVE_ROOM, datas, options, SendOptions.SendReliable);
    }
    public void SendTeams(List<string> serializedPlayersTeamStrings)
    {
        List<object> datas = new List<object>();

        foreach (var playersTeamStrings in serializedPlayersTeamStrings)
        {
            datas.Add(playersTeamStrings);
        }
        datas.ToArray();

        Debug.Log("sending Teams !");

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_TEAMS, datas, options, SendOptions.SendReliable);
    }
    public void SendUpdateHeroCountDown()
    {
        object[] datas = new object[]
        {
            m_ChooseHeroCountDown
        };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.Others
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_HERO_COUNTDOWN, datas, options, SendOptions.SendUnreliable);
    }
    public void SendLockHero(Hero _hero)
    {
        object[] datas = new object[]
        {
            PhotonNetwork.LocalPlayer.ActorNumber,
            _hero.ID
        };
        Debug.Log("sending lock hero : " + _hero.name + " for " + PhotonNetwork.LocalPlayer.NickName);

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_LOCK_HERO, datas, options, SendOptions.SendReliable);
    }
    public void SendRandomHero(int actor, byte _hero)
    {
        object[] datas = new object[]
        {
             actor,
             _hero
        };
        Debug.Log("SendRandomHero to " + RoomData.RD.m_playersDictionary[actor].NickName + " Hero : " + GameDataManager.GDM.m_HerosDict[_hero].HeroName); ;

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_RANDOM_HERO, datas, options, SendOptions.SendReliable);
    }
    public void SendLoadGameScene()
    {
        object[] datas = new object[]
        {
            GlobalVariables.m_GameSceneIndex
        };

        Debug.Log("sending load scene");

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_LOAD_GAME_SCENE, datas, options, SendOptions.SendReliable);
    }
    public void SendSceneLoaded()
    {
        object[] datas = new object[]
        {
            PhotonNetwork.LocalPlayer.ActorNumber
        };

        Debug.Log("sending scene loaded !");

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_SCENE_LOADED, datas, options, SendOptions.SendReliable);
    }
    public void SendActiveGameScene()
    {
        object[] datas = new object[]
        {
            true
        };

        Debug.Log("sending active game scene scene");

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SEND_ACTIVE_GAME_SCENE, datas, options, SendOptions.SendReliable);
    }

    #endregion

    #region COROUTINE REGION
    private IEnumerator ReadyTimeCoroutine()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            while (m_RoomState == CurrentRoomState.READY_COUNTDOWN)
            {
                Debug.Log("ReadyTimeCouretine");
                yield return new WaitForSeconds(1f);
                m_ReadyCountDown--;
                if (m_ReadyCountDown > 0)
                {
                    UIManager.UIM.UpdateReadyCountdownUI(m_ReadyCountDown);
                    SendUpdateReadyCountDown();
                }
                else
                {

                    var nonReadyPlayers = GetNonReadyPlayers();


                    if (nonReadyPlayers.Count > 0)
                    {
                        if (nonReadyPlayers.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
                        {
                            nonReadyPlayers.Remove(PhotonNetwork.LocalPlayer.ActorNumber);
                            foreach (var player in nonReadyPlayers)
                            {
                                SendLeaveRoom(player);
                            }
                            PhotonNetwork.LeaveRoom();
                        }
                        else
                        {
                            foreach (var player in nonReadyPlayers)
                            {
                                SendLeaveRoom(player);
                            }
                        }
                    }
                    else
                    {
                        //Show Heros Panel
                        SendUpdateMatchState(CurrentRoomState.CHOOSE_COUNTDOWN);
                    }
                    StopCoroutine(ReadyTimeCoroutine());
                }
            }
        }
    }
    private IEnumerator ChooseHeroCoroutine()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            m_ChooseHeroCountDown = (byte)GlobalVariables.m_ChooseHeroCountDown;
            while (m_RoomState == CurrentRoomState.CHOOSE_COUNTDOWN)
            {
                yield return new WaitForSeconds(1);
                m_ChooseHeroCountDown--;

                if (m_ChooseHeroCountDown > 0)
                {
                    SendUpdateHeroCountDown();
                    UIManager.UIM.UpdateChooseHeroCountdownUI(m_ChooseHeroCountDown);
                }
                else
                {
                    if (RoomData.RD.m_PlayersHero.Count == PhotonNetwork.CurrentRoom.PlayerCount)
                    {
                        Debug.Log("Send load scene");
                        SendLoadGameScene();
                        StopCoroutine(ChooseHeroCoroutine());
                    }
                    else
                    {
                        Debug.Log("Set random Heros ");

                        SetRandomHeros();
                    }

                }
            }
        }
    }
    private IEnumerator LoadSceneCoroutine()
    {
        sceneInLoading = true;

        yield return new WaitForSeconds(1);

        var playerList = RoomData.RD.m_playersDictionary.Values.ToList();

        AddPlayersLoadObjects(playerList);

        Debug.Log("LoadSceneCoroutine AddPlayersLoadObjects");


        PlayerState.m_Instance.OverrideState(State.IN_LOADING_PANEL);
        Debug.Log("LoadSceneCoroutine OverrideState");


        if (!sceneLoaded)
        {
            loadOperation = SceneManager.LoadSceneAsync(GlobalVariables.m_GameSceneIndex);
            Debug.Log("LoadSceneCoroutine LoadSceneAsync");

            loadOperation.allowSceneActivation = false;
            sceneLoaded = true;

            while (!loadOperation.isDone)
            {
                // Check if the load has finished
                if (loadOperation.progress >= 0.9f)
                {
                    if (!SceneLoadedSended)
                    {
                        SendSceneLoaded();
                        SceneLoadedSended = true;
                        StopCoroutine(LoadSceneCoroutine());
                    }

                }
                yield return null;
            }

        }
    }

    private IEnumerator ActivateSceneCoroutine()
    {
        yield return new WaitForSeconds(5f);
        //send active scene
        SendActiveGameScene();
        StopCoroutine(ActivateSceneCoroutine());
    }
    #endregion


    #region IInRoomCallbacks
    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayer(newPlayer);

        AddPlayerObjects(newPlayer);

        Debug.Log("New Player : " + newPlayer.NickName);

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            SendUpdateMatchState(CurrentRoomState.READY_COUNTDOWN);
        }
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemovePlayer(otherPlayer);

        RemovePlayerObjects(otherPlayer);


        Debug.Log(otherPlayer.NickName + " Leave ! ");

        RoomData.RD.ClearPlayerData();

        PlayerState.m_Instance.OverrideState(State.INLOBBY);

        SendUpdateMatchState(CurrentRoomState.WAITING);
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {

    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {

    }
    #endregion
}
