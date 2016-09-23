using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioMixerMuteButton : MonoBehaviour
{
    public AudioMixer audioMixer;
    public int volumeState;
    public GameObject muteGraphic;
    public TextMesh muteText;

    void Update()
    {
        if (ValleyInput.GetMuteButtonDown())
        {
            var vol = volumeState + 1;
            if (vol > 2)
                vol = 0;
            volumeState = vol;

            audioMixer.SetFloat("Master Volume", volumeState < 2 ? 0f : -80f);
            audioMixer.SetFloat("Music Volume", volumeState < 1 ? 0f : -80f);
            
            muteText.text = volumeState == 1 ? "Mute Music" : "Mute All";
        }

        //Only show it in the title scene
        muteGraphic.SetActive(volumeState > 0 && SceneManager.GetActiveScene().buildIndex == 1);
    }
}