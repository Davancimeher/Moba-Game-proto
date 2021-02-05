using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AvatarUIHandler : MonoBehaviour
{
    [HideInInspector] public Avatar m_Avatar;

    public TextMeshProUGUI m_AvatarIdText;
    public Button m_avatarButton;
    public Image m_AvatarImage;
    public Image m_AvatarImageSelected;

    public void Init(Avatar _avatar)
    {
        m_Avatar = _avatar;

        //m_AvatarIdText.text = m_Avatar.ID.ToString();
        m_AvatarImage.sprite = _avatar.AvatarSprite;
        m_avatarButton.onClick.AddListener(() => GameDataManager.GDM.OnClickAvatarButton(m_Avatar));
        if (!GameDataManager.GDM.m_AvatarsHandlers.ContainsKey(_avatar.ID))
            GameDataManager.GDM.m_AvatarsHandlers.Add(_avatar.ID, this);
    }

    public void SetSelected(bool value)
    {
        m_AvatarImageSelected.gameObject.SetActive(value);
    }
}
