/**
 * Author: Daniel Wilches
 */

using UnityEngine;
using System.Collections;

/**
 * Sample for reading using polling by yourself. In case you are fond of that.
 */
public class SampleUserPolling_JustRead : MonoBehaviour
{
    public SerialControllerKA _serialControllerKa;

    // Initialization
    void Start()
    {
        _serialControllerKa = GameObject.Find("SerialControllerKA").GetComponent<SerialControllerKA>();
	}

    // Executed each frame
    void Update()
    {
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
