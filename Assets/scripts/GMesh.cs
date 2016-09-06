using UnityEngine;
using System.Collections.Generic;

public class GMesh
{
    public static void MakeTri
        (
        //required params
        Mesh mesh, Vector3 vert1, Vector3 vert2, Vector3 vert3, 

        //optional
        bool calculateBounds = true, bool recalculateNormals = true, int submesh = 0
        )
    {
        var verts = new List<Vector3>(mesh.vertices);
        var tris = new List<int>(mesh.triangles);
        var vert1int = verts.Count; //first vert

        verts.Add(vert1);
        verts.Add(vert2);
        verts.Add(vert3);

        tris.Add(vert1int);
        tris.Add(vert1int + 1);
        tris.Add(vert1int + 2);

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, submesh, calculateBounds);
        if (recalculateNormals)
            mesh.RecalculateNormals();
    }

    public static void MakeQuad
        (
        //required params
        Mesh mesh, Vector3 vert1, Vector3 vert2, Vector3 vert3, Vector3 vert4,

        //optional
        bool calculateBounds = true, bool recalculateNormals = true, int submesh = 0
        )
    {
        var verts = new List<Vector3>(mesh.vertices);
        var tris = new List<int>(mesh.triangles);
        var vert1int = verts.Count; //first vert

        verts.Add(vert1);
        verts.Add(vert2);
        verts.Add(vert3);
        verts.Add(vert4);

        tris.Add(vert1int);
        tris.Add(vert1int + 1);
        tris.Add(vert1int + 2);

        tris.Add(vert1int + 2);
        tris.Add(vert1int + 3);
        tris.Add(vert1int);

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, submesh, calculateBounds);
        if (recalculateNormals)
            mesh.RecalculateNormals();
    }
}
