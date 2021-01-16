using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TowerProjectile : MonoBehaviour
{
    public int Damage;
    public TowerTarget m_ActualTowerTarget; 

    private float t;

    private void Update()
    {
        if(m_ActualTowerTarget.HealthManager.Health > 0)
        {
            t += Time.deltaTime * 0.08f;
            transform.position = Vector3.Lerp(transform.position, m_ActualTowerTarget.m_TowerHitPoint.transform.position, t);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 11) return;

        TowerTarget tt = other.gameObject.GetComponent<TowerTarget>();

        if (tt == null) return;

        if (tt.ViewID != m_ActualTowerTarget.ViewID) return;

        if (PhotonNetwork.IsMasterClient)
        {
            HealthManager hm = other.gameObject.GetComponent<HealthManager>();
            if (hm == null) return;

            PhotonView pv = other.gameObject.GetComponent<PhotonView>();

            if (pv == null) return;

            if (hm.m_ChampionManager.state != ActualState.DEAD && hm.m_ChampionManager.state != ActualState.REGEN_HEALTH)
            {
                hm.ExecuteDamageRPC(InGameManager.IGM.m_MasterPhotonView,pv.Owner, Damage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
