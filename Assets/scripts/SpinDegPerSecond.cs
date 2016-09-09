using UnityEngine;

public class SpinDegPerSecond : MonoBehaviour
{
    public float degreesPerSecond;
    public Vector3 axis = Vector3.up;
    public Space space = Space.Self;

    void Update()
    {
        transform.Rotate(axis.normalized * degreesPerSecond * Time.deltaTime, space);
    }
}
