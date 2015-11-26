using UnityEngine;
using System.Collections;

public class PistonPlatform : MonoBehaviour {

	//set these in inspector
	public PistonButton pistonButton;
	public GameObject button;

	public float launchDist;
	public Vector2 launchDir;
	public float launchSpeed;
	public float retractSpeed;
	public float launchPauseTime;
	public float retractPauseTime;


	private Vector3 vel3;
	private Vector2 vel2;
	private Vector3 startPos;
	private float curDist;
	private int[] pistonType;
	private float returnDist;
	public bool moving;

	// Use this for initialization
	void Awake () {
		startPos = transform.position;
		returnDist = .05f;
		moving = false;
		button = transform.parent.FindChild ("Button").gameObject;
		pistonButton = button.GetComponent<PistonButton>();
		pistonButton.pistonPlatform = this;
	}
	
	// Update is called once per frame
	public IEnumerator Launch(){
		if (!moving && launchDist>0f && Vector2.SqrMagnitude(launchDir)>0f && launchSpeed>0f && retractSpeed>0f) {
			moving = true;
			yield return new WaitForSeconds(launchPauseTime);
			gameObject.AddComponent<Rigidbody2D>();
			rigidbody2D.fixedAngle = true;
			rigidbody2D.mass = 1000f;
			rigidbody2D.gravityScale = 0f;
			vel3 = new Vector3 (launchDir.x,launchDir.y,0f);
			vel3 = Vector3.Normalize(vel3) * launchSpeed;
			vel2 = vel3;
			while (curDist<launchDist){
				curDist = Vector3.Distance(transform.position,startPos);
				rigidbody2D.velocity = vel2;
				yield return null;
			}
			rigidbody2D.velocity = Vector2.zero;
			StartCoroutine (Retract());
		}
	}

	public IEnumerator Retract(){
		yield return new WaitForSeconds(retractPauseTime);
		vel3 = Vector3.Normalize(vel3) * retractSpeed;
		vel2 = vel3;
		while (curDist>returnDist){
			curDist = Vector3.Distance(transform.position,startPos);
			rigidbody2D.velocity = -vel2;
			yield return null;
		}
		rigidbody2D.velocity = Vector2.zero;
		Destroy(rigidbody2D);

		transform.position = startPos;
		moving = false;
		yield return null;
	}
}
