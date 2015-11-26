using UnityEngine;
using System.Collections;

public class TankBlock : MonoBehaviour {



	private GameObject newBullet;
	private GameObject newMissle;
	public GameObject flipper;

	private GameObject character1;
	private GameObject character2;

	private GameObject turretTip;

	public Animator animator;

	public Collider2D obstacle;
	public Collider2D obstacleAgain;
	private Collider2D leftSide;
	private Collider2D rightSide;

	public Vector3 bPos;
	private Vector3 pPos;
	private Vector3 p1Pos;
	private Vector3 p2Pos;

	public Vector3 aDir;
	public Vector3 muzzleSpot;
	private Vector3 sDir;
	
	public Vector2 botRight1;
	public Vector2 botRight2;
	public Vector2 botLeft1;
	public Vector2 botLeft2;
	private Vector2 hitDir;
	private Vector2 inFront;

	private string bulletsPath;
	private string misslesPath;

	public int dir;
	public int damage;
	public int tinyShootDamage;
	public int bigShootDamage;
	public int currentHealth;
	public int startingHealth;

	public float smallShootDist;
	public float bigShootDist;
	public float dist2Player;
	public float dist2Player1;
	public float dist2Player2;
	public float time2SmallShoot;
	public float smallShootTimer;
	public float smallShootForce;
	public float time2BigShoot;
	public float bigShootTimer;
	public float bigShootForce;
	public float rattleDist;
	public float rattleTimer;
	public float time2Rattle;
	public float flipTimer1;
	public float time2flip1;
	public float flipTorque;
	public float flipForce1;
	public float flipForce2;
	public float maxRollSpeed;
	public float rollForce;
	public float startingFriction;
	public float startingBounce;
	public float vel;
	public float knockDist;

	public bool bigShoot;
	public bool flipStage2;
	public bool contact;
	public bool standing;
	public bool char1;
	public bool char2;

	// Use this for initialization
	void Awake () {
		startingHealth = 30;
		currentHealth = startingHealth;

		char1 = false;
		char2 = false;
			 
		dist2Player1 = 100f;
		dist2Player2 = 100f;

		if (GameObject.Find("Hero1")){
			char1 = true;
			character1 = GameObject.Find ("Hero1");
		}
		if (GameObject.Find("Hero2")){
			char2 = true;
			character2 = GameObject.Find ("Hero2");
		}
		turretTip = GameObject.Find ("TurretTip");

		startingBounce = rigidbody2D.collider2D.sharedMaterial.bounciness;
		startingFriction = rigidbody2D.collider2D.sharedMaterial.friction;

		maxRollSpeed = 2f;
		rollForce = 6f;

		time2SmallShoot = .4f;
		time2BigShoot = 2f;


		smallShootTimer = 0f;
		bigShootTimer = 0f;

		smallShootForce = 40f;
		bigShootForce = 1050f;

		tinyShootDamage = 5;
		bigShootDamage = 40;

		smallShootDist = 1.4f;
		bigShootDist = .5f;

		bulletsPath = "Prefabs/EnemyProjectiles/Tur_Stan_Bullet";
		misslesPath = "Prefabs/EnemyProjectiles/Missle";
		rattleDist = .1f;
		time2Rattle = 2.5f;
		rattleTimer = time2Rattle;

		time2flip1 = 1.5f;
		flipTimer1 = time2flip1;
		flipForce1 = 80f;

		flipForce2 = 300f;
		flipTorque = 200f;

		knockDist = .45f;
		contact = false;
	}



	bool OnCollisionEnter2D(Collision2D collision){

		if (collision.collider.CompareTag ("Player")) {
			contact = true;	
		}
		else {
			contact = false;
		}
		return contact;
	}

