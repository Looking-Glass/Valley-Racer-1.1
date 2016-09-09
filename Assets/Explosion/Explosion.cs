using UnityEngine;

public class Explosion : MonoBehaviour
{
    public KeyCode explodeKey = KeyCode.K;
    public GameObject explosionController;

    void Start()
    {
        //EventManager.playerDeath += Explode;
    }

    void Update()
    {
        if (Input.GetKeyDown(explodeKey))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (explosionController.activeSelf)
            explosionController.SetActive(false);
        explosionController.SetActive(true);

        var audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.Play();
    }

    void OnDestroy()
    {
        //EventManager.playerDeath -= Explode;
    }
}
