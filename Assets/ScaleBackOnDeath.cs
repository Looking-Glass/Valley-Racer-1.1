using UnityEngine;

public class ScaleBackOnDeath : MonoBehaviour
{
    private float _scaleBackFactor = 2;
    public Transform manTransform;

    public bool ScaleBackActive;

    private void Update()
    {
        if (ScaleBackActive)
        {
            transform.position = Vector3.Lerp(this.transform.position, manTransform.position, 0.2f);
            transform.localEulerAngles = new Vector3(30, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    public void ScaleBack()
    {
        transform.localScale *= .7f;
        //GetComponentInChildren<hypercubeCamera>().perspectiveFactor = 0;
        ScaleBackActive = true;
    }
}