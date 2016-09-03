using UnityEngine;

[RequireComponent(typeof(HeightmapToVert))]
public class MountainRepeater : MonoBehaviour
{
    float repeatAfterWidth;
    
    void Start()
    {
        repeatAfterWidth = GetComponent<HeightmapToVert>().peakTexture.width;
    }
    
    void Update()
    {
        if (transform.localPosition.x < -repeatAfterWidth * 1.5f || transform.localPosition.x > repeatAfterWidth * 0.5f)
            transform.localPosition += Vector3.right * -Mathf.Sign(transform.localPosition.x) * repeatAfterWidth * 2;
    }
}