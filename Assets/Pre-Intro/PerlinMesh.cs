using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PerlinMesh : MonoBehaviour
{
    Mesh mesh;
    MeshFilter mf;
    List<Vector3> verts;
    List<int> tris;
    List<Vector2> uvs;
    public int gridSize = 16;
    PerlinTexture perlinTexture;

    void Start()
    {
        perlinTexture = GetComponent<PerlinTexture>();

        mesh = new Mesh();
        verts = new List<Vector3>();
        tris = new List<int>();
        uvs = new List<Vector2>();

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                var x = (float)j / gridSize - 0.5f;
                var y = (float)i / gridSize - 0.5f;

                verts.Add(new Vector3(x, y));

                if (j > 0 && i > 0)
                {
                    tris.Add(j + i * gridSize);
                    tris.Add(j + (i - 1) * gridSize);
                    tris.Add(j - 1 + (i - 1) * gridSize);

                    tris.Add(j - 1 + (i - 1) * gridSize);
                    tris.Add(j - 1 + i * gridSize);
                    tris.Add(j + i * gridSize);
                }

                var uvX = j % 2;
                var uvY = i % 2;
                uvs.Add(new Vector2(uvX, uvY));
            }
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetUVs(0, uvs);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        mf = GetComponent<MeshFilter>();
        mf.mesh = mesh;

        var pg = GetComponent<GridTexture>();
        pg.MakeTexture();
        GetComponent<MeshRenderer>().material.mainTexture = pg.tex;
    }

    void Update()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                var vi = j + i * gridSize;
                var perlin = perlinTexture.GetPerlin(j, i, gridSize);
                verts[vi] = new Vector3(verts[vi].x, verts[vi].y, perlin);
            }
        }

        mesh.SetVertices(verts);
        mesh.RecalculateNormals();
    }
}
