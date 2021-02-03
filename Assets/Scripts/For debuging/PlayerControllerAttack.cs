using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAttack : MonoBehaviour
{
    public Animator Animator;
    public bool inAttack = false;
    public GameObject fx3;
    public GameObject fx2;  

    private Transform fxParent;
    private Vector3 fxPosition;
    private Vector3 fxScale;
    private Quaternion fxRotation;
    private GameObject fxholder;

    private int lastAttack;

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
            {
                SaveTransform(fx2);
                Animator.SetTrigger("Attack2");

            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!inAttack)
            {
                SaveTransform(fx3);
                Animator.SetTrigger("Attack3");
            }
        }
    }

    private void SaveTransform(GameObject fx)
    {
        fxParent = fx.transform.parent;
        fxPosition = fx.transform.localPosition;
        fxRotation = fx.transform.localRotation;
        fxScale = fx.transform.localScale;
    }
   
    public void ShowFx(int id)
    {
        lastAttack = id;
        if (id == 2)
        {
            fx2.SetActive(true);
            fx2.transform.parent = null;
        }
        else
        {
            fx3.SetActive(true);
            fx3.transform.parent = null;
        }
    }

    public void ResetParent()
    {
        if(lastAttack == 2)
        {
            fx2.transform.SetParent(fxParent, false);

            fx2.transform.localPosition = fxPosition;
            fx2.transform.localRotation = fxRotation;
            fx2.transform.localScale = fxScale;
        }
        else
        {
            fx3.transform.SetParent(fxParent, false);

            fx3.transform.localPosition = fxPosition;
            fx3.transform.localRotation = fxRotation;
            fx3.transform.localScale = fxScale;
        }
    }
}
