using UnityEngine;

public class EventSubscriber : MonoBehaviour
{
    EventManager em;

    void Start()
    {
        em = FindObjectOfType<EventManager>();
        em.playerDeath += PlayerDied;
    }

    void PlayerDied()
    {
        Debug.Log("player died fired in " + name);
    }

    void OnDestroy()
    {
        //Needed because otherwise Garbage Collector will see EM still referencing this
        //And it will never truly be destroyed/recycled
        em.playerDeath -= PlayerDied;
    }
}