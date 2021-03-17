using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviourTest : StateMachineBehaviour
{
    PlayerControllerAttack _PlayerControllerAttack;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _PlayerControllerAttack = animator.gameObject.GetComponent<PlayerControllerAttack>();
        _PlayerControllerAttack.inAttack = true;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _PlayerControllerAttack.inAttack = false;
        _PlayerControllerAttack.GetNextAttaque();
    }
}
