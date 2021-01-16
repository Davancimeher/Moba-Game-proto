using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpellsCategory
{
    NONE,
    STUN,
    SLOW,
    SILENT,
}
public class SpellsTesting : MonoBehaviour
{
    public float spellTime;
    public Transform fearTarget;

    public SpellsCategory actualSpell;
    public bool inSpeel = false;
    public Animator Animator;
    private float oldspeed;
    private float oldRotatespeed;   
    public PlayerMouvementTest playerMouvement;
    public void SetSpell()
    {
        switch (actualSpell)
        {
            case SpellsCategory.STUN:
                oldspeed = playerMouvement.speed;
                playerMouvement.speed = 0f;
                oldRotatespeed = playerMouvement.turnSmoothTime;
                playerMouvement.turnSmoothTime =0f;
                Debug.Log("startSTUN");
                inSpeel = true;
                break;
            case SpellsCategory.SLOW:

                Animator.SetFloat("RunSpeed", 0.2f);
                oldspeed = playerMouvement.speed;
                oldRotatespeed = playerMouvement.turnSmoothTime;
                playerMouvement.speed = 5f;
                playerMouvement.turnSmoothTime = 0.7f;
                Debug.Log("startSlow");
                inSpeel = true;
                break;
            case SpellsCategory.SILENT:

                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!inSpeel)
            {
                SetSpell();
            }
        }
        updateValue();
    }
    public void updateValue()
    {
        if (inSpeel)
        {
            spellTime -= Time.deltaTime;

            if (spellTime <= 0)
            {
                CancelSpell();
                inSpeel = false;
                spellTime = 5f;
            }
        }
    }
    public void CancelSpell()
    {
        Animator.SetFloat("RunSpeed", 1f);
        playerMouvement.speed = oldspeed;
        playerMouvement.turnSmoothTime = oldRotatespeed;
    }
    public void FearAction()
    {
        if(actualSpell == SpellsCategory.SILENT)
        transform.Translate(-Vector3.forward * Time.deltaTime, fearTarget);
    }
}
