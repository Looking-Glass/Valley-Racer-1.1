using UnityEngine;

//LEGACY TO DELETE

public class ScaleBackCameraOnDeathLegacyToDelete : MonoBehaviour
{
    public Transform flyingRiderTransform;
    public float easeSpeed = 100;
    public bool scaleBackActive;

    private void Update()
    {
        if (scaleBackActive)
        {
            //Eased Follow
            if (transform.position != flyingRiderTransform.position)
            {
                var mov = easeSpeed;
                mov *= Time.deltaTime;
                mov *= Vector3.Distance(flyingRiderTransform.position, transform.position);
                transform.position = Vector3.MoveTowards(transform.position, flyingRiderTransform.position, mov);
            }
            transform.localEulerAngles = new Vector3(30, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    public void ScaleBack()
    {
        transform.localScale *= .7f;
        //GetComponentInChildren<hypercubeCamera>().perspectiveFactor = 0;
        scaleBackActive = true;
    }
}