using UnityEngine;


/// <summary>
/// For testing purposes only.
/// </summary>
public class EventPublisherExample : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Arbitrarily chose to test with the playerDeath event.
            Debug.Log("playerDeath() fired from " + name);
            EventManager.playerDeath();
        }
    }
}
