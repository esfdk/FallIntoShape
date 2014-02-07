using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainGameCode : MonoBehaviour {
	
	public int activeObjectIndex = 0;
	public float rotationSpeed = 100.0F;
	public float velocity = 100.0F;
	public float maxVelocity = 500f, minVelocity = 0f, velocityChange = 10f;
	public GUIText timeText, scoreText, endText;

	private List<GameObject> playObjects;
	private List<Vector3> originalPositions;
	private Hashtable moveParams = new Hashtable();
	private int currentScore = 0;
	private float timeLeft, endTime;
	private bool switchingObject;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		endText.enabled = false;
		switchingObject = false;

		// Adds the objects to a list for easier control.
		playObjects = new List<GameObject>();
		playObjects.Add(GameObject.Find("Object_01"));
		playObjects.Add(GameObject.Find("Object_02"));
		playObjects.Add(GameObject.Find("Object_03"));
		playObjects.Add(GameObject.Find("Object_04"));
		playObjects.Add(GameObject.Find("Object_05"));
		playObjects.Add(GameObject.Find("Object_06"));
		playObjects.Add(GameObject.Find("Object_07"));
		playObjects.Add(GameObject.Find("Object_08"));
		playObjects.Add(GameObject.Find("Object_09"));
		playObjects.Add(GameObject.Find("Object_10"));

		// Add the object's initial positions to another list.
		originalPositions = new List<Vector3>();
		foreach(var go in playObjects)
		{
			var p = go.transform.position;
			originalPositions.Add(new Vector3(p.x, p.y, p.z));
		}

		timeLeft = 50f;
		endTime = 5f;
	}
	
	/// <summary>
	/// Update is called once per frame
	/// </summary>
	void Update () 
	{
		// Shape movement
		float rotationY = 0;
		float rotationZ = 0;

		// Controls through ChainJam API calling.
		if (ChainJam.GetButtonPressed(ChainJam.BUTTON.LEFT)) rotationY = -1;
		else if (ChainJam.GetButtonPressed(ChainJam.BUTTON.RIGHT)) rotationY = 1;
		
		if (ChainJam.GetButtonPressed(ChainJam.BUTTON.UP)) rotationZ = -1;
		else if (ChainJam.GetButtonPressed(ChainJam.BUTTON.DOWN)) rotationZ = 1;

		if(ChainJam.GetButtonPressed(ChainJam.BUTTON.A)) velocity += velocityChange;
		if(ChainJam.GetButtonPressed(ChainJam.BUTTON.B)) velocity -= velocityChange;

		if (velocity > maxVelocity) velocity = maxVelocity;
		if (velocity < minVelocity) velocity = minVelocity;

		rotationY *= rotationSpeed * Time.deltaTime;
		rotationZ *= rotationSpeed * Time.deltaTime;

		// Move the current object.
		if (!switchingObject)
		{
			playObjects[activeObjectIndex].transform.Rotate(rotationZ, rotationY, 0);
			var v = playObjects[activeObjectIndex].transform.forward * Time.deltaTime * velocity;
			playObjects[activeObjectIndex].rigidbody.velocity = v;
		}

		// Camera movement
		var p = playObjects[activeObjectIndex].transform.position;
		transform.position = new Vector3(p.x, p.y + 5, p.z);

		if (timeLeft > 0f) timeLeft -= Time.deltaTime;
		if (timeLeft <= 0f) EndGame();

		UpdateTexts();
	}

	/// <summary>
	/// Switch to the next play object.
	/// </summary>
	public void NextObject()
	{
		// If there are "levels" left, switch to the next one
		if(activeObjectIndex < playObjects.Count -1)
		{
			playObjects[activeObjectIndex].transform.position = originalPositions[activeObjectIndex];
			playObjects[activeObjectIndex].transform.rigidbody.velocity = Vector3.zero;
			activeObjectIndex++;
			
			IncreaseScore();
			PauseMovement(true);

			// Use iTween to move camera to the next play object smoothly.
			var temp = playObjects[activeObjectIndex].transform.position;
			var tempVector = new Vector3(temp.x, temp.y + 5, temp.z);

			moveParams = new Hashtable();
			moveParams.Add(iT.MoveTo.path, new Vector3[] {this.transform.position, tempVector});
			moveParams.Add(iT.MoveTo.time, 1);
			moveParams.Add(iT.MoveTo.easetype, "linear");
			moveParams.Add(iT.MoveTo.oncompletetarget, this.gameObject);
			moveParams.Add(iT.MoveTo.oncompleteparams, false);
			moveParams.Add(iT.MoveTo.oncomplete, "PauseMovement");
			
			iTween.MoveTo(this.gameObject, moveParams);
		}
		// Else end the game.
		else
		{
			IncreaseScore();
			EndGame();
		}
	}

	/// <summary>
	/// Update the score and time left GUI texts.
	/// </summary>
	private void UpdateTexts()
	{
		scoreText.text = "Score: " + currentScore;
		timeText.text = "Time left: " + (int)timeLeft;
	}

	/// <summary>
	/// Increase the score if it is not at the limit already.
	/// </summary>
	private void IncreaseScore()
	{
		if (currentScore < 10) currentScore++;
	}

	/// <summary>
	/// End the game and show the end text.
	/// </summary>
	private void EndGame()
	{
		// Set text
		endText.text = "You managed to gather " + currentScore + " points!";

		// Enabled the end text and disable the others
		if (!endText.enabled)
		{
			endText.enabled = true;
			scoreText.enabled = false;
			timeText.enabled = false;
		}

		// End the game totally after a short delay.
		if (endTime > 0f) endTime -= Time.deltaTime;
		if (endTime <= 0f)
		{
			ChainJam.AddPoints(currentScore);
			ChainJam.GameEnd();
		}
	}

	/// <summary>
	/// Tells the current play object to pause movement based on the input.
	/// </summary>
	/// <param name="pause">If set to <c>true</c>, pause.</param>
	private void PauseMovement(bool pause)
	{
		switchingObject = pause;
	}

	/// <summary>
	/// Reset the score.
	/// </summary>
	public void Reset()
	{
		currentScore = 0;
	}
}