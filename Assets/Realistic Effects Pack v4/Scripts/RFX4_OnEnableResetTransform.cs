using UnityEngine;
using System.Collections;

public class RFX4_OnEnableResetTransform : MonoBehaviour {

    Transform t;
    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 startScale;
    bool isInitialized;

	void OnEnable () {
	    if(!isInitialized)
        {
            isInitialized = true;
            t = transform;
            startPosition = t.localPosition;
            startRotation = t.localRotation;
            startScale = t.localScale;
        }
        else
        {
            t.localPosition = startPosition;
            t.localRotation = startRotation;
            t.localScale = startScale;
        }
	}

    void OnDisable()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            t = transform;
            startPosition = t.localPosition;
            startRotation = t.localRotation;
            startScale = t.localScale;
        }
        else
        {
            t.localPosition = startPosition;
            t.localRotation = startRotation;
            t.localScale = startScale;
        }
    }
}
