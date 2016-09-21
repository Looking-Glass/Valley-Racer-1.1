using UnityEngine;

public class GridTexture : MonoBehaviour
{
    public int lineWidth;
    public int texSize = 128;
    public Color bgColor = Color.black;
    public Color lineColor = Color.white;
    public Texture2D tex;

    public void MakeTexture()
    {
        tex = new Texture2D(texSize, texSize);

        for (int i = 0; i < texSize; i++)
        {
            for (int j = 0; j < texSize; j++)
            {
                var color = lineColor;

                var lineBotLeft = 0;
                var lineTopRight = 0;

                for (int k = 0; k < lineWidth; k++)
                {
                    if (k % 2 == 0)
                        lineBotLeft += 1;
                    else
                        lineTopRight += 1;
                }

                if (i >= lineBotLeft && j >= lineBotLeft && i < texSize - lineTopRight && j < texSize - lineTopRight)
                    color = bgColor;

                tex.SetPixel(j, i, color);
            }
        }

        tex.Apply();
    }
}
