using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "GoalTrigger")
		{
			var mgc = GameObject.Find("Main Camera").GetComponent<MainGameCode>();
			mgc.NextObject();
		}
	}
}
