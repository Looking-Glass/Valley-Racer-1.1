using UnityEngine;
using System.Collections;

public class DisplayMotoSpin : MonoBehaviour {

	public float degreesPerSecond;
	void Update () {
		this.transform.Rotate (Vector3.up * degreesPerSecond * Time.deltaTime);
	}
}
