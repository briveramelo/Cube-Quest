using UnityEngine;
using System.Collections;

public class BackgroundParalax : MonoBehaviour {

	private Vector2 velocity;
	private Rigidbody2D referenceBody;
	public float paralaxMultiplier;

	// Use this for initialization
	void Awake () {
		velocity = new Vector2 (0, 0); 
		paralaxMultiplier = .5f;
		referenceBody = GameObject.Find ("MainCamera").gameObject.rigidbody2D;
	}
	
	// Update is called once per frame
	void Update () {
		velocity = referenceBody.velocity;
		transform.Translate (velocity * Time.deltaTime * paralaxMultiplier);
	}
}
