﻿using UnityEngine;

public class HighScoreSetScript : MonoBehaviour
{
    public TextMesh PlayerInitialsText;
    public TextMesh PlayerScoreText;
    
    void Start()
    {
        PlayerInitialsText.text = "";
        PlayerScoreText.text = "";

        for (var i = 1; i <= 10; i++)
        {
            PlayerInitialsText.text += PlayerPrefs.GetString("HS" + i.ToString() + "_name") + "\n";
            PlayerScoreText.text += ScoreKeeper.ConvertToNiceDigits(PlayerPrefs.GetFloat("HS" + i.ToString() + "_score")) + "\n";
        }
    }
}
