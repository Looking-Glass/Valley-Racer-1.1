﻿using UnityEngine;
using System.Collections.Generic;

public class MountainSpawner : MonoBehaviour
{

    public GameObject mountain;
    public string levelString;
    public Texture2D[] levelTextures;
    public char[] levelKeys;
    Dictionary<char, GameObject> levelDictionary;
    Dictionary<char, GameObject> cloneDictionary;
    int levelBarIndex;
    GameObject currentBar;
    GameObject nextBar;
    
    void Awake()
    {
        //keep two dictionary, mostly gonna use the levelDictionary unless we have two of the same mountain squares in succession.
        levelDictionary = new Dictionary<char, GameObject>();
        cloneDictionary = new Dictionary<char, GameObject>();

        if (levelTextures.Length == levelKeys.Length)
        {
            for (var i = 0; i < levelKeys.Length; i++)
            {

                if (levelTextures[i] != null)
                {
                    var newMtn = Instantiate(mountain);
                    var hm = newMtn.GetComponent<HeightmapToVert>();
                    hm.peakTexture = levelTextures[i];
                    hm.hillTexture = levelTextures[Random.Range(0, levelTextures.Length)];
                    hm.SetupMountains();
                    var newMtn2 = Instantiate(newMtn);
                    newMtn2.transform.localPosition -= Vector3.right * levelTextures[i].width;

                    var mtnBar = new GameObject
                    {
                        name = "mountainBar",
                        tag = "MountainBars"
                    };

                    newMtn.transform.parent = mtnBar.transform;
                    newMtn2.transform.parent = mtnBar.transform;

                    var mtnClone = Instantiate(mtnBar);

                    mtnBar.SetActive(false);
                    mtnClone.SetActive(false);

                    levelDictionary.Add(levelKeys[i], mtnBar);
                    cloneDictionary.Add(levelKeys[i], mtnClone);
                }
                else
                {
                    Debug.LogWarning("No texture in " + gameObject.name + " at levelTextures[" + i + "]");
                }
            }
        }
        else
        {
            Debug.LogWarning("Level keys and level values should be the same length in " + gameObject.name);
        }

        levelBarIndex = 0;
        nextBar = levelDictionary[levelString[levelBarIndex]];
        nextBar.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (nextBar.transform.position.z < -(nextBar.GetComponentInChildren<HeightmapToVert>().peakTexture.height + 1f) / 2f)
        {
            levelBarIndex++;
            if (levelBarIndex >= levelString.Length)
            {
                levelBarIndex = 0;
            }
            ChangeLevelBars();
        }
    }

    void ChangeLevelBars()
    {
        if (currentBar != null)
        {
            currentBar.gameObject.SetActive(false);
        }
        currentBar = nextBar;
        var barChar = levelString[levelBarIndex];
        nextBar = currentBar != levelDictionary[barChar] ? levelDictionary[barChar] : cloneDictionary[barChar];

        nextBar.SetActive(true);
        var forwardMovement = currentBar.transform.position + Vector3.forward * (currentBar.GetComponentInChildren<HeightmapToVert>().peakTexture.height - 1);
        nextBar.transform.localPosition = forwardMovement;

        for (var i = 0; i < nextBar.transform.childCount; i++)
        {
            nextBar.transform.GetChild(i).localPosition = currentBar.transform.GetChild(i).localPosition;
        }

        var currentVerts = currentBar.GetComponentInChildren<MeshFilter>().sharedMesh.vertices;
        var nextVerts = nextBar.GetComponentInChildren<MeshFilter>().sharedMesh.vertices;

        var w1 = levelTextures[0].width;
        for (var i = 0; i < w1 + 1; i++)
        {
            nextVerts[i] = new Vector3(
                nextVerts[i].x,
                currentVerts[nextVerts.Length - w1 - 1 + i].y,
                nextVerts[i].z
            );
        }

        var mf = nextBar.GetComponentInChildren<MeshFilter>();
        mf.sharedMesh.vertices = nextVerts;
        mf.sharedMesh.RecalculateBounds();
        mf.sharedMesh.RecalculateNormals();

    }
}
