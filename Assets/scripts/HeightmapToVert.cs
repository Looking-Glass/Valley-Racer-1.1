using UnityEngine;

public class HeightmapToVert : MonoBehaviour
{
    public Texture2D peakTexture;
    public Texture2D hillTexture;
    public float maxHeight = 2;
    public float bumpinessLevel = 0.3f;
    public GameObject peak;
    public float percentRandom;
    MeshFilter meshFilter;
    Mesh mesh;
    Mesh bumpyMesh;
    MeshCollider meshCollider;
    Material material;
    public Material volumeMaterial;
    public bool makeVolumetricExperimental;
    Color mtnColor = Color.black;

    public void SetupMountains()
    {

        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        material = GetComponent<MeshRenderer>().material;
        mesh = new Mesh();

        //adding a texture
        Texture2D mtnTexture = new Texture2D(256, 256);

        //read the texture and set the mesh based on it
        int w = peakTexture.width;
        int h = peakTexture.height;
        int w1 = w + 1; //for alignment
        Vector3[] verts = new Vector3[w1 * h];
        bool[] peaks = new bool[verts.Length];
        Vector2[] uvs = new Vector2[verts.Length];



        //create vertices
        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w1; x++)
            {
                float heightColor;
                if (x != w)
                {
                    heightColor = hillTexture.GetPixels()[(y * w) + x].r;
                }
                else
                {
                    heightColor = hillTexture.GetPixels()[(y * w) + 0].r;
                }
                verts[(y * w1) + x] = new Vector3(x, heightColor * maxHeight, y);
                uvs[(y * w1) + x] = new Vector2((float)x / (w1 - 1), (float)y / (h - 1));

                if (y > 0)
                { //don't draw peaks at y = 0. it is just annoying. invisible peaks show up.
                    float peakColor;
                    if (x != w)
                    {
                        peakColor = peakTexture.GetPixels()[(y * w) + x].g;
                    }
                    else
                    {
                        peakColor = peakTexture.GetPixels()[(y * w) + 0].g;
                    }
                    bool randomPeak = Random.value < percentRandom && y != 0 && y != h - 1;

                    if (peakColor > 0 || randomPeak)
                    {
                        if (x != 0)
                        { //so that there's only one peak per peak
                            GameObject newPeak = Instantiate(peak);
                            newPeak.transform.position = verts[(y * w1) + x];
                            newPeak.transform.parent = transform;
                            if (randomPeak)
                            {
                                newPeak.name = "RANDOM PEAK";
                            }
                        }
                        peaks[(y * w1) + x] = true;
                    }
                }
            }
        }
        mesh.vertices = verts;

        //colors
        var newMtnPixels = mtnTexture.GetPixels();
        for (var i = 0; i < newMtnPixels.Length; i++)
        {
            newMtnPixels[i] = mtnColor;
        }
        mtnTexture.SetPixels(newMtnPixels);

        //create triangles
        int index = 0;
        var triangs = new int[(w1 - 1) * (h - 1) * 6];
        for (var y = 0; y < h - 1; y++)
        {
            for (var x = 0; x < w1 - 1; x++)
            {
                if (peaks[((y + 1) * w1) + x] || peaks[(y * w1) + x + 1])
                {
                    //if there's a peak right above it or directly right
                    triangs[index++] = (y * w1) + x;
                    triangs[index++] = ((y + 1) * w1) + x;
                    triangs[index++] = ((y + 1) * w1) + x + 1;

                    triangs[index++] = ((y + 1) * w1) + x + 1;
                    triangs[index++] = (y * w1) + x + 1;
                    triangs[index++] = (y * w1) + x;
                }
                else
                {
                    //draw normal
                    triangs[index++] = (y * w1) + x;
                    triangs[index++] = ((y + 1) * w1) + x;
                    triangs[index++] = (y * w1) + x + 1;

                    triangs[index++] = ((y + 1) * w1) + x;
                    triangs[index++] = ((y + 1) * w1) + x + 1;
                    triangs[index++] = (y * w1) + x + 1;
                }
            }
        }
        mesh.triangles = triangs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshCollider.sharedMesh = mesh;

        //add a little gravelliness to help visibility of ground
        for (var i = 0; i < verts.Length; i++)
        {
            verts[i] += Vector3.up * Random.Range(-0.5f, 0.5f) * bumpinessLevel;
            if (peaks[i])
            {
                verts[i] += Vector3.up * Random.Range(1.5f, 2.75f);

                //color the peaks a special color
                Color peakBaseColor = Color.Lerp(mtnColor, Color.white, .33f);
                Color peakColor = Color.Lerp(peakBaseColor, Color.white, .33f);
                int peakRadius = mtnTexture.width / 32;
                int textureX = Mathf.RoundToInt(mtnTexture.width * uvs[i].x);
                int textureY = Mathf.RoundToInt(mtnTexture.height * uvs[i].y);

                //base coat
                for (var j = -peakRadius; j < peakRadius; j++)
                {
                    for (var k = -peakRadius; k < peakRadius; k++)
                    {
                        if (mtnTexture.GetPixel(textureX + j, textureY + k) != peakColor)
                        {
                            mtnTexture.SetPixel(textureX + j, textureY + k, peakBaseColor);
                        }
                    }
                }

                mtnTexture.SetPixel(Mathf.RoundToInt(mtnTexture.width * uvs[i].x), Mathf.RoundToInt(mtnTexture.height * uvs[i].y), peakColor);
                for (var j = 0; j < peakRadius; j++)
                {

                    //peaks in a diamond
                    mtnTexture.SetPixel(textureX + j, textureY, peakColor);
                    mtnTexture.SetPixel(textureX - j, textureY, peakColor);
                    mtnTexture.SetPixel(textureX, textureY + j, peakColor);
                    mtnTexture.SetPixel(textureX, textureY - j, peakColor);
                    for (var k = 1; k <= j; k++)
                    {
                        mtnTexture.SetPixel(textureX + k, textureY + j - k, peakColor);
                        mtnTexture.SetPixel(textureX - k, textureY + j - k, peakColor);
                        mtnTexture.SetPixel(textureX + k, textureY - j + k, peakColor);
                        mtnTexture.SetPixel(textureX - k, textureY - j + k, peakColor);
                    }
                }
            }
            if (i != 0 && (i + 1) % w1 == 0)
            { //this matches the verts along x = 0 to the ones at x = full width.
                verts[i - (w1 - 1)] = new Vector3(verts[i - (w1 - 1)].x, verts[i].y, verts[i - (w1 - 1)].z);
            }
        }

        bumpyMesh = new Mesh();
        bumpyMesh.bounds = mesh.bounds;
        bumpyMesh.vertices = verts;
        bumpyMesh.uv = uvs;
        bumpyMesh.triangles = mesh.triangles;
        bumpyMesh.RecalculateNormals();
        bumpyMesh.RecalculateBounds();
        meshFilter.mesh = bumpyMesh;

        //finish up texture changes.
        mtnTexture.Apply();
        material.mainTexture = mtnTexture;
        
        #region flat faces inside mountain (experimental, and garbage)
        if (makeVolumetricExperimental)
        {
            Vector3[] newVerts = new Vector3[verts.Length * 2];
            for (var i = 0; i < verts.Length; i++)
            {
                newVerts[i] = verts[i];
                newVerts[verts.Length + i] = verts[i] + Vector3.down * 20;
            }

            //now the triangles
            int[] newTriangs = new int[(newVerts.Length - 2) * 6];
            var newIndex = 0;
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    newTriangs[newIndex++] = (y * w1) + x;
                    newTriangs[newIndex++] = (y * w1) + (x + 1);
                    newTriangs[newIndex++] = (y * w1) + x + verts.Length;

                    newTriangs[newIndex++] = (y * w1) + (x + 1) + verts.Length;
                    newTriangs[newIndex++] = (y * w1) + x + verts.Length;
                    newTriangs[newIndex++] = (y * w1) + (x + 1);
                }
            }

            GameObject volumeChild = new GameObject("mtnVolume");
            volumeChild.transform.parent = transform;
            MeshFilter volumeMeshFilter = volumeChild.AddComponent<MeshFilter>();
            MeshRenderer volumeMeshRenderer = volumeChild.AddComponent<MeshRenderer>();
            Mesh volumeMesh = new Mesh();

            volumeMesh.vertices = newVerts;
            volumeMesh.triangles = newTriangs;
            volumeMesh.RecalculateBounds();
            volumeMesh.RecalculateNormals();
            volumeMeshFilter.mesh = volumeMesh;
            volumeMeshRenderer.material = volumeMaterial;
        }
    }
    #endregion
}
