using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HeightmapConverter : MonoBehaviour
{
    public Texture2D heightmap;
    public GMesh gmesh;
    public Vector2 coords;
    public int drawDistanceWidth;
    public int drawDistanceHeight;

    void Awake()
    {
        gmesh = new GMesh(gameObject);
        InitiateRangeMesh();
        MakePeaks();
    }

    void Update()
    {
        //Movement for testing
        if (ValleyInput.GetAxis().sqrMagnitude > 0f)
        {
            transform.Translate(new Vector3(ValleyInput.GetAxis().x, 0, ValleyInput.GetAxis().y) * Time.deltaTime * 10f);
        }

        //Watch Movement each frame
        WatchMovement();
    }

    void WatchMovement()
    {
        if (Mathf.Abs(transform.position.x) > 1f || Mathf.Abs(transform.position.z) > 1f)
        {
            if (Mathf.Abs(transform.position.x) > 1f)
            {
                coords = VectorEdit.SetX(coords, coords.x - Mathf.Sign(transform.position.x));
                transform.position = VectorEdit.SetX(transform.position,
                    transform.position.x - Mathf.Sign(transform.position.x));
            }

            if (Mathf.Abs(transform.position.z) > 1f)
            {
                coords = VectorEdit.SetY(coords, coords.y - Mathf.Sign(transform.position.z));
                transform.position = VectorEdit.SetZ(transform.position,
                    transform.position.z - Mathf.Sign(transform.position.z));
            }

            MakePeaks();
        }
    }

    void InitiateRangeMesh()
    {
        for (int i = 0; i < drawDistanceHeight; i++)
        {
            for (int j = 0; j < drawDistanceWidth; j++)
            {
                gmesh.verts.Add(Vector3.zero);

                for (int k = 0; k < 6; k++)
                {
                    gmesh.tris.Add(0);
                }
            }
        }
        gmesh.Apply();
    }

    void MakePeaks()
    {
        for (int i = 0; i < drawDistanceHeight; i++)
        {
            for (int j = 0; j < drawDistanceWidth; j++)
            {
                var x = (int)(j + coords.x) % heightmap.width;
                var y = (int)(i + coords.y) % heightmap.height;
                var x1 = (int)(j - 1 + coords.x) % heightmap.width;
                var y1 = (int)(i - 1 + coords.y) % heightmap.height;

                var centerX = drawDistanceWidth / 2;
                var centerY = drawDistanceHeight / 2;

                var num = i * drawDistanceWidth + j;

                var color = heightmap.GetPixel(x, y);

                var height = Mathf.PerlinNoise(x * 0.1f, y * 0.1f) - 0.5f;
                height += Mathf.PerlinNoise(x * 0.01f, y * 0.01f) - 0.5f;
                height += (Mathf.PerlinNoise(x * 0.5f, y * 0.5f) - 0.5f) * 0.3f;
                height += color.g * 2f;

                gmesh.verts[num] = new Vector3(j - centerX, height, i - centerY);

                //If the vert is not one of the first (0 for i or j), make triangles for it.
                if (i > 0 && j > 0)
                {
                    var vert00 = (i - 1) * drawDistanceWidth + (j - 1);
                    var vert01 = i * drawDistanceWidth + (j - 1);
                    var vert11 = i * drawDistanceWidth + j;
                    var vert10 = (i - 1) * drawDistanceWidth + j;

                    //If this is a peak (vert11) or if there is one to the bottom left (vert00), draw the triangles nicer by rotating their creation order
                    if (heightmap.GetPixel(x, y).g > 0 || heightmap.GetPixel(x1, y1).g > 0)
                    {
                        var vert00Copy = vert00;
                        vert00 = vert01;
                        vert01 = vert11;
                        vert11 = vert10;
                        vert10 = vert00Copy;
                    }

                    gmesh.tris[num * 6 + 0] = vert00;
                    gmesh.tris[num * 6 + 1] = vert01;
                    gmesh.tris[num * 6 + 2] = vert11;

                    gmesh.tris[num * 6 + 3] = vert11;
                    gmesh.tris[num * 6 + 4] = vert10;
                    gmesh.tris[num * 6 + 5] = vert00;
                }
            }
        }
        gmesh.Apply();
    }
}
