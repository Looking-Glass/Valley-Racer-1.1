using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class HeightmapConverter : MonoBehaviour
{
    public Texture2D heightmap;
    public GMesh gmesh;
    public GMesh colliderGMesh;
    public Vector2 coords;
    public int drawDistanceWidth;
    public int drawDistanceHeight;
    public int colliderSize;
    public ObjectPool colliderPool;
    public int anchorOffsetY;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    void Awake()
    {
        gmesh = new GMesh();
        colliderGMesh = new GMesh();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter.sharedMesh = new Mesh();
        meshCollider.sharedMesh = new Mesh();
        InitiateMesh();
        MakePeaks();
    }

    void Update()
    {
        //Movement for testing
        /*
        if (ValleyInput.GetAxis().sqrMagnitude > 0f)
        {
            transform.Translate(new Vector3(ValleyInput.GetAxis().x, 0, ValleyInput.GetAxis().y) * Time.deltaTime * 2f);
        }
        */

        //Watch Movement each frame
        WatchMovement();
    }

    void WatchMovement()
    {
        if (Mathf.Abs(transform.localPosition.x) > 1f || Mathf.Abs(transform.localPosition.z) > 1f)
        {
            if (Mathf.Abs(transform.localPosition.x) > 1f)
            {
                coords = coords.SetX(coords.x - Mathf.Sign(transform.localPosition.x));
                transform.localPosition = transform.localPosition.SetX(transform.localPosition.x - Mathf.Sign(transform.localPosition.x));
            }

            if (Mathf.Abs(transform.localPosition.z) > 1f)
            {
                coords = coords.SetY(coords.y - Mathf.Sign(transform.localPosition.z));
                transform.localPosition = transform.localPosition.SetZ(transform.localPosition.z - Mathf.Sign(transform.localPosition.z));
            }

            MakePeaks();
        }
    }

    void InitiateMesh()
    {
        for (int i = 0; i < drawDistanceHeight; i++)
        {
            for (int j = 0; j < drawDistanceWidth; j++)
            {
                gmesh.verts.Add(Vector3.zero);

                gmesh.colors.Add(Color.white);

                for (int k = 0; k < 6; k++)
                {
                    gmesh.tris.Add(0);
                }

                var uv = new Vector2((float)j / (drawDistanceWidth - 1), (float)i / (drawDistanceHeight - 1));
                gmesh.uvs.Add(uv);

                //ground collider mesh
                if (Mathf.Abs(j - drawDistanceWidth / 2) < colliderSize &&
                    Mathf.Abs(i - drawDistanceHeight / 2) < colliderSize)
                {
                    colliderGMesh.verts.Add(Vector3.zero);

                    for (int k = 0; k < 6; k++)
                    {
                        colliderGMesh.tris.Add(0);
                    }
                }
            }
        }
        gmesh.Apply(meshFilter.sharedMesh);
    }

    void MakePeaks()
    {
        var w = heightmap.width;
        var h = heightmap.height;
        var peakHeight = 2f;
        var colliderVertIndex = 0;

        //Reset the peak colliders
        for (int i = 0; i < colliderPool.pool.Count; i++)
        {
            colliderPool.pool[i].SetActive(false);
        }

        for (int i = 0; i < drawDistanceHeight; i++)
        {
            for (int j = 0; j < drawDistanceWidth; j++)
            {
                var x = (int)(j + coords.x) % w;
                var y = (int)(i + coords.y) % h;
                var x1 = (int)(j - 1 + coords.x) % w;
                var y1 = (int)(i - 1 + coords.y) % h;
                var px = j + coords.x;
                var py = i + coords.y;

                var centerX = drawDistanceWidth / 2;
                var centerY = drawDistanceHeight / 2;

                var num = i * drawDistanceWidth + j;

                var pixel = heightmap.GetPixel(x, y);
                var newPeakHeight = Mathf.PerlinNoise(px * 0.5f, py * 0.5f) + peakHeight;

                var height = Mathf.PerlinNoise(px * 0.1f, py * 0.1f) - 0.5f;
                height += Mathf.PerlinNoise(px * 0.01f, py * 0.01f) - 0.5f;
                height += (Mathf.PerlinNoise(px * 0.5f, py * 0.5f) - 0.5f) * 0.3f;
                height += pixel.g * newPeakHeight;

                gmesh.verts[num] = new Vector3(j - centerX, height, i - centerY);

                var vertexColor = new Color(1, 0, 0.5f);
                gmesh.colors[num] = vertexColor;

                //If the vert is not one of the first (0 for i or j), make triangles for it.
                if (i > 0 && j > 0)
                {
                    var vert00 = (i - 1) * drawDistanceWidth + (j - 1);
                    var vert01 = i * drawDistanceWidth + (j - 1);
                    var vert11 = i * drawDistanceWidth + j;
                    var vert10 = (i - 1) * drawDistanceWidth + j;

                    //If this is a peak (vert11) or if there is one to the bottom left (vert00), draw the triangles nicer by rotating their creation order
                    if (pixel.g > 0 || heightmap.GetPixel(x1, y1).g > 0)
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

                    //Peak and ground colliders
                    if (Mathf.Abs(j - drawDistanceWidth / 2) < colliderSize &&
                        Mathf.Abs((i + anchorOffsetY) - drawDistanceHeight / 2) < colliderSize)
                    {
                        //Peaks
                        if (pixel.g > 0)
                        {
                            var obj = colliderPool.ActivateObject();
                            if (obj != null)
                                obj.transform.position = gmesh.verts[num] - Vector3.up * newPeakHeight + transform.position;
                        }

                        //Ground
                        colliderGMesh.verts[colliderVertIndex] = gmesh.verts[num] - Vector3.up * pixel.g * newPeakHeight;

                        var cw = colliderSize * 2 - 1;
                        if (colliderVertIndex > cw && colliderVertIndex % cw > 0)
                        {
                            colliderGMesh.tris[colliderVertIndex * 6 + 0] = colliderVertIndex - 1 - cw;
                            colliderGMesh.tris[colliderVertIndex * 6 + 1] = colliderVertIndex - 1;
                            colliderGMesh.tris[colliderVertIndex * 6 + 2] = colliderVertIndex;

                            colliderGMesh.tris[colliderVertIndex * 6 + 3] = colliderVertIndex;
                            colliderGMesh.tris[colliderVertIndex * 6 + 4] = colliderVertIndex - cw;
                            colliderGMesh.tris[colliderVertIndex * 6 + 5] = colliderVertIndex - 1 - cw;
                        }

                        colliderVertIndex++;
                    }
                }
            }
        }
        gmesh.Apply(meshFilter.sharedMesh);
        colliderGMesh.Apply(meshCollider.sharedMesh);

        //This is a stupid hack but it's proven necessary
        meshCollider.enabled = false;
        meshCollider.enabled = true;
    }
}
