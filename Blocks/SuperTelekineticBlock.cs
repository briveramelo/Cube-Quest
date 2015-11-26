using UnityEngine;
using System.Collections;
using System.Linq;

public class SuperTelekineticBlock : MonoBehaviour {

	//does magnetic hold, hover spot

	public GameObject[] allGameObjects;
	public GameObject character;
	
	public Collider2D[] allObjects;
	public RaycastHit2D[] allfloaters;
	
	public WeaponBlockScript weaponBlockScript;
	public WeaponDetectorScript weaponDetectorScript;
	public Controller controller;
	public Player player;
	public ArmRotate armRotate;
	
	public Transform wepDTran;
	public Transform heroSpot;

	private Quaternion heroRotation;

	public RaycastHit2D[] rayThings;
	public RaycastHit2D[] rayExps;
	public RaycastHit2D[] lines;
	
	public string[] dontBlowStrings;
	public string[] weaponStrings;
	public string characterName;
	
	public Quaternion targetRot;
	public Quaternion bodyTarg;

	public Vector3 moveDir;
	public Vector3 pointDir;
	public Vector3 playerStickDir;
	public Vector3 playerStickPoint;
	public Vector3 blockStickDir;
	public Vector3 blockStickPoint;
	
	public Vector3 feetDir;
	public Vector3 hitPointThing;
	public Vector3 hitPointExp;
	public Vector3 holdSpot;
	public Vector3 holdDir;
	public Vector3 crossDir;
	public Vector3 bPos;
	public Vector3 thingPos;
	public Vector3 exDir;
	public Vector3 lastFDir;
	
	public Vector2 maxFloatVelocity;
	public Vector2 velocity;

	public float[] timeCheck;
	public float triggerDelay;
	public float maxAway;
	public float minAway;
	public float newHoldRadius;
	public float change;
	public float exMultiplier;
	public float exForceBaseline;
	public float exForce;
	public float separationDist;
	public float holdDist1;
	public float holdDist2;
	public float loseRadius;
	public float holdAwayRadius;
	public float brakeForce;
	public float maxVelocity;
	public float vel;
	public float accel;
	public float lastVel;
	public float pullInForce;
	public float brakeSpeedFactor;
	public float brakeDistanceFactor;
	public float pullFactor;
	public float brakeTorque;
	public float stickForce;
	public float playerHeight;
	public float stickHeight;
	public float playerBrakeSpeedFactor;
	public float playerBrakeDistanceFactor;
	public float playerPullForce;
	public float playerPullFactor;
	public float playerHoverDist;
	public float blockHoverDist;
	public float hoverLoseRadius;
	public float maxPlayerVelocity;
	public float playerVel;
	public float playerBrakeForce;
	public float exRadius;
	public float rotationAng;
	public float angleTol1;
	public float angleTol2;
	public float rotoSpeed;
	public float floatingSpeed;
	public float leftAngle;
	public float liftForce;
	public float angleTol;
	public float recoRotoSpeed;
	public float corRotoSpeed;
	public float targRotAng;
	public float aimTol;
	public float startingAng;
	public float startingHoldAwayRadius;
	public int startingHealth;
	public int currentHealth;
	
	public bool magneto;
	public bool teleHovering;
	public bool rotating;
	public bool flat;
	public bool flattening;
	public bool recovering;
	public bool orbiting;
	public bool pushing;
	
	public int side;
	public int timeInt;
	public int i;
	public int j;
	public int p;
	public int holdCount;
	public int numNull;
	public int wepDSID;
	

