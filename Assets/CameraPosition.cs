using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var p = GameObject.Find("Sphere").transform.position;
		transform.position = new Vector3(p.x, p.y + 20, p.z);
	}
}
