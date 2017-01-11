using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class MetaSceneScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("XboxSelect") ||

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            Input.GetKeyDown("joystick button 6")
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            Input.GetKeyDown("joystick button 10")
#endif

            )
        {
            System.Diagnostics.Process.Start("MenuScene.app");
            Application.Quit();
        }
    }
}
