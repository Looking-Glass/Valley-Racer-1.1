using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]
public class BillboardedHyperSprite : MonoBehaviour {
	Vector3 tempPosition;
	Quaternion tempRotation;
	bool running = true;
	public Transform hypercubeTransform;

	void Start () {
		if (hypercubeTransform == null && GameObject.FindGameObjectWithTag("Hypercube")) {
			hypercubeTransform = GameObject.FindGameObjectWithTag ("Hypercube").transform;
		}
		tempPosition = this.transform.position;
		tempRotation = this.transform.rotation;
		StartCoroutine (ResetPositionRotation());
	}

	void Update() {
		tempPosition = this.transform.position;
		tempRotation = this.transform.rotation;

		this.transform.eulerAngles = new Vector3 (
			hypercubeTransform.eulerAngles.x,
			hypercubeTransform.eulerAngles.y,
			this.transform.eulerAngles.z
		);
	}

	IEnumerator ResetPositionRotation () {
		while (running) {
			yield return new WaitForEndOfFrame ();
			this.transform.position = tempPosition;
			this.transform.rotation = tempRotation;
		}
	}
}


