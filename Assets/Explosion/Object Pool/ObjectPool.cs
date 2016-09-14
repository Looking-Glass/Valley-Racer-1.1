using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject poolObject;
    public List<GameObject> pool;
    public int minimumPool = 50;
    public int maximumPool = 100;
    public bool startPoolEnabled;
    int index;

    void Awake()
    {
        pool = new List<GameObject>();

        //Create Pool
        for (int i = 0; i < minimumPool; i++)
        {
            AddOneToPool();
        }
    }

    void AddOneToPool()
    {
        var obj = (GameObject)Instantiate(poolObject, transform, true);
        if (!startPoolEnabled)
            obj.SetActive(false);
        pool.Add(obj);
    }

    public GameObject ActivateObject()
    {
        var obj = pool[index];
        obj.SetActive(false); //so if it was already active, it doesn't miss OnEnable()
        obj.SetActive(true);
        index += 1;

        if (index == pool.Count)
        {
            if (pool[0].activeSelf && pool.Count < maximumPool)
                AddOneToPool();
            else
                index = 0;
        }

        return obj;
    }
}