using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AutoAttack", menuName = "GameData/Attacks", order = 1)]
public class Attack : ScriptableObject
{
    public int Damage;
    public int AttackRate;
    public Sprite AttackSprite;
    public GameObject Indicator;
    public Spell AttackSpell;
    public GameObject AttackFX;
}
