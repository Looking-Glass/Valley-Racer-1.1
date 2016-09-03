using System;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class serialManager : MonoBehaviour
{
    public string portName;
    public SerialPort port;
    public int baudRate = 115200;
    private string data = "";
    private EventManager manager;
    private ScoreKeeper scoreKeeper;
    private Boolean started = false;

    //game stuff
    public int credits = 0;
    public Sprite insertCoinSprite;
    public Sprite pressASprite;
    public bool debugInsertCoin;


    // Use this for initialization
    void Start()
    {
        if (!started)
        {
            port = new SerialPort(portName, baudRate);
            if (!port.IsOpen)
                port.Open();
            Debug.Log("port is open:  " + port);
            started = true;
        }
        try
        {
            scoreKeeper = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<ScoreKeeper>();
            EventManager.StartListening("PlayerDeath", OnPlayerDeath);
        }
        catch (Exception e)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

        //every frame, read through the serial buffer and parse the touchscreen events
        //why not do it as the data comes in?  because serial events aren't supported in mono!
        try
        {
            if (port.BytesToRead > 0)
            {
                data = port.ReadExisting();
                Debug.Log(data);
                if (SceneManager.GetActiveScene().name == "intro_scene")  //title screen
                    if (data.IndexOf("start") >= 0)
                    {
                        InsertCoin();
                    }
                //				String[] lines=data.Split('\n');
                //				debug
            }
        }
        catch (Exception e)
        {  //trouble reading, maybe the port is not open
            Debug.Log(e);
        }

        //debug mode stuff
        if (debugInsertCoin)
        {
            InsertCoin();
            debugInsertCoin = false;
        }

        //here's the game stuff
        //if we're on the title screen
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
    }

    void InsertCoin()
    {
        credits += 1;
    }

    void OnPlayerDeath()
    {
        int score = (int)(scoreKeeper.CurrentScore);
        Debug.Log("score:  " + score);
        port.Write(score.ToString() + "\n");

        //go into detached view for about 5 seconds (or 2 secs minimum until a button is pressed) then cycle back to main screen

    }

    void OnApplicationQuit()
    {
        port.Close();
    }

}
