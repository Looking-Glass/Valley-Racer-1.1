using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SerialPressEnter : MonoBehaviour
{
    public float timeBuffer = 2f;
    float timer;
    AsyncOperation async;
    public SpriteBlink spriteBlink;
    bool startQueued;

    void Start()
    {
        spriteBlink = GetComponent<SpriteBlink>();
        StartCoroutine(load());
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBuffer)
        {
            if (ValleyInput.GetEnterButtonDown() && !startQueued)
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
                        StartCoroutine(Confirm());
                    }
                }
                else
                {
                    StartCoroutine(Confirm());
                }
                startQueued = true;
            }
        }
    }

    IEnumerator Confirm()
    {
        spriteBlink.blinkInterval = 0.01f;
        yield return new WaitForSeconds(1.5f);
        if (EventManager.playerPressedStart != null)
            EventManager.playerPressedStart();
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
}
