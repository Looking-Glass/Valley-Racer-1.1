using UnityEngine;

public class PerlinTexture : MonoBehaviour
{
    public int texSize = 128;
    Material material;
    Texture2D tex2D;
    public float perlinSize = 10f;
    public float perlinSpeed = 0.1f;
    [Range(1, 8)]
    public int perlinOverlapCount = 1;
    float perlinDistance = 1f;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        tex2D = new Texture2D(texSize, texSize);
        material.mainTexture = tex2D;

        UpdatePerlin();

        //Important but easy to forget
        tex2D.Apply();
    }

    void Update()
    {
        UpdatePerlin();
    }

    void UpdatePerlin()
    {
        for (int i = 0; i < tex2D.height; i++)
        {
            for (int j = 0; j < tex2D.width; j++)
            {

                for (int k = 0; k <= perlinOverlapCount; k++)
                {
                    
                }

                var x = (float)j / tex2D.width;
                var y = (float)i / tex2D.width;

                var perlinXY1 = new Vector2((perlinDistance + x) * perlinSize, y * perlinSize);
                var perlinXY2 = new Vector2((-perlinDistance + x) * perlinSize, y * perlinSize);

                var perlin1 = Mathf.PerlinNoise(perlinXY1.x, perlinXY1.y);
                var perlin2 = Mathf.PerlinNoise(perlinXY2.x, perlinXY2.y);

                var perlinAvg = Mathf.Lerp(perlin1, perlin2, 0.5f);

                var color = new Color(perlinAvg, perlinAvg, perlinAvg);
                tex2D.SetPixel(j, i, color);
            }
        }

        tex2D.Apply();

        perlinDistance += perlinSpeed * Time.deltaTime;
    }
}
