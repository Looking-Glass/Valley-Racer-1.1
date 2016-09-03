using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour
{

    public float CurrentScore;
    public float bestDistance;

    public Text scoreText;

    public bool inMiles;
    public bool EnterName;
    public Text InitialsText;
    public GameObject NewHighScore;
    int rankToReplace;

    public void PlaceScore(float newScore)
    {
        rankToReplace = 0;
        for (var i = 10; i >= 1; i--)
        {
            string rankNumber = i.ToString();
            string rankKey = "HS" + rankNumber + "_score";
            float scoreToBeat = PlayerPrefs.GetFloat(rankKey, 0);
            if (newScore >= scoreToBeat)
            {
                rankToReplace = i;
            }
        }

        if (rankToReplace > 0)
        {
            NewHighScore.SetActive(true);
        }
        else
        {
            //TODO: this is where the timer goes which will count down until you can restart by hitting enter.
            GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<BonusSceneToggle>().enabled = true;
        }
    }

    void ReplaceScore(float newScore, string newInitials)
    {
        for (var i = 10; i >= rankToReplace; i--)
        {
            string rankScoreKey = "HS" + i.ToString() + "_score";
            string rankInitialsKey = "HS" + i.ToString() + "_name";

            string movingRankScoreKey = "HS" + (i - 1).ToString() + "_score";
            string movingRankInitialsKey = "HS" + (i - 1).ToString() + "_name";

            if (i != rankToReplace)
            {
                PlayerPrefs.SetFloat(rankScoreKey, PlayerPrefs.GetFloat(movingRankScoreKey));
                PlayerPrefs.SetString(rankInitialsKey, PlayerPrefs.GetString(movingRankInitialsKey));
            }
            else
            {
                PlayerPrefs.SetFloat(rankScoreKey, newScore);
                PlayerPrefs.SetString(rankInitialsKey, newInitials);
            }
        }
    }

    //arcade controls
    enum KeyUpDown
    {
        none, up, down
    }
    KeyUpDown keyUpDown = KeyUpDown.none;

    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,.!@#$%^&*()-+=_:;<>";
    int charIndex = 0;


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F9) && Input.GetKey(KeyCode.LeftAlt))
        {
            ResetScoreInPrefs();
        }

        if (NewHighScore.activeSelf) //1. when player dies, place the score (PlaceScore). 2. if the score is in the top 10, prompt for initials. 3. When the player hits enter, set the new score and move the others down.
        {
            //before anything, erase the end
            if (InitialsText.text.Length > 0)
            {
                InitialsText.text = InitialsText.text.Substring(0, InitialsText.text.Length - 1);
            }

            //check for up or down stick presses
            CheckUpDown();

            //now add the character we potentially might add
            if (InitialsText.text.Length < 3)
                InitialsText.text += alphabet[charIndex];

            //if you hit enter or a, SOLIDIFY that character choice
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button16) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                if (InitialsText.text.Length == 3)
                {
                    ReplaceScore(CurrentScore, InitialsText.text);
                    GetComponent<BonusSceneToggle>().ToggleScene();
                        //TODO: make this less instant, make it so your initials flash a bit then it takes you to high score.
                }
                else //SOLIDIFYYYYYYYY
                {
                    InitialsText.text += alphabet[charIndex];
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace)) //todo: change this to b button
            {
                if (InitialsText.text.Length > 0)
                    InitialsText.text = InitialsText.text.Substring(0, InitialsText.text.Length - 1);
            }



            /*
             * this code is for the pc version
             * 
            //before anything, erase the underscore there may be at the end. if the timer has us adding it, then we will do that again at the end
            if (InitialsText.text.EndsWith("_"))
            {
                InitialsText.text = InitialsText.text.Substring(0, InitialsText.text.Length - 1);
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (InitialsText.text.Length > 0)
                    InitialsText.text = InitialsText.text.Substring(0, InitialsText.text.Length - 1);
            }

            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button16) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                ReplaceScore(CurrentScore, InitialsText.text);
                GetComponent<BonusSceneToggle>().ToggleScene(); //TODO: make this less instant, make it so your initials flash a bit then it takes you to high score.
            }

            else if (Input.anyKeyDown)
            {
                //make sure it's not an underscore
                if (!string.IsNullOrEmpty(Input.inputString) && Input.inputString[0] != "_".ToCharArray()[0])
                {
                    if (Input.inputString.Length > 0 && InitialsText.text.Length < 3)
                        InitialsText.text += Input.inputString[0].ToString().ToUpper();
                }
            }
            */


            //now let's make this text centered
            //930 because it looks better offset -30 pixels already
            InitialsText.GetComponent<RectTransform>().anchoredPosition = new Vector2(930 - InitialsText.preferredWidth / 2, InitialsText.GetComponent<RectTransform>().anchoredPosition.y);

            //here we add our underscore blinking cursor /whatever


            //here we count up on the timer, if it's halfway to max we make it disappear, 
            //if its above or equal to max we reset the timer

            //make the timer work later todo
            /*
            cursorTimer += Time.deltaTime;
            if (cursorTimer < cursorTimerMax * 0.5f)
            {
                if (InitialsText.text.Length < 3)
                {
                    InitialsText.text += alphabet[charIndex];
                }
            }
            else
            {
                //nothing
            }

            //max timer reset
            if (cursorTimer > cursorTimerMax)
            {
                cursorTimer = 0;
            }
            */
        }
    }

    void CheckUpDown()
    {
        var vert = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(vert) > 0.1)
        {
            if (vert > 0 && keyUpDown != KeyUpDown.up)
            {
                FireChar(-1);
                keyUpDown = KeyUpDown.up;
            }

            if (vert < 0 && keyUpDown != KeyUpDown.down)
            {
                FireChar(1);
                keyUpDown = KeyUpDown.down;
            }
        }
        else
        {
            keyUpDown = KeyUpDown.none;
        }
    }

    void FireChar(int i)
    {
        charIndex += i;
        if (charIndex >= alphabet.Length)
            charIndex -= alphabet.Length;
        if (charIndex < 0)
            charIndex += alphabet.Length;
    }

    public void AddToScore(float dist)
    {
        CurrentScore += Mathf.Abs(dist);
        if (CurrentScore > bestDistance)
        {
            bestDistance = CurrentScore;
        }

        var distToDisplay = CurrentScore;
        var bestDistToDisplay = bestDistance;

        if (inMiles)
        {
            distToDisplay = DistInKM(distToDisplay);
            bestDistToDisplay = DistInKM(bestDistToDisplay);
        }

        scoreText.text = ConvertToNiceDigits(distToDisplay) + " KM";
    }

    public static float DistInKM(float dist)
    {
        float distToReturn = dist * 0.004f;
        distToReturn *= 1.609f;
        return distToReturn;
    }

    public static string ConvertToNiceDigits(float n)
    {
        string digits = n.ToString("0.0");
        return digits;
    }

    public void OnPlayerDeath()
    {
        PlaceScore(CurrentScore);
    }

    void ResetScoreInPrefs()
    {
        PlayerPrefs.DeleteAll();
        bestDistance = 0;
    }

    void OnEnable()
    {
        if (PlayerPrefs.HasKey("HiScore"))
        {
            bestDistance = PlayerPrefs.GetFloat("HiScore");
        }
    }
}
