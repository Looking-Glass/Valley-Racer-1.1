﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hypercube;
using UnityEngine.SceneManagement;

public class TouchscreenManager : MonoBehaviour
{
    int debugCounter = 0;
    float buffer;
    float bufferThreshold = 2f;

    void Update()
    {
        buffer += Time.deltaTime;
        //don't let buffer advance if it's scene 2 and biker is still alive
        if (SceneManager.GetActiveScene().buildIndex == 2 && FindObjectOfType<BikeController>() != null)
        {
            buffer = 0;
        }

        if (input.touchPanel == null)
        {
            if (debugCounter == 100)
            {
                debugCounter = 0;
                print("touchscreen null");
            }
            debugCounter++;
            return;

        }
        var touches = input.touchPanel.touches;
        foreach (touch touch in touches)
        {
            //hypercube exit checker update
            if (touch.posX < 0.1f && touch.posY > 0.9f)
            {
                continue;
            }

            //scene 1 (intro screen)
            if (buffer > bufferThreshold && SceneManager.GetActiveScene().buildIndex == 1)
            {
                FindObjectOfType<SerialPressEnter>().StartGame();
                buffer = 0;
            }

            //scene 2 (biker scene)
            if (buffer > bufferThreshold && SceneManager.GetActiveScene().buildIndex == 2)
            {
                //this is handled in BikerInput.cs
                if (FindObjectOfType<BikeController>() == null) //if the player is dead. i know this is lazy
                {
                    FindObjectOfType<SceneToggle>().ToggleScene();
                    buffer = 0;
                }
            }

            //scene 3 (hi score screen)
            if (buffer > bufferThreshold && SceneManager.GetActiveScene().buildIndex == 3)
            {
                FindObjectOfType<SceneToggle>().ToggleScene();
                buffer = 0;
            }
        }
    }
}
