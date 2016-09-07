using UnityEngine;

public class Explosion : MonoBehaviour
{
    public KeyCode explodeKey = KeyCode.K;
    public GameObject explosionController;
    EventManager em;

    void Start()
    {
        em = FindObjectOfType<EventManager>();
        Debug.Log(em + " found by! " + name);
        EventManager.playerDeath += Explode;
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
        EventManager.playerDeath -= Explode;
    }
}
