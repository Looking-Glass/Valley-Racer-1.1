using UnityEngine;
using System.Collections;

public class MountainRepeater : MonoBehaviour {

	float repeatAfterWidth;

	// Use this for initialization
	void Start () {
		repeatAfterWidth = (GetComponent<HeightmapToVert> ().peakTexture.width);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.localPosition.x < -repeatAfterWidth * 1.5f || this.transform.localPosition.x > repeatAfterWidth * 0.5f) {
			this.transform.localPosition += Vector3.right * -Mathf.Sign (transform.localPosition.x) * repeatAfterWidth * 2;
		}
	}
}
