using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusSceneToggle : MonoBehaviour
{
    private float timer;
    public float littleBuffer = 1f;
    public int sceneToGo;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > littleBuffer) //this is just a little buffer
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button16) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                ToggleScene();
    }

    public void ToggleScene()
    {
        SceneManager.LoadScene(sceneToGo);
    }
}
