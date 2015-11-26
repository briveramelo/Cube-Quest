using UnityEngine;
using System.Collections;

public class CrumbleToDust : MonoBehaviour {

	private bool crumbling;
	private DestructablePlatform destructable;

	// Use this for initialization
	void Awake () {
		crumbling = false;
		destructable = GetComponent<DestructablePlatform> ();
	}

	void OnCollisionStay2D (Collision2D col){
		if (col.gameObject.name == "Hero1") {
			StartCoroutine(CrumbleApart());
		}
	}

	public IEnumerator CrumbleApart(){
		if (!crumbling){
			crumbling = true;
			StartCoroutine (destructable.Damage (4, -6));
			yield return new WaitForSeconds(.2f);
			crumbling = false;
		}
	}
}
