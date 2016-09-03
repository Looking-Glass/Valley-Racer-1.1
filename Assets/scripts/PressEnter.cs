﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PressEnter : MonoBehaviour
{

    public float timeBuffer = 2f;
    float timer;
    
	// Update is called once per frame
	void Update ()
	{
	    timer += Time.deltaTime;
	    if (timer > timeBuffer)
	    {
	        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter) ||
	            Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button16))
	        {
	            var serialManager = GameObject.FindWithTag("SerialManager");
	            if (serialManager != null)
	            {
	                if (serialManager.GetComponent<ValleySerialPolling>().credits > 0)
	                {
	                    serialManager.GetComponent<ValleySerialPolling>().credits -= 1;
	                    var sm = GameObject.FindWithTag("SerialManager");
                        var ac = sm.GetComponent<ValleySerialPolling>().sounds[Random.Range(1, 3)];
                        var asource = sm.GetComponent<AudioSource>();
	                    asource.clip = ac;
                        asource.Play();
                        SceneManager.LoadScene(1);
	                }
	            }
	            else
	            {
	                SceneManager.LoadScene(1);
                }
            }
	    }
	}
}