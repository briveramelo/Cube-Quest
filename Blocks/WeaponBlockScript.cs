using UnityEngine;
using System.Collections;
using System.Linq;

public class WeaponBlockScript : MonoBehaviour {

	//This script determines if the weapon block is locked in to the weapon slot if it gets close enough
	//and then adds firing physics once the player decides to shoot it off

	public GameObject blockSpawner;
	public GameObject weaponDetector1;
	public GameObject weaponDetector2;

	public WeaponDetectorScript[] weaponDetectorScripts;

	public Transform weaponSpot; 

	public Vector3 hPos; //player position
	public Vector3 bPos; //block position
	public Vector3 fDir; //fire direction
	public Vector3 slot1;
	public Vector3 slot2;
	public Vector3 slot3;
	public Vector3 aDir;

	public float startingFriction;
	public float startingBounce;
	public float distAway;
	public float damageVelocity;
	public float attractionRadius;
	public float absVel;
	public float maxForce;
	public float slowDist1;
	public float slowDist2;
	public float moveSpeed;
	public float xAmp;
	public float yAmp;
	public float xFreq;
	public float yFreq;
	public float deadSpeed;
	public float dist2Player1;
	public float dist2Player2;
	public float force;
	public int damage2Deal;
	public float damageFactor;

	public int blockDamage;
	public int blockType;
	public int toggleCount;
	public int i;
	public int wepDSID; //weapondetectorscript ID

	public bool lockedin; //checks that the weapon is locked into the hand spot
	public bool nowProjectile; //now the block is a Projectile (and has been fired)
	public bool attract;
	public bool distract;

	public GameObject character1;
	public GameObject character2;

	public AudioSource hurtNoise;
	public AudioSource hitNoise;

	public ChargeDisplay chargeDisplayScript1;
	public ChargeDisplay chargeDisplayScript2;
	public Player player1;
	public Player player2;

	public bool char1;
	public bool char2;
	public bool doDamage;

	void Awake(){

		xAmp = 3f*Random.value+3f;
		yAmp = 3f*Random.value+3f;
		
		xFreq = 2f * Random.value + 1f;
		yFreq = 2f * Random.value + 1f;

		deadSpeed = 5f * Random.value + 3f;
		maxForce = 30f;
		slowDist1 = .5f;
		slowDist2 = 1f;
		attractionRadius = 3f;
		damageVelocity = 2f;

		startingBounce = rigidbody2D.collider2D.sharedMaterial.bounciness;
		startingFriction = rigidbody2D.collider2D.sharedMaterial.friction;

		if (GameObject.Find ("BlockSpawner")){
			blockSpawner = GameObject.Find ("BlockSpawner");
		}

		char1 = false;
		char2 = false;
		dist2Player1 = 100f;
		dist2Player2 = 100f;
		weaponDetectorScripts = new WeaponDetectorScript[2];

		if (GameObject.Find ("Hero1")){
			char1 = true;
			character1 = GameObject.Find ("Hero1");
			player1 = character1.GetComponent<Player> ();
			weaponDetector1 = GameObject.Find ("WeaponDetector1");
			weaponDetectorScripts[0] = weaponDetector1.GetComponent<WeaponDetectorScript> ();
			chargeDisplayScript1 = GameObject.Find ("ChargeBar1").GetComponent<ChargeDisplay> ();
		}
		if (GameObject.Find ("Hero2")){
			char2 = true;
			character2 = GameObject.Find ("Hero2");
			player2 = character2.GetComponent<Player> ();
			weaponDetector2 = GameObject.Find ("WeaponDetector2");
			weaponDetectorScripts[1] = weaponDetector2.GetComponent<WeaponDetectorScript> ();
			chargeDisplayScript2 = GameObject.Find ("ChargeBar2").GetComponent<ChargeDisplay> ();
		}


		toggleCount = 0;
		moveSpeed = 60f;

		lockedin = false;
		nowProjectile = false;

		blockDamage = 20;

		rigidbody2D.gravityScale = 0f;
		transform.localScale = Vector3.one;
		collider2D.isTrigger = false;
		fDir = Vector3.zero;
		rigidbody2D.velocity = Vector2.zero;

		if (name == "RegularBlock"){
			nowProjectile = true;
			transform.localScale = Vector3.one * 3f;
			rigidbody2D.gravityScale = 1f;
		}


		//MINI COPTER BLOCK IS -1
		if (GetComponent<Explode>()){
			blockType = 2;
		}
		else if (GetComponent<Freeze>()){
			blockType = 3;
		}
		else if (GetComponent<NeutralSuper>()){
			blockType = 4;
		}
		else if (GetComponent<ExplodeSuper>()){
			blockType = 5;
		}
		else if (GetComponent<SuperFreezeBlock>()){
			blockType = 6;
		}
		else if (GetComponent<TelekineticBlock>()){
			blockType = 7;
		}
		else if (GetComponent<SuperTelekineticBlock>()){
			blockType = 8;
		}
		else if (GetComponent<AcidBlock>()){
			blockType = 9;
		}
		/*else if (GetComponent<SuperAcidBlock()){
			blockType = 10;
		}*/
		else if (GetComponent<SuperTurretBlock>()){
			blockType = 12;
		}
		else if (GetComponent<TurretBlock>()){
			blockType = 11;
		}
		else if (GetComponent<TeleportalBlock>()){
			blockType = 13;
		}
		//else if (GetComponent<SuperTeleportalBlock>()){
		//	blockType = 14;
		//}
		else if (GetComponent<ShockBlock>()){
			blockType = 15;
		}
		else { //neutral block
			blockType = 1;
		}

	}

