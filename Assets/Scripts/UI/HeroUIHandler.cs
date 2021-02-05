using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUIHandler : MonoBehaviour
{
    [HideInInspector] public Hero m_Hero;

    public Button m_HeroButton;
    public Image m_HeroImage;
    public Image m_HeroSelected;

    public void Init(Hero _hero)
    {
        m_Hero = _hero;

        m_HeroImage.sprite = m_Hero.HeroSprite;

        m_HeroButton.onClick.AddListener(() => GameDataManager.GDM.OnClickHeroButton(m_Hero));
        if (!GameDataManager.GDM.m_HeroHandlers.ContainsKey(m_Hero.ID))
            GameDataManager.GDM.m_HeroHandlers.Add(m_Hero.ID, this);
    }
    public void LockHero()
    {
        m_HeroButton.interactable = false;
    }
    public void UnlockHero()
    {
        m_HeroButton.interactable = true;
        Debug.Log("UnlockHero from : " + "HeroUIHandler");
    }
}
