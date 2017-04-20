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
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBuffer)
        {
            if (ValleyInput.GetEnterButtonDown() && !startQueued)
            {
                StartGame();
            }
        }
    }

    public void StartGame()
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

    IEnumerator Confirm()
    {
        spriteBlink.blinkInterval = 0.01f;
        yield return new WaitForSeconds(1.5f);
        if (EventManager.playerPressedStart != null)
            EventManager.playerPressedStart();
        SceneManager.LoadScene(2);
    }
    
}
