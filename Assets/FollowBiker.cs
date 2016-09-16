using UnityEngine;
using System.Collections;

public class FollowBiker : MonoBehaviour
{
    public float easedSpeed;
    public Transform toFollow;
    public MotoInput motoInput;
    public Vector3 offset;

    void LateUpdate()
    {
        var followPos = toFollow.position + offset;
        var easedMov = easedSpeed * Time.deltaTime;
        easedMov *= Vector3.Distance(transform.position, followPos);
        easedMov = Mathf.Max(0.0001f, easedMov);
        transform.position = Vector3.MoveTowards(transform.position, followPos, easedMov);
        transform.localEulerAngles = transform.localEulerAngles.SetZ(motoInput.GetEasedInput() * -10f);
    }
}
