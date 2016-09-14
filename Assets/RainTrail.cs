using UnityEngine;
using System.Collections;

public class RainTrail : MonoBehaviour
{
    public float dropInterval = 0.01f;
    public float dropLifetime = 0.3f;
    public ObjectPool dropPool;
    public Transform rainBoundsTransform;
    Bounds rainBounds;
    public float fallSpeed = 10f;
    public MotoController motoController;
    SpriteRenderer sr;

    void Start()
    {
        rainBounds = new Bounds(rainBoundsTransform.position, rainBoundsTransform.lossyScale);
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Spawn(false));
        //StartCoroutine(PutDrop());
    }

    void Update()
    {
        var vel = Vector3.down * fallSpeed * Time.deltaTime;
        if (motoController.enabled)
            vel += motoController.GetVelocity();

        transform.Translate(vel, Space.World);

        transform.LookAt(transform.position + vel, Vector3.up);
        transform.Rotate(new Vector3(90, 0, 90));

        if (transform.position.y < rainBounds.min.y)
        {
            sr.enabled = false;
            StartCoroutine(Spawn());
        }

        //Left and right repeating
        if (transform.position.x > rainBounds.max.x)
            transform.Translate(Vector3.left * rainBounds.size.x);
        if (transform.position.x < rainBounds.min.x)
            transform.Translate(Vector3.right * rainBounds.size.x);

        //Forward repeating
        if (transform.position.z < rainBounds.min.z)
            transform.Translate(Vector3.forward * rainBounds.size.z);
    }

    IEnumerator Spawn(bool respawn = true)
    {
        yield return new WaitForSeconds(Random.value * 5f);
        sr.enabled = true;
        transform.position = new Vector3(
            Random.Range(rainBounds.min.x, rainBounds.max.x),
            respawn ? rainBounds.max.y : Random.Range(rainBounds.min.y, rainBounds.max.y),
            Random.Range(rainBounds.min.z, rainBounds.max.z)
            );
    }

    IEnumerator PutDrop()
    {
        while (true)
        {
            var newDrop = dropPool.ActivateObject();
            newDrop.transform.position = transform.position;
            StartCoroutine(DeactivateDrop(newDrop));
            yield return new WaitForSeconds(dropInterval);
        }
    }

    IEnumerator DeactivateDrop(GameObject drop)
    {
        yield return new WaitForSeconds(dropLifetime);
        drop.SetActive(false);
    }
}
