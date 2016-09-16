using UnityEngine;
using System.Collections;

public class CameraTimedFlow : TimedFlow
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoAct();
        }
    }

    public override void OnBeginAct()
    {

    }

    public override void OnContinueAct()
    {
        UpdateCamera();
    }

    public override void OnEndAct()
    {
        UpdateCamera();
    }

    void UpdateCamera()
    {
        var lerp = GetTimeNormalized(3f);

        var newAngle = Mathf.LerpAngle(0f, 20f, lerp);
        transform.localEulerAngles = transform.localEulerAngles.SetX(newAngle);
    }
}
