using System;
using UnityEngine;

public class MotoController : MonoBehaviour
{

    public GameObject motoChildHolder;
    public float rotationJerkinessDamp = 0.2f;
    public float slopeGravity = 4f;
    public float travelSpeed = 12f;
    public float strafeSpeed = 2f;
    public float strafeSensitivity = 5f;
    public float levelSizeBoundaries = 8f;
    public float turnTilt = 36f;
    public int numberToAverageNormal = 4;
    public float averageRadius = 0.66f;
    [Range(-1f, 0f)]
    public float hitRadius = -0.55f;
    public bool keyboardForwardControls;
    public bool controlHypercube = true;
    public LayerMask layerMask;
    public GameObject explosion;

    Quaternion tempRot;
    GameObject hyperCube;
    Vector3 mountainMovement;
    Vector3 mountainBarMovement;
    float currentHorizSpeed;

    Vector3 theNormal;
    Vector3 theHit;
    Ray[] theRay;

    ScoreKeeper scoreKeeper;
    
    void Start()
    {
        hyperCube = GameObject.FindGameObjectWithTag("Hypercube");
        transform.position = new Vector3(hyperCube.transform.position.x, transform.position.y, transform.position.z);
        if (GameObject.FindGameObjectWithTag("ScoreKeeper"))
        {
            scoreKeeper = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<ScoreKeeper>();
        }
    }
    
    void Update()
    {
        mountainMovement = new Vector3(); // reset mountain movement

        //get the horizontal movement/do some things with it, then hold it as a float 0-1 to use
        //note that i TURNED OFF keyboard input for the horiz axis, b/c i don't like the mushiness.
        float newHorizInput = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newHorizInput = -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newHorizInput = 1f;
        }

        float newHorizSpeed = 0f;
        if (newHorizInput > currentHorizSpeed)
        {
            newHorizSpeed = currentHorizSpeed + strafeSensitivity * Time.deltaTime;
        }
        else if (newHorizInput < currentHorizSpeed)
        {
            newHorizSpeed = currentHorizSpeed - strafeSensitivity * Time.deltaTime;
        }

        if (Mathf.Abs(newHorizInput - currentHorizSpeed) > strafeSensitivity * Time.deltaTime)
        {
            currentHorizSpeed = newHorizSpeed;
        }

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
                    //DEATH
                    explosion.GetComponent<Explosion>().Explode();
                    //hyperCube.GetComponent<CameraShake> ().Shake (0.5f, .5f);
                    motoChildHolder.transform.SetParent(null);
                    while (motoChildHolder.transform.childCount > 0)
                    {
                        var holderBaby = motoChildHolder.transform.GetChild(0);
                        holderBaby.SetParent(null); //remove all the children so they can be freee.
                        holderBaby.localScale = new Vector3(holderBaby.localScale.x, holderBaby.localScale.x, holderBaby.localScale.x);
                        holderBaby.gameObject.AddComponent<Rigidbody>();
                        holderBaby.gameObject.AddComponent<BoxCollider>();
                        holderBaby.gameObject.layer = 0;
                        if (holderBaby.childCount > 0)
                        {
                            for (var i = 0; i < holderBaby.childCount; i++)
                            {
                                holderBaby.GetChild(i).gameObject.layer = 0;
                            }
                        }
                    }
                    gameObject.SetActive(false);
                    hyperCube.GetComponent<ScaleBackOnDeath>().ScaleBack();

                    //start the timer which will trigger the player death part.
                    scoreKeeper.OnPlayerDeath();
                    var smgo = GameObject.FindGameObjectWithTag("SerialManager");
                    if (smgo != null)
                    {
                        var sm = smgo.GetComponent<serialManager>();
                        if (sm != null)
                            sm.OnPlayerDeath();
                    }


                    //was going to make this trigger the leap motion skip to end but probably best to do in scorekeeper

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
                hyperCube.transform.position = new Vector3(
                    hyperCube.transform.position.x,
                    motoChildHolder.transform.position.y * 0.8f + .8f,
                    hyperCube.transform.position.z
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
            hyperCube.transform.localEulerAngles = new Vector3(
                hyperCube.transform.localEulerAngles.x,
                hyperCube.transform.localEulerAngles.y,
                -currentHorizSpeed * (turnTilt / 3)
            );
        }

        //take all the little mountain squares and move them horizontally
        GameObject[] mountains = GameObject.FindGameObjectsWithTag("Mountains");
        foreach (GameObject mountain in mountains)
        {
            mountain.transform.localPosition += mountainMovement;
        }

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
        {
            mountainBar.transform.localPosition += mountainBarMovement;
        }
        //also add the distance travelled to score.
        if (scoreKeeper != null)
        {
            scoreKeeper.AddToScore(mountainBarMovement.z);
        }
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
