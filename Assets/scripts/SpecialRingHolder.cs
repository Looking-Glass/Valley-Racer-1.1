using UnityEngine;
using System.Collections;

public class SpecialRingHolder : MonoBehaviour {

	public Vector3 rotation;
	public Vector3 scale = Vector3.one;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.eulerAngles = rotation;
		this.transform.localScale = scale;
	}
}
