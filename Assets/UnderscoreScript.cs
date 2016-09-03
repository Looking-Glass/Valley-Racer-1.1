using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class UnderscoreScript : MonoBehaviour
{

    public Text initialsText;

    void Update()
    {
        
        print("preferred width " + initialsText.preferredWidth);
        print("xpos " + GetComponent<RectTransform>().anchoredPosition.x);

        var newWidth = initialsText.preferredWidth;
        var rekt = initialsText.GetComponent<RectTransform>();
        
        rekt.anchoredPosition = new Vector2(960 - (newWidth/2), rekt.anchoredPosition.y);
        

        var rectTrans = GetComponent<RectTransform>();
        rectTrans.anchoredPosition = new Vector2(-960 + initialsText.preferredWidth, rectTrans.anchoredPosition.y);
    }
}
