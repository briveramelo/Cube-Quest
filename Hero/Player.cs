using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent (typeof (Controller))]
public class Player : MonoBehaviour {
	
	private GameObject[] allExploders;
	private GameObject[] allFrozeSploders;
	private GameObject arm;
	private GameObject shield;
	private GameObject tractorBeam;
	private GameObject hammer;
	private CheckPoint checkPoint;
	
	private WeaponBlockScript[] activatedBlocks;
	private WeaponBlockScript[] allWeapons;
	private GetDamage getDamage;
	private HUD hud;
	private HealthBar healthBar;
	private Controller controller;
	public WeaponDetectorScript weaponDetectorScript;
	private ChargeDisplay chargeDisplay;
	private ArmRotate armRotateScript;
	private ZoomZone zoomZone;
	private GatherAllKnowledge allKnow;


	public Transform thePlatform;


	public PolygonCollider2D[] polyCols;
	public CircleCollider2D cirCol;
	public CircleCollider2D weaponCol;

	public Collider2D[] platforms;
	private Collider2D[] wallsL;
	private Collider2D[] wallsR;
	private Collider2D[] thingsInBirthArea;
	private PolygonCollider2D tractorBeamCol;
	private PolygonCollider2D shieldCol;
	private BoxCollider2D hamCol;


	private RaycastHit2D[] floorline1;
	private RaycastHit2D[] floorline2;
	private RaycastHit2D[] floorline3;
	private RaycastHit2D[] floorline4;


	private SpriteRenderer spriteHero;
	private SpriteRenderer spriteArm;
	private SpriteRenderer spriteShield;
	private SpriteRenderer spriteHammer;


	private HingeJoint2D armHinge;
	private HingeJoint2D shieldHinge;
	private HingeJoint2D hammerHinge;


	public AudioSource death;


	private Animator heroAnimator;


	public Quaternion armRot;


	private Vector3[] spawnSpots;
	private Vector3 pPos;
	public Vector3 deathSpot;


	public Vector2[] conAnchs;
	public Vector2 maxVelocity;

	private Vector2 topLeft1;
	private Vector2 topLeft2;
	private Vector2 topLeft3;
	private Vector2 topLeft4;
	private Vector2 kDir;
	private Vector2 wallCheckSpot1;
	private Vector2 wallCheckSpot2;
	private Vector2 wallCheckSpot3;
	private Vector2 maxFloatVelocity;
	private Vector2 floorDetectionBoxHeights;
	private Vector2[] floorCheckSpots;

	public float speed;
	public float startingFriction;
	public float startingBounce;
	public float angleTol;
	public float startingRunningSpeed;
	public float startingJumpSpeed;
	public float startingMaxVelocity;
	public float runningSpeed;
	public float jumpSpeed;
	public float startingFallSpeed;
	public float floatingSpeed;
	public float charger;
	public float totalHealth;

	private float unitTimeFrame;
	private float jetSpeed;
	private float lastHorAx;
	private float forceX;
	private float forceY;
	private float fallSpeed;
	private float fallTimeDelay;
	private float lastFrameVertAx;
	private float time2Respawn;
	private float runningVelThresh;
	private float runningCrouchVelThresh;
	private float addLeftX;
	private float addRightX;
	private float addX;
	private float knockbackFactor;
	private float baselineKnockback;
	private float wallJumpLagTime;
	private float absVelx;
	private float absVely;
	private float recoRotoSpeed;
	private float liftForce;
	private float minMoveRad;


	public int currentHealth;
	public int startingHealth;
	public int damage;
	public int lives;


	private int i;
	private int j;
	private int k;
	private int m;
	private int count;
	private int dir;

	public bool crouching;
	public bool uncrouching;
	public bool onMyKnees;
	public bool startingColState;
	public bool char1;
	public bool char2;
	public bool alive;
	public bool standing;
	public bool followP1;
	public bool armAngleCorrect;
	public bool teleHovering;
	public bool copterHovering;
	public bool jetSet;
	public bool buyingWares;

	private bool wallDelay;
	private bool jumpDelay;
	private bool fastFall;
	private bool wallRPresent;
	private bool wallLPresent;
	private bool locationFreeze;
	private bool recoverRotating;
	private bool jetUsed;


