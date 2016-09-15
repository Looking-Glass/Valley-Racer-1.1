using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GMesh
{
    public List<Vector3> verts;
    public List<int> tris;
    public List<Vector2> uvs;

    public Mesh mesh;
    public Texture2D tex;

    public Material mat;
    public MeshFilter mf;
    public MeshRenderer mr;


    /// <summary>
    /// Have to feed it a reference to this gameObject.
    /// </summary>
    public GMesh(GameObject gameObject)
    {
        if (mf == null)
            mf = gameObject.GetComponent<MeshFilter>();

        if (mr == null)
            mr = gameObject.GetComponent<MeshRenderer>();


        if (mesh == null)
        {
            verts = new List<Vector3>();
            tris = new List<int>();
            uvs = new List<Vector2>();

            mesh = new Mesh();
        }
        else
        {
            verts = mesh.vertices.ToList();
            tris = mesh.triangles.ToList();
            uvs = mesh.uv.ToList();
        }
    }

    public void Apply()
    {
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mf.mesh = mesh;
    }
}
