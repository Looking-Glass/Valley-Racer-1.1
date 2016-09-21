using UnityEngine;

public class PerlinTexture : MonoBehaviour
{
    public float perlinSize = 10f;
    public float perlinSpeed = 0.1f;
    float perlinDistance = 1f;

    void Update()
    {
        perlinDistance += perlinSpeed * Time.deltaTime;
    }

    public float GetPerlin(int x, int y, int texSize)
    {
        var x1 = (float)x / texSize;
        var y1 = (float)y / texSize;

        var perlinXY1 = new Vector2((perlinDistance + x1) * perlinSize, y1 * perlinSize);
        var perlinXY2 = new Vector2((-perlinDistance + x1) * perlinSize, y1 * perlinSize);

        var perlin1 = Mathf.PerlinNoise(perlinXY1.x, perlinXY1.y);
        var perlin2 = Mathf.PerlinNoise(perlinXY2.x, perlinXY2.y);

        var perlinAvg = Mathf.Lerp(perlin1, perlin2, 0.5f);

        return perlinAvg;
    }
}
