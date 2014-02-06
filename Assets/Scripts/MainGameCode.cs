using UnityEngine;
using System.Collections.Generic;

public class MainGameCode : MonoBehaviour {

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	public int activeObjectIndex = 0;

	public GUIText timeText, scoreText, endText;

	private List<GameObject> playObjects;
	private List<Vector3> originalPositions;
	private int currentScore = 0;
	private float timeLeft, endTime;

	// Use this for initialization
	void Start () 
	{
		endText.enabled = false;

		playObjects = new List<GameObject>();
		playObjects.Add(GameObject.Find("FirstObject(Star)"));
		playObjects.Add(GameObject.Find("SecondObject(Capsule)"));

		originalPositions = new List<Vector3>();
		foreach(var go in playObjects)
		{
			var p = go.transform.position;
			originalPositions.Add(new Vector3(p.x, p.y, p.z));
		}

		timeLeft = 50f;
		endTime = 5f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Shape movement
		float rotationY = 0;
		float rotationZ = 0;

		// ChainJam API calling.
		if (ChainJam.GetButtonPressed(ChainJam.BUTTON.LEFT)) rotationY = -1;
		else if (ChainJam.GetButtonPressed(ChainJam.BUTTON.RIGHT)) rotationY = 1;
		
		if (ChainJam.GetButtonPressed(ChainJam.BUTTON.UP)) rotationZ = -1;
		else if (ChainJam.GetButtonPressed(ChainJam.BUTTON.DOWN)) rotationZ = 1;

		rotationY *= rotationSpeed * Time.deltaTime;
		rotationZ *= rotationSpeed * Time.deltaTime;

		playObjects[activeObjectIndex].transform.Rotate(rotationZ, rotationY, 0);
		playObjects[activeObjectIndex].transform.position += playObjects[activeObjectIndex].transform.forward * Time.deltaTime *5;

		// Camera movement
		var p = playObjects[activeObjectIndex].transform.position;
		transform.position = new Vector3(p.x, p.y + 5, p.z);

		if (timeLeft > 0f) timeLeft -= Time.deltaTime;
		if (timeLeft <= 0f) EndGame();

		UpdateTexts();
	}

	public void NextObject()
	{
		if(activeObjectIndex < playObjects.Count -1)
		{
			playObjects[activeObjectIndex].transform.position = originalPositions[activeObjectIndex];
			activeObjectIndex++;
			IncreaseScore();
		}
		else
		{
			IncreaseScore();
			EndGame();
		}
	}

	private void UpdateTexts()
	{
		scoreText.text = "Score: " + currentScore;
		timeText.text = "Time left: " + (int)timeLeft;
	}

	private void IncreaseScore()
	{
		if (currentScore < 10) currentScore++;
	}

	private void EndGame()
	{
		endText.text = "You managed to gather " + currentScore + " points!";

		if (!endText.enabled)
		{
			endText.enabled = true;
			scoreText.enabled = false;
			timeText.enabled = false;
		}

		if (endTime > 0f) endTime -= Time.deltaTime;
		if (endTime <= 0f)
		{
			ChainJam.AddPoints(currentScore);
			ChainJam.GameEnd();
		}
	}

	public void Reset()
	{
		currentScore = 0;
	}
}