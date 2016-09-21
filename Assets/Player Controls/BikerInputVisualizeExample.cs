using UnityEngine;

public class BikerInputVisualizeExample : MonoBehaviour
{
    BikerInput bikerInput;
    public Transform input;
    public Transform followedInput;
    public Transform easedFollowInput;
    public float movementDistance = 2f;

    void Start()
    {
        bikerInput = FindObjectOfType<BikerInput>();
    }

    void Update()
    {
        input.localPosition = new Vector3(
            ValleyInput.GetAxis().x * movementDistance,
            input.localPosition.y,
            input.localPosition.z);

        followedInput.localPosition = new Vector3(
            bikerInput.GetFollowInput() * movementDistance,
            followedInput.localPosition.y,
            followedInput.localPosition.z);

        easedFollowInput.localPosition = new Vector3(
            bikerInput.GetEasedInput() * movementDistance,
            easedFollowInput.localPosition.y,
            easedFollowInput.localPosition.z);
    }
}
