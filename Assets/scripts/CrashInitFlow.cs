using UnityEngine;
using hypercube;

public class CrashInitFlow : TimedFlow
{
    /* Flow:
     * on init there is a crash sound
     * the camera rotation starts (this camera rotation continues literally forever)
     * Moto child holder lerps from miniscule Z scale to full (1) z scale
     * Camera lerps out along with it to make the player the center of the hypercube
     * Hypercube Perspective and Tube factors lerp down to like half of what they are now
     * The lerp has an ease out like a square root graph
     * once this lerp is over, another flow is triggered which
     * 1. shakes camera
     * 2. does explosion
     * 3. plays explosion sound
     */

    public AudioSource bikeHit;
    public SpinDegPerSecond cameraRotator;
    public Transform hypercubeTransform;
    float originHypercubeZRot;
    public Transform biker;
    float originMotoChildZScale;
    Vector3 originHypercubePosition;
    public hypercubeCamera hypercubeCamera;
    float originTubeFactor;
    float originPerspectiveFactor;
    public Explosion explosion;
    public CameraShake cameraShake;
    Vector3 originHypercubeScale;

    void Start()
    {
        EventManager.playerDeath += OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        DoAct();
    }

    public override void OnBeginAct()
    {
        //Trigger crash sound
        bikeHit.Play();

        //Set original values for things
        originHypercubeZRot = hypercubeTransform.localEulerAngles.z;
        originMotoChildZScale = biker.localScale.z;
        originTubeFactor = hypercubeCamera.tubeFactor;
        originPerspectiveFactor = hypercubeCamera.perspectiveFactor;
        originHypercubeScale = hypercubeTransform.localScale;
        originHypercubePosition = hypercubeTransform.position;
    }

    public override void OnContinueAct()
    {
        //The lerp:
        var lerp = GetTimeNormalized(0.1f);

        //Turn off hypercube's follow script
        hypercubeTransform.GetComponent<FollowBiker>().enabled = false;

        //return hypercube to 0 deg rotation on z
        hypercubeTransform.localEulerAngles = hypercubeTransform.localEulerAngles.SetZ(Mathf.LerpAngle(originHypercubeZRot, 0f, lerp));

        //Rotate hypercube a bit to have a better view
        hypercubeTransform.localEulerAngles = hypercubeTransform.localEulerAngles.SetX(Mathf.LerpAngle(originHypercubeZRot, 15f, lerp));

        //make moto child holder return to 1, 1, 1 scale
        biker.localScale = biker.localScale.SetZ(Mathf.Lerp(originMotoChildZScale, 1f, lerp));

        //make camera move to player's position
        hypercubeTransform.position = new Vector3(
            Mathf.Lerp(originHypercubePosition.x, biker.position.x, lerp),
            Mathf.Lerp(originHypercubePosition.y, biker.position.y, lerp),
            Mathf.Lerp(originHypercubePosition.z, biker.position.z, lerp)
            );

        //Set perspective and tubefactors lower
        hypercubeCamera.tubeFactor = Mathf.Lerp(originTubeFactor, 0f, lerp);
        hypercubeCamera.perspectiveFactor = Mathf.Lerp(originPerspectiveFactor, 0f, lerp);

        //Make the hypercube a little bigger
        hypercubeTransform.localScale = Vector3.Lerp(originHypercubeScale, originHypercubeScale * 2.5f, lerp);
    }

    public override void OnEndAct()
    {
        //Explode and camera shake
        explosion.transform.position = biker.position;
        explosion.Explode();
        cameraShake.enabled = true;

        //Turn off bike
        biker.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.playerDeath -= OnPlayerDeath;
    }
}
