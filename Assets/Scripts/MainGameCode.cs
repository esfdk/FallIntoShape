using UnityEngine;
using System.Collections.Generic;

public class MainGameCode : MonoBehaviour {

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	public int activeObjectIndex = 0;
	private List<GameObject> playObjects;
	private List<Vector3> originalPositions;
	private int currentScore = 0;

	// Use this for initialization
	void Start () 
	{
		playObjects = new List<GameObject>();
		playObjects.Add(GameObject.Find("FirstObject(Star)"));
		playObjects.Add(GameObject.Find("SecondObject(Capsule)"));

		originalPositions = new List<Vector3>();
		foreach(var go in playObjects)
		{
			var p = go.transform.position;
			originalPositions.Add(new Vector3(p.x, p.y, p.z));
		}
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
		playObjects[activeObjectIndex].transform.position += playObjects[activeObjectIndex].transform.forward * Time.deltaTime *5;

		// Camera movement
		var p = playObjects[activeObjectIndex].transform.position;
		transform.position = new Vector3(p.x, p.y + 5, p.z);
	}

	public void NextObject()
	{
		if(activeObjectIndex < playObjects.Count -1)
		{
			playObjects[activeObjectIndex].transform.position = originalPositions[activeObjectIndex];
			activeObjectIndex++;
			currentScore++;
		}
		else
		{
			currentScore++;

		}

	}

	public void Reset()
	{
		currentScore = 0;
	}
}