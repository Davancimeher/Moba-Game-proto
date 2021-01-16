using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTeamsHandler : MonoBehaviour
{
    [HideInInspector] public Player m_Player;

    [HideInInspector] public Hero m_Hero;

    public Image m_HeroImage;
    public TextMeshProUGUI m_PlayerName;

    public void Init(Player _player)
    {
        m_Player = _player;
        m_PlayerName.text = _player.NickName;
    }
    public void SetHero(byte _heroID)
    {
        if (GameDataManager.GDM.m_HerosDict.ContainsKey(_heroID))
        {
            Hero hero = GameDataManager.GDM.m_HerosDict[_heroID];
            m_Hero = hero;
            m_HeroImage.sprite = m_Hero.HeroSprite;

            if (GameDataManager.GDM.m_HeroHandlers.ContainsKey(_heroID))
            {
                GameDataManager.GDM.m_HeroHandlers[hero.ID].LockHero();
            }
            if(GameDataManager.GDM.m_SelectedHero == hero)
            {
                UIManager.UIM.m_LockHeroButton.interactable = false;
            }
        }
       
    }
}
