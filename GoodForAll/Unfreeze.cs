using UnityEngine;
using System.Collections;

public class Unfreeze : MonoBehaviour {

	public float startingBounce;
	public float startingFriction;
	
	public float playerStartingBounce;
	public float playerStartingFriction;

	public float startingJumpSpeed;
	public float startingRunningSpeed;
	public float startingMaxVelocity;
	public float time2Freeze;

	public string materialName;

	public int i;
	public int freezeQ;

	public bool mydickiscold;
	public bool running;
	public bool startColState;

	public Player player;

	// Use this for initialization
	void Awake () {
		mydickiscold = false;
		freezeQ = 0;
		time2Freeze = 3f;
		running = false;

		//startingbounce and friction are fed from the freeze script

		if (gameObject.CompareTag("Player")) {
			if (gameObject == GameObject.Find ("Hero1")){
				player = GameObject.Find ("Hero1").GetComponent<Player> ();
			}
			else{
				player = GameObject.Find ("Hero2").GetComponent<Player> ();
			}

			startingJumpSpeed = player.startingJumpSpeed;
			startingRunningSpeed = player.startingRunningSpeed;
			startingMaxVelocity = player.startingMaxVelocity;
		
			playerStartingBounce = player.startingBounce;
			playerStartingFriction = player.startingFriction;
		}
	}

	void Update(){
		if (mydickiscold) {
			freezeQ++;
			StartCoroutine(KeepItChilly());
		}
	}

	public IEnumerator KeepItChilly(){
		mydickiscold = false;

		while (running){
			yield return new WaitForEndOfFrame();
		}

		while (freezeQ>0){
			running = true;
			yield return new WaitForSeconds(time2Freeze);
			running = false;
			freezeQ -= 1;
		}
		Destroy (this);
		yield return null;
	}

	// Update is called once per frame

	
	void OnDestroy(){
		foreach (Collider2D col in GetComponents<Collider2D>()){
			startColState = col.enabled;
			col.enabled = false;
			col.sharedMaterial.bounciness = startingBounce;
			col.sharedMaterial.friction = startingFriction;
			col.enabled = startColState;
		}
		if (gameObject.CompareTag("Player")) {
			if (!player.onMyKnees){
				player.jumpSpeed = startingJumpSpeed;
				player.runningSpeed = startingRunningSpeed;
				player.maxVelocity.x = startingMaxVelocity;
			}
			else{
				player.jumpSpeed = startingJumpSpeed * .5f;
				player.runningSpeed = startingRunningSpeed * .5f;
				player.maxVelocity.x = startingMaxVelocity * .5f;
			}
		}
	}

}