	// Update is called once per frame
	void Update () {
		bPos = transform.position;

		if (char1){
			p1Pos = character1.transform.position;
			dist2Player1 = Vector3.Distance(bPos,p1Pos);
		}
		if (char2){
			p2Pos = character2.transform.position;
			dist2Player2 = Vector3.Distance(bPos,p2Pos);
		}
		if ( dist2Player1 <= dist2Player2 ){
			pPos = p1Pos;
			dist2Player = dist2Player1;
		}
		else{
			pPos = p2Pos;
			dist2Player = dist2Player2;
		}

		aDir = Vector3.Normalize(pPos - bPos);
		vel = Mathf.Abs (rigidbody2D.velocity.x);

		standing = DetectFloor ();
		dir = Animate (vel, aDir);

		if (!bigShoot && standing && Mathf.Abs (rigidbody2D.velocity.x) < maxRollSpeed && dist2Player > smallShootDist) {
			rigidbody2D.AddForce(rollForce * Mathf.Sign (aDir.x) * Vector2.right);
		}

		if (!bigShoot && dist2Player<smallShootDist && !contact && bigShootTimer<=0 && smallShootTimer<=0){
			muzzleSpot = turretTip.transform.position;
			sDir = Vector3.Normalize(pPos - muzzleSpot);
			newBullet = Instantiate ( Resources.Load (bulletsPath), muzzleSpot ,Quaternion.identity) as GameObject;
			newBullet.rigidbody2D.AddForce( (sDir + Vector3.up * .05f) * smallShootForce );
			newBullet.rigidbody2D.AddTorque( 350f );
			rigidbody2D.AddForce (-dir * Vector2.right * smallShootForce); 
			smallShootTimer = time2SmallShoot;

		}

		if (smallShootTimer>0){
			smallShootTimer -= Time.deltaTime;
		}
		if (bigShootTimer>0){
			bigShootTimer -= Time.deltaTime;
		}

		if (( contact || dist2Player < bigShootDist) && bigShootTimer <= 0) {
			bigShoot = true;
		}

		if (bigShoot){
			if ( rattleTimer > 0 ){
				rattleTimer -= Time.deltaTime;
				Rattle();
				if ( rattleTimer <= 0 ){
					muzzleSpot = new Vector3(dir * .7f,.35f,0) + transform.position ;
					sDir = Vector3.Normalize(pPos - muzzleSpot);

					newMissle = Instantiate ( Resources.Load (misslesPath) , muzzleSpot , Quaternion.identity ) as GameObject;

					if (dist2Player<=smallShootDist){
						newMissle.rigidbody2D.AddForce( sDir * bigShootForce );
					}
					else if (dist2Player>smallShootDist){
					    newMissle.rigidbody2D.AddForce( aDir * bigShootForce );
					}

					newMissle.rigidbody2D.AddTorque( 250f );
					rigidbody2D.AddForce (-dir * Vector2.right * bigShootForce/3f); 
					bigShootTimer = time2BigShoot;
					bigShoot = false;
					rattleTimer = time2Rattle;
				}
			}
		}

		if (Mathf.Abs (rigidbody2D.velocity.x)<1f){
			//flipOut(dir);
		}

	}



	public bool DetectFloor(){
		botLeft1 = new Vector2(-.22f+transform.position.x,-.3f+transform.position.y);
		botLeft2 = new Vector2(-.22f+transform.position.x,-.35f+transform.position.y);
		leftSide = Physics2D.Linecast (botLeft1, botLeft2).collider;
		
		botRight1 = new Vector2(.22f+transform.position.x,-.3f+transform.position.y);
		botRight2 = new Vector2(.22f+transform.position.x,-.35f+transform.position.y);
		rightSide = Physics2D.Linecast (botLeft1, botLeft2).collider;
		
		Debug.DrawLine (botLeft1, botLeft2);
		Debug.DrawLine (botRight1, botRight2);	
		
		if (leftSide && rightSide){
			if (leftSide.tag == rightSide.tag && !leftSide.isTrigger && Mathf.Abs (rigidbody2D.transform.rotation.eulerAngles.z)<5f){
				standing = true;
			}
			else{
				standing = false;
			}
		}
		else{
			standing = false;
		}
		
		return standing;
	}
	
	void Rattle(){
		transform.position += new Vector3 (Random.insideUnitCircle.x,Random.insideUnitCircle.y,0) * rattleDist;
		//might cause glitches into nearby objects
	}
	
	public IEnumerator Damage(int damageReceived){

		//include animation of healthbar in future

		if (currentHealth - damageReceived < 0 ){
			damageReceived = currentHealth;
		}
		
		currentHealth -= damageReceived;

		if (currentHealth <= 0) {
			Destroy (transform.parent.gameObject);
		}

		yield return null;

	}


	int Animate(float vel, Vector3 aDir){
		
		if (aDir.x > 0) {
			transform.localScale = new Vector3(-1,1,0);
			dir = 1;
		}
		else{
			transform.localScale = new Vector3(1,1,0);
			dir = -1;
		}
		
		if (vel>0.1f){
			animator.SetInteger ("AnimState", 1);
		}
		else {
			animator.SetInteger ("AnimState", 0);
		}
		return dir;
	}

	
	void flipOut(int dir){
		inFront = transform.position + Vector3.right * dir * knockDist + Vector3.up * -.2f;
		obstacle = Physics2D.OverlapPoint (inFront);
		Debug.DrawLine (transform.position, inFront);	
		
		if ( obstacle && !obstacle.CompareTag("Tank") && !obstacle.CompareTag("Platform") && !obstacle.CompareTag("Player") && !obstacle.isTrigger  ){
			if ( obstacle.gameObject.rigidbody2D ) {
				flipTimer1 -= Time.deltaTime;
				if ( flipTimer1<=0 ){
					obstacle.rigidbody2D.AddForce(Vector2.up * flipForce1);
					flipTimer1 = time2flip1;
					flipStage2 = true;
					flipper = obstacle.gameObject;
				}
				
			}
			else{
				flipTimer1 = time2flip1;
			}
		}
		if (flipStage2) {
			
			if (flipper.rigidbody2D.velocity.y<-.1f){
				hitDir = Random.insideUnitCircle;
				hitDir = new Vector2 ( dir* Mathf.Abs (hitDir.x) , Mathf.Abs (hitDir.y) ); 
				flipper.rigidbody2D.AddForce(hitDir * flipForce2);
				flipper.rigidbody2D.AddTorque(flipTorque);
				flipStage2 = false;
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