	/*Animation BreakDown
	 * 0 - Standing Still
	 * 1 - Running
	 * 2 - Squatting
	 * 3 - Reverse Squatting
	 * 4 - Squatting Still
	 * 5 - Squat Running
	 */


	void Awake() {

		controller = GetComponent<Controller> ();
		getDamage = GetComponent<GetDamage> ();
		allKnow = GameObject.Find ("theOmniscient").GetComponent<GatherAllKnowledge> ();
		crouching = false;
		uncrouching = false;
		onMyKnees = false;
		Physics2D.IgnoreLayerCollision (8, 8);

		conAnchs = new Vector2[] {
			new Vector2 (0.025f,0.37f),			
			new Vector2 (0.025f, 0.315f),
			new Vector2 (0.025f, 0.2605f),
			new Vector2 (0.025f, 0.2055f),
			new Vector2 (0.015f, 0.15f),
		};

		if (GameObject.Find ("CheckPoints")) {
			checkPoint = GameObject.Find ("CheckPoints").GetComponent<CheckPoint>();
		}

		char1 = false;
		char2 = false;
		teleHovering = false;
		copterHovering = false;

		recoRotoSpeed = 25f;
		angleTol = 3f;
		liftForce = 75f;
		jetSpeed = 300f;
		unitTimeFrame = .083f;
		platforms = new Collider2D[20];
		minMoveRad = .4f;

		if (GameObject.Find("ZoomZone")){
			zoomZone = GameObject.Find("ZoomZone").GetComponent<ZoomZone>();
		}

		if (gameObject.name == "Hero1"){
			char1 = true;
			hud = GameObject.Find("HUD1").GetComponent<HUD> ();
			healthBar = GameObject.Find ("HealthBar1").GetComponent<HealthBar> ();
			arm = GameObject.Find("Arm1");
			shield = GameObject.Find("Shield1");
			hammer = GameObject.Find("Hammer1");
			tractorBeam = GameObject.Find("TractorBeam1");
			weaponDetectorScript = GameObject.Find ("WeaponDetector1").GetComponent<WeaponDetectorScript> ();
			chargeDisplay = GameObject.Find ("ChargeBar1").GetComponent<ChargeDisplay> ();
		}
		else{
			char2 = true;
			hud = GameObject.Find("HUD2").GetComponent<HUD> ();
			healthBar = GameObject.Find ("HealthBar2").GetComponent<HealthBar> ();
			arm = GameObject.Find("Arm2");
			shield = GameObject.Find("Shield2");
			hammer = GameObject.Find("Hammer2");
			tractorBeam = GameObject.Find("TractorBeam2");

			weaponDetectorScript = GameObject.Find ("WeaponDetector2").GetComponent<WeaponDetectorScript> ();
			chargeDisplay = GameObject.Find ("ChargeBar2").GetComponent<ChargeDisplay> ();

		}

		polyCols = GetComponents<PolygonCollider2D> ();
		cirCol = GetComponent<CircleCollider2D> ();

		hamCol = hammer.GetComponent<BoxCollider2D> ();
		shieldCol = shield.GetComponent<PolygonCollider2D> ();
		tractorBeamCol = tractorBeam.GetComponent<PolygonCollider2D> ();
		weaponCol = weaponDetectorScript.GetComponent<CircleCollider2D> ();

		spriteArm = arm.GetComponent<SpriteRenderer> ();
		spriteShield = shield.GetComponent<SpriteRenderer> ();
		spriteHero = GetComponent<SpriteRenderer> ();
		spriteHammer = hammer.GetComponent<SpriteRenderer> ();

		startingBounce = cirCol.sharedMaterial.bounciness;
		startingFriction = cirCol.sharedMaterial.friction;
		armRotateScript = arm.GetComponent<ArmRotate> ();
		armRot = arm.transform.rotation;
		armHinge = arm.GetComponent<HingeJoint2D>();
		shieldHinge = shield.GetComponent<HingeJoint2D>();
		hammerHinge = hammer.GetComponent<HingeJoint2D> ();

		alive = true;
		//transform.position = Vector3.zero;

		if (Application.loadedLevelName == "BattleArena"){
			spawnSpots = new Vector3[6];
			spawnSpots[0] = new Vector3 (0f,-2f,0f);
			spawnSpots [1] = new Vector3 (-2f,-1f,0f);
			spawnSpots [2] = new Vector3 (2f,-1f,0f);
			spawnSpots [3] = new Vector3 (0f,0f,0f);
			spawnSpots [4] = new Vector3 (0f,-2f,0f);
			spawnSpots [5] = new Vector3 (0f,-2f,0f);
		}
		else if (Application.loadedLevelName == "BattleShrine"){
			spawnSpots = new Vector3[6];
			spawnSpots [0] = new Vector3 (0f,2.5f,0f);
			spawnSpots [1] = new Vector3 (-1f,0f,0f);
			spawnSpots [2] = new Vector3 (1f,0f,0f);
			spawnSpots [3] = new Vector3 (0f,-2.5f,0f);
			spawnSpots [4] = new Vector3 (-3.5f,-.75f,0f);
			spawnSpots [5] = new Vector3 (3.5f,-.75f,0f);
		}
		else{
			spawnSpots = new Vector3[6];
			spawnSpots [0] = new Vector3 (0f,-2f,0f);
			spawnSpots [1] = new Vector3 (-2f,-1f,0f);
			spawnSpots [2] = new Vector3 (2f,-1f,0f);
			spawnSpots [3] = new Vector3 (0f,0f,0f);
			spawnSpots [4] = new Vector3 (0f,-2f,0f);
			spawnSpots [5] = new Vector3 (0f,-2f,0f);
		}


		wallJumpLagTime = .7f;

		runningVelThresh = .3f;
		runningCrouchVelThresh = .1f;
		wallDelay = false;

		heroAnimator = GetComponent<Animator>();

		time2Respawn = 2f;

		startingHealth = 100;

		fallTimeDelay = 1f;
		runningSpeed = 7.5f;
		floatingSpeed = runningSpeed;
		jumpSpeed = 200f;
		fallSpeed = -50f;
		maxVelocity = new Vector2 (2f, 5f);
		maxFloatVelocity = new Vector2 (2.5f, 2.5f);
		floorDetectionBoxHeights = new Vector2 (0.023f,0.2f);
		forceX = 0;
		forceY = 0;
		i = 0;
		damage = 0;


		startingJumpSpeed = jumpSpeed;
		startingRunningSpeed = runningSpeed;
		startingFallSpeed = fallSpeed;
		startingMaxVelocity = maxVelocity.x;

		followP1 = true;
	}
	
