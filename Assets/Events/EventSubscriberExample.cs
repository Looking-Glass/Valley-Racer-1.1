using UnityEngine;

/// <summary>
/// For testing purposes only.
/// </summary>
public class EventSubscriberExample : MonoBehaviour
{
    void Start()
    {
        EventManager.playerDeath += PlayerDied;
    }

    void PlayerDied()
    {
        Debug.Log("playerDeath() recieved in " + name);
    }

    void OnDestroy()
    {
        //Needed because otherwise Garbage Collector will see EM still referencing this
        //And it will never truly be destroyed/recycled
        EventManager.playerDeath -= PlayerDied;
    }
}