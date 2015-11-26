using UnityEngine;
using System.Collections;

public class Smasher : MonoBehaviour {

	private SliderJoint2D slider;
	public Vector3 targetPos;
	public Vector3 startPos;
	private float smashSpeed;
	private float retractSpeed;
	public float distAway;
	private float distThresh;
	public bool moving;
	public bool smashing;
	public bool retracting;

	// Use this for initialization
	void Awake () {
		startPos = transform.position;
		targetPos = transform.GetChild(0).transform.position;
		smashSpeed = 70f;
		retractSpeed = 10f;
		distThresh = 0.015f;
		slider = GetComponent<SliderJoint2D> ();
		slider.connectedAnchor = startPos;
	}

	public IEnumerator SmashThings(){
		smashing = true;
		moving = true;
		distAway = Vector2.Distance (transform.position, targetPos);
		rigidbody2D.velocity = -Vector2.up * smashSpeed/10f;
		while (distAway>distThresh && rigidbody2D.velocity.magnitude>distThresh){
			rigidbody2D.velocity = -Vector2.up * smashSpeed/10f;
			distAway = Vector2.Distance (transform.position, targetPos);
			yield return null;
		}
		smashing = false;
		StartCoroutine (ResetPosition ());
		yield return null;
	}

	public IEnumerator ResetPosition(){
		yield return new WaitForSeconds (.5f);
		retracting = true;
		distAway = Vector2.Distance (transform.position, startPos);
		while (distAway>distThresh){
			rigidbody2D.velocity = Vector2.up * retractSpeed/10f;
			distAway = Vector2.Distance (transform.position, startPos);
			yield return null;
		}
		transform.position = startPos;
		rigidbody2D.velocity = Vector2.zero;
		moving = false;
		retracting = false;
		yield return null;
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.name == "Hero1") {
			Player player = col.gameObject.GetComponent<Player>();
			if (player.alive && smashing){
				StartCoroutine(player.Death());
			}
		}
		else if (!col.gameObject.CompareTag("Platform") && !col.gameObject.CompareTag("Exploder")){
			Destroy(col.gameObject);
		}
	}

}