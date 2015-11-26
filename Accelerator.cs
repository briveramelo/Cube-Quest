using UnityEngine;
using System.Collections;

public class Accelerator : MonoBehaviour {

	public float acceleratorFactor;

	// Use this for initialization
	void Awake () {
		acceleratorFactor = 1f;
	}
	
	void OnTriggerStay2D(Collider2D col){
		if (col.name == "Hero1") {
			col.rigidbody2D.AddForce(col.rigidbody2D.velocity * acceleratorFactor);
		}
	}
}
