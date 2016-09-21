using UnityEngine;

public class GradientTexture : MonoBehaviour
{
    public int texSize = 128;
    Material material;
    Texture2D tex2D;
    

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        tex2D = new Texture2D(texSize, texSize);
        material.mainTexture = tex2D;

        for (int i = 0; i < tex2D.height; i++)
        {
            for (int j = 0; j < tex2D.width; j++)
            {
                var color = new Color(
                    (float)j / (tex2D.width - 1), 
                    (float)i / (tex2D.height - 1), 
                    0);
                tex2D.SetPixel(j, i, color);
            }
        }

        //Important but easy to forget
        tex2D.Apply();
    }
}
