using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartSceneController : MonoBehaviour
{

    public KeyCode key = KeyCode.Space;
    bool resetNextFrame;
    float timer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            if (resetNextFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyUp(key) || Input.GetKeyUp(KeyCode.R))
            {
                GameObject[] mtnBars = GameObject.FindGameObjectsWithTag("MountainBars");
                for (var i = 0; i < mtnBars.Length; i++)
                {
                    mtnBars[i].SetActive(false);
                }
                resetNextFrame = true;
            }
        }
    }
}
