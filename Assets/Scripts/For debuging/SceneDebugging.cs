using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDebugging : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject inv;
    public GameObject towers;

    public GameObject settingsPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showInv()
    {
        inv.SetActive(!inv.activeSelf);
    }

    public void showTower()
    {
        towers.SetActive(!towers.activeSelf);
    }
    public void showSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void SetLow()
    {
        QualitySettings.SetQualityLevel(0);
    }
    public void SetMeduim()
    {
        QualitySettings.SetQualityLevel(2);
    }
    public void SetHigh()
    {
        QualitySettings.SetQualityLevel(3);
    }
}
