using UnityEngine;

public class SettingChanges : MonoBehaviour
{
    public Color[] mountainColors;
    public Color[] skyColors;
    SpriteRenderer skyRenderer;
    public int index;
    ScoreKeeper scoreKeeper;
    public float settingInterval = 300;
    float prevMod;
    
    void Start()
    {
        scoreKeeper = GetComponent<ScoreKeeper>();
        skyRenderer = GameObject.FindGameObjectWithTag("Sky").GetComponent<SpriteRenderer>();

    }
    
    void Update()
    {
        float modDistance = scoreKeeper.CurrentScore % settingInterval;
        if (modDistance < prevMod)
            index++;
        prevMod = modDistance;

        Color finalSkyColor = skyColors[index];
        Color finalMtnColor = mountainColors[index];

        if (modDistance > settingInterval * 0.8f)
        {
            finalSkyColor = Color.Lerp
                (skyColors[index], skyColors[index + 1], (modDistance - 0.8f * settingInterval) / (0.2f * settingInterval));
            finalMtnColor = Color.Lerp
                (mountainColors[index], mountainColors[index + 1], (modDistance - 0.8f * settingInterval) / (0.2f * settingInterval));
        }

        skyRenderer.color = finalSkyColor;

        GameObject[] mtnList = GameObject.FindGameObjectsWithTag("Mountains");

        for (var i = 0; i < mtnList.Length; i++)
        {
            mtnList[i].GetComponent<Renderer>().material.color = finalMtnColor;
        }
    }
}