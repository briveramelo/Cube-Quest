using UnityEngine;
using System.Collections;

public class TeleportalCloseSound : MonoBehaviour {

	public AudioSource teleSound;


	// Use this for initialization
	void Awake () {
		StartCoroutine (SoundOff ());
	}
	
	public IEnumerator SoundOff(){
		teleSound.Play ();
		while (teleSound.isPlaying) {
			yield return null;
		}
		Destroy (this.gameObject);
		yield return null;
	}
}
