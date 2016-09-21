using UnityEngine;

public class BikerInput : MonoBehaviour
{
    [Range(0.1f, 100f)]
    public float followSpeed = 15f;
    [Range(0.1f, 100f)]
    public float easedSpeed = 15f;
    float followInput;
    float easedFollowInput;

    //Note: This script runs first in script execution order.
    void Update()
    {
        var input = ValleyInput.GetAxis().x;

        var followMov = followSpeed * Time.deltaTime;
        followInput = Mathf.MoveTowards(followInput, input, followMov);

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
}
