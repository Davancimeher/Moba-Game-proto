using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Canvas m_Canvas;
    private void Start()
    {
        m_Canvas = GetComponent<Canvas>();

        m_Canvas.worldCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position+ Camera.main.transform.forward);
    }
}
