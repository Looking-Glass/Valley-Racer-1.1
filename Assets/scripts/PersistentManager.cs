using System.Collections;
using UnityEngine;

/// <summary>
/// The persistent manager is a parent to all children who should be persistent across scenes
/// </summary>
public class PersistentManager : MonoBehaviour
{
    public static PersistentManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }
}
