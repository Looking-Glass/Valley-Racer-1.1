using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	float totalTime;
	float timer;
	float strength;
	Vector3 heldPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0) {

			var x1 = Random.Range (-strength, strength);
			var y1 = Random.Range (-strength, strength);
			var z1 = Random.Range (-strength, strength);

			this.transform.position = heldPosition + new Vector3 (x1, y1, z1);

			timer -= Time.deltaTime;
			strength = Mathf.Lerp (0f, strength, timer / totalTime);
		}
	}

	public void Shake (float t, float str) {
		totalTime = t;
		timer = t;
		strength = str;
		heldPosition = this.transform.position;
	}
}
