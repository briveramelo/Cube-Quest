using UnityEngine;
using System.Collections;

public class Crumb : MonoBehaviour {



	// Use this for initialization
	void Awake () {
		StartCoroutine (Dissolve (Random.Range(5,10) * .1f));
		Physics2D.IgnoreLayerCollision (gameObject.layer, 8);//dont hit player
	}
	
	public IEnumerator Dissolve(float timer){
		yield return new WaitForSeconds (timer);
		Destroy (gameObject);
	}
}
