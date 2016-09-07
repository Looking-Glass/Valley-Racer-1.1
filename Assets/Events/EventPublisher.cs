using UnityEngine;

public class EventPublisher : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var em = FindObjectOfType<EventManager>();

            if (em != null && em.playerDeath != null)
                em.playerDeath();
        }
    }
}