	void OnCollisionEnter2D(Collision2D col){

		if (rigidbody2D){
			i = 0;
			force = Mathf.Abs (Vector2.Dot(col.contacts[0].normal,col.relativeVelocity) * rigidbody2D.mass);
			doDamage = false;
			if (force>5f && Vector2.Distance(Vector2.zero,rigidbody2D.velocity)>2f) {
				if (nowProjectile && col.gameObject.GetComponent<GetDamage>()){
					doDamage = true;
					if (blockType==7 || blockType==8){ //telekinetic blocks
						if (col.gameObject.GetComponent<Player>()){
							//only hurt on mismatch (so the owner of the block doesn't hurt himself or his other Telekineticblocks)
							if ((col.gameObject.GetComponent<Player>().char1 && wepDSID==1) || (col.gameObject.GetComponent<Player>().char2 && wepDSID==2)){
								doDamage = false;
							}
						}
						else if (col.gameObject.GetComponent<TelekineticBlock>()){
							if ((col.gameObject.GetComponent<TelekineticBlock>().wepDSID==wepDSID)){
								doDamage = false;
							}
						}
						else if (col.gameObject.GetComponent<SuperTelekineticBlock>()){
							if ((col.gameObject.GetComponent<SuperTelekineticBlock>().wepDSID==wepDSID)){
								doDamage = false;
							}
						}
					}
				}
			}
			if (doDamage){
				GetDamage getDamage = col.gameObject.GetComponent<GetDamage>();
				damageFactor = force/maxForce;
				
				if (damageFactor>1){
					damageFactor = 1;
				}
				hurtNoise.volume = damageFactor;
				hurtNoise.Play ();
				
				damage2Deal = Mathf.RoundToInt(damageFactor * blockDamage);
				StartCoroutine(getDamage.SendDamage(damage2Deal,1));
			}
		}
	}

	public IEnumerator Project(){
		lockedin = false;
		nowProjectile = true;
		attract = false;
		distract = false;
		toggleCount = 0;
		collider2D.isTrigger = false;


		StartCoroutine (SelfDestruct ());
		yield return null;
	}

	public IEnumerator SelfDestruct(){
		yield return new WaitForSeconds (90f);
		if (this.gameObject && blockType!=7 && blockType!=8 && blockType!=11 && blockType!=12 && blockType!=13){
			Destroy (this.gameObject);
		}
	}

	public IEnumerator LockIn(){
		lockedin = true;
		attract = false;
		distract = false;
		nowProjectile = false;
		collider2D.isTrigger = true;
		toggleCount = 1;
		yield return null;
	}

	void Update(){
		bPos = transform.position;
		
		if (char1) {
			dist2Player1 = Vector3.Distance(weaponDetector1.transform.position,bPos);
		}
		if (char2) {
			dist2Player2 = Vector3.Distance(weaponDetector2.transform.position,bPos);
		}

		if (toggleCount>0){

			hPos = weaponDetectorScripts[wepDSID-1].hPos;
			slot1 = weaponDetectorScripts[wepDSID-1].slot1;
			slot2 = weaponDetectorScripts[wepDSID-1].slot2;
			slot3 = weaponDetectorScripts[wepDSID-1].slot3;

			ToggleSpot();
		}
	}

	void ToggleSpot(){
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
		StopAllCoroutines ();
	}

	void OnApplicationQuit(){
		gameObject.collider2D.sharedMaterial.friction = startingFriction;
		gameObject.collider2D.sharedMaterial.bounciness = startingBounce;
	}
}