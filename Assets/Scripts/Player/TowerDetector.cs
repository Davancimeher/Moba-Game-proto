using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDetector : MonoBehaviour
{
    public TowerTarget m_TowerTarget;
    public ChampionManager m_ChampionManager;   
    public PhotonView photonView;

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.layer == 14)
            {
                var tower = other.gameObject.GetComponent<Tower>();
                if (tower != null)
                {
                    if (m_TowerTarget.teamIndex != tower.m_TeamIndex)
                    {
                        m_ChampionManager.m_UnderTower = true;
                        m_ChampionManager.m_Tower = tower;
                        tower.EnableIndicator();
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.layer == 14)
            {
                var tower = other.gameObject.GetComponent<Tower>();
                if (tower != null)
                    if (m_TowerTarget.teamIndex != tower.m_TeamIndex)
                    {
                        m_ChampionManager.m_UnderTower = false;
                        m_ChampionManager.m_Tower = null;
                        tower.DisableIndicator();
                    }
            }
        }
    }
}