	//this gets called by the Damage IEnumerator in this script
	public IEnumerator Death(){
		alive = false;
		lives -= 1;
		hud.lives -= 1;
		allKnow.lives -= 1;
		deathSpot = transform.position;

		if (lives<=0){
			Caching.CleanCache();
			Application.Quit();
			//UnityEditor.EditorApplication.isPlaying = false;
		}

		if (checkPoint) {
			StartCoroutine(checkPoint.ResetToCheckPoint());
		}

		death.Play ();
		StartCoroutine (weaponDetectorScript.DestroyHeld ());
		StartCoroutine (weaponDetectorScript.CatalogWeapons ());
		StartCoroutine (weaponDetectorScript.Distract ());
		StartCoroutine (weaponDetectorScript.StopChargeNoise ());
		StartCoroutine (chargeDisplay.StopCharger ());
		StartCoroutine (chargeDisplay.ResetAnimation ());
		StartCoroutine (ReverseCrouch ());

		getDamage.enabled = false;
		spriteHero.enabled = false;
		spriteArm.enabled = false;
		spriteShield.enabled = false;
		spriteHammer.enabled = false;

		rigidbody2D.velocity = new Vector2 (0f,0f);
		rigidbody2D.gravityScale = 0f;
		weaponCol.enabled = false;
		shieldCol.enabled = false;
		weaponDetectorScript.occupied = true;

		if (GetComponent<Unfreeze> ()) {
			Component.Destroy(gameObject.GetComponent<Unfreeze>());
		}

		foreach (Collider2D col in GetComponents<Collider2D>()) {
			col.enabled = false;
		}

		runningSpeed = 0f;
		jumpSpeed = 0f;
		fallSpeed = 0f;
		maxVelocity = Vector2.zero;
		rigidbody2D.Sleep ();
		locationFreeze = true;

		yield return new WaitForSeconds (time2Respawn);
		StartCoroutine (Rebirth ());
		if (Application.loadedLevelName == "HeliRoom") {
			HeliRoom heliRoom = GameObject.Find("HeliRoom_Manager").GetComponent<HeliRoom>();
			StartCoroutine(heliRoom.SpawnCopters());
		}
	}

