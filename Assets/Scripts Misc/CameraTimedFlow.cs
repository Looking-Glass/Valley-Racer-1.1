using UnityEngine;
using System.Collections;

public class CameraTimedFlow : TimedFlow
{
    public Vector3 finalBikeFollowOffset;
    public Transform initCamTransform;
    public Transform finalCamTransform;
    public Transform hypercubeHolder;
    public Transform biker;
    public float finalBikerScaleZ;
    public SpriteRenderer sky;
    public Mountains mountains;
    public Texture2D mainHeightmap;
    public BikeController bikeController;
    public FollowBiker followBiker;


    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.5f);
        DoAct();
    }

    IEnumerator DelayedMountains()
    {
        yield return new WaitForSeconds(1f);
        mountains.ReplaceHeightmap(mainHeightmap);
    }

    public override void OnBeginAct()
    {

    }

    public override void OnContinueAct()
    {
        var lerpTime = GetTimeNormalized(0.5f);
        hypercubeHolder.transform.localPosition = Vector3.Lerp(initCamTransform.localPosition,
            finalCamTransform.localPosition, lerpTime);
        hypercubeHolder.transform.localScale = Vector3.Lerp(initCamTransform.localScale, finalCamTransform.localScale, lerpTime);

        var newEulX = Mathf.Lerp(initCamTransform.localEulerAngles.x, finalCamTransform.localEulerAngles.x, lerpTime);
        hypercubeHolder.localEulerAngles = hypercubeHolder.localEulerAngles.SetX(newEulX);

        var newEulY = Mathf.Lerp(0, 360, lerpTime);
        hypercubeHolder.localEulerAngles = hypercubeHolder.localEulerAngles.SetY(newEulY);

        biker.localScale = biker.localScale.SetZ(Mathf.Lerp(1f, finalBikerScaleZ, lerpTime));
    }

    public override void OnEndAct()
    {
        sky.enabled = true;
        bikeController.controlsOn = true;
        followBiker.enabled = true;
        StartCoroutine(DelayedMountains());
    }
}
