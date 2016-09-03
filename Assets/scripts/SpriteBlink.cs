using UnityEngine;
using System.Collections;

public class SpriteBlink : MonoBehaviour {

	SpriteRenderer sr;
	public float blinkInterval = 0.5f;
	float timer;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > blinkInterval) {
			sr.enabled = !sr.enabled;
			timer = 0;
		}
	}
}
