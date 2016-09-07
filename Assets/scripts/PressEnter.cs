using UnityEngine;
using UnityEngine.SceneManagement;

public class PressEnter : MonoBehaviour
{
    public float timeBuffer = 2f;
    float timer;
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBuffer)
        {
            if (ValleyInput.GetEnterButtonDown())
            {
                var serialManager = GameObject.FindWithTag("SerialManager");
                if (serialManager != null)
                {
                    if (serialManager.GetComponent<ValleySerialPolling>().credits > 0)
                    {
                        serialManager.GetComponent<ValleySerialPolling>().credits -= 1;
                        var sm = GameObject.FindWithTag("SerialManager");
                        var ac = sm.GetComponent<ValleySerialPolling>().sounds[Random.Range(1, 3)];
                        var asource = sm.GetComponent<AudioSource>();
                        asource.clip = ac;
                        asource.Play();
                        SceneManager.LoadScene(2);
                    }
                }
                else
                {
                    SceneManager.LoadScene(2);
                }
            }
        }
    }
}
