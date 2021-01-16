using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugLoadScene : MonoBehaviour
{
    private bool inLoading = false;
    private bool PLayerReady = false;


    AsyncOperation asyncOperation = null ;
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!inLoading)
                StartCoroutine(load());
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (PLayerReady)
                asyncOperation.allowSceneActivation = true;
        }

    }

    private IEnumerator load()
    {
        yield return null;

        asyncOperation = SceneManager.LoadSceneAsync(1);

        asyncOperation.allowSceneActivation = false;

        inLoading = true;
        
        while (!asyncOperation.isDone)
        {

            if (asyncOperation.progress >= 0.9f)
            {
                PLayerReady = true;

                StopCoroutine(load());
            }
            yield return null;
        }
    }

}