	public IEnumerator Rebirth(){
		int rando = Random.Range (0,6);

		if (!followP1){
			transform.position = spawnSpots[rando];
		}
		else{
			transform.position = deathSpot;
		}
		transform.rotation = Quaternion.identity;

		locationFreeze = false;
		rigidbody2D.WakeUp ();
		spriteHero.enabled = true;
		spriteArm.enabled = true;
		spriteShield.enabled = false;
		getDamage.enabled = true;
		weaponDetectorScript.occupied = false;
		weaponDetectorScript.collider2D.enabled = true;
		controller.weaponState = 0;
		tractorBeamCol.enabled = true;
		crouching = false;
		uncrouching = false;
		onMyKnees = false;


		if (GetComponent<Unfreeze> ()) {
			Component.Destroy (gameObject.GetComponent<Unfreeze> ());
		}

		cirCol.enabled = false;

		foreach (Collider2D col in polyCols) {
			col.enabled = false;
		}
	
		weaponCol.enabled = true;
		shieldCol.enabled = false;

		StartCoroutine (Unfreeze ());
		StartCoroutine (ResetStand ());


		rigidbody2D.fixedAngle = true;
		rigidbody2D.gravityScale = 1f;
		currentHealth = startingHealth;
		rigidbody2D.velocity = new Vector2 (0f,0f);

		StartCoroutine (healthBar.RebirthHealth ());
		
		alive = true;
		controller.weaponState = 2;
		StartCoroutine (armRotateScript.SwitchHands ());
		if (zoomZone) {
			StartCoroutine (zoomZone.SetOldHeight());
		}
		yield return null;
	}

	public IEnumerator Unfreeze(){
		runningSpeed = startingRunningSpeed;
		jumpSpeed = startingJumpSpeed;
		fallSpeed = startingFallSpeed;
		maxVelocity.x = startingMaxVelocity;

		foreach (Collider2D col in GetComponents<Collider2D>()) {
			startingColState = col.enabled;
			col.enabled = false;
			col.sharedMaterial.bounciness = startingBounce;
			col.sharedMaterial.friction = startingFriction;
			col.enabled = startingColState;
		}
	
		yield return null;
	}

	//this gets called by the GetDamage Script
	public IEnumerator Damage(int damageReceived){
		count += 1;

		if (currentHealth - damageReceived < 0 ){
			damageReceived = currentHealth;
		}
		
		currentHealth -= damageReceived;
		StartCoroutine (healthBar.AnimateHealth (damageReceived));
		//getting called twice for some reason. This could be causing the noanimation/notimer glitch
	
		if (currentHealth <= 0 && alive) {
			StartCoroutine (Death ());
		}

		yield return null;
	}

