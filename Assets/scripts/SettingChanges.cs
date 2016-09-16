using UnityEngine;

public class SettingChanges : MonoBehaviour
{
    public Color[] mountainColors;
    public Color[] skyColors;
    public SpriteRenderer skyRenderer;
    public MeshRenderer mountainRenderer;
    public int index;
    ScoreKeeper scoreKeeper;
    public float settingInterval = 300;
    float prevMod;
    
    void Start()
    {
        scoreKeeper = GetComponent<ScoreKeeper>();
    }
    
    void Update()
    {
        float modDistance = scoreKeeper.CurrentScore % settingInterval;
        if (modDistance < prevMod)
            index = SafeAdd(index, skyColors.Length);
        prevMod = modDistance;

        Color finalSkyColor = skyColors[index];
        Color finalMtnColor = mountainColors[index];

        if (modDistance > settingInterval * 0.8f)
        {
            finalSkyColor = Color.Lerp
                (skyColors[index], skyColors[SafeAdd(index, skyColors.Length)], (modDistance - 0.8f * settingInterval) / (0.2f * settingInterval));
            finalMtnColor = Color.Lerp
                (mountainColors[index], mountainColors[SafeAdd(index, mountainColors.Length)], (modDistance - 0.8f * settingInterval) / (0.2f * settingInterval));
        }

        skyRenderer.color = finalSkyColor;

        mountainRenderer.material.color = finalMtnColor;
    }

    int SafeAdd(int num, int max)
    {
        var i = num + 1 >= max ? 0 : num + 1;
        return i;
    }
}