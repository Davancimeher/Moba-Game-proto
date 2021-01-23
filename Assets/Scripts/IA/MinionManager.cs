using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MinionManager : MonoBehaviour
{
    public NavMeshAgent m_agent;
    public Transform m_Distination;
    public Animator m_Animator; 
    public void InitMinion(Transform _distination)
    {
        m_Distination = _distination;
        StartBehaviour();
    }
    public void StartBehaviour()
    {
        m_agent.SetDestination(m_Distination.position);
    }
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateValues();
        }
    }
    public void UpdateValues()
    {
        m_Animator.SetFloat("Distance", m_agent.remainingDistance);
        if (m_agent.remainingDistance < 2 && m_agent.hasPath && m_agent.isActiveAndEnabled)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
