using UnityEngine;
using UnityEngine.UI;

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

    //arcade controls
    enum KeyUpDown
    {
        none, up, down
    }
    KeyUpDown keyUpDown = KeyUpDown.none;

    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.!?_<>";
    int charIndex;

    void OnEnable()
    {
        if (PlayerPrefs.HasKey("HiScore"))
            bestDistance = PlayerPrefs.GetFloat("HiScore");
    }

    void Update()
    {
        //Alt + F9 resets scores
        if (Input.GetKeyDown(KeyCode.F9) && Input.GetKey(KeyCode.LeftAlt))
        {
            ResetScoreInPrefs();
        }

        //1. when player dies, place the score (PlaceScore). 
        //2. if the score is in the top 10, prompt for initials. 
        //3. When the player hits enter, set the new score and move the others down.
        if (NewHighScore.activeSelf) 
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

            if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("1") || Input.GetButtonDown("17"))
            {
                if (InitialsText.text.Length > 0)
                    InitialsText.text = InitialsText.text.Substring(0, InitialsText.text.Length - 1);
            }
            
            //930 because it looks better offset -30 pixels already
            InitialsText.GetComponent<RectTransform>().anchoredPosition = new Vector2
                (930 - InitialsText.preferredWidth / 2, InitialsText.GetComponent<RectTransform>().anchoredPosition.y);
        }
    }

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
            //TODO: May throw error, check this out
            GetComponent<BonusSceneToggle>().enabled = true;
        }
    }

    void ReplaceScore(float newScore, string newInitials)
    {
        for (var i = 10; i >= rankToReplace; i--)
        {
            string rankScoreKey = "HS" + i + "_score";
            string rankInitialsKey = "HS" + i + "_name";

            string movingRankScoreKey = "HS" + (i - 1) + "_score";
            string movingRankInitialsKey = "HS" + (i - 1) + "_name";

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

    void CheckUpDown()
    {
        var vert = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(vert) > 0.1)
        {
            if (vert > 0.1f && keyUpDown != KeyUpDown.up)
            {
                FireChar(-1);
                keyUpDown = KeyUpDown.up;
            }

            if (vert < -0.1f && keyUpDown != KeyUpDown.down)
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

        if (inMiles)
        {
            distToDisplay = DistInKM(distToDisplay);
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
    
}
