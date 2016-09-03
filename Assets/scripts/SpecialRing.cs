using UnityEngine;
using System.Collections;

public class SpecialRing : MonoBehaviour {

	public GameObject specialSprite;
	public float radius;
	public int numberOfSprites;
	public float degreesPerSecond;

	// Use this for initialization
	void Start () {
		for (var i = 0; i < numberOfSprites; i++) {

			GameObject newSpecialSprite = (GameObject)Instantiate (specialSprite, this.transform.position, Quaternion.identity);
			newSpecialSprite.transform.parent = this.transform;
			newSpecialSprite.GetComponent<SpriteRenderer> ().enabled = true;

			float angleRadians = (float)i / numberOfSprites;
			angleRadians *= Mathf.PI * 2;

			Vector3 movePosition = new Vector3 (
                Mathf.Cos (angleRadians) * radius,
				0,
				Mathf.Sin (angleRadians) * radius
            );
			newSpecialSprite.transform.Translate (movePosition);

		}
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localEulerAngles += Vector3.up * degreesPerSecond * Time.deltaTime;
	}

	void OnDrawGizmos () {
		for (var i = 0; i < numberOfSprites; i++) {
			float angleRadians = (float)i / numberOfSprites;
			angleRadians *= Mathf.PI * 2;

			Vector3 vecToDraw = new Vector3 (
				Mathf.Cos (angleRadians) * radius,
				0,
				Mathf.Sin (angleRadians) * radius
			);

			Gizmos.color = Color.cyan;
			Gizmos.DrawLine (this.transform.position, this.transform.position + vecToDraw);
		}
	}
}
