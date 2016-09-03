﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ValleySerialPolling : MonoBehaviour
{
	public SerialControllerKA _serialControllerKa;
	public int credits;
	public Sprite pressASprite;
	public Sprite insertCoinSprite;
	public int scoreTier;
	public int ticketInterval = 30;
	public int ticketEvery = 30; //this keeps going up

	public AudioClip[] sounds;

	
	// Executed each frame
	void Update()
	{
		//---------------------------------------------------------------------
		// Send data
		//---------------------------------------------------------------------

		// If you press one of these keys send it to the serial device. A
		// sample serial device that accepts this input is given in the README.
		/*
		if (Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log("Sending A");
			SerialControllerKA.SendSerialMessage("A");
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			Debug.Log("Sending Z");
			SerialControllerKA.SendSerialMessage("Z");
		}
		*/

		//hacky reset on ticket interval
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			ticketEvery = ticketInterval;
		}

		//during game loop
		if (SceneManager.GetActiveScene().buildIndex == 1)
		{
			var scoreKeeper = GameObject.FindWithTag("ScoreKeeper").GetComponent<ScoreKeeper>();
			if (scoreKeeper != null)
			{
				//how many tickets do we print?
				scoreTier =
					Mathf.FloorToInt(scoreKeeper.CurrentScore /
									 scoreKeeper.gameObject.GetComponent<SettingChanges>().settingInterval + 1);

				//don't get too crazy man
				if (scoreTier > 10)
					scoreTier = 10;

				//now print them every ticketEvery
				if (scoreKeeper.CurrentScore > ticketEvery)
				{
					_serialControllerKa.SendSerialMessage(scoreTier.ToString());
					print("printed " + scoreTier + " ticket(s)");
					ticketEvery += ticketInterval;
				}
			}
		}

		//---------------------------------------------------------------------
		// Receive data
		//---------------------------------------------------------------------

		string message = _serialControllerKa.ReadSerialMessage();
		if (message != null)
			message = message.Trim();

		if (message == "start!")
		{
			AddCredit();
		}

		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			var blinkyText = GameObject.FindWithTag("BlinkyText");
			if (credits > 0)
			{
				blinkyText.GetComponent<SpriteRenderer>().sprite = pressASprite;
			}
			else
			{
				blinkyText.GetComponent<SpriteRenderer>().sprite = insertCoinSprite;
			}

			GameObject.Find("Credits Text").GetComponent<TextMesh>().text = "CREDITS: " + credits;
		}

		// Check if the message is plain data or a connect/disconnect event.
		if (ReferenceEquals(message, SerialControllerKA.SERIAL_DEVICE_CONNECTED))
			Debug.Log("Connection established");
		else if (ReferenceEquals(message, SerialControllerKA.SERIAL_DEVICE_DISCONNECTED))
			Debug.Log("Connection attempt failed or disconnection detected");

		//cheats
		if (Input.GetKeyDown(KeyCode.F10))
		{
			AddCredit();
		}
	}

	public void AddCredit()
	{
		credits += 1;
		var audioSource = GetComponent<AudioSource>();
		audioSource.clip = sounds[0];
		audioSource.Play();
	}


}
