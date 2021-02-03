using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisactivateFXByTime : MonoBehaviour
{
    public float FXTime;
    private float time;
    private CustomTransform MyCustomTransform = null;

    private void OnEnable()
    {
        if (MyCustomTransform == null) MyCustomTransform = new CustomTransform(transform);

        transform.parent = null;
        time = FXTime;
        StartCoroutine(fxTime());
    }

    private IEnumerator fxTime()
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        ResetParent();
        yield return null;
    }

    private void ResetParent()
    {
        gameObject.SetActive(false);
        transform.SetParent(MyCustomTransform.m_Parent, false);
        transform.localPosition = MyCustomTransform.m_LocalPosition;
        transform.localRotation = MyCustomTransform.m_LocalRotation;
        transform.localScale = MyCustomTransform.m_LocalScale;
    }
}
