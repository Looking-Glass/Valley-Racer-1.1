using UnityEngine;

public class EscapeQuit : MonoBehaviour
{
    //elegant code
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
    }
}