	public IEnumerator Jump(){
		yield return StartCoroutine (DetectFloor ());
		
		if (standing){
			rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x,0);
			rigidbody2D.AddForce (new Vector2 (0, jumpSpeed));
		}
		yield return null;
	}

	public IEnumerator DetectFloor() {
		//DONT USE A BOX! THEY'RE TOO SMALL TO DETECT PROPERLY. USE LINES!
		/*if (transform.localScale.x > 0) {
			addLeftX =  0.000f;
			addRightX = 0.045f;
		}
		else {
			addLeftX = -0.045f;
			addRightX = 0.000f;
		}*/

		floorCheckSpots = new Vector2[8];
		floorCheckSpots = new Vector2[]{
			new Vector2 (transform.position.x + transform.localScale.x * (cirCol.center.x + floorDetectionBoxHeights.x), transform.position.y),
			new Vector2 (transform.position.x + transform.localScale.x * (cirCol.center.x + floorDetectionBoxHeights.x), transform.position.y - floorDetectionBoxHeights.y),
			new Vector2 (transform.position.x + transform.localScale.x * (cirCol.center.x + .5f*floorDetectionBoxHeights.x), transform.position.y),
			new Vector2 (transform.position.x + transform.localScale.x * (cirCol.center.x + .5f*floorDetectionBoxHeights.x), transform.position.y - floorDetectionBoxHeights.y),
			new Vector2 (transform.position.x - transform.localScale.x * (cirCol.center.x - floorDetectionBoxHeights.x), transform.position.y),
			new Vector2 (transform.position.x - transform.localScale.x * (cirCol.center.x - floorDetectionBoxHeights.x), transform.position.y - floorDetectionBoxHeights.y),
			new Vector2 (transform.position.x - transform.localScale.x * (cirCol.center.x - .5f*floorDetectionBoxHeights.x), transform.position.y),
			new Vector2 (transform.position.x - transform.localScale.x * (cirCol.center.x - .5f*floorDetectionBoxHeights.x), transform.position.y - floorDetectionBoxHeights.y)
		};

		floorline1 = Physics2D.LinecastAll (floorCheckSpots[0],floorCheckSpots[1]);
		floorline2 = Physics2D.LinecastAll (floorCheckSpots[2],floorCheckSpots[3]);
		floorline3 = Physics2D.LinecastAll (floorCheckSpots[4],floorCheckSpots[5]);
		floorline4 = Physics2D.LinecastAll (floorCheckSpots[6],floorCheckSpots[7]);

		platforms = new Collider2D[20];
		thePlatform = null;
		i = 0;
		foreach (RaycastHit2D ray in floorline1) {
			if (ray.collider){
				platforms[i] = ray.collider.collider2D;
				i++;
			}
		}
		foreach (RaycastHit2D rays in floorline2) {
			if (rays.collider){
				platforms[i] = rays.collider.collider2D;
				i++;
			}
		}
		foreach (RaycastHit2D ray in floorline3) {
			if (ray.collider){
				platforms[i] = ray.collider.collider2D;
				i++;
			}
		}
		foreach (RaycastHit2D ray in floorline4) {
			if (ray.collider){
				platforms[i] = ray.collider.collider2D;
				i++;
			}
		}
		standing = false;
		m = 0;
		while (m<i){
			if (!platforms[m].isTrigger || platforms[m].tag.Contains ("Platform")) {
				//if (plat != cirCol && plat != shieldCol && plat != hamCol){
					standing = true;
					jetUsed = false;
					thePlatform = platforms[m].transform;
					break;
				//}
				/*if (plat != cirCol && plat != shieldCol && plat != hamCol){
					standing = true;
					thePlatform = plat.transform;
					break;
				}*/
			}
			m++;
		}
		Debug.DrawLine (floorCheckSpots[0],floorCheckSpots[1]);
		Debug.DrawLine (floorCheckSpots[2],floorCheckSpots[3]);
		Debug.DrawLine (floorCheckSpots[4],floorCheckSpots[5]);
		Debug.DrawLine (floorCheckSpots[6],floorCheckSpots[7]);
		yield return null;
	}

	public IEnumerator DetectWall(){

		wallCheckSpot1 = new Vector2( transform.position.x + transform.localScale.x * cirCol.center.x  - 3*floorDetectionBoxHeights.x, transform.position.y+.12f);
		wallCheckSpot2 = new Vector2( transform.position.x + transform.localScale.x * cirCol.center.x, transform.position.y+.02f);
		wallCheckSpot3 = new Vector2( transform.position.x + transform.localScale.x * cirCol.center.x  + 3*floorDetectionBoxHeights.x, transform.position.y+.12f);

		wallsL = Physics2D.OverlapAreaAll (wallCheckSpot1, wallCheckSpot2);
		wallsR = Physics2D.OverlapAreaAll (wallCheckSpot2, wallCheckSpot3);

		wallRPresent = false;
		wallLPresent = false;

		foreach (Collider2D wall in wallsR) {
			if (wall.tag.Contains("Platform") && !wall.isTrigger){
				wallRPresent = true;
				dir = -1;
				break;
			}
		}
		foreach (Collider2D wally in wallsL) {
			if (wally.tag.Contains("Platform") && !wally.isTrigger){
				wallLPresent = true;
				dir = 1;
				break;
			}
		}
		//Debug.DrawLine (wallCheckSpot1, wallCheckSpot2);
		//Debug.DrawLine (wallCheckSpot2, wallCheckSpot3);
		yield return null;
	}

	public IEnumerator WallJump(){
		yield return StartCoroutine (DetectWall ());

		if ((wallLPresent || wallRPresent) && !standing) {
			rigidbody2D.velocity = Vector3.zero;
			rigidbody2D.AddForce( new Vector2( jumpSpeed * dir * .707f, jumpSpeed*1.1f));
			wallLPresent = false;
			wallRPresent = false;
			wallDelay = true;
			yield return new WaitForSeconds (wallJumpLagTime);
			wallDelay = false;
		}
		yield return null;
	}

	void FreezeLocation(){
		transform.position = deathSpot;
	}

	public IEnumerator TriggerJump(){
		StartCoroutine(Jump());
		StartCoroutine(WallJump());
		StartCoroutine (JetJump ());
		yield return null;
	}

	public IEnumerator JetJump(){
		if (jetSet && !standing && !wallLPresent && !wallRPresent && !jetUsed) {
			rigidbody2D.AddForce( Vector2.up * jetSpeed);
			jetUsed = true;
		}
		yield return null;
	}

	// Update is called once per frame
	void Update () {
		if (locationFreeze) {
			FreezeLocation();
		}
		else if (buyingWares) {
			//
		}
		else if (alive){
			speed = rigidbody2D.velocity.x;
			forceX = 0f;
			forceY = 0f;
			Animate ();

			absVelx = Mathf.Abs (rigidbody2D.velocity.x);
			absVely = Mathf.Abs (rigidbody2D.velocity.y);

			if (controller.vertAx < -.95f && rigidbody2D.velocity.y <= 0f && !fastFall && lastFrameVertAx>-.95f && rigidbody2D.gravityScale !=0) {
				fastFall = true;
				StartCoroutine(FastFall());
			}

			if (copterHovering) {
				if(absVelx<maxFloatVelocity.x) {
					forceX = floatingSpeed * controller.horAx;
				}
				if (absVely <maxFloatVelocity.y){
					forceY = floatingSpeed * controller.vertAx;
				}

			}
			else {
				if(absVelx < maxVelocity.x) {
					if (controller.leftAxSize>minMoveRad){
						if (Mathf.Abs(Mathf.Cos(controller.armAng*Mathf.Deg2Rad))>Mathf.Cos(78.75f*Mathf.Deg2Rad)){
							forceX = runningSpeed * (controller.leftAxSize-minMoveRad)/(1-minMoveRad) * Mathf.Sign(controller.horAx);
							if (wallDelay){
								if (Mathf.Sign (forceX)!=dir){
									forceX = 0f;
								}
							}
						}
					}
				}
			}

			lastFrameVertAx = controller.vertAx;
			rigidbody2D.AddForce(new Vector2(forceX, forceY));
		}

	}

	public IEnumerator FastFall(){
		rigidbody2D.AddForce(new Vector2(0f, fallSpeed));
		yield return new WaitForSeconds (fallTimeDelay);
		fastFall = false;
	}


	void Animate(){

		//orient direction he is facing
		if (controller.horAx<0 && lastHorAx>=0 && transform.localScale.x==1) {
			transform.localScale = new Vector3(-1,1,1);
			armAngleCorrect = true;
		}
		else if (controller.horAx>0 && lastHorAx<=0  && transform.localScale.x==-1) {
			transform.localScale = new Vector3(1,1,1);
			armAngleCorrect = true;
		}
		else{
			armAngleCorrect = false;
		}

		if (armAngleCorrect && Mathf.Abs(controller.horAx2)<=0.05){
			if (controller.armAng<=180 && controller.armAng>=0){
				controller.armAng = 180f-controller.armAng;
			}
			else if (controller.armAng<=0 && controller.armAng>=-180){
				controller.armAng = -180f-controller.armAng;
			}
		}
		
		//if (

		//enable running
		if (!crouching && !onMyKnees && !uncrouching){
			if (Mathf.Abs (controller.horAx)>0 && Mathf.Abs (rigidbody2D.velocity.x)>runningVelThresh){
				heroAnimator.SetInteger ("AnimState", 1);
			}
			else {
				heroAnimator.SetInteger ("AnimState", 0);
			}
		}
		else if (onMyKnees){
			if (Mathf.Abs (controller.horAx)>0 && Mathf.Abs (rigidbody2D.velocity.x)>runningCrouchVelThresh){
				heroAnimator.SetInteger ("AnimState", 5);
			}
			else {
				heroAnimator.SetInteger ("AnimState", 4);
			}
		}
		lastHorAx = controller.horAx;
	}

	public IEnumerator Crouch(){
		if (!crouching && !uncrouching && alive && !onMyKnees) {
			j = 0;
			jumpSpeed *= .8f;
			runningSpeed *= .8f;
			maxVelocity.x *= .8f;
			crouching = true;
			if (heroAnimator.GetInteger("AnimState")==0 || heroAnimator.GetInteger("AnimState")==1){
				heroAnimator.SetInteger ("AnimState", 2);
			}
			yield return new WaitForSeconds(unitTimeFrame);

			while (j<polyCols.Length-1) {
				polyCols[j].enabled = false;
				polyCols[j+1].enabled = true;
				armHinge.connectedAnchor = conAnchs[j+1];
				shieldHinge.connectedAnchor = conAnchs[j+1];
				hammerHinge.connectedAnchor = conAnchs[j+1];
				j++;

				if (heroAnimator.GetInteger("AnimState")==2){
					heroAnimator.SetInteger ("AnimState", 4);
				}

				yield return new WaitForSeconds(unitTimeFrame);
			}
			crouching = false;
			onMyKnees = true;
			yield return null;
		}
	}

	public IEnumerator ReverseCrouch(){
		if (!crouching && !uncrouching && onMyKnees){
			j = conAnchs.Length-1;
			uncrouching = true;
			heroAnimator.SetTrigger("Reversal");

			while (j>0) {
				armHinge.connectedAnchor = conAnchs[j-1];
				shieldHinge.connectedAnchor = conAnchs[j-1];
				hammerHinge.connectedAnchor = conAnchs[j-1];
				j--;
				yield return new WaitForSeconds(unitTimeFrame);
			}

			heroAnimator.ResetTrigger("Reversal");
			heroAnimator.SetInteger ("AnimState", 0);
			polyCols[1].enabled = false;
			polyCols[0].enabled = true;

			uncrouching = false;
			onMyKnees = false;
			jumpSpeed *= 5f/4f;
			runningSpeed *= 5f/4f;
			maxVelocity.x *= 5f/4f;
			yield return null;
		}
	}

	public IEnumerator ResetStand(){
		j = polyCols.Length-1;
		while (j>0) {
			polyCols[j].enabled = false;
			j--;
			yield return null;
		}
		polyCols[0].enabled = true;
		cirCol.enabled = true;
		armHinge.connectedAnchor = conAnchs[0];
		shieldHinge.connectedAnchor = conAnchs[0];
		hammerHinge.connectedAnchor = conAnchs[0];
		onMyKnees = false;
		heroAnimator.SetInteger ("AnimState", 0);
		yield return null;
	}

	public IEnumerator StandUpStraight(){
		recoverRotating = true;
		while (recoverRotating){
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.identity,Time.deltaTime * recoRotoSpeed);
			
			rigidbody2D.AddForce(Vector2.up*liftForce*Time.deltaTime);
			
			if (transform.rotation.eulerAngles.z<angleTol || (360f-transform.rotation.eulerAngles.z)<angleTol){
				transform.rotation = Quaternion.identity;
				recoverRotating = false;
				rigidbody2D.fixedAngle = true;
			}
			yield return null;
		}
		yield return null;
	}

	void OnApplicationQuit(){
		cirCol.sharedMaterial.bounciness = startingBounce;
		cirCol.sharedMaterial.friction = startingFriction;

		foreach (Collider2D col in polyCols) {
			startingColState = col.enabled;
			col.enabled = false;
			col.sharedMaterial.bounciness = startingBounce;
			col.sharedMaterial.friction = startingFriction;
		}

		cirCol.enabled = true;
		polyCols [0].enabled = true;
	}

}