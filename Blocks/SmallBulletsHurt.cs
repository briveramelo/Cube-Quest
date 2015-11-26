using UnityEngine;
using System.Collections;

public class SmallBulletsHurt : MonoBehaviour {

	public int blockType;
	public int smallBulletDamage;
	public bool hit;
	public bool first;
	public bool second;
	public bool destroyed;
	public float force;
	public float time2destroy;
	public float forceThresh;

	private GetDamage getDamage;

	void Awake(){
		smallBulletDamage = 5;
		hit = false;
		first = true;
		second = false;
		time2destroy = .7f;
		forceThresh = .5f;
	}

	void OnCollisionEnter2D(Collision2D col){

		if (col.rigidbody || col.gameObject.CompareTag("DPlatform")){
			if (first) {
				first = false;
				second = true;
			}
			else if (second){
				second = false;
				StartCoroutine (SelfDestruct ());
			}

			if (!hit){
				force = Mathf.Abs (Vector2.Dot(col.contacts[0].normal,col.relativeVelocity) * rigidbody2D.mass);
				
				if (force>forceThresh) {
					if (col.gameObject.GetComponent<GetDamage>() && !hit){
						getDamage = col.gameObject.GetComponent<GetDamage>();
						hit = true;
					}

					if (hit){
						StartCoroutine(getDamage.SendDamage(smallBulletDamage,blockType));
					}
				}
			}
		}

	}

	void OnCollisionStay2D(){
		if (!destroyed) {
			StartCoroutine(SelfDestruct());
		}
	}

	public IEnumerator SelfDestruct(){
		yield return new WaitForSeconds (time2destroy);
		Destroy (this.gameObject);
	}
	
}

