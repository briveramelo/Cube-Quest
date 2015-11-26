using UnityEngine;
using System.Collections;
using System.Linq;

public class FreezeGround : MonoBehaviour {

	public Animator freezeAnimator;
	private float lifeTimer;
	
	//AnimState key
	//1 goes from growth/shine to idle
	//2 goes from idle to shine
	//3 goes from idle/shine to disappear

	// Use this for initialization
	void Awake () {
		freezeAnimator = GetComponent<Animator> ();
		freezeAnimator.SetInteger ("AnimState", 0);
		lifeTimer = 6f;
		StartCoroutine(Disappear());
	}
	
	// Update is called once per frame


	private IEnumerator Disappear(){
		yield return new WaitForSeconds (lifeTimer);
		freezeAnimator.SetInteger ("AnimState", 3);
		yield return new WaitForSeconds (freezeAnimator.GetCurrentAnimatorStateInfo(0).length);
		Destroy (this.gameObject);
	}

}
