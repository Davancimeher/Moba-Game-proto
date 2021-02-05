using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager GDM;

    public Dictionary<byte, Avatar> m_AvatarsDict = new Dictionary<byte, Avatar>();

    public Dictionary<byte, Hero> m_HerosDict = new Dictionary<byte, Hero>();

    public Dictionary<byte, AvatarUIHandler> m_AvatarsHandlers = new Dictionary<byte, AvatarUIHandler>();

    public Dictionary<byte, HeroUIHandler> m_HeroHandlers = new Dictionary<byte, HeroUIHandler>();

    public GameObject MyHeroParent;

    public List<GameObject> TeamHerosParents;

    public Dictionary<int, GameObject> TeamGOHeros = new Dictionary<int, GameObject>();

    private ExitGames.Client.Photon.Hashtable m_PlayerCostumes = new ExitGames.Client.Photon.Hashtable();

    public Hero m_SelectedHero = null;

    private byte AvatarId;
    private AvatarUIHandler SelectedAvatar;
    private HeroUIHandler SelectedHero;
    private GameObject selectedHeroGO;

    private void Awake()
    {
        #region singleton
        if (GDM == null)
        {
            GDM = this;
        }
        else
        {
            if (GDM != this)
            {
                GDM = this;
            }
        }
        #endregion  
    }
    private void Start()
    {
        LoadData();
        LoadDataPrefs();
    }

    private void LoadData()
    {
        var avatarDict = new List<Avatar>(Resources.LoadAll<Avatar>(GlobalVariables.m_AvatarsPath));
        foreach (var avatar in avatarDict)
        {
            if (!m_AvatarsDict.ContainsKey(avatar.ID))
            {
                m_AvatarsDict.Add(avatar.ID, avatar);
            }
        }
        var HerosList = new List<Hero>(Resources.LoadAll<Hero>(GlobalVariables.m_HerosPath));

        foreach (var hero in HerosList)
        {
            if (!m_HerosDict.ContainsKey(hero.ID))
            {
                m_HerosDict.Add(hero.ID, hero);
            }
        }

        UIManager.UIM.GenerateAvatarsButtons();
        UIManager.UIM.GenerateHerosButtons();
        //
    }

    public void OnClickAvatarButton(Avatar _avatar)
    {
        SetAvatar(_avatar);
    }
    public void OnClickHeroButton(Hero _Hero)
    {
        SetHero(_Hero);
    }

    private void SetAvatar(Avatar _avatar)
    {
        m_PlayerCostumes.Clear();
        m_PlayerCostumes.Add(GlobalVariables.m_AvatarPlayerCostumes, _avatar.ID);
        PhotonNetwork.LocalPlayer.SetCustomProperties(m_PlayerCostumes);
        Debug.Log("SetAvatar " + PhotonNetwork.LocalPlayer.CustomProperties[GlobalVariables.m_AvatarPlayerCostumes]);
        //save in player Prefs
        AvatarId = _avatar.ID;
        SaveDataPrefs();
        SetSelectedAvatar(m_AvatarsHandlers[AvatarId]);
        UIManager.UIM.m_AvatarProfil.sprite = m_AvatarsDict[AvatarId].AvatarSprite;
    }
    private void SetHero(Hero _hero)
    {
        SetSelectedHero(_hero);
    }
    private void SaveDataPrefs()
    {
        PlayerPrefs.SetInt(GlobalVariables.m_AvatarPlayerCostumes, AvatarId);
    }
    private void LoadDataPrefs()
    {
        if (!PlayerPrefs.HasKey(GlobalVariables.m_AvatarPlayerCostumes))
        {
            AvatarId = m_AvatarsDict.Keys.First();
            SetAvatar(m_AvatarsDict[AvatarId]);
            SetSelectedAvatar(m_AvatarsHandlers[AvatarId]);
        }
        else
        {
            AvatarId = (byte)PlayerPrefs.GetInt(GlobalVariables.m_AvatarPlayerCostumes);
            SetAvatar(m_AvatarsDict[AvatarId]);
            SetSelectedAvatar(m_AvatarsHandlers[AvatarId]);
            UIManager.UIM.m_AvatarProfil.sprite = m_AvatarsDict[AvatarId].AvatarSprite;
        }
        //
    }
    private void SetSelectedAvatar(AvatarUIHandler _avatarUI)
    {
        if (SelectedAvatar != null)
            m_AvatarsHandlers[SelectedAvatar.m_Avatar.ID].SetSelected(false);

        if (m_AvatarsHandlers.ContainsKey(_avatarUI.m_Avatar.ID))
        {
            SelectedAvatar = m_AvatarsHandlers[_avatarUI.m_Avatar.ID];
            SelectedAvatar.SetSelected(true);
        }
    }

    private void SetSelectedHero(Hero Hero)
    {
        if (SelectedHero != null)
        {
            if (Hero == SelectedHero.m_Hero) return;
            m_HeroHandlers[SelectedHero.m_Hero.ID].m_HeroSelected.gameObject.SetActive(false);
            Destroy(selectedHeroGO);
        }

        if (m_HeroHandlers.ContainsKey(Hero.ID))
        {
            SelectedHero = m_HeroHandlers[Hero.ID];
            SelectedHero.m_HeroSelected.gameObject.SetActive(true);
            selectedHeroGO = Instantiate(Hero.HeroPrefab, MyHeroParent.transform);
        }

        if (SelectedHero != Hero)
        {
            m_SelectedHero = Hero;

            UIManager.UIM.m_LockHeroButton.interactable = true;
        }
    }

    public void SetSelectedHero(byte heroId)
    {
        if (m_HerosDict.ContainsKey(heroId))
        {
            Hero hero = m_HerosDict[heroId];
            SetSelectedHero(hero);
        }

    }
    public void SetHeroId(byte heroId)
    {
        if (m_HerosDict.ContainsKey(heroId))
        {
            m_PlayerCostumes.Clear();
            m_PlayerCostumes.Add(GlobalVariables.m_HeroPlayerCostumes, heroId);
            PhotonNetwork.LocalPlayer.SetCustomProperties(m_PlayerCostumes);
            RoomData.RD.MyHero = m_HerosDict[heroId];
        }
    }

    public void SetTeamPlayersName()
    {
        ResetDictionarys();
        InitTeamTextUI();

        MyHeroParent.GetComponentInChildren<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;

        foreach (var player in RoomData.RD.m_MyTeamPlayers.Values)
        {
            if(player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                TeamGOHeros.Add(player.ActorNumber, null);
                Debug.Log("Team : " + player.NickName);
            }
        }
        List<int> playersTeamActors = TeamGOHeros.Keys.ToList();

        for (int i = 0; i < playersTeamActors.Count; i++)
        {
            string NickName = RoomData.RD.m_playersDictionary[playersTeamActors[i]].NickName;
            TeamHerosParents[i].GetComponentInChildren<TextMeshProUGUI>().text = NickName;
            TeamHerosParents[i].SetActive(true);
        }
    }
    public void SetTeamPlayerHero(int playerActor,byte heroid)
    {
        if (TeamGOHeros[playerActor] != null)
        {
            Destroy(TeamGOHeros[playerActor]);
            TeamGOHeros[playerActor] = null;
        }
       var index= TeamGOHeros.Keys.ToList().IndexOf(playerActor);

        var selectedHeroGO = Instantiate(m_HerosDict[heroid].HeroPrefab, TeamHerosParents[index].transform);
        TeamGOHeros[playerActor] = selectedHeroGO;
    }

    public void ResetDictionarys()
    {
        foreach (var go in TeamGOHeros.Values)
        {
            Destroy(go);
        }
        TeamGOHeros.Clear();
    }
    public void InitTeamTextUI()
    {
        foreach (var item in TeamHerosParents)
        {
            if (item.activeSelf)
            {
                item.GetComponentInChildren<TextMeshProUGUI>().text = "";
                item.SetActive(false);
            }
        }
    }
}
