using UnityEngine;

public class MotoInput : MonoBehaviour
{
    [Range(0.1f, 100f)]
    public float followSpeed = 1f;
    [Range(0.1f, 100f)]
    public float easedSpeed = 1f;
    float followInput;
    float easedFollowInput;

    //Note: This script runs first in script execution order.
    void Update()
    {
        var input = Input.GetAxisRaw("Horizontal");

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
}
