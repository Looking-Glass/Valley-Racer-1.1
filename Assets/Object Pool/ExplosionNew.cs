using System.Collections;
using UnityEngine;

public class ExplosionNew : MonoBehaviour
{
    float timer;
    public float interval = 0.03f;
    public float lifetime = 4f;
    public KeyCode explodeKey = KeyCode.K;
    public ObjectPool explosionPool;
    public float spawnRadius = 1f;
    public float spawnSizeMinimum = 1f;
    public float spawnSizeMaximum = 2f;

    void Update()
    {
        if (Input.GetKeyDown(explodeKey))
        {
            StartCoroutine(Explode());
            StartCoroutine(ExplosionLifetime());
        }
    }

    IEnumerator Explode()
    {
        while (true)
        {
            var explosion = explosionPool.ActivateObject();
            explosion.transform.localEulerAngles = new Vector3
                (Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            explosion.transform.localPosition = new Vector3
                (Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            explosion.transform.localScale = Vector3.one * Random.Range(spawnSizeMinimum, spawnSizeMaximum);

            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator ExplosionLifetime()
    {
        yield return new WaitForSeconds(lifetime);

        StopCoroutine(Explode());
    }
}
