using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;

/// <summary>
/// Basic idea is this: 
/// When a hand is detected:
///     disappear the Press A graphic
///     Start a countdown (3, 2, 1)
///     if the hand disappears
///         return the Press A graphic
/// 
/// In the high score screen, same logic
///     (possibly choose a different spot for the countdown)
/// 
/// 
/// </summary>
public class LeapStartGame : MonoBehaviour
{
    public Transform three21;
    public GameObject pressEnter;
    public LeapServiceProvider leapService;
    const float waitTime = 0.666f;
    bool ready;
    int handCount;

    void Awake()
    {
        leapService = FindObjectOfType<LeapServiceProvider>();
        StartCoroutine(LeadIn());
    }

    void Update()
    {
        if (!ready) return;

        //check for a change in hand count, if there is one, call the handcound change func
        if (leapService == null)
        {
            leapService = FindObjectOfType<LeapServiceProvider>();
        }
        else
        {
            var hands = leapService.CurrentFrame.Hands;
            if (hands.Count != handCount) OnHandCountChange();
            handCount = hands.Count;
        }
    }

    void OnHandCountChange()
    {
        var hands = leapService.CurrentFrame.Hands;
        if (hands.Count > 0)
        {
            SetPressEnter(false);
            StartCoroutine(StartCountdown());
        }
        else
        {
            StopAllCoroutines();
            for (int i = 0; i < three21.childCount; i++)
            {
                three21.GetChild(i).gameObject.SetActive(false);
            }
            SetPressEnter(true);
        }
    }

    void SetPressEnter(bool tf)
    {
        if (pressEnter != null)
        {
            pressEnter.GetComponent<SpriteRenderer>().enabled = tf;
            pressEnter.GetComponent<SpriteBlink>().enabled = tf;
        }
    }

    IEnumerator LeadIn()
    {
        yield return new WaitForSeconds(0.5f);
        ready = true;
    }

    IEnumerator StartCountdown()
    {
        three21.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        three21.GetChild(0).gameObject.SetActive(false);
        three21.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        three21.GetChild(1).gameObject.SetActive(false);
        three21.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        //do the scene switching the way it's supposed to happen
        if (pressEnter != null)
            pressEnter.GetComponent<SerialPressEnter>().StartGame();
        else
            FindObjectOfType<SceneToggle>().ToggleScene();
    }
}