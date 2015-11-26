using UnityEngine;
using System.Collections;

public class FreezeNoise : MonoBehaviour {

	
	public AudioSource explosion;
	public float time2Play;
	public float timer;
	public bool play;
	
	// Use this for initialization
	void Awake () {
		play = true;
		time2Play = explosion.clip.length;
		StartCoroutine (Disappear ());
	}
	
	private IEnumerator Disappear(){
		explosion.Play ();
		yield return new WaitForSeconds (time2Play);
		Destroy (this.gameObject);
	}
}
