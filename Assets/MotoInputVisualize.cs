using UnityEngine;

public class MotoInputVisualize : MonoBehaviour
{
    MotoInput motoInput;
    public Transform input;
    public Transform followedInput;
    public Transform easedFollowInput;
    public float movementDistance = 2f;

    void Start()
    {
        motoInput = FindObjectOfType<MotoInput>();
    }

    void Update()
    {
        input.localPosition = new Vector3(
            Input.GetAxisRaw("Horizontal") * movementDistance,
            input.localPosition.y,
            input.localPosition.z);

        followedInput.localPosition = new Vector3(
            motoInput.GetFollowInput() * movementDistance,
            followedInput.localPosition.y,
            followedInput.localPosition.z);

        easedFollowInput.localPosition = new Vector3(
            motoInput.GetEasedInput() * movementDistance,
            easedFollowInput.localPosition.y,
            easedFollowInput.localPosition.z);
    }
}
