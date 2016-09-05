using UnityEngine;

public class SpriteBlink : MonoBehaviour
{
    public SpriteRenderer sr;
    public float blinkInterval = 0.5f;
    float timer;

    void Start()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (sr == null)
            return;

        timer += Time.deltaTime;
        if (timer > blinkInterval)
        {
            sr.enabled = !sr.enabled;
            timer = 0;
        }
    }
}
