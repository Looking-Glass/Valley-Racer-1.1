using UnityEngine;

public class ScaleBackCameraOnDeath : MonoBehaviour
{
    public Transform flyingRiderTransform;
    public bool scaleBackActive;

    private void Update()
    {
        if (scaleBackActive)
        {
            //This is a bad lerp but the worst that happens at 1200fps is the camera follows the player exactly
            transform.position = Vector3.Lerp(transform.position, flyingRiderTransform.position, 0.2f);
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