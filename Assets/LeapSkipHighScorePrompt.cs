using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;

public class LeapSkipHighScorePrompt : MonoBehaviour
{
    public GameObject highScoreDisplayToSkip;
    public LeapServiceProvider leapService;
    float timer;

    void Update()
    {
        leapService = FindObjectOfType<LeapServiceProvider>();
        if (leapService != null)
        {
            var hands = leapService.CurrentFrame.Hands;
            if (hands.Count > 0)
            {
                highScoreDisplayToSkip.SetActive(false);
                timer += Time.deltaTime;
                if (timer >= 3f)
                {
                    FindObjectOfType<SceneToggle>().ToggleScene();
                }
            }
        }
    }
}
