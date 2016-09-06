using UnityEngine;

public class SphereUVChange : MonoBehaviour
{
    public float uvMovespeed = 1f;
    float movementTimer;
    Mesh mesh;
    Vector2[] originalUVs;
    public Gradient gradient;
    Material material;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        material = GetComponent<MeshRenderer>().material;
        originalUVs = new Vector2[mesh.uv.Length];
        for (int i = 0; i < mesh.uv.Length; i++)
        {
            originalUVs[i] = mesh.uv[i];
        }
    }

    void OnEnable()
    {
        movementTimer = 0;
    }

    void Update()
    {
        movementTimer += uvMovespeed * Time.deltaTime;
        if (movementTimer > 0.45f)
            gameObject.SetActive(false); //This is the death of the object. Should be in an object pool so this is fine.

        var uvs = new Vector2[mesh.uv.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2
                (originalUVs[i].x, originalUVs[i].y + movementTimer);
        }

        material.color = gradient.Evaluate(movementTimer * 2);
        mesh.uv = uvs;
    }
}