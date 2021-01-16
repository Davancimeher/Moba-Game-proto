using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerTargetType
{
    CHAMPION,
    MINION
}
public class TowerTarget : MonoBehaviour
{
    public TowerTargetType TowerTargetType;
    public Transform m_TowerHitPoint;
    public HealthManager HealthManager;
    [HideInInspector]  public byte teamIndex;
   public int ViewID;
}
