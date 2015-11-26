using UnityEngine;
using System.Collections;

public class ZoomZone : MonoBehaviour {

	private CameraFollowPlayer1 cameraScript;
	private WeaponDetectorScript weaponDetectorScript;
	public float newDefaultSize;
	private float defaultSize;
	public float newHeight;
	private float defaultVertDistAway;
	private bool activated;
	public float heightSpeed;
	public bool blockTrigger;
	private bool used;

	// Use this for initialization
	void Awake () {
		cameraScript = GameObject.Find ("MainCamera").GetComponent<CameraFollowPlayer1> ();
		weaponDetectorScript = GameObject.Find ("WeaponDetector1").GetComponent<WeaponDetectorScript> ();
	}

	void OnTriggerStay2D(Collider2D col){
		if (!activated && col.gameObject.name == "Hero1") {
			if (blockTrigger){
				if (weaponDetectorScript.numBlocks>0 && !used){
					StartCoroutine(SetNewHeight());
					used = true;
				}
			}
			else{
				StartCoroutine(SetNewHeight());
			}
		}
	}

	public IEnumerator SetNewHeight(){
		activated = true;
		cameraScript.defaultSize = newDefaultSize;
		while (activated && Mathf.Abs (cameraScript.defaultVertDistAway-newHeight)>.01f){
			cameraScript.defaultVertDistAway = Mathf.MoveTowards (cameraScript.defaultVertDistAway, newHeight, heightSpeed * Time.deltaTime);
			yield return null;
		}
	}

	public IEnumerator SetOldHeight(){
		cameraScript.defaultSize = cameraScript.startingDefaultSize;
		activated = false;
		while (!activated && Mathf.Abs (cameraScript.defaultVertDistAway-cameraScript.startingDefaultVertDistAway)>.01f){
			cameraScript.defaultVertDistAway = Mathf.MoveTowards (cameraScript.defaultVertDistAway, cameraScript.startingDefaultVertDistAway, heightSpeed * Time.deltaTime);
			yield return null;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.name == "Hero1") {
			StartCoroutine(SetOldHeight());
		}
	}
}
