using UnityEngine;
using System.Collections;
using System.Linq;

public class JumpBlock : MonoBehaviour {

	public GetDamage getDamage;
	public GameObject character1;
	public GameObject character2;

	public Animator animator;

	private RaycastHit2D[] aboveObjects1;
	private RaycastHit2D[] aboveObjects2;
	private RaycastHit2D[] aboveObjects;

	private RaycastHit2D[] belowObjects1;
	private RaycastHit2D[] belowObjects2;
	private RaycastHit2D[] belowObjects3;
	private RaycastHit2D[] belowObjects;

	public Collider2D[] platforms;
	public Collider2D[] hitObjects;

	private string[] hurtThings;

	private Vector3 p1Pos;
	private Vector3 p2Pos;
	private Vector3 pPos;
	private Vector3 jPos;
	private Vector3 aDir;
	public Vector3 jumpVectorR;
	public Vector3 jumpVectorL;
	public Vector3 attackVectorR;
	public Vector3 attackVectorL;

	private Vector2 spot1;
	private Vector2 spot2;
	private Vector2 spot3;
	private Vector2 spot4;
	private Vector2 spot5;

	private Vector2 botLeft;
	private Vector2 topRight;

	public float dist2Player1;
	public float dist2Player2;
	public float dist2Player;
	public float spinDist;
	public float stompDist;


	public float dangerZone;
	public float time2Spin;
	public float spinTimer;
	public float vely;
	public float startingFriction;
	public float startingBounce;
	public float jForce;
	public float aForce;
	public float jumpTimer;
	public float time2Jump;
	public float stompForce;
	public float rotateTimer;
	public float time2Rotate;
	public float rotoSpeed;
	public float stompTimer;
	public float time2Stomp;
	public float recoveryTimer;
	public float time2Recover;

	public int spinDamage;
	public int stompDamage;
	public int currentHealth;
	public int startingHealth;
	public int blockType;

	public bool startReRotating;
	public bool attack;
	public bool ready2Attack;
	public bool standing;
	public bool allowJump;
	public bool spinAttack;
	public bool spinAttacking;
	public bool stompAttacking;
	public bool stompAttack;
	public bool char1;
	public bool char2;

	// Use this for initialization
	void Awake () {
		blockType = -2;
		startingHealth = 10;
		currentHealth = startingHealth;

		startingBounce = rigidbody2D.collider2D.sharedMaterial.bounciness;
		startingFriction = rigidbody2D.collider2D.sharedMaterial.friction;

		char1 = false;
		char2 = false;
		dist2Player1 = 100f;
		dist2Player2 = 100f;

		if (GameObject.Find ("Hero1")) {
			char1 = true;
			character1 = GameObject.Find ("Hero1");
		}
		if (GameObject.Find ("Hero2")){
			char2 = true;
			character2 = GameObject.Find ("Hero2");
		}


		getDamage = GetComponent<GetDamage> ();

		dangerZone = 2.5f;
		spinDist = .6f;
		stompDist = 3f;
		ready2Attack = false;
		attack = false;
		attackVectorR = Vector3.Normalize(new Vector3 (1, 4,0));
		attackVectorL = Vector3.Normalize(new Vector3 (-1, 4,0));
		aForce = 300f;

		allowJump = true;
		time2Jump = 1f;
		jForce = 200f;
		jumpVectorR = Vector3.Normalize(new Vector3 (1, 2,0));
		jumpVectorL = Vector3.Normalize(new Vector3 (-1, 2,0));

		standing = false;

		hurtThings = new string[] {"Player","Blank"};
		spinDamage = 15;
		stompDamage = 25;
		stompForce = 400f;
		time2Stomp = 1f;
		stompTimer = 0f;

		time2Spin = 1f;
		spinTimer = 0f;
		spinAttack = false;
		spinAttacking = false;
		time2Rotate = .4f;
		rotateTimer = time2Rotate;
		rotoSpeed = 30f;
		time2Recover = 1.5f;
		recoveryTimer = 0f;
		startReRotating = false;
	}

	private bool DetectFloor() {
		botLeft = new Vector2(-.019f+transform.position.x,-.16f+transform.position.y);
		topRight = new Vector2(.019f+transform.position.x,-.14f+transform.position.y);
		
		// Show the diagonal of the rectangular detection box
		Debug.DrawLine (botLeft, topRight);

		// All colliders existing in the rectangular detection box
		platforms = Physics2D.OverlapAreaAll (botLeft, topRight);
		standing = false;

		// checks to see that the platform in the box is not a trigger
		// if it's not a trigger, standing is truth
		foreach (Collider2D plat in platforms){
			if (plat.name.Contains ("JumpyBlock") || plat.isTrigger){
			}
			else if (!plat.isTrigger){
				standing = true;
			}
		}
		
		return standing;
	}

