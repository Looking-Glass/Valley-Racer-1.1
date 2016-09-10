using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressEnter : MonoBehaviour
{
    public float timeBuffer = 2f;
    float timer;
    AsyncOperation async;

    void Start()
    {
        StartCoroutine(load());
    }

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
                        BlinkTextConfirm();
                    }
                }
                else
                {
                    BlinkTextConfirm();
                }
            }
        }
    }

    void BlinkTextConfirm()
    {
        if (EventManager.gameStart != null)
            EventManager.gameStart();
        async.allowSceneActivation = true;
    }

    IEnumerator load()
    {
        Debug.LogWarning("ASYNC LOAD STARTED - " +
           "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH " + Time.time);
        async = SceneManager.LoadSceneAsync(2);
        async.allowSceneActivation = false;
        yield return async;
        Debug.Log("Level done loading " + Time.time);
    }

    public void ActivateScene()
    {
        async.allowSceneActivation = true;
    }
}
