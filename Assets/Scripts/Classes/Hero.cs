using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "GameData/Hero", order = 1)]
public class Hero : ScriptableObject
{
    public byte ID = 0;
    public string HeroName;
    public Sprite HeroSprite;
    public GameObject HeroPrefab;

    [Header("Champion Stats")]
    public int Health;
    public float Speed;

    [Header("Champion Auto Attack")]
    public Attack AutoAttack;

    [Header("Champion Attack 1")]
    public Attack Attack1;
    [Header("Champion Attack 2")]
    public Attack Attack2;
    [Header("Champion Attack 3")]
    public Attack Attack3;

}
