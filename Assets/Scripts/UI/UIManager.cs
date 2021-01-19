using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager UIM;

    [Header("Start Button")]
    public Button m_startButton;

    [Header("Matchmaking type")]
    public TMP_Dropdown m_MacthmakingDropDown;
    public TextMeshProUGUI m_MacthmakingDropDownLabes;

    [Header("UI Text")]

    public TextMeshProUGUI m_MatchmakingTimeText;
    public TextMeshProUGUI m_PlayerInLobbyCount;
    public TextMeshProUGUI m_ReadyCountDownText;
    public TextMeshProUGUI m_ChooseHeroCountDownText;

    [Header("UI Panels")]

    public GameObject m_MainPanel;
    public GameObject m_MatchMakingPanel;
    public GameObject m_ReadyPanel;
    public GameObject m_HeroPanel;
    public GameObject m_HeroInvorinement;
    public GameObject m_LoadingPanel;


    public TMP_InputField m_PlayersNameInputField;

    [Header("Avatars Network")]

    public GameObject m_AvatarNetworkPrefab;
    public Transform m_AvatarNetworkPrefabsParent;

    [Header("Avatars")]

    public GameObject m_AvatarListingPrefab;
    public Transform m_AvatarLayoutGroup;

    [Header("Heros")]
    public GameObject m_HeroListingPrefab;
    public Transform m_HeroLayoutGroup;

    [Header("Players Team")]
    public GameObject m_PlayersTeamPrefab;
    public Transform m_TeamLayoutGroup;

    [Header("Lock Hero Button")]    
    public Button m_LockHeroButton;

    [Header("Loading")]
    public TextMeshProUGUI m_LoadingProgressText;

    [Header("Profil")]
    public GameObject m_ProfilCanvas;
    public Image m_AvatarProfil;
    public TextMeshProUGUI m_ProfileName;
    public TextMeshProUGUI m_PingText;

    [Header("Load Scene Network")]
    public GameObject m_PlayerNetworkPrefab;
    public Transform m_PlayerNetworkPrefabsParent;
        
    private int MyPing;
    private int timeInMatchMaking = 0;

    private void Awake()
    {
        #region singleton
        if (UIM == null)
        {
            UIM = this;
        }
        else
        {
            if (UIM != this)
            {
                UIM = this;
            }
        }
        #endregion
    }

    public void ChangeStartButtonState(bool state)
    {
        m_startButton.interactable = state;
    }
   
    public void StartMatchMakingTime()
    {
        PlayerState.m_Instance.OverrideState(State.INLOBBY);

        StartCoroutine(MatchMakingTimeCoroutine());
    }
    public void StopMatchMaking()
    {
        PlayerState.m_Instance.OverrideState(State.IN_MAIN_PANEL);

        timeInMatchMaking = 0;
    }
    public void UpdateLobbyTimeUI(int time_S)
    {
        string minutes = (time_S / 60).ToString("0");
        string seconds = (time_S % 60).ToString("00");
        m_MatchmakingTimeText.text = $"{minutes}:{seconds}";
    }
    public void OverridePlayerInLobbyCount(int _playerCount, int _maxPlayers)
    {
        m_PlayerInLobbyCount.text = _playerCount + "/" + _maxPlayers;
        if (_playerCount == _maxPlayers)
        {
            PlayerState.m_Instance.OverrideState(State.IN_READY_PANEL);

        }
        else
        {
            PlayerState.m_Instance.OverrideState(State.INLOBBY);
        }
    }
    public void InitPlayerInLobbyCount(int _playerCount, int _maxPlayers, bool isActive)
    {
        m_PlayerInLobbyCount.text = _playerCount + "/" + _maxPlayers;
        m_PlayerInLobbyCount.gameObject.SetActive(isActive);
        if (_playerCount == _maxPlayers && _maxPlayers != 0)
        {
            ShowReadyPanel();
        }
    }

    public void ShowMainPanel()
    {
        if (!m_MainPanel.activeSelf)
        {
            m_MainPanel.SetActive(true);
        }
    }
    public void HideMainPanel()
    {
        if (m_MainPanel.activeSelf)
        {
            m_MainPanel.SetActive(false);
        }
    }

    public void ShowMatchMakingPanel()
    {
        if (!m_MatchMakingPanel.activeSelf)
        {
            m_MatchMakingPanel.SetActive(true);
        }
    }
    public void HideMatchMakingPanel()
    {
        if (m_MatchMakingPanel.activeSelf)
        {
            m_MatchMakingPanel.SetActive(false);
        }
    }

    public void ShowReadyPanel()
    {
        if (!m_ReadyPanel.activeSelf)
        {
            m_ReadyPanel.SetActive(true);
        }
    }
    public void HideReadyPanel()
    {
        if (m_ReadyPanel.activeSelf)
        {
            m_ReadyPanel.SetActive(false);
        }
    }

    public void ShowHeroPanel()
    {
        if (!m_HeroPanel.activeSelf)
        {
            m_HeroPanel.SetActive(true);
        }
        if (!m_HeroInvorinement.activeSelf)
        {
            m_HeroInvorinement.SetActive(true);
        }
        if (m_ProfilCanvas.activeSelf)
        {
            m_ProfilCanvas.SetActive(false);
        }

    }
    public void HideHeroPanel()
    {
        if (m_HeroPanel.activeSelf)
        {
            m_HeroPanel.SetActive(false);
        }
        if (m_HeroInvorinement.activeSelf)
        {
            m_HeroInvorinement.SetActive(false);
        }
        if (!m_ProfilCanvas.activeSelf)
        {
            m_ProfilCanvas.SetActive(true);
        }
    }

    public void ShowLoadingPanel()
    {
        if (!m_LoadingPanel.activeSelf)
        {
            m_LoadingPanel.SetActive(true);
        }
    }
    public void HideLoadingPanel()
    {
        if (m_LoadingPanel.activeSelf)
        {
            m_LoadingPanel.SetActive(false);
        }
    }
    public void UpdateReadyCountdownUI(int time_S)
    {
        string minutes = (time_S / 60).ToString("0");
        string seconds = (time_S % 60).ToString("00");
        m_ReadyCountDownText.text = $"{minutes}:{seconds}";
    }
    public void UpdateChooseHeroCountdownUI(int time_S)
    {
        string minutes = (time_S / 60).ToString("0");
        string seconds = (time_S % 60).ToString("00");
        m_ChooseHeroCountDownText.text = $"{minutes}:{seconds}";
    }
    private IEnumerator MatchMakingTimeCoroutine()
    {
        while (PlayerState.m_Instance.m_State == State.INLOBBY)
        {
            yield return new WaitForSeconds(1f);
            timeInMatchMaking++;
            UpdateLobbyTimeUI(timeInMatchMaking);
        }
        StopCoroutine(MatchMakingTimeCoroutine());
    }
    public void StartPingCouretine()
    {
        StartCoroutine(PingCoroutine());
    }
    private IEnumerator PingCoroutine()
    {
        while (PhotonNetwork.IsConnected)
        {
            MyPing = PhotonNetwork.GetPing();
            m_PingText.text = MyPing.ToString();
            yield return new WaitForSeconds(5f);
        }

    }
    // Game data
    public void GenerateAvatarsButtons()
    {
        Debug.Log("m_AvatarsDict " + GameDataManager.GDM.m_AvatarsDict.Values.Count);
        foreach (var avatar in GameDataManager.GDM.m_AvatarsDict.Values)
        {
            GameObject uiAvatarData = Instantiate(m_AvatarListingPrefab, m_AvatarLayoutGroup);
            AvatarUIHandler avatarUIHandler = uiAvatarData.GetComponent<AvatarUIHandler>();
            avatarUIHandler.Init(avatar);
        }
    }
    public void GenerateHerosButtons()
    {
        foreach (var hero in GameDataManager.GDM.m_HerosDict.Values)
        {
            GameObject uiHeroData = Instantiate(m_HeroListingPrefab, m_HeroLayoutGroup);
            HeroUIHandler heroUIHandler = uiHeroData.GetComponent<HeroUIHandler>();
            heroUIHandler.Init(hero);
        }
    }


    public void GeneratePlayerTeams(List<Player> players)
    {

        foreach (var player in players)
        {
            GameObject playerTeamUIObj = Instantiate(m_PlayersTeamPrefab, m_TeamLayoutGroup);
            
            PlayerTeamsHandler playerTeamUI = playerTeamUIObj.GetComponent<PlayerTeamsHandler>();
            playerTeamUI.Init(player);
        }
    }
    public void ResetHerosButtons()
    {
        foreach (var heroUI in GameDataManager.GDM.m_HeroHandlers.Values)
        {
            heroUI.m_HeroButton.interactable = true;
        }   
    }

    //lock button
    public void SendLockHero()
    {
        Hero _myHero = GameDataManager.GDM.m_SelectedHero;
        CurrentRoomManager.CRM.SendLockHero(_myHero);
        m_LockHeroButton.interactable = false;
    }
    public void PanelManaging(State state)
    {
        switch (state)
        {
            case State.DISCONECTED:

                break;
            case State.CONNECTED:

                break;
            case State.IN_MAIN_PANEL:
                ShowMainPanel();
                HideMatchMakingPanel();
                HideReadyPanel();
                HideHeroPanel();
                HideLoadingPanel();

                break;
            case State.INLOBBY:
                ShowMatchMakingPanel();
                HideMainPanel();
                HideReadyPanel();
                HideHeroPanel();
                HideLoadingPanel();

                break;
            case State.IN_READY_PANEL:
                ShowReadyPanel();
                HideMainPanel();
                HideMatchMakingPanel();
                HideHeroPanel();
                HideLoadingPanel();

                break;
            case State.IN_HERO_PANEL:
                ShowHeroPanel();
                HideMainPanel();
                HideMatchMakingPanel();
                HideReadyPanel();
                HideLoadingPanel();
                break;
            case State.IN_LOADING_PANEL:    
                ShowLoadingPanel();
                HideHeroPanel();
                HideMainPanel();
                HideMatchMakingPanel();
                HideReadyPanel();
                break;
        }
    }
}
