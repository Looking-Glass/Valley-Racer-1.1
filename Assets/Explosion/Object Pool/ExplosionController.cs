using System.Collections;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float interval = 0.01f;
    public float lifetime = 4f;
    public ObjectPool explosionPool;
    public float spawnRadius = 1f;
    public float spawnSizeMinimum = 1f;
    public float spawnSizeMaximum = 2f;
    public float liftup = 1f;
    float timer;

    void OnEnable()
    {
        if (explosionPool.pool.Count > 0) //for an elusive reason, doesn't work at first (even though the pool is initated in Awake)
            StartCoroutine(Explode());
        timer = 0;
        transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > lifetime)
            gameObject.SetActive(false);

        transform.Translate(Vector3.up * liftup * Time.deltaTime);
    }

    IEnumerator Explode()
    {
        while (true)
        {
            var explosion = explosionPool.ActivateObject();
            explosion.transform.localEulerAngles = new Vector3
                (Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            explosion.transform.position = new Vector3
                (Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            explosion.transform.position += transform.position;
            explosion.transform.localScale = Vector3.one * Random.Range(spawnSizeMinimum, spawnSizeMaximum);

            var lerp = Mathf.Clamp01((timer * 10 - 9) / lifetime);
            var intervalNew = Mathf.Lerp(interval, interval * 2f, lerp);

            yield return new WaitForSeconds(intervalNew);
        }
    }
}
