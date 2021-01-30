using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAttack : MonoBehaviour
{
    public Animator Animator;
    bool inAttack = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(!inAttack)
            Animator.SetTrigger("Attack1");
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!inAttack)
                Animator.SetTrigger("Attack2");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!inAttack)
                Animator.SetTrigger("Attack3");
        }
    }
}
