using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public PhotonView m_MasterPhotonView;
    public static InGameManager IGM;
    public GameObject MyChampion;
    public UltimateJoystick m_joystick;
    //public CameraController m_CameraController;
    public CinemachineFreeLook m_Camera;
    public GameObject m_ChampionCanvas;
    public ChampionManager m_MyChampionManager;

    public List<Transform> SpawnPointsTeam1 = new List<Transform>();
    public List<Transform> SpawnPointsTeam2 = new List<Transform>();

    [Header("Attacks UI")]

    public GameObject m_AutoAttackObject;
    public GameObject m_Attack1Object;
    public GameObject m_Attack2Object;
    public GameObject m_Attack3Object;

    public GameObject m_CancelButton;

    public GameObject BlockButtons;

    [Header("Recall UI")]

    public Button m_RecallSpellObject;
    public Spell m_RecallSpell;

    [Header("Spells UI")]
    public GameObject PassiveSpell1;

    [HideInInspector]
    public Vector3 mySpawnPoint;

    [Header("Respawn UI")]
    public TextMeshProUGUI respawnTimeUI;
    public GameObject respawnCanvas;
    public GameObject JoystickCanvas;
    public GameObject PlayerHUDCanvas;

    public List<Tower> m_Towers = new List<Tower>();

    private void Awake()
    {
        #region singleton
        if (IGM == null)
        {
            IGM = this;
        }
        else
        {
            if (IGM != this)
            {
                IGM = this;
            }
        }
        #endregion  

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        PhotonNetwork.AddCallbackTarget(this);

    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    private void SpawnChampion()
    {
        Hero myHero = RoomData.RD.MyHero;
        byte myTeamIndex = RoomData.RD.m_PlayersTeams[PhotonNetwork.LocalPlayer.ActorNumber];
        int myPlayerIndex = RoomData.RD.m_MyTeamPlayers.Keys.ToList().IndexOf(PhotonNetwork.LocalPlayer.ActorNumber);
        Vector3 myPlayerPosition;

        if (myTeamIndex == 1)
        {
            myPlayerPosition = SpawnPointsTeam1[myPlayerIndex].position;
        }
        else
        {
            myPlayerPosition = SpawnPointsTeam2[myPlayerIndex].position;
        }

        mySpawnPoint = myPlayerPosition;

        object[] myCustomInitData = new object[]
        {
            myHero.Health
        };
        MyChampion = PhotonNetwork.Instantiate(Path.Combine("Prefabs/InGameChampions", myHero.HeroName), myPlayerPosition, Quaternion.identity, 0, myCustomInitData);

        m_MyChampionManager = MyChampion.GetComponent<ChampionManager>();

        m_MyChampionManager.InitChampionManager(myHero, m_AutoAttackObject, m_Attack1Object, m_Attack2Object, m_Attack3Object, m_CancelButton, m_RecallSpellObject);

        m_MyChampionManager.m_LagPlayerSync.SpawnPosition = myPlayerPosition;

       //  m_CameraController.SetUpCamera(MyChampion.transform);
        m_Camera.Follow = MyChampion.transform;

    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            SpawnChampion();
        }
    }
    public void InitTowers()
    {
        foreach (var tower in m_Towers)
        {
            tower.InitTower();
        }
    }
    public void StopTowers()
    {
        foreach (var tower in m_Towers)
        {
            tower.InitTower();
        }
    }
}
