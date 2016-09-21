using System.Collections;
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
        StartCoroutine(ToggleBlink());
    }

    IEnumerator ToggleBlink()
    {
        while (enabled)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
