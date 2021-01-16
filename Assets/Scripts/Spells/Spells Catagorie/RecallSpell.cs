using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecallSpell", menuName = "GameData/Spells/Passive/RecallSpell", order = 1)]

public class RecallSpell : Spell
{
    public override void SetSpell(SpellManager _spellManager)
    {
       
       
    }
    public override void ResetSpell(SpellManager _spellManager)
    {
      

    }
    public override void SetRpcVaribales(SpellManager _spellManager)
    {
        _spellManager.ChampionManager.ExecuteSetRecall();

    }
    public override void ResetRpcVaribales(SpellManager _spellManager)
    {
        _spellManager.ChampionManager.ExecuteEndRecall();
    }
}
