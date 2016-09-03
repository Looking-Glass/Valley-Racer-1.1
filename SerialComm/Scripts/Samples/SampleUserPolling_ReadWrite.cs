/**
 * Author: Daniel Wilches
 */

using UnityEngine;
using System.Collections;

/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class SampleUserPolling_ReadWrite : MonoBehaviour
{
	public SerialControllerKA _serialControllerKa;
	
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

		//---------------------------------------------------------------------
		// Receive data
		//---------------------------------------------------------------------

		string message = _serialControllerKa.ReadSerialMessage();

		if (message == null)
			return;

		// Check if the message is plain data or a connect/disconnect event.
		if (ReferenceEquals(message, SerialControllerKA.SERIAL_DEVICE_CONNECTED))
			Debug.Log("Connection established");
		else if (ReferenceEquals(message, SerialControllerKA.SERIAL_DEVICE_DISCONNECTED))
			Debug.Log("Connection attempt failed or disconnection detected");
		else
			Debug.Log("Message arrived: " + message);
	}
}
