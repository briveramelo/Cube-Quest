using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	public GameObject paul;

	public WeaponDetectorScript weaponDetectorScript;
	public Animator shieldAnimator;

	public AudioSource grow;
	public AudioSource forcefield;

	public Vector3 pushDir;
	
	public float pushForce;
	public float pulseTime;
	public float growTime;
	public float extraTime;

	public int animState;
	public int speed;

	public bool growing;
	public bool pulsing;
	public bool turning;
	public bool enter;

	//AnimState 1 means pulse
	//AnimState 0 means grow
	//Once either of those are finished, it will default back to stillness

	// Use this for initialization
	void Awake () {
		pushForce = 100f;
		shieldAnimator = GetComponent<Animator> ();
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<PolygonCollider2D> ().enabled = false;
		pulseTime = .833f;
		growTime = 1f;
		extraTime = .083f;
		shieldAnimator.SetInteger ("AnimState", 2); //awakenstill
		animState = 2;
		shieldAnimator.speed = 0;
		rigidbody2D.centerOfMass = Vector2.zero;
		rigidbody2D.mass = 0f;
		enter = false;

		if (gameObject.name == "Shield1"){
			weaponDetectorScript = GameObject.Find("WeaponDetector1").GetComponent<WeaponDetectorScript>();
		}
		else {
			weaponDetectorScript = GameObject.Find("WeaponDetector2").GetComponent<WeaponDetectorScript>();
		}

	}

	void OnCollisionEnter2D(Collision2D col){
		paul = col.gameObject;
		forcefield.Play ();
		if (!pulsing && !growing){
			StartCoroutine(PulseShield());
		}
	}

	public IEnumerator GrowShield(){
		growing = true;
		grow.Play ();
		shieldAnimator.speed = 2.0325f;
		shieldAnimator.SetInteger ("AnimState", 0); //grow
		shieldAnimator.Play ("ShieldGrowAnimation", -1, 0);
		animState = 0;
		yield return new WaitForSeconds (growTime/shieldAnimator.speed);
		growing = false;
		shieldAnimator.SetInteger ("AnimState", 2); //still
	}

	public IEnumerator TurnShieldL(int speedDump){
		speed = speedDump;
		turning = true;
		shieldAnimator.speed = speed;
		shieldAnimator.SetInteger ("AnimState", 3); //turn
		shieldAnimator.Play ("ShieldGrowAnimation", -1, 0);
		animState = 3;
		yield return new WaitForSeconds (growTime);
		turning = false;
		shieldAnimator.SetInteger ("AnimState", 2); //still
		animState = 2;
		yield return null;
	}

	public IEnumerator PulseShield(){
		pulsing = true;
		shieldAnimator.speed = 1;
		shieldAnimator.SetInteger ("AnimState", 1); //pulse
		shieldAnimator.Play ("ShieldPulseAnimation", -1, 0);
		animState = 1;
		yield return new WaitForSeconds (pulseTime);
		pulsing = false;
		shieldAnimator.SetInteger ("AnimState", 2); //still
		animState = 2;
	}

	public IEnumerator GoStill(){
		shieldAnimator.speed = 0;
		shieldAnimator.SetInteger ("AnimState", 0); //still
		animState = 0;
		yield return null;
	}


	public IEnumerator PushAway(){
		if (!enter) {
			rigidbody2D.AddForce(weaponDetectorScript.fDir * pushForce);
			StartCoroutine(PulseShield());
			enter = true;
			yield return new WaitForSeconds (1f);
			enter = false;
		}

		yield return null;
	}

}