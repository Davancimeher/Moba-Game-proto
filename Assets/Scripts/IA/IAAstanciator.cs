using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IAAstanciator : MonoBehaviour
{
    public List<GameObject> team1Minions;
    public List<GameObject> team2Minions;

    private void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Team1SpawnCoroutine());
            StartCoroutine(Team2SpawnCoroutine());
        }
    }
    public void InstanciateMinions(int teamIndex)
    {
        Transform distination;
        Transform spawnPoint;
        if (teamIndex == 1)
        {
            distination = InGameManager.IGM.Team2MinionsSpawnPoint;
            spawnPoint = InGameManager.IGM.Team1MinionsSpawnPoint;
        }
        else
        {
            distination = InGameManager.IGM.Team1MinionsSpawnPoint;
            spawnPoint = InGameManager.IGM.Team2MinionsSpawnPoint;
        }

        GameObject minion = PhotonNetwork.InstantiateRoomObject(Path.Combine("Prefabs/Minions", "Team" + teamIndex.ToString()), spawnPoint.position, Quaternion.identity, 0);
        MinionManager minionManager = minion.GetComponent<MinionManager>();
        minionManager.InitMinion(distination);
    }

    public IEnumerator Team1SpawnCoroutine()
    {
        while (PhotonNetwork.IsMasterClient)
        {
            InstanciateMinions(1);
            yield return new WaitForSeconds(10f);
        }
    }
    public IEnumerator Team2SpawnCoroutine()
    {
        while (PhotonNetwork.IsMasterClient)
        {
            InstanciateMinions(2);
            yield return new WaitForSeconds(10f);
        }
    }
}
