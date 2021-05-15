using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
   public ChampionManager ChampionManager;

    private void OnTriggerEnter(Collider other)
    {

        if (ChampionManager.m_MyPhotonView.IsMine)
        {

            if (other.gameObject.CompareTag(GlobalVariables.m_EnemyTag))
            {
                //HIT a ENEMY
                HealthManager HealthManager = other.gameObject.GetComponent<HealthManager>();
                TowerHealthManager towerHealthManager = other.GetComponentInParent<TowerHealthManager>();


                if (HealthManager != null)
                {
                    if(HealthManager.m_ChampionManager.state != ActualState.DEAD && HealthManager.m_ChampionManager.state != ActualState.REGEN_HEALTH)
                    {
                        PhotonView photonView = other.gameObject.GetComponent<PhotonView>();

                        if(ChampionManager.m_ActualAttack != ChampionManager.m_AutoAttack)
                        {
                            ChampionManager.ActualFxHolder.FxCollision.enabled = false;
                        }
                        else
                        {
                            ChampionManager.m_HitObject.SetActive(false);
                        }


                        HealthManager.ExecuteDamageRPC(ChampionManager.m_MyPhotonView, photonView.Owner, ChampionManager.m_ActualAttack.Damage);

                        HealthManager.ShowDamage(ChampionManager.m_ActualAttack.Damage);

                        if (ChampionManager.m_UnderTower && !ChampionManager.m_InPriorityList)
                        {
                            Debug.LogError("Attack under tower");
                            //send RPC to Master to add this champion to proiority target
                            ChampionManager.m_Tower.ExecuteAddTargetToMaster(ChampionManager.m_MyPhotonView.ViewID);
                        }

                        if(ChampionManager.m_ActualAttack.AttackSpell != null)
                        {
                            SpellManager spellManager = other.gameObject.GetComponent<SpellManager>();

                            if(spellManager != null)
                            {
                                spellManager.ExecuteSpellRPC(ChampionManager.m_MyPhotonView, photonView.Owner, ChampionManager.m_ActualAttack.AttackSpell.SpellID);
                            }
                        }
                    }
                }
                else if(towerHealthManager != null)
                {
                    if (towerHealthManager.m_Health > 0)
                    {
                        if(ChampionManager.m_ActualAttack == ChampionManager.m_AutoAttack)
                        {
                            PhotonView TowerphotonView = other.gameObject.GetComponentInParent<PhotonView>();

                            ChampionManager.m_HitObject.SetActive(false);
                            towerHealthManager.ExecuteDamageRPC(TowerphotonView, ChampionManager.m_AutoAttack.Damage);

                            towerHealthManager.ShowDamage(ChampionManager.m_ActualAttack.Damage);
                        }
                    }
                }
                else
                {
                    return;
                }

            }
        }
    }
}
