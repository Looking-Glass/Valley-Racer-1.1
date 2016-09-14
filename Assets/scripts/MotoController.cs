using System;
using UnityEngine;

public class MotoController : MonoBehaviour
{

    public GameObject motoChildHolder;
    public float travelSpeed = 12f;
    public float strafeSpeed = 2f;
    public float strafeSensitivity = 5f;
    public float turnTilt = 36f;
    public float rotationJerkinessDamp = 0.2f;
    public float slopeGravity = 4f;
    public int numberToAverageNormal = 4;
    public float averageRadius = 0.66f;
    [Range(-1f, 0f)]
    public float hitRadius = -0.55f;
    public bool keyboardForwardControls;
    public bool controlHypercube = true;
    public LayerMask layerMask;
    public Transform torso;
    public Transform head;
    Quaternion tempRot;
    GameObject hypercubeHolder;
    Vector3 mountainMovement;
    Vector3 mountainBarMovement;
    Vector3 gettableVelocity;
    MotoInput motoInput;

    Vector3 theNormal;
    Vector3 theHit;
    Ray[] theRay;

    ScoreKeeper scoreKeeper;

    void Start()
    {
        hypercubeHolder = GameObject.FindGameObjectWithTag("Hypercube");
        transform.position = new Vector3(hypercubeHolder.transform.position.x, transform.position.y, transform.position.z);
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        motoInput = GetComponent<MotoInput>();
    }

    void Update()
    {
        //Reset mountain movement
        mountainMovement = new Vector3();

        //Horizontal speed
        var currentHorizSpeed = 0f;
        if (motoInput != null)
            currentHorizSpeed = motoInput.GetEasedInput();

        //cast a ray downward from MotoHolder
        var motoRay = new Ray(transform.position + Vector3.up * 100, -Vector3.up * 200);
        RaycastHit motoRayHit;
        var didHit = Physics.Raycast(motoRay, out motoRayHit, 100f, layerMask);

        if (didHit)
        {
            //put the motoChildHolder at that spot.
            motoChildHolder.transform.position = motoRayHit.point;

            //what happens when he hits a peak
            if (motoRayHit.collider.tag == "Peak")
            {
                //explode him if he hits a peak head on
                if (motoRayHit.normal.z < hitRadius)
                {
                    //Fire death event
                    EventManager.playerDeath();

                    //turn off this component. no more.
                    enabled = false;
                }
                else
                {
                    //otherwise send him to the side. this will either force him to blow up
                    //OR send him back into the valley, where you'll maybe have to recover from a wobble to keep control
                    mountainMovement -= Vector3.right * Mathf.Sign(motoRayHit.normal.x) * 0.5f;
                    currentHorizSpeed *= 0.3f;
                    GetComponent<AudioSource>().Play();
                }

            }

            //hypercube mostly follows the motorcycle on the y axis, plus an offset to make sure the motorcycle is near the bottom
            if (controlHypercube)
            {
                hypercubeHolder.transform.position = new Vector3(
                    hypercubeHolder.transform.position.x,
                    motoChildHolder.transform.position.y * 0.8f + .8f,
                    hypercubeHolder.transform.position.z
                );
            }

            //for debug
            theHit = motoRayHit.point;

            //get the avg angles around to make a better orient to ground
            if (numberToAverageNormal > 0)
            {
                Vector3 normalTotal = motoRayHit.normal;
                theRay = new Ray[numberToAverageNormal];
                for (var i = 0; i < numberToAverageNormal; i++)
                {

                    var angleToRotate = 360f / numberToAverageNormal;
                    angleToRotate *= i;
                    var almostForward = Vector3.forward * averageRadius;
                    almostForward = Quaternion.Euler(0, angleToRotate, 0) * almostForward;
                    Ray newRay = motoRay;
                    newRay.origin += almostForward;
                    Physics.Raycast(newRay, out motoRayHit);
                    normalTotal = normalTotal + motoRayHit.normal;

                    theRay[i] = newRay;
                }

                //set the raycastHit normal to the average of these
                motoRayHit.normal = normalTotal / (numberToAverageNormal + 1);
                theNormal = motoRayHit.normal;
            }

            //orient him to the ground
            var newFloat = Mathf.Atan2(motoRayHit.normal.y, motoRayHit.normal.x);
            var newRotation = Quaternion.Euler(new Vector3(0, 0, (newFloat * Mathf.Rad2Deg - 90) * 0.5f));
            motoChildHolder.transform.rotation = Quaternion.Lerp(tempRot, newRotation, rotationJerkinessDamp);

            //move him down the hill if there's a hill
            mountainMovement += new Vector3(-motoRayHit.normal.x, 0, 0) * Time.deltaTime * slopeGravity;
        }

        //input

        //horizontal input
        if (Math.Abs(currentHorizSpeed) > 0.001f)
        {
            mountainMovement -= Vector3.right * currentHorizSpeed * Time.deltaTime * strafeSpeed;
        }

        //save the rotation of the motoChildHolder for later
        tempRot = motoChildHolder.transform.rotation;
        //motorcycle tilt
        motoChildHolder.transform.localEulerAngles += Vector3.back * currentHorizSpeed * turnTilt;
        //make the cube tilt a little with it.
        if (controlHypercube)
        {
            hypercubeHolder.transform.localEulerAngles = new Vector3(
                hypercubeHolder.transform.localEulerAngles.x,
                hypercubeHolder.transform.localEulerAngles.y,
                -currentHorizSpeed * (turnTilt / 3)
            );

            //Make the body and head counter-tilt a little to look like a real turn
            torso.localEulerAngles = Vector3.down * currentHorizSpeed * turnTilt / 10f;
            head.localEulerAngles = Vector3.down * currentHorizSpeed * turnTilt / 6f;
        }

        //take all the little mountain squares and move them horizontally
        GameObject[] mountains = GameObject.FindGameObjectsWithTag("Mountains");
        foreach (GameObject mountain in mountains)
            mountain.transform.localPosition += mountainMovement;

        //take the mountain bars and move them forward, automatically or with keyboard controls.
        mountainBarMovement = new Vector3();
        var vertInput = 1f;
        if (keyboardForwardControls)
        {
            vertInput = Input.GetAxis("Vertical");
        }
        var currentSpeedDamp = 1f - Mathf.Abs(currentHorizSpeed * 0.3f);
        mountainBarMovement += Vector3.back * travelSpeed * Time.deltaTime * vertInput * currentSpeedDamp;

        GameObject[] mountainBars = GameObject.FindGameObjectsWithTag("MountainBars");
        foreach (GameObject mountainBar in mountainBars)
            mountainBar.transform.localPosition += mountainBarMovement;

        //also add the distance travelled to score.
        if (scoreKeeper != null)
            scoreKeeper.AddToScore(mountainBarMovement.z);

        gettableVelocity = new Vector3(mountainMovement.x, mountainBarMovement.y, mountainBarMovement.z);
    }

    public Vector3 GetVelocity()
    {
        return gettableVelocity;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(new Ray(theHit, theNormal));

        Gizmos.color = Color.red;
        if (theRay != null)
        {
            foreach (var t in theRay)
            {
                Gizmos.DrawRay(t.origin, t.direction * 200);
            }
        }
    }
}
