using UnityEngine;

public class EventSubscriber : MonoBehaviour
{
    void Start()
    {
        EventManager.playerDeath += PlayerDied;
    }

    void PlayerDied()
    {
        Debug.Log("player died fired in " + name);
    }

    void OnDestroy()
    {
        //Needed because otherwise Garbage Collector will see EM still referencing this
        //And it will never truly be destroyed/recycled
        EventManager.playerDeath -= PlayerDied;
    }
}