using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SilentSpell", menuName = "GameData/Spells/SilentSpell", order = 1)]
public class SilentSpell : Spell
{
  
    public override void SetSpell(SpellManager _spellManager)
    {
        _spellManager.ChampionManager.canAttack = false;
        InGameManager.IGM.BlockButtons.SetActive(true);
    }
    public override void ResetSpell(SpellManager _spellManager)
    {
        _spellManager.ChampionManager.canAttack = true;
        InGameManager.IGM.BlockButtons.SetActive(false);
    }

    public override void SetRpcVaribales(SpellManager _spellManager)
    {
    }
    public override void ResetRpcVaribales(SpellManager _spellManager)
    {
    }
  
}
