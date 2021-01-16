using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpellType
{
    PASSIVE,
    ATTACK
}
public abstract class Spell : ScriptableObject
{
    public SpellType SpellType;
    public bool HasRpcCalls;
    public bool CanAttack;
    public int SpellID;
    public  float SpellDuration;
    public Sprite SpellSprite;
    public GameObject SpellFX;

    public abstract void SetSpell(SpellManager _spellManager);
    public abstract void ResetSpell(SpellManager _spellManager);
    public abstract void ResetRpcVaribales(SpellManager _spellManager);
    public abstract void SetRpcVaribales(SpellManager _spellManager);
}
