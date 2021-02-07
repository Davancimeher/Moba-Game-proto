using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAttack : MonoBehaviour
{
    public Animator Animator;
    public bool inAttack = false;

    public GameObject fx1;
    public GameObject fx3;
    public GameObject fx2;

    public Queue<AttackInput> attackInputs = new Queue<AttackInput>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (inAttack)
            {
                AttackInput attackInput = new AttackInput("Attack1");
                attackInputs.Enqueue(attackInput);
            }
            else
            {
                AttackInput attackInput = new AttackInput("Attack1");
                ExecuteAttaque(attackInput);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (inAttack)
            {
                AttackInput attackInput = new AttackInput("Attack2");
                attackInputs.Enqueue(attackInput);
            }
            else
            {
                AttackInput attackInput = new AttackInput("Attack2");
                ExecuteAttaque(attackInput);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (inAttack)
            {
                AttackInput attackInput = new AttackInput("Attack3");
                attackInputs.Enqueue(attackInput);
            }
            else
            {
                AttackInput attackInput = new AttackInput("Attack3");
                ExecuteAttaque(attackInput);
            }
        }
    }
    public void ShowFx(int id)
    {
        if (id == 1)
        {
            fx1.SetActive(true);
        }
        else if (id == 2)
        {
            fx2.SetActive(true);
        }
        else
        {
            fx3.SetActive(true);
        }
    }

    public void GetNextAttaque()
    {
        if (attackInputs.Count > 0)
            ExecuteAttaque(attackInputs.Dequeue());
    }

    public void ExecuteAttaque(AttackInput attackInput)
    {
        Animator.SetTrigger(attackInput.AnimatorIndex);
    }
}

internal class CustomTransform : MonoBehaviour
{
    public Transform m_Parent;
    public Vector3 m_LocalPosition;
    public Quaternion m_LocalRotation;
    public Vector3 m_LocalScale;

    public CustomTransform(Transform _transform)
    {
        m_Parent = _transform.parent;
        m_LocalPosition = _transform.localPosition;
        m_LocalRotation = _transform.localRotation;
        m_LocalScale = _transform.localScale;
    }
}
public class AttackInput
{
    public string AnimatorIndex;

    public AttackInput(string animatorIndex)
    {
        AnimatorIndex = animatorIndex;
    }
}
