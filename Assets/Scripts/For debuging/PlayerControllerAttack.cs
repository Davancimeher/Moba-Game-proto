using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAttack : MonoBehaviour
{
    public Animator Animator;
    public bool inAttack = false;
    public GameObject fx;

    private Transform fxParent;
    private Vector3 fxPosition;
    private Quaternion fxRotation;
    private GameObject fxholder;
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
            {
                Animator.SetTrigger("Attack3");
                SaveTransform();
            }
        }
    }

    private void SaveTransform()
    {
   
        fxParent = fx.transform.parent;
        fxPosition = fx.transform.position;
        fxRotation = fx.transform.rotation;
    }
    public void ShowFx()
    {
        Debug.Log("fxPosition : " + fxPosition + ", fxRotation : " + fxRotation);
        fx.SetActive(true);
        fx.transform.parent = null;
    }
    public void ResetParent()
    {
        fx.transform.parent = fxParent;
        fx.transform.position = Vector3.zero;
    }
}
