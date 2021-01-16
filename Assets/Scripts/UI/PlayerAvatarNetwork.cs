using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAvatarNetwork : MonoBehaviour
{
    public TextMeshProUGUI m_PlayerNameText;
    public TextMeshProUGUI m_AvatarIdText;
    public Image m_AvatarImage;
    public Image m_HeroImage;
    public GameObject m_WaitingAnimation;
    public Image m_SceneLoadedPanel;
    public Player m_Player = null;
    public void Init(Player player)
    {
        if (m_Player == null) m_Player = player;

        m_PlayerNameText.text = player.NickName;
        var avatarId = (byte)player.CustomProperties[GlobalVariables.m_AvatarPlayerCostumes];

        if (GameDataManager.GDM.m_AvatarsDict.ContainsKey(avatarId))
        {
            m_AvatarImage.sprite = GameDataManager.GDM.m_AvatarsDict[avatarId].AvatarSprite;
            m_AvatarImage.color = Color.white;
        }
    }
    public void ResetReadyUIObject()
    {
        m_AvatarImage.color = Color.white;
    }
    public void SetPlayerReady()
    {
        m_AvatarImage.color = Color.green;
    }

    public void LoadSceneInit(Player player)
    {
        if (m_Player == null) m_Player = player;

        m_PlayerNameText.text = player.NickName;
        var avatarId = (byte)player.CustomProperties[GlobalVariables.m_AvatarPlayerCostumes];
        var heroId = (byte)player.CustomProperties[GlobalVariables.m_HeroPlayerCostumes];
        m_HeroImage.gameObject.SetActive(true);

        if (GameDataManager.GDM.m_AvatarsDict.ContainsKey(avatarId))
        {
            m_HeroImage.sprite = GameDataManager.GDM.m_AvatarsDict[avatarId].AvatarSprite;
            m_HeroImage.color = Color.white;

        }

        if (GameDataManager.GDM.m_HerosDict.ContainsKey(heroId))
        {
            m_AvatarImage.sprite = GameDataManager.GDM.m_HerosDict[heroId].HeroSprite;
            m_AvatarImage.color = Color.white;
        }
        m_WaitingAnimation.SetActive(true);
    }
    public void SetSceneLoaded()
    {
        m_WaitingAnimation.SetActive(false);
        m_SceneLoadedPanel.gameObject.SetActive(true);
        m_AvatarImage.color = Color.green;
    }
    public void ResetLoadUIObject()
    {
        m_WaitingAnimation.SetActive(true);
        m_SceneLoadedPanel.gameObject.SetActive(false);
        m_AvatarImage.color = Color.white;
    }
}
