using UnityEngine;
using System.Collections;

public class ExplodeNoise : MonoBehaviour {

	public AudioSource explosion;
	public float time2Play;
	public float timer;

	// Use this for initialization
	void Awake () {

		explosion = GetComponent<AudioSource> ();

		time2Play = explosion.clip.length;
		StartCoroutine (Disappear ());
	}

	private IEnumerator Disappear(){
		explosion.Play ();
		yield return new WaitForSeconds (time2Play);
		Destroy (this.gameObject);
	}


}
