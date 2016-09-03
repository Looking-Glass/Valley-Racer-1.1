using UnityEngine;
using System.Collections;
using Leap.Unity;

public class LeapMotoControls : MonoBehaviour
{

    /* Leap motion controls for the motorcycle.
     * beautiful smooth movement.
     * butter */

    public bool selfCheck;
    public BonusSceneToggle bonusSceneToggle;
    LeapServiceProvider leapServiceProvider;

    void Start()
    {
        leapServiceProvider = GetComponent<LeapServiceProvider>();
    }

    void Update()
    {
        if (selfCheck)
        {
            var hands = leapServiceProvider.CurrentFrame.Hands;
            if (hands.Count == 1)
            {
                if (hands[0].PalmPosition.y > 0.6f)
                {
                    if (bonusSceneToggle != null)
                    {
                        bonusSceneToggle.GetComponent<BonusSceneToggle>().ToggleScene();
                    }
                }
            }
        }
    }

    public float GetLeapSteeringValue()
    {
        var steeringValue = 0f;

        //this line makes it easier than writing the whole thing out
        var hands = leapServiceProvider.CurrentFrame.Hands;

        //only do this shit if both hands are detected
        //TODO: if only one hand is detected, measure the roll of the single hand
        if (CheckForTwoHands())
        {
            //make room for the hand positions
            var leftHandPos = new Vector3();
            var rightHandPos = new Vector3();

            //get the hand positions
            for (var i = 0; i < hands.Count; i++)
            {
                if (hands[i].IsLeft)
                {
                    leftHandPos = hands[i].WristPosition.ToVector3();
                }
                if (hands[i].IsRight)
                {
                    rightHandPos = hands[i].WristPosition.ToVector3();
                }
            }

            //print("left hand: " + leftHandPos + "  right hand: " + rightHandPos);

            //this is great, now i need a number between -1 and 1 that represents the "turn" of the bike
            var steeringTriangleOp = rightHandPos.y - leftHandPos.y;
            var steeringTriangleAdj = rightHandPos.x - leftHandPos.x;

            var steeringTriangleTheda = Mathf.Atan2(steeringTriangleOp, steeringTriangleAdj);
            steeringTriangleTheda *= Mathf.Rad2Deg;
            steeringTriangleTheda = Mathf.Clamp(steeringTriangleTheda, -45, 45);

            //now let's get a final -1 to 1 value.
            steeringValue = -steeringTriangleTheda / 45f;
        }

        return steeringValue;
    }

    public bool CheckForTwoHands()
    {
        bool twoHands = leapServiceProvider.CurrentFrame.Hands.Count == 2;
        return twoHands;
    }
}
