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
        if (timer > littleBuffer) //this is just a little buffer
            if (ValleyInput.GetEnterButtonDown())
                ToggleScene();
    }

    void OnEnable()
    {
        timer = 0;
    }

    public void ToggleScene()
    {
        SceneManager.LoadScene(sceneToGo);
    }
}
