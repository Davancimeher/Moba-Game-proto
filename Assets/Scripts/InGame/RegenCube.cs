using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenCube : MonoBehaviour
{
    public byte TeamIndex;

    private void OnTriggerEnter(Collider other)
    {
        PhotonView pv = other.gameObject.GetComponent<PhotonView>();
        if (pv == null) return;

        if (pv.IsMine)
        {
            HealthManager healthManager = other.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                if(!healthManager.InRegen && healthManager.teamIndex == TeamIndex)
                     healthManager.StartRegen();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        PhotonView pv = other.gameObject.GetComponent<PhotonView>();
        if (pv == null) return;

        if (pv.IsMine)
        {
            HealthManager healthManager = other.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                if (!healthManager.InRegen && healthManager.teamIndex == TeamIndex)
                    healthManager.StartRegen();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PhotonView pv = other.gameObject.GetComponent<PhotonView>();
        if (pv == null) return;

        if (pv.IsMine)
        {
            HealthManager healthManager = other.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.InRegen = false;
            }
        }
    }
}
