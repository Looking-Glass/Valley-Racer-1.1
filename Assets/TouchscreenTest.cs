using System.Collections;
using System.Collections.Generic;
using hypercube;
using UnityEngine;

public class TouchscreenTest : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {

            if (input.touchPanel.touches.Length > 0)
            {
                print(input.touchPanel.touches[0].getLocalPos());
            }
            else
            {
                print("no touches");
            }
        }
    }
}