using UnityEngine;

public class Explosion : MonoBehaviour
{
    public KeyCode explodeKey = KeyCode.K;
    public GameObject explosionController;

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
}
