/**
 * Author: Daniel Wilches
 */

using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

/**
 * This class allows a Unity program to continually check for messages from a
 * serial device.
 * 
 * It creates a Thread that communicates with the serial port and continually
 * polls the messages on the wire.
 * That Thread puts all the messages inside a Queue, and this SerialControllerKA
 * class polls that queue by menas of invoking SerialThreadKA.GetSerialMessage().
 * 
 * The serial device must send its messages separated by a newline character.
 * Neither the SerialControllerKA nor the SerialThreadKA perform any validation
 * on the integrity of the message. It's up to the one that makes sense of the
 * data.
 */
public class SerialControllerKA : MonoBehaviour
{
    [Tooltip("Port name with which the SerialPort object will be created.")]
    public string portName = "COM7";

    [Tooltip("Baud rate that the serial device is using to transmit data.")]
    public int baudRate = 9600;

    [Tooltip("Reference to an scene object that will receive the events of connection, " +
             "disconnection and the messages from the serial device.")]
    public GameObject messageListener;

    [Tooltip("After an error in the serial communication, or an unsuccessful " +
             "connect, how many milliseconds we should wait.")]
    public int reconnectionDelay = 1000;

    [Tooltip("Maximum number of unread data messages in the queue. " +
             "New messages will be discarded.")]
    public int maxUnreadMessages = 1;

    // Constants used to mark the start and end of a connection. There is no
    // way you can generate clashing messages from your serial device, as I
    // compare the references of these strings, no their contents. So if you
    // send these same strings from the serial device, upon reconstruction they
    // will have different reference ids.
    public const string SERIAL_DEVICE_CONNECTED = "__Connected__";
    public const string SERIAL_DEVICE_DISCONNECTED = "__Disconnected__";

    // Internal reference to the Thread and the object that runs in it.
    private Thread thread;
    private SerialThreadKA _serialThreadKa;

    SerialPort serialPort;
    
    //kyles mod
    string portTextFilename = "port.txt";
    
    // ------------------------------------------------------------------------
    // Invoked whenever the SerialControllerKA gameobject is activated.
    // It creates a new thread that tries to connect to the serial device
    // and start reading from it.
    // ------------------------------------------------------------------------
    void OnEnable()
    {
        //kyle bs
        if (System.IO.File.Exists(Application.dataPath + "/" + portTextFilename))
        {
            string portImportName = System.IO.File.ReadAllText(Application.dataPath + "/" + portTextFilename);
            portName = portImportName;
        }
        else
        {
            System.IO.File.WriteAllText(Application.dataPath + "/" + portTextFilename, portName);
        }
        _serialThreadKa = new SerialThreadKA(portName, baudRate, reconnectionDelay,
                                        maxUnreadMessages);
        thread = new Thread(new ThreadStart(_serialThreadKa.RunForever));
        thread.Start();
}

    // ------------------------------------------------------------------------
    // Invoked whenever the SerialControllerKA gameobject is deactivated.
    // It stops and destroys the thread that was reading from the serial device.
    // ------------------------------------------------------------------------
    void OnDisable()
    {
        // The SerialThreadKA reference should never be null at this point,
        // unless an Exception happened in the OnEnable(), in which case I've
        // no idea what face Unity will make.
        if (_serialThreadKa != null)
        {
            _serialThreadKa.RequestStop();
            _serialThreadKa = null;
        }

        // This reference shouldn't be null at this point anyway.
        if (thread != null)
        {
            thread.Join();
            thread = null;
        }

        if (serialPort != null)
        {
            serialPort.Close();
        }
    }

    // ------------------------------------------------------------------------
    // Polls messages from the queue that the SerialThreadKA object keeps. Once a
    // message has been polled it is removed from the queue. There are some
    // special messages that mark the start/end of the communication with the
    // device.
    // ------------------------------------------------------------------------
    void Update()
    {
        // If the user prefers to poll the messages instead of receiving them
        // via SendMessage, then the message listener should be null.
        if (messageListener == null)
            return;

        // Read the next message from the queue
        string message = _serialThreadKa.ReadSerialMessage();
        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SERIAL_DEVICE_CONNECTED))
            messageListener.SendMessage("OnConnectionEvent", true);
        else if (ReferenceEquals(message, SERIAL_DEVICE_DISCONNECTED))
            messageListener.SendMessage("OnConnectionEvent", false);
        else
            messageListener.SendMessage("OnMessageArrived", message);
    }

    // ------------------------------------------------------------------------
    // Returns a new unread message from the serial device. You only need to
    // call this if you preferrred to not provide a message listener.
    // ------------------------------------------------------------------------
    public string ReadSerialMessage()
    {
        // Read the next message from the queue
        return _serialThreadKa.ReadSerialMessage();
    }

    // ------------------------------------------------------------------------
    // Puts a message in the outgoing queue. The thread object will send the
    // message to the serial device when it considers it appropriate.
    // ------------------------------------------------------------------------
    public void SendSerialMessage(string message)
    {
        _serialThreadKa.SendSerialMessage(message);
    }

}
