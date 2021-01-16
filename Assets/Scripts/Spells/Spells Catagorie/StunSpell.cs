using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StunSpell", menuName = "GameData/Spells/StunSpell", order = 1)]
public class StunSpell : Spell
{
    private float OldMouvementSpeed;
    private float OldRoatationSpeed;

    public override void SetSpell(SpellManager _spellManager)
    {
        OldMouvementSpeed = _spellManager.ChampionManager.m_PlayerMouvement.speed;
        OldRoatationSpeed = _spellManager.ChampionManager.m_PlayerMouvement.turnSmoothTime;


        _spellManager.ChampionManager.m_PlayerMouvement.speed = 0f;
        _spellManager.ChampionManager.m_PlayerMouvement.turnSmoothTime = 0f;
        _spellManager.ChampionManager.canAttack = false;
        Debug.LogError("set StunSpell");

    }

    public override void ResetSpell(SpellManager _spellManager)
    {
        _spellManager.ChampionManager.m_PlayerMouvement.speed = OldMouvementSpeed;
        _spellManager.ChampionManager.m_PlayerMouvement.turnSmoothTime = OldRoatationSpeed;
        _spellManager.ChampionManager.canAttack = true;

    }

    public override void ResetRpcVaribales(SpellManager _spellManager)
    {
    }

    public override void SetRpcVaribales(SpellManager _spellManager)
    {
    }
}
