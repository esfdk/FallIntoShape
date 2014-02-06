using UnityEngine;
using System.Collections.Generic;

public class ShapeMovement : MonoBehaviour {

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	public int activeObjectIndex = 0;
	private List<GameObject> playObjects;

	// Use this for initialization
	void Start () 
	{
		playObjects = new List<GameObject>();
		playObjects.Add(GameObject.Find("FirstObject(Cube)"));
		playObjects.Add(GameObject.Find("SecondObject(Capsule)"));
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Shape movement
		float rotationY = Input.GetAxis("Horizontal") * rotationSpeed;
		float rotationZ = Input.GetAxis("Vertical") * rotationSpeed;
		rotationZ *= Time.deltaTime;
		rotationY *= Time.deltaTime;
		playObjects[activeObjectIndex].transform.Rotate(rotationZ, rotationY, 0);
		playObjects[activeObjectIndex].transform.position += transform.forward * Time.deltaTime *5;	

		// Camera movement
		var p = playObjects[activeObjectIndex].transform.position;
		transform.position = new Vector3(p.x, p.y + 20, p.z);
	}

	void NextObject()
	{
		if(activeObjectIndex < playObjects.Count -1)
		{
			activeObjectIndex++;
		}
	}
}