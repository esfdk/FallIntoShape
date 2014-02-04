using UnityEngine;
using System.Collections;

public class ShapeMovement : MonoBehaviour {

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	private Transform dot;
	// Use this for initialization
	void Start () {
		dot = GameObject.Find("dot").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(true){
			float rotationY = Input.GetAxis("Horizontal") * rotationSpeed;
			float rotationZ = Input.GetAxis("Vertical") * rotationSpeed;
			rotationZ *= Time.deltaTime;
			rotationY *= Time.deltaTime;
			transform.Rotate(rotationZ, rotationY, 0);
			transform.position += transform.forward * Time.deltaTime *5;
		}
		else
		{
			float translationX = Input.GetAxis("Horizontal") * speed;
			float translationZ = Input.GetAxis("Vertical") * speed;
			translationX *= Time.deltaTime;
			translationZ *= Time.deltaTime;
		}

		//transform.position = Vector3.MoveTowards(transform.position, dot.position, 0.05f);
	}
}
