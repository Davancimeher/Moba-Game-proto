using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    private ChampionManager championManager = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (championManager == null)
            championManager = animator.gameObject.GetComponent<ChampionManager>();

        championManager.EnableHitObject();
    }
}
