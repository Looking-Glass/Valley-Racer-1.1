using System.Collections;
using System.Collections.Generic;
using hypercube;
using Leap.Unity;
using UnityEngine;

public class HighScoreSkip : MonoBehaviour
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
                FindObjectOfType<SceneToggle>().enabled = true;

                timer += Time.deltaTime;
                if (timer >= 3f)
                {
                    FindObjectOfType<SceneToggle>().ToggleScene();
                }
            }
        }
        if (input.frontScreen.touches.Length > 0)
        {
            highScoreDisplayToSkip.SetActive(false);
            FindObjectOfType<SceneToggle>().enabled = true;
            FindObjectOfType<SceneToggle>().littleBuffer = 2f;
        }
    }
}
