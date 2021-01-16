using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static int m_MaxPlayersInRoom = 4;
    public static int m_PlayersInTeam = 2;
    public static int m_ReadyCountDown = 30;
    public static int m_ChooseHeroCountDown = 15;


    public const string m_AvatarsPath = "Avatars";
    public const string m_HerosPath = "Heros";
    public const string m_SpellsPath = "Spells";


    public const string m_AvatarPlayerCostumes = "Avatar";
    public const string m_HeroPlayerCostumes = "Hero";

    public static int m_GameSceneIndex = 1;
    public static List<byte> m_TeamIndex = new List<byte>
    {
        1,
        2
    };
    //Tags
    public static string m_EnemyTag="Enemy";
    public static string m_TeamateTag="Team";

    public static Dictionary<int, int> RespawnTimePerLevel = new Dictionary<int, int>()
    {
        { 1,6},
        { 2,8},
        { 3,10},
        { 4,12},
        { 5,14},
        { 6,16},
    };
}
