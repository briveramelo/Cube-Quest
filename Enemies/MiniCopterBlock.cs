using UnityEngine;
using System.Collections;
using System.Linq;

public class MiniCopterBlock : MonoBehaviour {

	public GetDamage getDamage;
	
	private WeaponDetectorScript[] weaponDetectorScripts;
	public Transform heroSpot1;
	public Transform heroSpot2;
	
	public Animator animator;
	public Collider2D[] thingsIHit;

	public Quaternion target;

	public Vector3 slot1;
	public Vector3 slot2;
	public Vector3 slot3;
	public Vector3 pPos; //player position
	public Vector3 p1Pos;
	public Vector3 p2Pos;
	public Vector3 bPos; //block position
	public Vector3 aDir; //attack direction
	public Vector3 hPos;
	
	public Vector2 spot;

	public float moveSpeed; //movespeed
	public float dist2Player;
	public float dist2Player1;
	public float dist2Player2;
	public float cockingTimer;
	public float time2Cock;
	public float launchForce;
	public float attackTimer;
	public float time2Attack;
	public float afterAttackTimer;
	public float time2Refract;
	public float cockSpeed;
	public float absVel;
	public float maxAbsVel;
	public float maxAbsAccel;
	public float flightHeight;
	public float rotoSpeed;
	public float lastFrameVelX;
	public float accelX;
	public float copterTilt;
	public float startingFriction;
	public float startingBounce;
	public float boxSize;
	public float damage;
	public float velThresh;
	public float floatyFactor;
	public float attackDist;
	public float baselineFire;
	public float fireSpeed;
	public float velX;
	public float angVel;
	public float maxAngVel;
	public float reboundForce;
	public float damageVelocity;
	public float colliderSpeed;
	private float defaultHeight;

	public int toggleCount;
	public int baselineDamage;
	public int startingHealth;
	public int currentHealth;
	public int wepDSID;
	public int blockType;

	public bool cocking;
	public bool attacking;
	public bool recovering;
	public bool lockedin;
	public bool allowFloat;
	public bool char1;
	public bool char2;
	public bool damageable;
	public bool delaying;


	// Use this for initialization
	void Awake () {
		startingHealth = 10;
		currentHealth = startingHealth;

		startingBounce = rigidbody2D.collider2D.sharedMaterial.bounciness;
		startingFriction = rigidbody2D.collider2D.sharedMaterial.friction;

		char1 = false;
		char2 = false;
		weaponDetectorScripts = new WeaponDetectorScript[2];

		if (GameObject.Find ("Hero1")) {
			char1 = true;
			heroSpot1 = GameObject.Find ("Arm1").transform;
			weaponDetectorScripts[0] = GameObject.Find ("WeaponDetector1").GetComponent<WeaponDetectorScript> ();
		}
		if (GameObject.Find ("Hero2")){
			char2 = true;
			heroSpot2 = GameObject.Find ("Arm2").transform;
			weaponDetectorScripts[1] = GameObject.Find ("WeaponDetector2").GetComponent<WeaponDetectorScript> ();
		}

		dist2Player1 = 100f;
		dist2Player2 = 100f;

		floatyFactor = .3f;
		maxAngVel = 10f;
		rotoSpeed = 5f;
		velThresh = 2f;
		moveSpeed = 50f;
		maxAbsVel = 2f;
		maxAbsAccel = 2f;
		cockSpeed = 30f;
		attackDist = .65f;
		time2Cock = 1.2f;
		cockingTimer = time2Cock;
		cocking = false;
		attacking = false;
		launchForce = 600f;
		time2Attack = 2f;
		attackTimer = 0f;
		baselineDamage = 20;
		boxSize = .21f;
		time2Refract = 1f;
		afterAttackTimer = 0;
		flightHeight = .35f;
		lastFrameVelX = 0f;
		damageVelocity = 2f;
		blockType = -1;
		defaultHeight = .35f;

		toggleCount = 0;

		fireSpeed = 15f;
		baselineFire = 300f;
		reboundForce = 50f;

		animator.SetInteger ("AnimState", 1);
	}

	
	//this gets called by the GetDamage Script
	public IEnumerator Damage(int damageReceived){

		//include something to animate future healthbar
		if (currentHealth - damageReceived < 0 ){
			damageReceived = currentHealth;
		}

		currentHealth -= damageReceived;

		if (currentHealth <= 0) {
			Destroy(this.gameObject);
		}

		yield return null;
		
	}

	void OnCollisionEnter2D(Collision2D col){
		damageable = false;
		if (col.rigidbody) {
			if (col.gameObject.GetComponent<GetDamage>()){
				getDamage = col.gameObject.GetComponent<GetDamage>();
				damageable = true;
			}
			colliderSpeed = col.rigidbody.velocity.magnitude;
		}
		else if (col.transform.parent){ //so if this is a child gameobject with a collider, its rigidbody is on the parent, so find it there
			if (col.transform.parent.rigidbody2D){
				if (col.transform.parent.GetComponent<GetDamage>()){
					getDamage = col.transform.parent.GetComponent<GetDamage>();
					damageable = true;
				}
				colliderSpeed = col.transform.parent.rigidbody2D.velocity.magnitude;
			}
		}
		if (damageable && !delaying){
			if (attacking && attackTimer>0 && (rigidbody2D.velocity.magnitude - colliderSpeed) > damageVelocity){ //if launching
				if (!col.collider.GetComponent<MiniCopterBlock>()){ //don't hurt other copters
					StartCoroutine(Delaying());
					StartCoroutine(getDamage.SendDamage(baselineDamage,blockType));
				}
			}
			else if (transform.localScale.x==3 && (rigidbody2D.velocity.magnitude - colliderSpeed) > damageVelocity){ //if being shot
				StartCoroutine(Delaying());
				StartCoroutine(getDamage.SendDamage(baselineDamage,blockType));
			}
		}
	}

