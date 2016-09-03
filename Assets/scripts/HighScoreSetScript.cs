using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScoreSetScript : MonoBehaviour
{

    public TextMesh PlayerInitialsText;
    public TextMesh PlayerScoreText;

	// Use this for initialization
	void Start ()
	{

	    PlayerInitialsText.text = "";
	    PlayerScoreText.text = "";

	    for (var i = 1; i <= 10; i++)
	    {
	        PlayerInitialsText.text += PlayerPrefs.GetString("HS" + i.ToString() + "_name") + "\n";
	        PlayerScoreText.text += ScoreKeeper.ConvertToNiceDigits(ScoreKeeper.DistInKM(PlayerPrefs.GetFloat("HS" + i.ToString() + "_score"))) + "km\n"; //\n is a linebreak
	    }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
