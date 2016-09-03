using UnityEngine;

public class Explosion : MonoBehaviour
{
    float timer;
    public float lifetime = 0.1f;
    SpriteRenderer sr;
    AudioSource audioSource;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifetime)
            sr.enabled = false;
    }

    public void Explode()
    {
        timer = 0;
        sr.enabled = true;
        audioSource.Play();
    }
}
