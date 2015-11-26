using UnityEngine;
using System.Collections;

public class PuffAway : MonoBehaviour {

	private float time2Play;
	// Use this for initialization
	void Awake () {
		time2Play = Random.Range (10, 30) * .1f;
		StartCoroutine (Disappear ());
	}
	
	private IEnumerator Disappear(){
		yield return new WaitForSeconds (time2Play);
		Destroy (this.gameObject);
	}
}
