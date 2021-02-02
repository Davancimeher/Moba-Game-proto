using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisactivateFXByTime : MonoBehaviour
{
    public float FXTime;
    private float time;
    public Transform Parent;
    public PlayerControllerAttack test;

    void OnEnable()
    {
        time = FXTime;
        StartCoroutine(fxTime());
    }
    IEnumerator fxTime()
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        this.Parent.gameObject.SetActive(false);
        test.ResetParent();
        yield return null;
    }
}
