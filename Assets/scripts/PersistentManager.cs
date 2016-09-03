using UnityEngine;

/// <summary>
/// The persistent manager is a singleton which gets its own age in game time, 
/// and if it's not the only one (and is older than the rest) it destroys itself.
/// 
/// Potential gotcha: if any of the persistent children have game breaking stuff
/// in their Awake() function. preferably all children should only have init stuff
/// in their Start().
/// </summary>
public class PersistentManager : MonoBehaviour
{
    [HideInInspector]
    public float ageInTime;

    void Awake()
    {
        ageInTime = Time.time;

        var others = GameObject.FindGameObjectsWithTag(gameObject.tag);
        for (int i = 0; i < others.Length; i++)
        {
            var otherPM = others[i].GetComponent<PersistentManager>();
            if (otherPM != null && otherPM.ageInTime < ageInTime)
                Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
