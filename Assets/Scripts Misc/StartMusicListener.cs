using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMusicListener : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource audioSource2;

    void Start()
    {
        EventManager.gameStart += OnGameStart;

    }

    void Update()
    {
        //if it's the intro scene, stop the biking music
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            audioSource.Stop();
            audioSource2.Stop();
        }
    }

    void OnGameStart()
    {
        audioSource.Play();
        audioSource2.PlayScheduled(AudioSettings.dspTime + audioSource.clip.length);
    }

    void OnDestroy()
    {
        EventManager.gameStart -= OnGameStart;
    }
}
