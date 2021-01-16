using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This script calculate the current fps and show it to a text ui.
/// </summary>
public class UiDisplayFps : MonoBehaviour
{
    public float updateInterval = 2F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    public TextMeshProUGUI guiText;

    void Start()
    {
        guiText.material.color = Color.white;
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;

            var fpss =(int) Mathf.Round(fps);

            guiText.text = fpss + " FPS";

            if (fps < 30)
                guiText.color = Color.yellow;
            else
                if (fps < 10)
                guiText.color = Color.red;
            else
                guiText.color = Color.green;
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}