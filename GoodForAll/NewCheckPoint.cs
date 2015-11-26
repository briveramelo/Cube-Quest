using UnityEngine;
using System.Collections;

public class NewCheckPoint : MonoBehaviour {

	private string checkpointSoundString;
	private CheckPoint checkPoint;

	// Use this for initialization
	void Awake () {
		checkpointSoundString = "Prefabs/Effects/CoinSound2";
		checkPoint = GameObject.Find ("CheckPoints").GetComponent<CheckPoint> ();
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name == "Hero1") {
			checkPoint.currentCheckPoint = gameObject;
			checkPoint.currentCheckPointSpot = transform.position;
			Instantiate (Resources.Load (checkpointSoundString), transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
