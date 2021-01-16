using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public static RoomData RD;

    public Dictionary<int, Player> m_playersDictionary = new Dictionary<int, Player>();

    [HideInInspector]
    public List<int> m_PlayersReady = new List<int>();

    public Dictionary<int, byte> m_PlayersTeams = new Dictionary<int, byte>();

    public Dictionary<int, Player> m_MyTeamPlayers = new Dictionary<int, Player>();

    public Dictionary<int, byte> m_PlayersTeamHero = new Dictionary<int, byte>();

    public Dictionary<int, byte> m_PlayersHero = new Dictionary<int, byte>();

    public List<int> m_PlayersSceneReady = new List<int>();

    public Dictionary<int, GameObject> PlayersChampions = new Dictionary<int, GameObject>();

    public Hero MyHero;

    private void Awake()
    {
        #region singleton
        if (RD == null)
        {
            RD = this;
        }
        else
        {
            if (RD != this)
            {
                RD = this;
            }
        }
        #endregion  
    }

    public void ClearAllData()
    {
        ClearPlayerData();
        m_playersDictionary.Clear();
    }
    public void ClearPlayerData()
    {
        m_PlayersReady.Clear();
        m_PlayersTeams.Clear();
        m_MyTeamPlayers.Clear();
        m_PlayersTeamHero.Clear();
        m_PlayersHero.Clear();
        m_PlayersSceneReady.Clear();
    }
}   
