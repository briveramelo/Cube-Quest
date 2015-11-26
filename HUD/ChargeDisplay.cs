using UnityEngine;
using System.Collections;

public class ChargeDisplay : MonoBehaviour {


	public Animator chargeAnimator;

	public Controller controller;
	public WeaponDetectorScript weaponDetectorScript;

	public Vector3 setSpot;
	public WeaponBlockScript weaponBlockScript;
	public SpriteRenderer spriteR;

	public float chargeTimer;
	public float shakeTime;
	public float charger;
	public float maxChargeTime;
	public float lastFrameTrigger;
	public float playbackSpeed;
	public float maxShakeSize;
	
	public bool exit;
	public bool lastFrameOccupied;
	public bool allowCharge;
	public bool setPlayNoise;
	public bool lastPlayNoise;
	public bool playNoise;
	public bool set;
	public bool rBumper;
	public bool charge;
	public bool startShake;
	public bool shaking;
	public bool enter;

	void Awake() {
		spriteR = GetComponent<SpriteRenderer>();
		spriteR.enabled = true;
		maxChargeTime = 1.9f;
		shakeTime = .6f;
		maxShakeSize = 0.15f;
		chargeTimer = maxChargeTime;

		rBumper = false;
		exit = false;
		charge = false;

		lastFrameTrigger = 0;
		chargeAnimator = GetComponent<Animator> ();
		//playbackSpeed = chargeAnimator.GetCurrentAnimatorStateInfo (0).length / maxTime;
		playbackSpeed = 5.167f/(maxChargeTime - shakeTime);
		chargeAnimator.speed = playbackSpeed;
		chargeAnimator.SetInteger("AnimState",0);

		if (gameObject.name == "ChargeBar1") {
			controller = GameObject.Find ("Hero1").GetComponent<Controller> ();
			weaponDetectorScript = GameObject.Find ("WeaponDetector1").GetComponent<WeaponDetectorScript> ();
			setSpot = new Vector3 (-2.5f, -1.5f, 0f);
			transform.position = setSpot;
		}
		else{
			controller = GameObject.Find ("Hero2").GetComponent<Controller> ();
			weaponDetectorScript = GameObject.Find ("WeaponDetector2").GetComponent<WeaponDetectorScript> ();
			setSpot = new Vector3 (2.5f, -1.5f, 0f);
			transform.position = setSpot;
		}
	}

	public IEnumerator StartCharger(){
		if (weaponDetectorScript.occupied){
			yield return StartCoroutine(ResetAnimation());
			StartCoroutine (Charge());
			startShake = true;
			charger = 0f;
			chargeAnimator.speed = playbackSpeed;
			chargeAnimator.SetInteger("AnimState",1);
			chargeTimer = maxChargeTime;
		}
		yield return null;
	}

	//call this when charging and toggle,fire,death
	public IEnumerator StopCharger(){
		yield return new WaitForEndOfFrame();
		charge = false;
		chargeAnimator.speed = 0f;
		chargeTimer = maxChargeTime;
		charger = 0f;
		transform.position = setSpot;
	}

	public IEnumerator ResetAnimation(){
		yield return new WaitForEndOfFrame();
		chargeAnimator.speed = playbackSpeed;
		chargeAnimator.SetInteger("AnimState",0);
	}

	public IEnumerator ShakeAnimation(){
		startShake = false;
		chargeAnimator.SetInteger("AnimState",2);
		yield return null;
	}

	public IEnumerator Charge(){
		while (chargeTimer>0 && controller.rightShoot>0) {
			chargeTimer -= Time.deltaTime; //countdown!
			charger += Time.deltaTime * (100f/maxChargeTime);
			
			if (chargeTimer<shakeTime){
				if (startShake) {
					StartCoroutine(ShakeAnimation());
				}
				shaking = true;
				transform.position = setSpot + new Vector3 (Random.insideUnitCircle.x,Random.insideUnitCircle.y,0) * ((shakeTime - chargeTimer)/(shakeTime)) * maxShakeSize;
			}
			yield return new WaitForEndOfFrame();
		}
		shaking = false;
		StartCoroutine (weaponDetectorScript.LaunchOverIt ());
		StartCoroutine(ResetAnimation());
		yield return null; 
	}

	void Update(){
		if (!shaking) {
			transform.position = setSpot;
		}
	}
}