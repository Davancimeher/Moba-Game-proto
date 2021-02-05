using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

    public GameObject m_SettingsPopup;
    public Button SettingsBtn;
    public Button Resume;
    public Button Disconnect;
    void Start()
    {
        SettingsBtn.onClick.AddListener(() => OpenSettings(true));
        Resume.onClick.AddListener(() => OpenSettings(false));
        Disconnect.onClick.AddListener(() => OpenSettings(false));
    }

    private void OpenSettings(bool action)
    {
        m_SettingsPopup.SetActive(action);
    }
}
