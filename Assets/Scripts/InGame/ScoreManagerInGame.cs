using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerInGame : MonoBehaviour
{
    public int Team1Kills;
    public int Team2Kills;

    public void AddTeam1Kills()
    {
        Team1Kills++;
    }
    public void AddTeam2Kills()
    {
        Team2Kills++;
    }
}
