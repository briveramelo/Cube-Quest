using UnityEngine;
using System.Collections;

public class ShockBlock : MonoBehaviour {

	public WeaponBlockScript weaponBlockScript;
	public AudioSource shockingSound; //inspector attachment
	public Animator shockAnimator;

	public Collider2D[] cols;
	public bool[] doDamage;

	public float shockRadius;
	public float disintegrateTime;
	public float distFactor;
	public float activationDelay;

	public int j;
	public int stickerShock;
	public int reducedRate;

	// Use this for initialization
	void Awake () {
		weaponBlockScript = GetComponent<WeaponBlockScript> ();
		activationDelay = .5f;
		shockAnimator = GetComponent<Animator> ();
		shockAnimator.SetInteger ("AnimState", 0);
		shockRadius = .75f;
		stickerShock = 4;
		disintegrateTime = 2f;
		j = 0;
	}
	
	public IEnumerator FlipTheSwitch(){
		yield return new WaitForSeconds (activationDelay);
		shockAnimator.SetInteger ("AnimState", 1);
		//shockingSound.Play ();
		StartCoroutine (Disintegrate ());
		StartCoroutine (DangerHighVoltage ());
		yield return null;
	}

	public IEnumerator DangerHighVoltage(){

		cols = Physics2D.OverlapCircleAll (transform.position, shockRadius);
		doDamage = new bool[cols.Length];
		j = 0;
		while (j<cols.Length){
			if (cols[j].GetComponent<GetDamage>()){
				doDamage[j] = true;
				distFactor = 1-Vector2.Distance(transform.position,cols[j].transform.position)/shockRadius;
				reducedRate = Mathf.RoundToInt(distFactor * stickerShock);
				GetDamage damageScript = cols[j].GetComponent<GetDamage>();
				StartCoroutine(damageScript.SendDamage(reducedRate, weaponBlockScript.blockType));
			}
			j++;
		}

		yield return new WaitForSeconds (.2f);
		StartCoroutine (DangerHighVoltage ());
	}

	public IEnumerator Disintegrate(){
		yield return new WaitForSeconds (disintegrateTime);
		Destroy (this.gameObject);
	}
}
