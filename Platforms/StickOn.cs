using UnityEngine;
using System.Collections;

public class StickOn : MonoBehaviour {


	private Vector3 stickDir;
	private Vector3 hitDir;
	public float hitForce;
	public float stickForce;
	private Vector3 hitPoint;
	private Vector3 stickPoint;

	// Use this for initialization
	void Awake () {
	}

	void OnCollisionEnter2D(Collision2D coller){
		if (coller.gameObject.rigidbody2D) {
			hitPoint = coller.contacts[0].point;
			hitDir = Vector3.Normalize(hitPoint-coller.transform.position);
			coller.gameObject.rigidbody2D.velocity = Vector2.zero;
			coller.gameObject.rigidbody2D.AddForce(hitDir * hitForce);
		}
	}

	void OnCollisionStay2D(Collision2D col){
		if (col.gameObject.rigidbody2D) {
			stickPoint = col.contacts[0].point;
			stickDir = Vector3.Normalize(stickPoint-col.transform.position);
			col.gameObject.rigidbody2D.AddForce(stickDir * stickForce);
		}
	}
}
