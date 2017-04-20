using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HypercubeExitChecker : MonoBehaviour
{


    public string launcherPath = "C:\\VolumeLauncher\\VolumeLauncher.exe";
    public float timer;

    public float beginOverlayTime = 1f;
    public float cornerMargin = .1f;
    MeshRenderer hypercubeMesh;
    public Texture[] exitTextPngs;

    public bool runningCoroutine = false;

    void Start()
    {
        string dataPath;
#if UNITY_EDITOR


#endif

#if UNITY_STANDALONE_OSX
        dataPath = Application.dataPath;
        if (dataPath.LastIndexOf("VolumeLauncher")>=0f)
        launcherPath = dataPath.Substring(0, dataPath.LastIndexOf("VolumeLauncher")) + "VolumeLauncher/VolumeLauncher.app";
#endif

#if UNITY_STANDALONE_WIN
        dataPath = Application.dataPath;
        if (dataPath.LastIndexOf("VolumeLauncher")>=0f)
        launcherPath = dataPath.Substring(0, dataPath.LastIndexOf("VolumeLauncher")) +"\\VolumeLauncher\\VolumeLauncher.exe";
#endif

        hypercubeMesh = GameObject.Find("HypercubeCastMesh").transform.GetChild(0).GetComponent<MeshRenderer>();
        Material[] mats = hypercubeMesh.materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (hypercube.input.touchPanel == null) //Volume not connected via USB
            return;

        uint touches = hypercube.input.touchPanel.touchCount;
        if (touches >= 1)
        {
            bool touchThisFrame = false;

            for (int i = 0; i < touches; i++)
            {
                if (hypercube.input.touchPanel.touches[0].posX < cornerMargin && hypercube.input.touchPanel.touches[0].posY > 1f - cornerMargin)
                    touchThisFrame = true;
            }

            CornerHoldCheck(touchThisFrame);

        }
        else
        {
            CornerHoldCheck(false);
        }
    }

    void CornerHoldCheck(bool touch)
    {
        if (touch && !runningCoroutine)
        {
            timer += Time.deltaTime;
        }
        else if (!touch && runningCoroutine)
        {
            StopCoroutine("FadeInExitText");
            FindObjectOfType<hypercube.castMesh>().updateMesh();

            runningCoroutine = false;
            timer = 0f;
        }

        if (timer > beginOverlayTime && !runningCoroutine)
            StartCoroutine("FadeInExitText");
    }

    IEnumerator FadeInExitText()
    {
        runningCoroutine = true;
        for (int i = 0; i < exitTextPngs.Length; i++)
        {
            Material[] mats = hypercubeMesh.materials;
            mats[0].SetTexture("_MainTex", exitTextPngs[i]);
            mats[1].SetTexture("_MainTex", exitTextPngs[i]);
        
            yield return new WaitForSecondsRealtime(1f);
        }

        //System.Diagnostics.Process.Start(launcherPath);

        Application.Quit();
    }
}