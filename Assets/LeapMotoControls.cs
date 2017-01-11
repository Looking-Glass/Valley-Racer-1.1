using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

/// <summary>
/// This is for converting leap input into a float -1 to 1 to be used as a player turn
/// </summary>
public class LeapMotoControls : MonoBehaviour
{
    public LeapServiceProvider leapService;

    /// <summary>
    /// Returns between -1 and 1 (to be used as player turn)
    /// </summary>
    public bool GetLeapMotoAngle(ref float leapTurnValue)
    {
        var hands = leapService.CurrentFrame.Hands;
        if (hands.Count > 0)
        {
            float compensation = hands[0].IsRight ? 1 : -1;
            compensation *= 0.1f;
            leapTurnValue = -Mathf.Clamp((hands[0].PalmNormal.ToVector3().x + compensation) * 1.1f, -1f, 1f);
            return true;
        }
        return false;
    }
}
