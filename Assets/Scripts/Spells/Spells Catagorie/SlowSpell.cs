using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowSpell", menuName = "GameData/Spells/SlowSpell", order = 1)]
public class SlowSpell : Spell
{
    public float AnimationSlowMultiplier;
    public float MouvementSpeedSlowMultiplier;
    private float MouvementRotationSpeedMultiplier = 0.7f;

    private float OldMouvementSpeed;
    private float OldRoatationSpeed;
    private float OldAnimationMultiplier;

    public override void SetSpell(SpellManager _spellManager)
    {
        OldAnimationMultiplier = 1f;
        OldMouvementSpeed = _spellManager.ChampionManager.m_PlayerMouvement.speed;
        OldRoatationSpeed = _spellManager.ChampionManager.m_PlayerMouvement.turnSmoothTime;

        
        _spellManager.ChampionManager.m_Animator.SetFloat("RunSpeed", 0.2f);
        _spellManager.ChampionManager.m_PlayerMouvement.speed = MouvementSpeedSlowMultiplier;
        _spellManager.ChampionManager.m_PlayerMouvement.turnSmoothTime = MouvementRotationSpeedMultiplier;

        if(!CanAttack)
        {
            _spellManager.ChampionManager.canAttack = CanAttack;
        }
        Debug.LogError("set SlowSpell");
    }

    public override void ResetSpell(SpellManager _spellManager)
    {
        _spellManager.ChampionManager.m_Animator.SetFloat("RunSpeed", OldAnimationMultiplier);
        _spellManager.ChampionManager.m_PlayerMouvement.speed = OldMouvementSpeed;
        _spellManager.ChampionManager.m_PlayerMouvement.turnSmoothTime = OldRoatationSpeed;

        if (!CanAttack)
        {
            _spellManager.ChampionManager.canAttack = true;
        }
    }
    public override void SetRpcVaribales(SpellManager _spellManager)
    {
        OldAnimationMultiplier = 1f;
        _spellManager.ChampionManager.m_Animator.SetFloat("RunSpeed", 0.2f);
    }
    public override void ResetRpcVaribales(SpellManager _spellManager)
    {
        _spellManager.ChampionManager.m_Animator.SetFloat("RunSpeed", OldAnimationMultiplier);
    }
}
