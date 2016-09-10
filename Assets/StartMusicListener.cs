using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMusicListener : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        EventManager.gameStart += OnGameStart;
    }

    void Update()
    {
        //if it's the intro scene, stop the biking music
        if (SceneManager.GetActiveScene().buildIndex == 1)
            audioSource.Stop();
    }

    void OnGameStart()
    {
        audioSource.Play();
    }

    void OnDestroy()
    {
        EventManager.gameStart -= OnGameStart;
    }
}
