using UnityEngine;
using System.Collections;

public class HighScoresScrollScript : MonoBehaviour
{
    private float _speed = 1;
    
    void Update ()
    {
        var modSpeed = 0.5f * Input.GetAxisRaw("Vertical");
        if (modSpeed > 0)
            modSpeed *= 4;
	    this.transform.localPosition += Vector3.up * Time.deltaTime * (_speed + modSpeed);

        if (this.transform.localPosition.y > 11) //so I hardcoded the Y value, whatever give me a break!!
        {
            this.transform.localPosition -= 14.5f * Vector3.up;
        }
	}
}
