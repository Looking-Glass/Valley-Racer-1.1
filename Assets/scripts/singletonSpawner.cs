using UnityEngine;

public class singletonSpawner : MonoBehaviour
{
    public GameObject singletonToSpawn;

    void Start()
    {
        if (!GameObject.Find(singletonToSpawn.name + "_SIG"))
        {
            var singleton = Instantiate(singletonToSpawn);
            singleton.name = singletonToSpawn.name + "_SIG";
            DontDestroyOnLoad(singleton);
        }
    }
}