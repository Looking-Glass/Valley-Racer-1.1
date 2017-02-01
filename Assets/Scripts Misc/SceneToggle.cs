using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToggle : MonoBehaviour
{
    float timer;
    public float littleBuffer = 1f;
    public int sceneToGo;

    void Update()
    {
        timer += Time.deltaTime;
        if (ValleyInput.GetEnterButtonDown())
            ToggleScene();
    }

    void OnEnable()
    {
        timer = 0;
    }

    public void ToggleScene()
    {
        if (timer > littleBuffer) //this is just a little buffer
            SceneManager.LoadScene(sceneToGo);
    }
}