	// Use this for initialization
	void Awake () {

		timeCheck = new float[]{Time.realtimeSinceStartup,0}; //reset to try again
		timeInt=0;

		maxVelocity = 3f;
		brakeForce = 2000f;
		holdAwayRadius = 1f;
		aimTol = 0.1f;
		rotoSpeed = 2f;
		corRotoSpeed = 1f;
		maxAway = 3f;
		minAway = 0.2f;
		angleTol = 3f;
		triggerDelay = 0.5f;
		timeInt = 0;
		i = 0;
		timeCheck = new float[]{0,0};
		
		rotating = false;
		recovering = false;
		pushing = false;
		teleHovering = false;


		startingHealth = 50;
		currentHealth = startingHealth;
		liftForce = 75f;
		recoRotoSpeed = 25f;
		angleTol = 3f;
		
		weaponBlockScript = GetComponent<WeaponBlockScript> ();
		
		angleTol1 = 5f;
		angleTol2 = 3f;
		
		maxFloatVelocity = new Vector2 (3f, 3f);
		floatingSpeed = 200f;
		exRadius = 7f;
		playerPullForce = 3000f;
		
		exForceBaseline = 300f;
		separationDist = .3f;
		maxPlayerVelocity = 7f;
		playerBrakeForce = 2500f;
		
		//holding out
		brakeForce = 1000f;
		brakeTorque = 5000f;
		pullInForce = 7500f;

		startingHoldAwayRadius = 1f;
		holdAwayRadius = startingHoldAwayRadius;
		loseRadius = 6f;
		hoverLoseRadius = .2f;
		holdCount = 0;
		playerHeight = .43f;
		stickHeight = 0.55f;
		
		side = 0; //1=top, 2=right, 3=bottom, 4=left
		maxVelocity = 3f;
		magneto = true;
		teleHovering = false;
		rotating = false;
		flattening = false;
		orbiting = false;
		flat = false;
		dontBlowStrings = new string[] {"TestPlatform","Arm1","WeaponDetector1","Arm2","WeaponDetector2","TractorBeam1","TractorBeam2","Shield1","Shield2"};
		weaponStrings = new string[] {"SuperNeutral","SuperExploder","Neutral","Exploder","Frozesploder","MiniCopter","Telekinetic"};
		GameObject.Find ("theOmniscient").GetComponent<GatherAllKnowledge> ().numTelekinetic ++;
	}

	public IEnumerator Welcome(){ //Gets the player controller IDs
		transform.localScale = Vector3.one * 3f;
		//blows away things a little bit for a fun entry effect, and also to clear space
		rigidbody2D.gravityScale = 0;
		if (wepDSID == 1) {
			characterName = "Hero1";
			wepDTran = GameObject.Find ("WeaponDetector1").transform;
			character = GameObject.Find("Hero1");
			armRotate = GameObject.Find("Arm1").GetComponent<ArmRotate>();
		}
		else if (wepDSID == 2){
			characterName = "Hero2";
			wepDTran = GameObject.Find ("WeaponDetector2").transform;
			character = GameObject.Find("Hero2");
			armRotate = GameObject.Find("Arm2").GetComponent<ArmRotate>();
		}
		weaponDetectorScript = wepDTran.GetComponent<WeaponDetectorScript>();
		controller = character.GetComponent<Controller>();
		player = character.GetComponent<Player>();
		heroSpot = armRotate.transform;
		heroRotation = character.transform.rotation;
		allObjects = Physics2D.OverlapCircleAll (transform.position,exRadius);
		allGameObjects = new GameObject[allObjects.Length];
		
		i = 0;
		foreach (Collider2D coller in allObjects){
			allGameObjects[i] = coller.gameObject;
			i++;
		}

		i = 0;
		foreach (Collider2D thing in allObjects){
			if (!dontBlowStrings.Contains(thing.name) && thing.name != characterName && thing.gameObject != this.gameObject){
				bPos = transform.position;
				thingPos = thing.transform.position;
				exDir = Vector3.Normalize(thingPos - bPos);
				
				float dist2exploder = Vector3.Distance(bPos,thingPos);
				
				rayThings = Physics2D.RaycastAll (bPos,exDir,dist2exploder);
				foreach (RaycastHit2D rayThing in rayThings){
					if (rayThing.collider.gameObject == thing.gameObject){
						hitPointThing = rayThing.point;
						break;
					}
				}
				rayExps = Physics2D.RaycastAll (thingPos,-exDir,dist2exploder);
				foreach (RaycastHit2D rayExp in rayExps){
					if (rayExp.collider.gameObject == this.gameObject){
						hitPointExp = rayExp.point;
						break;
					}
				}
				
				dist2exploder = Vector3.Distance(hitPointExp,hitPointThing);
				exMultiplier = Mathf.Exp(-.6f * dist2exploder);
				
				exForce = exForceBaseline * exMultiplier;
				
				if (weaponStrings.Contains (thing.tag)){
					if (thing.GetComponent<WeaponBlockScript>()){
						if (thing.GetComponent<WeaponBlockScript>().toggleCount<=0){
							thing.rigidbody2D.AddForce ( exDir * exForce ) ;
						}
					}
					else if (thing.GetComponent<MiniCopterBlock>()){
						if (thing.GetComponent<MiniCopterBlock>().toggleCount<=0){
							thing.rigidbody2D.AddForce ( exDir * exForce ) ;
						}
					}
				}
				else {
					if (thing.rigidbody2D){
						thing.rigidbody2D.AddForce ( exDir * exForce ) ;
					}
				}
			}
			i++;
		}
		yield return StartCoroutine(Telekinize());
		
	}

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
		rotationAng = transform.rotation.eulerAngles.z;
		
