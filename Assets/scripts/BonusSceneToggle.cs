using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BonusSceneToggle : MonoBehaviour
{
    private float timer;
    public LeapMotoControls leapMotoControls;
    public int sceneToGo;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f) //this is just a little buffer so that people can't reset the scene 20 thousand times in 1 second.
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button16) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                ToggleScene();
            }

            if (leapMotoControls != null) leapMotoControls.selfCheck = true;
        }
    }

    public void ToggleScene()
    {
        SceneManager.LoadScene(sceneToGo);
    }
}
