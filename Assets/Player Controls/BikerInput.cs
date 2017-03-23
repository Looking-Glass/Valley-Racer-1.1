using hypercube;
using Leap.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BikerInput : MonoBehaviour
{
    [Range(0.1f, 100f)]
    public float followSpeed = 15f;
    [Range(0.1f, 100f)]
    public float easedSpeed = 15f;

    [Tooltip(
        "Touch scale: higher = smaller section in the middle where value is actually lerped. " +
        "10000 = literally 99.999% left or right, 1 = gradient between sides. default is 3.")]
    public float touchScale = 3;
    float followInput;
    float easedFollowInput;
    LeapServiceProvider leapService;

    //Note: This script runs first in script execution order.
    void Update()
    {
        float bikeInput = ValleyInput.GetAxis().x;
        bikeInput += GetLeapInput();
        bikeInput += GetTouchInput();

        var followMov = followSpeed * Time.deltaTime;
        followInput = Mathf.MoveTowards(followInput, bikeInput, followMov);

        var easedMov = easedSpeed * Time.deltaTime;
        easedMov *= Mathf.Abs(followInput - easedFollowInput);
        easedMov = Mathf.Max(0.0001f, easedMov);
        easedFollowInput = Mathf.MoveTowards(easedFollowInput, followInput, easedMov);
    }

    public float GetFollowInput()
    {
        return followInput;
    }

    public float GetEasedInput()
    {
        return easedFollowInput;
    }

    /// <summary>
    /// This is for a special case: when the player hits the side of a peak
    /// </summary>
    public void ResetEasedInput()
    {
        easedFollowInput = 0f;
    }

    //Leap
    /// <summary>
    /// Returns between -1 and 1 (to be used as player turn)
    /// </summary>
    public float GetLeapInput()
    {
        //temporarily disabled leap
        /*
        if (leapService == null) leapService = FindObjectOfType<LeapServiceProvider>();
        if (leapService != null && leapService.CurrentFrame != null)
        {
            var hands = leapService.CurrentFrame.Hands;
            if (hands.Count > 0)
            {
                float compensation = hands[0].IsRight ? 1 : -1;
                compensation *= 0.1f;
                return -Mathf.Clamp((hands[0].PalmNormal.ToVector3().x + compensation) * 1.1f, -1f, 1f);
            }
        }
        */
        return 0f;
    }

    //Touch
    public float GetTouchInput()
    {
        float touchInput = 0f;
        if (input.touchPanel == null)
            return 0;
        var touches = input.touchPanel.touches;
        foreach (touch touch in touches)
        {
            touchInput = Mathf.Lerp(-1 * touchScale, 1 * touchScale, touch.posX);
        }

        return touchInput;
    }
}
