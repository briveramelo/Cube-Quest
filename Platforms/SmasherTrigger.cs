using UnityEngine;
using System.Collections;

public class SmasherTrigger : MonoBehaviour {

	private Smasher[] smashers;
	public bool[] moving;
	private float delaySmash;
	public bool ready; 
	private int j;
	private int i;

	// Use this for initialization
	void Awake () {
		smashers = transform.GetComponentsInChildren<Smasher> ();
		delaySmash = .1f;
		moving = new bool[smashers.Length];
		ready = true;
	}
	
	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.name == "Hero1") {
			StartCoroutine(CheckMoving());
			if (ready){
				StartCoroutine(TriggerSmashes());
			}
		}
	}

	public IEnumerator CheckMoving(){
		i = 0;
		foreach (Smasher smashScript in smashers) {
			moving [i] = smashScript.moving;
			i++;
		}

		j = 0;
		foreach (bool check in moving) {
			if (check){
				j++;
				ready = false;
				break;
			}
		}
		if (j == 0) {
			ready = true;
		}
		yield return null;
	}

	public IEnumerator TriggerSmashes(){
		foreach (Smasher smasher in smashers){
			StartCoroutine(smasher.SmashThings());
			yield return new WaitForSeconds (delaySmash);
		}
		yield return null;
	}
}
