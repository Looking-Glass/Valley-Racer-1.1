using UnityEngine;

public class HighScoreScrollScript : MonoBehaviour
{
    private float _speed = 1;

    void Update()
    {
        //Use the joystick to control the speed of scrolling
        var speedModifier = 0.5f * Input.GetAxisRaw("Vertical");
        if (speedModifier > 0)
            speedModifier *= 4;
        transform.localPosition += Vector3.up * Time.deltaTime * (_speed + speedModifier);

        if (transform.localPosition.y > 11) //so I hardcoded the Y value, whatever give me a break!!
            transform.localPosition -= 14.5f * Vector3.up;
    }
}