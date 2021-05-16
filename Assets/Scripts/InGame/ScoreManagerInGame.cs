using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManagerInGame : MonoBehaviour
{
    public static ScoreManagerInGame SM;
    public TextMeshProUGUI m_Team1KillsText;
    public TextMeshProUGUI m_Team2KillsText;    

    public int Team1Kills;
    public int Team2Kills;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Team1Kills);
            stream.SendNext(Team2Kills);
        }
        else
        {
            Team1Kills = (int)stream.ReceiveNext();
            Team2Kills = (int)stream.ReceiveNext();
            UpdateUI();
        }
    }

    private void Awake()
    {
        if(SM != this)
        {
            SM = this;
        }
    }
    private void OnDestroy()
    {
        SM = null;
    }

    
    public void AddTeamKills(byte teamId)
    {
        if (teamId == 1) Team1Kills++;
        else Team2Kills++;

        UpdateUI();
    }
    private void UpdateUI()
    {
        m_Team1KillsText.text = Team1Kills.ToString();
        m_Team2KillsText.text = Team2Kills.ToString();
    }
}
