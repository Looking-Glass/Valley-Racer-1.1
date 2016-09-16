using UnityEngine;
using System.Collections;

public class BikeController : MonoBehaviour
{
    public Transform mountains;
    public float travelSpeed;
    public float turnSpeed;
    public LayerMask ground;
    public LayerMask peaks;
    public AudioSource hitMountain;
    public ScoreKeeper scoreKeeper;
    public bool controlsOn;
    MotoInput motoInput;
    Transform b_Main;
    Transform b_Torso;
    Transform b_Head;

    void Start()
    {
        motoInput = GetComponent<MotoInput>();
        b_Main = transform.FindDeepChild("b_Main");
        b_Torso = transform.FindDeepChild("b_Torso");
        b_Head = transform.FindDeepChild("b_Head");
    }

    void Update()
    {
        var bikeInput = 0f;
        if (controlsOn)
            bikeInput = motoInput.GetEasedInput();

        //Moving the mountains
        var mountainMovement = Vector3.zero;
        mountainMovement += Vector3.left * bikeInput * turnSpeed;
        mountainMovement += Vector3.back * travelSpeed;
        mountainMovement = mountainMovement.normalized * travelSpeed * Time.deltaTime;
        mountains.Translate(mountainMovement);
        scoreKeeper.AddToScore(mountainMovement.z);

        //Grounding
        var rayHit = new RaycastHit();
        var ray = new Ray(transform.position + Vector3.up * 100, Vector3.down);
        if (Physics.Raycast(ray, out rayHit, 200, ground))
            transform.position = rayHit.point;

        //Rotating
        var mainRot = new Vector3(-5, 0, -45);
        var torsoRot = new Vector3(0, 0, -12);
        var headRot = new Vector3(0, 0, 30);
        var rotLerp = bikeInput * 0.5f + 0.5f;
        b_Main.localEulerAngles = Vector3.Lerp(-mainRot, mainRot, rotLerp);
        b_Torso.localEulerAngles = Vector3.Lerp(-torsoRot, torsoRot, rotLerp);
        b_Head.localEulerAngles = Vector3.Lerp(-headRot, headRot, rotLerp);

        //Collision with peaks
        if (Physics.Raycast(ray, out rayHit, 200, peaks))
        {
            //Death
            if (rayHit.normal.z < -0.5f)
            {
                if (EventManager.playerDeath != null)
                    EventManager.playerDeath();
                gameObject.SetActive(false);
            }
            else
            {
                mountains.Translate(Vector3.left * Mathf.Sign(rayHit.normal.x) * 0.5f);
                motoInput.ResetEasedInput();
                hitMountain.Play();
            }
        }
    }
}