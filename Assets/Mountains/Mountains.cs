using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Mountains : MonoBehaviour
{
    public Texture2D heightmap;
    public GMesh gmesh;
    public GMesh colliderGMesh;
    public Vector2 imgCoords;
    public Vector2 perlinCoords;
    public int drawDistanceWidth;
    public int drawDistanceHeight;
    public int colliderSize;
    public ObjectPool colliderPool;
    public int anchorOffsetY;
    public bool keyboardControls; //for testing
    public Vector3 selfMovement; //for title
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
        if (keyboardControls && ValleyInput.GetAxis().sqrMagnitude > 0f)
        {
            transform.Translate(new Vector3(ValleyInput.GetAxis().x, 0, ValleyInput.GetAxis().y) * Time.deltaTime * 2f);
        }

        //Movement during title
        if (selfMovement != Vector3.zero)
        {
            transform.Translate(selfMovement * Time.deltaTime);
        }

        //Watch Movement each frame
        WatchMovement();
    }

    public void ReplaceHeightmap(Texture2D tex)
    {
        heightmap = tex;
        imgCoords = imgCoords.SetY(0);
    }

    void WatchMovement()
    {
        if (Mathf.Abs(transform.localPosition.x) > 1f || Mathf.Abs(transform.localPosition.z) > 1f)
        {
            if (Mathf.Abs(transform.localPosition.x) > 1f)
            {
                imgCoords = imgCoords.SetX(imgCoords.x - Mathf.Sign(transform.localPosition.x));
                perlinCoords = perlinCoords.SetX(perlinCoords.x - Mathf.Sign(transform.localPosition.x));
                transform.localPosition = transform.localPosition.SetX(transform.localPosition.x - Mathf.Sign(transform.localPosition.x));
            }

            if (Mathf.Abs(transform.localPosition.z) > 1f)
            {
                imgCoords = imgCoords.SetY(imgCoords.y - Mathf.Sign(transform.localPosition.z));
                perlinCoords = perlinCoords.SetY(perlinCoords.y - Mathf.Sign(transform.localPosition.z));
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
                for (int k = 0; k < 4; k++)
                {
                    gmesh.verts.Add(Vector3.zero);
                    gmesh.uvs.Add(Vector2.zero);
                }

                for (int k = 0; k < 6; k++)
                {
                    gmesh.tris.Add(0);
                }

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
                var x = (int)(j + imgCoords.x) % w;
                var y = (int)(i + imgCoords.y) % h;
                var px = j + perlinCoords.x;
                var py = i + perlinCoords.y;

                var centerX = drawDistanceWidth / 2;
                var centerY = drawDistanceHeight / 2;

                var num = i * drawDistanceWidth + j;

                //Save the pixel color
                Color[] pixels = new Color[4];
                pixels[0] = heightmap.GetPixel(x, y);

                //Get surrounding pixel colors (for uv setting later)
                pixels[1] = heightmap.GetPixel(x - 1, y);
                pixels[2] = heightmap.GetPixel(x - 1, y - 1);
                pixels[3] = heightmap.GetPixel(x, y - 1);

                //Save the height of the peak here for later 
                var newPeakHeight = Mathf.PerlinNoise(px * 0.5f, py * 0.5f) + peakHeight;
                var height = Mathf.PerlinNoise(px * 0.1f, py * 0.1f) - 0.5f;
                height += Mathf.PerlinNoise(px * 0.01f, py * 0.01f) - 0.5f;
                height += (Mathf.PerlinNoise(px * 0.5f, py * 0.5f) - 0.5f) * 0.3f;
                height += pixels[0].g * newPeakHeight;

                //Check if any surrounding pixels are green, this determines where the UV goes
                var uvShift = 0f;
                for (int k = 0; k < pixels.Length; k++)
                {
                    if (pixels[k].g > 0)
                        uvShift = 0.5f;
                }

                gmesh.verts[num * 4] = new Vector3(j - centerX, height, i - centerY);
                gmesh.uvs[num * 4] = new Vector2(0.5f + uvShift, 1f);

                if (i > 0 && j > 0)
                {
                    gmesh.verts[num * 4 - 1] = gmesh.verts[((i - 1) * drawDistanceWidth + j) * 4];
                    gmesh.verts[num * 4 - 2] = gmesh.verts[((i - 1) * drawDistanceWidth + j - 1) * 4];
                    gmesh.verts[num * 4 - 3] = gmesh.verts[(i * drawDistanceWidth + j - 1) * 4];

                    gmesh.uvs[num * 4 - 1] = new Vector2(0.5f + uvShift, 0f);
                    gmesh.uvs[num * 4 - 2] = new Vector2(0f + uvShift, 0f);
                    gmesh.uvs[num * 4 - 3] = new Vector2(0f + uvShift, 1f);

                    //Tris
                    var vert00 = num * 4;
                    var vert01 = num * 4 - 1;
                    var vert11 = num * 4 - 2;
                    var vert10 = num * 4 - 3;

                    
                    //If this is a peak or if there is one to the bottom left, draw the triangles nicer by rotating their creation order
                    if (pixels[0].g > 0 || pixels[2].g > 0)
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
                
                //Peak and ground colliders
                if (Mathf.Abs(j - drawDistanceWidth / 2) < colliderSize &&
                    Mathf.Abs((i + anchorOffsetY) - drawDistanceHeight / 2) < colliderSize)
                {
                    //Peaks
                    if (pixels[0].g > 0)
                    {
                        var obj = colliderPool.ActivateObject();
                        if (obj != null)
                            obj.transform.position = gmesh.verts[num * 4] - Vector3.up * newPeakHeight + transform.position;
                    }

                    //Ground
                    colliderGMesh.verts[colliderVertIndex] = gmesh.verts[num * 4] - Vector3.up * pixels[0].g * newPeakHeight;

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
        gmesh.Apply(meshFilter.sharedMesh);
        colliderGMesh.Apply(meshCollider.sharedMesh);

        //This is a stupid hack but it's proven necessary
        meshCollider.enabled = false;
        meshCollider.enabled = true;
    }
}
