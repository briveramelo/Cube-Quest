using UnityEngine;
using System.Collections;

public class CoinSound : MonoBehaviour {

	public AudioSource coinSound;
	public float playTime;
	public bool around;
	public float floatSpeed;

	void Awake(){
		around = true;
		playTime = 1f;
		floatSpeed = .25f;
		coinSound.Play ();
		StartCoroutine (PointsFloat ());
		StartCoroutine (SoundOff ());
	}
	
	//if picked up then disappear
	public IEnumerator SoundOff(){
		yield return new WaitForSeconds (playTime);
		around = false;
		Destroy (gameObject);
	}

	public IEnumerator PointsFloat(){
		while (around) {
			transform.position = Vector3.Lerp (transform.position,transform.position + Vector3.up, Time.deltaTime * floatSpeed);
			yield return null;
		}
		yield return null;
	}
}
