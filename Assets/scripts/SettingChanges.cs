using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SettingChanges : MonoBehaviour {

	public Color[] mountainColors;
	public Color[] skyColors;

	Material[] mtnMaterial;
	SpriteRenderer skyRenderer;

	public int index;

	ScoreKeeper scoreKeeper;

	float settingDist;
	public float settingInterval = 300;

	float prevMod;


	// Use this for initialization
	void Start () {
		scoreKeeper = GetComponent<ScoreKeeper> ();
		skyRenderer = GameObject.FindGameObjectWithTag ("Sky").GetComponent<SpriteRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {

		Color finalSkyColor;
		Color finalMtnColor;

		float modDistance = scoreKeeper.CurrentScore % settingInterval;
		if (modDistance < prevMod) {
			index++;
		}
		prevMod = modDistance;

		finalSkyColor = skyColors [index];
		finalMtnColor = mountainColors [index];

		if (modDistance > settingInterval * 0.8f) {
			finalSkyColor = Color.Lerp (skyColors [index], skyColors [index + 1], (modDistance - 0.8f * settingInterval) / (0.2f * settingInterval));

			finalMtnColor = Color.Lerp (mountainColors [index], mountainColors [index + 1], (modDistance - 0.8f * settingInterval) / (0.2f * settingInterval));
		}

		skyRenderer.color = finalSkyColor;

		GameObject[] mtnList = GameObject.FindGameObjectsWithTag ("Mountains");
		mtnMaterial = new Material[mtnList.Length];
		for (var i = 0; i < mtnList.Length; i++) {
			mtnMaterial[i] = mtnList[i].GetComponent<Renderer> ().material;
		}

		for (var i = 0; i < mtnMaterial.Length; i++) {
			mtnMaterial [i].color = finalMtnColor;
		}
	}

}
