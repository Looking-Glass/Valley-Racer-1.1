using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BillboardedHyperSprite : MonoBehaviour
{
    public Transform hypercubeTransform;

    void Start()
    {
        if (hypercubeTransform == null && GameObject.FindGameObjectWithTag("Hypercube"))
            hypercubeTransform = GameObject.FindGameObjectWithTag("Hypercube").transform;
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(
            hypercubeTransform.eulerAngles.x,
            hypercubeTransform.eulerAngles.y,
            transform.eulerAngles.z
        );
    }
}