	public IEnumerator Delaying(){
		delaying = true;
		yield return new WaitForSeconds (2f);
		delaying = false;
	}

	public IEnumerator CockTimer(){
		cocking = true;
		yield return new WaitForSeconds (time2Cock);
		cocking = false;
	}

	public IEnumerator Cock(){
		rigidbody2D.velocity = Vector3.zero;
		StartCoroutine (CockTimer ()); //see if this needs to finish

		while (cocking){
			if (rigidbody2D.velocity.magnitude<maxAbsVel){
				rigidbody2D.AddForce( -aDir * cockSpeed * Time.deltaTime);
			}
			yield return null;
		}

		StartCoroutine (ShootOnHim());
	}

	public IEnumerator ShootOnHim(){
		attacking = true;
		animator.SetInteger("AnimState",0);
		rigidbody2D.velocity = Vector3.zero;
		rigidbody2D.gravityScale = 1f;
		rigidbody2D.AddForce(aDir * launchForce);
		yield return new WaitForSeconds (time2Attack);
		attacking = false;
		StartCoroutine (RefractoryPeriod ());

	}

	public IEnumerator RefractoryTimer(){
		recovering = true;
		yield return new WaitForSeconds (time2Refract);
		animator.SetInteger("AnimState",1);
		rigidbody2D.gravityScale = 0;
	}

	public IEnumerator RefractoryPeriod (){
		yield return StartCoroutine (RefractoryTimer ());
		while (transform.localScale.x>1.1f) {
			transform.localScale = Vector3.Slerp (transform.localScale, Vector3.one,3*Time.deltaTime);
			yield return null;
		}
		transform.localScale = Vector3.one;
		recovering = false;
	}

	// Update is called once per frame
	void Update () {
		bPos = transform.position;

		if (char1) {
			p1Pos = heroSpot1.position;
			dist2Player1 = Vector3.Distance(p1Pos,bPos);
		}
		if (char2) {
			p2Pos = heroSpot2.position;
			dist2Player2 = Vector3.Distance(p2Pos,bPos);
		}
		if (dist2Player1 <= dist2Player2) {
			pPos = p1Pos;
			dist2Player = dist2Player1;
		}
		else{
			pPos = p2Pos;
			dist2Player = dist2Player2;
		}
		flightHeight = pPos.y + defaultHeight;
		aDir = Vector3.Normalize (pPos - bPos);

		velX = rigidbody2D.velocity.x;
		angVel = rigidbody2D.angularVelocity;

		accelX = (velX - lastFrameVelX) / Time.deltaTime;

		//checks for NaN errors and fixes
		if (float.IsNaN(accelX)) {
			accelX = 1f;
		}

		absVel = Mathf.Sqrt (Vector2.SqrMagnitude(rigidbody2D.velocity));

		copterTilt = -30f * (accelX / maxAbsAccel);

		if (toggleCount==0){
			if (!attacking && !recovering){
				target = Quaternion.Euler(0,0,copterTilt);
				transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotoSpeed);	
				if (!cocking){
					if (dist2Player>attackDist){ //dont overdo adding forces to get up to speed
						if (absVel<maxAbsVel){
							rigidbody2D.AddForce (aDir * moveSpeed * Time.deltaTime); //move to the player
							if (transform.position.y < flightHeight){
								rigidbody2D.AddForce (Vector2.up * moveSpeed * Time.deltaTime); //get to flightheight
							}
						}
					}
					else {
						//StartCoroutine(Cock());
					}
				}
			}

		}
		else if (toggleCount>0){
			ToggleSpot();
		}

		lastFrameVelX = rigidbody2D.velocity.x;
	}

	public IEnumerator BeFree(){
		animator.SetInteger("AnimState",0);
		//transform.localScale = Vector3.one * 3f;
		toggleCount = 0;

		collider2D.isTrigger = false;
		lockedin = false; //unlock the block. Let it be freeEEE!!
		cocking = false;
		attacking = false;
		StartCoroutine (RefractoryPeriod ());

		yield return null;
	}

	public IEnumerator LockIn(){
		cocking = false;
		recovering = false;
		attacking = false;

		lockedin = true;
		collider2D.isTrigger = true;
		toggleCount = 1;

		yield return null;
	}

	void ToggleSpot(){
		hPos = weaponDetectorScripts[wepDSID-1].hPos;
		slot1 = weaponDetectorScripts[wepDSID-1].slot1;
		slot2 = weaponDetectorScripts[wepDSID-1].slot2;
		slot3 = weaponDetectorScripts[wepDSID-1].slot3;

		switch(toggleCount){	
			
		case 1:
			transform.position = hPos;
			break;
			
		case 2:
			transform.position = slot1;
			break;
			
		case 3:
			transform.position = slot2;
			break;
			
		case 4:
			transform.position = slot3;
			break;
		}
	}


	void OnDestroy(){
		/*if (GameObject.Find ("HUD1")){
			GameObject.Find ("HUD1").GetComponent<HUD> ().numKilled += 1;
			GameObject.Find ("HUD1").GetComponent<HUD> ().showKills = true;
		}*/

	}

	void OnApplicationQuit(){
		gameObject.collider2D.sharedMaterial.friction = startingFriction;
		gameObject.collider2D.sharedMaterial.bounciness = startingBounce;
	}

}