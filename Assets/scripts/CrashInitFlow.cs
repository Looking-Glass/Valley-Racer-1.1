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
    //float timer;
    public Transform hypercubeTransform;
    float originHypercubeZRot;
    float originHypercubeXRot;
    public Transform motoChildTransform;
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

        //Start constant camera rotator
        cameraRotator.enabled = true;

        //Choose a random spin direction for camera
        cameraRotator.degreesPerSecond = Random.value > 0.6f //not a typo
            ? Mathf.Abs(cameraRotator.degreesPerSecond)
            : -Mathf.Abs(cameraRotator.degreesPerSecond);

        //Set original values for things
        originHypercubeZRot = hypercubeTransform.localEulerAngles.z;
        originHypercubeXRot = hypercubeTransform.localEulerAngles.x;
        originMotoChildZScale = motoChildTransform.localScale.z;
        originTubeFactor = hypercubeCamera.tubeFactor;
        originPerspectiveFactor = hypercubeCamera.perspectiveFactor;
        originHypercubeScale = hypercubeTransform.localScale;
        originHypercubePosition = hypercubeTransform.position;
    }

    public override void OnContinueAct()
    {
        //The lerp:
        var lerp = GetTimeNormalized(0.2f);

        //return hypercube to 0 deg rotation on z
        hypercubeTransform.localEulerAngles = VectorEdit.SetZ
            (hypercubeTransform.localEulerAngles, Mathf.LerpAngle(originHypercubeZRot, 0f, lerp));

        //Rotate hypercube a bit to have a better view
        //hypercubeTransform.localEulerAngles = VectorEdit.SetX(hypercubeTransform.localEulerAngles, Mathf.LerpAngle(originHypercubeZRot, 30f, lerp));

        //make moto child holder return to 1, 1, 1 scale
        //motoChildTransform.localScale = VectorEdit.SetZ(motoChildTransform.localScale, Mathf.Lerp(originMotoChildZScale, 1f, lerp));

        //make camera move to player's position
        /*
        hypercubeTransform.position = new Vector3(
            Mathf.Lerp(originHypercubePosition.x, motoChildTransform.position.x, lerp),
            Mathf.Lerp(originHypercubePosition.y, motoChildTransform.position.y, lerp),
            Mathf.Lerp(originHypercubePosition.z, motoChildTransform.position.z, lerp)
            );
            */

        //Rotate the player's bike by a bit and push it up
        motoChildTransform.Rotate(Vector3.right, Time.deltaTime * 30f, Space.World);

        //Move it back to sync with the rotating
        motoChildTransform.Translate(new Vector3(0, 0.1f, -0.5f) * Time.deltaTime * 1f);

        //Set perspective and tubefactors lower
        //hypercubeCamera.tubeFactor = Mathf.Lerp(originTubeFactor, 0f, lerp);
        //hypercubeCamera.perspectiveFactor = Mathf.Lerp(originPerspectiveFactor, 0f, lerp);

        //Make the hypercube a little bigger
        //hypercubeTransform.localScale = Vector3.Lerp(originHypercubeScale, originHypercubeScale * 2.5f, lerp);
    }

    public override void OnEndAct()
    {
        //Explode and camera shake
        explosion.transform.position = motoChildTransform.position;
        explosion.Explode();
        cameraShake.enabled = true;

        //Turn off bike and human meshrenderers
        var mrs = motoChildTransform.gameObject.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mrs.Length; i++)
        {
            mrs[i].enabled = false;
        }
    }

    void OnDestroy()
    {
        EventManager.playerDeath -= OnPlayerDeath;
    }
}