		if (rotationAng < angleTol1 || 360f - rotationAng < angleTol1 || Mathf.Abs(90f - rotationAng) < angleTol1 || Mathf.Abs(180f - rotationAng) < angleTol1 || Mathf.Abs(270f - rotationAng) < angleTol1) {
			flat = true;
		}
		else{
			flat = false;
		}
		
		if (flat && !teleHovering){
			if (rotationAng < angleTol1 || 360f - rotationAng < angleTol1){
				side = 1;
			}
			else if (Mathf.Abs (90f - rotationAng) < angleTol1){
				side = 4;
			}
			else if (Mathf.Abs (180f - rotationAng) < angleTol1){
				side = 3;
			}
			else if (Mathf.Abs (270f - rotationAng) < angleTol1){
				side = 2;
			}
		}
		
		if (!teleHovering && flat) {
			if (col.gameObject == character){
				allfloaters = Physics2D.LinecastAll(transform.position+Vector3.up*playerHeight-Vector3.right*.1f,transform.position+Vector3.up*playerHeight+Vector3.right*.1f);
				foreach (RaycastHit2D floater in allfloaters){
					if (floater.collider.gameObject == character){
						StartCoroutine(Flatten());
					}
				}
			}
		}
	}
	
	public IEnumerator Flatten(){
		
		switch (side) {
		case 1:
			targetRot = Quaternion.Euler(0f,0f,0f);
			break;
		case 2:
			targetRot = Quaternion.Euler(0f,0f,270f);
			break;
		case 3:
			targetRot = Quaternion.Euler(0f,0f,180f);
			break;
		case 4:
			targetRot = Quaternion.Euler(0f,0f,90f);
			break;
		}
		
		flattening = true;
		while (flattening) {
			rotationAng = Mathf.Abs (transform.rotation.eulerAngles.z);
			transform.rotation = Quaternion.Slerp(transform.rotation,targetRot,Time.deltaTime*recoRotoSpeed);
			if (rotationAng-targetRot.eulerAngles.z<angleTol2 || targetRot.eulerAngles.z-rotationAng<angleTol2){
				transform.rotation = Quaternion.identity;
				if (transform.rotation == Quaternion.identity){
					rigidbody2D.fixedAngle = true;
					//fix others too
					flattening = false;
					StartCoroutine(HoverSpot());
				}
			}
			yield return null;
		}
	}
	
	void OnDrawGizmos(){
		Gizmos.DrawWireSphere(playerStickPoint,.1f);
	}

	public IEnumerator HoverSpot(){
		teleHovering = true;
		player.teleHovering = true;
		
		character.rigidbody2D.gravityScale = 0;
		
		while (teleHovering && magneto){
			velocity = rigidbody2D.velocity;
			vel = Vector2.Distance(Vector2.zero,rigidbody2D.velocity);
			playerVel = Vector2.Distance(Vector2.zero,character.rigidbody2D.velocity);
			
			//direction feet point (down)
			feetDir = Vector3.Normalize(new Vector3( -Mathf.Sin(heroRotation.eulerAngles.z * Mathf.Deg2Rad) , Mathf.Cos(heroRotation.eulerAngles.z * Mathf.Deg2Rad) , 0f));

			playerStickPoint = transform.position + feetDir * stickHeight;
			blockStickPoint = heroSpot.position - feetDir * stickHeight;
			
			//Debug.DrawLine(transform.position,blockStickPoint);
			//Debug.DrawLine(heroSpot.position,playerStickPoint);
			
			playerStickDir = Vector3.Normalize(playerStickPoint-heroSpot.position);
			blockStickDir = Vector3.Normalize(blockStickPoint-transform.position);
			
			playerHoverDist = Vector3.Distance (heroSpot.position, playerStickPoint);
			blockHoverDist = Vector3.Distance (transform.position, blockStickPoint);
			
			
			playerBrakeSpeedFactor = playerVel/maxPlayerVelocity; //0-1
			if (playerBrakeSpeedFactor>1){
				playerBrakeSpeedFactor = 1;
			}
			playerPullFactor = playerHoverDist/hoverLoseRadius;//0-1
			playerBrakeDistanceFactor = 1-playerPullFactor;
			
			
			brakeSpeedFactor = vel/maxVelocity;
			if (brakeSpeedFactor > 1){
				brakeSpeedFactor = 1;
			}
			pullFactor = blockHoverDist/hoverLoseRadius;
			brakeDistanceFactor = 1-pullFactor;
			
			if (playerHoverDist>hoverLoseRadius || blockHoverDist>hoverLoseRadius){
				teleHovering = false;
				player.teleHovering = false;
				StartCoroutine (player.StandUpStraight());
				StartCoroutine(weaponDetectorScript.GravityMonitor());
			}
			
			//rigidbody2D.AddForce (blockStickDir * pullFactor * pullInForce * Time.deltaTime);
			//rigidbody2D.AddForce (-Vector2.ClampMagnitude(rigidbody2D.velocity,1f) * brakeSpeedFactor * brakeDistanceFactor * brakeForce * Time.deltaTime);
			
			if ( (controller.horAx>0 && velocity.x <=0) || (controller.horAx<0 && velocity.x >=0) || (velocity.x > -maxFloatVelocity.x && velocity.x <= 0) || (velocity.x < maxFloatVelocity.x && velocity.x >= 0)) {
				rigidbody2D.AddForce(Vector2.right*floatingSpeed*controller.horAx*Time.deltaTime);
			}
			if ( (controller.vertAx>0 && velocity.y <=0) || (controller.vertAx<0 && velocity.y >=0) || (velocity.y > -maxFloatVelocity.y && velocity.y <= 0) || (velocity.y < maxFloatVelocity.y && velocity.y >= 0)) {
				rigidbody2D.AddForce(Vector2.up*floatingSpeed*controller.vertAx*Time.deltaTime);
			}
			
			
			character.rigidbody2D.AddForce(playerStickDir * playerPullFactor * playerPullForce * Time.deltaTime);
			character.rigidbody2D.AddForce(-Vector2.ClampMagnitude(character.rigidbody2D.velocity,1f) * playerBrakeSpeedFactor * playerBrakeDistanceFactor * playerBrakeForce * Time.deltaTime);
			yield return null;
		}
		holdAwayRadius = startingHoldAwayRadius;
		character.rigidbody2D.gravityScale = 1;
		StartCoroutine (weaponDetectorScript.GravityMonitor ());
		yield return null;
	}
	
	public IEnumerator Telekinize(){
		while (magneto) {
			if (!teleHovering){
				vel = Vector2.Distance(Vector2.zero,rigidbody2D.velocity);
				bPos = transform.position;
				
				holdSpot = wepDTran.position + weaponDetectorScript.fDir*holdAwayRadius;
				
				holdDist1 = Vector3.Distance(bPos,holdSpot);
				holdDist2 = Vector3.Distance(bPos,wepDTran.position);
				//Debug.DrawLine(wepDTran.position,holdSpot);
				
				holdDir = Vector3.Normalize(holdSpot - bPos);
				
				brakeSpeedFactor = vel/maxVelocity; //0-1
				brakeDistanceFactor = 1-holdDist1/loseRadius;
				pullFactor = holdDist1/loseRadius;//0-1
				
				if (holdDist2>loseRadius){
					magneto = false;
					rigidbody2D.gravityScale = 1f;
					GetComponent<Animator>().SetInteger("AnimState",1);
					weaponDetectorScript.teleDisconnect.Play();
				}
				
				rigidbody2D.AddForce(-Vector2.ClampMagnitude(rigidbody2D.velocity,1f) * brakeSpeedFactor * brakeDistanceFactor * brakeForce * Time.deltaTime);
				rigidbody2D.AddForce(holdDir * pullInForce * pullFactor * Time.deltaTime);
			}
			yield return null;
		}
		StartCoroutine (player.StandUpStraight());
		StartCoroutine (SelfDestruct ());
		yield return null;
	}

	public IEnumerator RotateBodies(){
		rotating = true;
		rigidbody2D.fixedAngle = false;
		character.rigidbody2D.fixedAngle = false;
		startingAng = transform.rotation.eulerAngles.z;
		armRotate.rotoSpeed = rotoSpeed;
		
		while (teleHovering && controller.leftShoot>0){
			if (Mathf.Abs (controller.horAx2)<aimTol && Mathf.Abs (controller.vertAx2)<aimTol){
				if (heroRotation.eulerAngles.z<angleTol || 360-heroRotation.eulerAngles.z<angleTol){
					heroRotation = Quaternion.identity;
				}
				else{ 
					if (heroSpot.eulerAngles.z>0f && heroSpot.eulerAngles.z<180f){
						targRotAng = heroSpot.eulerAngles.z-90f;
					}
					else{
						targRotAng = heroSpot.eulerAngles.z+90f;
					}
					heroRotation = Quaternion.Slerp(heroRotation,Quaternion.Euler(0f,0f,targRotAng),Time.deltaTime*corRotoSpeed);
					transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.identity,Time.deltaTime*corRotoSpeed);
				}
				if (transform.rotation.eulerAngles.z<angleTol || 360-transform.rotation.eulerAngles.z<angleTol){
					transform.rotation = Quaternion.identity;
				}
			}
			else{
				bodyTarg = Quaternion.Euler(0f,0f,armRotate.target.eulerAngles.z-90f);
				heroRotation = Quaternion.Slerp(heroRotation,bodyTarg,Time.deltaTime*rotoSpeed);
				transform.rotation = Quaternion.Slerp(transform.rotation,bodyTarg,Time.deltaTime*rotoSpeed);
			}
			
			yield return null;
		}
		armRotate.rotoSpeed = armRotate.startingRotoSpeed;
		character.rigidbody2D.fixedAngle = true;
		rotating = false;
		yield return null;
	}

	public IEnumerator DoubleReset(){
		timeCheck = new float[]{0,0}; //reset to try again
		timeInt=0;
		yield return null;
	}

	public IEnumerator DoubleTriggerCheck(){
		if (timeInt == 1) {
			if(Time.realtimeSinceStartup-timeCheck[0]>2f){
				StartCoroutine(DoubleReset());
			}
		}

		timeCheck[timeInt] = Time.realtimeSinceStartup;
		if (timeInt<2){
			if (timeInt==0){
				timeInt=1;
			}
			else{	
				if(timeCheck[1]-timeCheck[0]<triggerDelay){ //if it passes the test (2 quick clicks)
					if (teleHovering){
						StartCoroutine(RotateBodies());
					}
					else{
						StartCoroutine(PushAndPull());
					}
				}
				StartCoroutine(DoubleReset());
			}
		}
		yield return null;
	}
	
	public IEnumerator PushAndPull(){
		pushing = true;
		weaponDetectorScript.pushing = pushing;

		while (controller.leftShoot>0) {
			pointDir = new Vector3 (controller.horAx2, controller.vertAx2,0f);
			moveDir = Vector3.Project(pointDir,weaponDetectorScript.fDir);
			change = 1-Vector3.Distance (moveDir, weaponDetectorScript.fDir);
			newHoldRadius += change;

			if (newHoldRadius>maxAway){
				newHoldRadius = maxAway;
			}
			else if (newHoldRadius<minAway){
				newHoldRadius = minAway;
			}

			if (magneto){
				holdAwayRadius = Mathf.Lerp(holdAwayRadius,newHoldRadius,Time.deltaTime);
			}
			yield return null;
		}

		pushing = false;
		weaponDetectorScript.pushing = pushing;
		yield return null;
	}

	public IEnumerator SelfDestruct(){
		yield return new WaitForSeconds (90f);
		Destroy (this.gameObject);
	}
}
