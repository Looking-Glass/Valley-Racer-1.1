using UnityEngine;

public class Explosion : MonoBehaviour
{
    public KeyCode explodeKey = KeyCode.K;
    public GameObject explosionController;
    EventManager em;

    void Start()
    {
        em = FindObjectOfType<EventManager>();
        em.playerDeath += Explode;
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
    }

    void OnDestroy()
    {
        em.playerDeath -= Explode;
    }
}
