using UnityEngine;
using System.Collections;

public class ProtectTheBounce : MonoBehaviour {

	private float startingBounce;
	private Collider2D coller;
	private float waitTime;
	public int maxBounces;
	public int currentNumBounces;

	// Use this for initialization
	void Awake () {
		coller = GetComponent<Collider2D> ();
		startingBounce = coller.sharedMaterial.bounciness;
		maxBounces = 3;
		waitTime = 2f;
	}

	void OnCollisionEnter2D(){
		currentNumBounces++;
		if (currentNumBounces >= maxBounces) {
			StartCoroutine ( Flatten() );
		}
	}

	public IEnumerator Flatten(){
		coller.enabled = false;
		coller.sharedMaterial.bounciness = 0;
		coller.enabled = true;
		yield return new WaitForSeconds (waitTime);
		StartCoroutine(Rebounce());
	}

	public IEnumerator Rebounce(){
		coller.enabled = false;
		coller.sharedMaterial.bounciness = startingBounce;
		coller.enabled = true;

		currentNumBounces = 0;
		yield return null;
	}
}
