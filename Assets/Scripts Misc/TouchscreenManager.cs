using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hypercube;
using UnityEngine.SceneManagement;

public class TouchscreenManager : MonoBehaviour
{
    hypercubeCamera h;

    void Start()
    {
        h = FindObjectOfType<hypercubeCamera>();
    }

    void Update()
    {
        if (input.touchPanel == null)
            return;
        var touches = input.touchPanel.touches;
        foreach (touch touch in touches)
        {
            //scene 1 (intro screen)
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                FindObjectOfType<SerialPressEnter>().StartGame();
            }

            //scene 2 (biker scene)
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                //this is handled in BikerInput.cs
                if (FindObjectOfType<BikeController>() == null) //if the player is dead. i know this is lazy
                {
                    FindObjectOfType<SceneToggle>().ToggleScene();
                }
            }

            //scene 3 (hi score screen)
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                FindObjectOfType<SceneToggle>().ToggleScene();
            }
        }
    }
}
