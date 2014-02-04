using UnityEngine;
using System.Collections;

public class ShapeMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;

	// Update is called once per frame
	void Update () {

		if(true){
			float rotationY = Input.GetAxis("Horizontal") * rotationSpeed;
			float rotationZ = Input.GetAxis("Vertical") * rotationSpeed;
			rotationZ *= Time.deltaTime;
			rotationY *= Time.deltaTime;
			transform.Rotate(rotationZ, rotationY, 0);
		}
		else
		{
			float translationX = Input.GetAxis("Horizontal") * speed;
			float translationZ = Input.GetAxis("Vertical") * speed;
			translationX *= Time.deltaTime;
			translationZ *= Time.deltaTime;
			transform.Translate(translationX, 0, translationZ);
		}
	}
}