	void Animate(bool standing, Vector3 aDir){
		animator.SetBool ("isStanding",standing);

		if (aDir.x > 0){
			transform.localScale = new Vector3(1,1,0);
		}
		else if (aDir.x < 0){
			transform.localScale = new Vector3(-1,1,0);
		}


	}

	public IEnumerator Damage(int damageReceived){

		// add script for healthbar on this guy

		if (currentHealth - damageReceived < 0 ){
			damageReceived = currentHealth;
		}
		
		currentHealth -= damageReceived;


		if (currentHealth <= 0) {
			Destroy(this.gameObject);
		}

		yield return null;

	}
	

	// Update is called once per frame
	void Update () {
		jPos = transform.position;

		if (char1) {
			p1Pos = character1.transform.position;
			dist2Player1 = Vector3.Distance (p1Pos,jPos);
		}
		if (char2){
			p2Pos = character2.transform.position;
			dist2Player2 = Vector3.Distance (p2Pos,jPos);
		}
		if ( dist2Player1 <= dist2Player2 ){
			pPos = p1Pos;
			dist2Player = dist2Player1;
		}
		else{
			pPos = p2Pos;
			dist2Player = dist2Player2;
		}


		aDir = Vector3.Normalize(pPos - jPos);
		vely = rigidbody2D.velocity.y;

		standing = DetectFloor ();
		Animate (standing,aDir);

		//Debug.DrawLine (transform.position, transform.position + transform.localScale.x * Vector3.right * dangerZone);

		//////////ATTACKING SCRIPT////////////////
	
		if (dist2Player < dangerZone && allowJump) {
			ready2Attack = true; // time-based
			if (standing){
				attack = true; //single frame (and really, only within the frame because it gets reset the same frame)
			}
		}

		if (attack){
			allowJump = false;
			attack = false;
			ready2Attack = true;

			jumpTimer = time2Jump;

			if (aDir.x > 0 ){
				rigidbody2D.AddForce (attackVectorR * aForce);
			}
			else if (aDir.x < 0 ){
				rigidbody2D.AddForce (attackVectorL * aForce);
			}
		}

		if (ready2Attack || spinAttacking || stompAttacking){

			if (!spinAttacking && !stompAttacking && rigidbody2D.velocity.y>1f){
				spot1 = new Vector2( Mathf.Cos(Mathf.Deg2Rad * 45f) * transform.localScale.x, Mathf.Sin(Mathf.Deg2Rad * 45f) ) * spinDist + new Vector2(transform.position.x,transform.position.y);
				spot2 = new Vector2( Mathf.Cos(Mathf.Deg2Rad * 80f) * transform.localScale.x, Mathf.Sin(Mathf.Deg2Rad * 80f) ) * spinDist + new Vector2(transform.position.x,transform.position.y);
				Debug.DrawLine (transform.position, new Vector3 (spot1.x,spot1.y,0));
				Debug.DrawLine (transform.position, new Vector3 (spot2.x,spot2.y,0));

				aboveObjects1 = Physics2D.LinecastAll(transform.position,spot1);
				aboveObjects2 = Physics2D.LinecastAll(transform.position,spot2);
				aboveObjects = aboveObjects1.Concat(aboveObjects2).ToArray();

				foreach (RaycastHit2D above in aboveObjects){
					if (hurtThings.Contains (above.collider.tag)){
						spinAttack = true;
						spinAttacking = true;
						spinTimer = time2Spin;
						break;
					}
				}
			}

			if (spinAttack){
				rigidbody2D.fixedAngle = false;
				spinAttack = false;
			}

			if (spinAttacking){

				if (spinTimer>0){
					rigidbody2D.angularVelocity = 2000f;
					spinTimer -= Time.deltaTime;
					hitObjects = Physics2D.OverlapCircleAll(transform.position,.115f);
					
					foreach (Collider2D hit in hitObjects){
						if (hit.GetComponent<GetDamage>()){

							getDamage = hit.GetComponent<GetDamage>();
							getDamage.StartCoroutine(getDamage.SendDamage(spinDamage,blockType));
							spinTimer = 0f;
							rigidbody2D.velocity = new Vector2 (0,0);
							rigidbody2D.AddForce ( Vector3.Normalize(new Vector3(-(transform.localScale.x * 3),1,0)) * 155f);
							break;
						}
						else if (rigidbody2D.velocity.y<-2f){
							spinTimer = 0f;
						}
					}
				}
				 
				if (spinTimer <= 0 && rotateTimer <= 0 ){
					ready2Attack = false;
					rotateTimer = time2Rotate;
				}

				if (rotateTimer>0){
					rotateTimer -= Time.deltaTime;
					Quaternion.Slerp( transform.rotation , Quaternion.identity , Time.deltaTime * rotoSpeed); //slerp back to upright rotation and fix angle
					if (rotateTimer<=0 || (Mathf.Abs (transform.rotation.eulerAngles.z) < 2f && Mathf.Abs (rigidbody2D.angularVelocity)<2f) ){
						spinAttacking = false;
						transform.rotation = Quaternion.identity;
						rigidbody2D.fixedAngle = true;
						recoveryTimer = time2Recover;
					}
				}

			}


			//END SPINNING ATTACK


			//START STOMPING ATTACK

			if (!stompAttacking && !spinAttacking && rigidbody2D.velocity.y<-.1f){
				spot3 = new Vector2( Mathf.Cos(Mathf.Deg2Rad * 50f) * transform.localScale.x, -Mathf.Sin(Mathf.Deg2Rad * 50f) ) * stompDist + new Vector2(transform.position.x,transform.position.y);
				spot4 = new Vector2( Mathf.Cos(Mathf.Deg2Rad * 65f) * transform.localScale.x, -Mathf.Sin(Mathf.Deg2Rad * 65f) ) * stompDist + new Vector2(transform.position.x,transform.position.y);
				spot5 = new Vector2( Mathf.Cos(Mathf.Deg2Rad * 80f) * transform.localScale.x, -Mathf.Sin(Mathf.Deg2Rad * 80f) ) * stompDist + new Vector2(transform.position.x,transform.position.y);

				Debug.DrawLine (transform.position, new Vector3 (spot3.x,spot3.y,0));
				Debug.DrawLine (transform.position, new Vector3 (spot4.x,spot4.y,0));
				Debug.DrawLine (transform.position, new Vector3 (spot5.x,spot5.y,0));

				belowObjects1 = Physics2D.LinecastAll(transform.position,spot3);
				belowObjects2 = Physics2D.LinecastAll(transform.position,spot4);
				belowObjects3 = Physics2D.LinecastAll(transform.position,spot5);

				belowObjects = belowObjects1.Concat(belowObjects2).ToArray();
				belowObjects = belowObjects.Concat(belowObjects3).ToArray();

				foreach (RaycastHit2D below in belowObjects){
					if (hurtThings.Contains (below.collider.tag)){
						stompAttack = true;
						stompAttacking = true;
						stompTimer = time2Stomp;
						break;
					}
				}
			}
			
			if (stompAttack){
				rigidbody2D.AddForce (aDir * stompForce);
				if (rigidbody2D.velocity.x>0 && aDir.x<0){
					rigidbody2D.AddForce (-Vector2.right * 80f);
				}
				else if (rigidbody2D.velocity.x<0 && aDir.x>0){
					rigidbody2D.AddForce (Vector2.right * 80f);
				}

				stompAttack = false;
			}
			
			if (stompAttacking){
				if (stompTimer>0){
					hitObjects = Physics2D.OverlapAreaAll(botLeft,topRight);
					
					foreach (Collider2D hit in hitObjects){
						if (hit.GetComponent<GetDamage>()){

							getDamage = hit.GetComponent<GetDamage>();
							getDamage.StartCoroutine(getDamage.SendDamage(spinDamage,blockType));

							stompTimer = 0f;
							break;
						}
						else if (hit.CompareTag("Platform")){
							stompTimer = 0f;
						}
					}
				}
				
				if (stompTimer>0){
					stompTimer -= Time.deltaTime;
				}
				if (stompTimer<=0){
					ready2Attack = false;
					stompAttacking = false;
					recoveryTimer = time2Recover;
				}

			}



		}
		if (recoveryTimer>0){
			recoveryTimer -= Time.deltaTime;
		}
		//END STOMP ATTACK

			

		//////END ATTACKING SCRIPT//////

		//Jump Around
		if (allowJump && standing){
			allowJump = false;
			jumpTimer = time2Jump;
			
			if (aDir.x > 0){
				rigidbody2D.AddForce (jumpVectorR * jForce);
			}
			else if (aDir.x < 0){
				rigidbody2D.AddForce (jumpVectorL * jForce);
			}
		}
		
		if (standing && rigidbody2D.velocity.y<.1f){
			jumpTimer -= Time.deltaTime;
			if (jumpTimer<=0 && recoveryTimer<=0){
				allowJump = true;
			}
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