using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Freeze : MonoBehaviour {

	public Player player;
	public WeaponBlockScript weaponBlockScript;

	public GameObject thingIHit;
	public Collider2D[] allColliders;
	public Collider2D[] allFrozen;
	public RaycastHit2D[] raycastsUp;
	public RaycastHit2D[] raycastsDown;
	public RaycastHit2D[] raycastsLeft;
	public RaycastHit2D[] raycastsRight;
	public ExplodeNoise explodeNoise;

	public Collider2D[] things2Ice;
	public GameObject[] allGameObjects;
	public GameObject frozenIce;

	public GetDamage getDamage;

	public AudioSource freeze;
	public Unfreeze unfreeze;
	
	private Dictionary<float,PhysicsMaterial2D> materialCache;

	public string startingName;
	
	public Vector3 hitPointThing;
	public Vector3 hitPointFro;
	public Vector3 bPos;
	public Vector3 thingPos;
	public Vector3 freDir;
	public Vector3 hitNormal;
	public Vector3 hitSpot;
	public Vector3 setSpot;
	
	public float exRadius;
	public float dist2frozeSploder;
	public float froMultiplier;
	public float froJumpSpeed;
	public float froRunningSpeed;
	public float froMaxVelocity;
	public float froBounce;
	public float froFriction;
	public float freezeTimer;
	public float timeFrozen;
	public float iceSheetOffset;
	public float timer;

	public float startingJumpSpeed;
	public float startingRunningSpeed;
	public float startingMaxVelocity;
	public float startingBounce;
	public float startingFriction;

	public float playerStartingFootBounce;
	public float playerStartingFootFriction;
	public float playerStartingBodyBounce;
	public float playerStartingBodyFriction; 
	public float setAngle;

	public int damage;
	public int i;
	public int j;
	public int k;
	public int f;
	public int damageBaseline;

	public bool isFrozen;
	public bool nowProjectile;
	public bool makeItHurt;
	public bool first;
	public bool done;
	public bool icecle;
	public bool startingEnabled;
	public bool itsAPlayer;

	//private Animator animator;
	
	void Awake() {
		damageBaseline = 35;

		materialCache = new Dictionary<float,PhysicsMaterial2D>();
		weaponBlockScript = GetComponent<WeaponBlockScript> ();

		isFrozen = false;
		timeFrozen = 2f;
		freezeTimer = 0f;
		iceSheetOffset = .0325f;

		timer = .1f;

		nowProjectile = false;

		exRadius = 7f;
		i = 0;
		first = true;
		icecle = false;
		itsAPlayer = false;
	}

	void OnCollisionEnter2D(Collision2D col){
		//gets nowProjectile from weaponblockscript
		if (nowProjectile && first) {
			first = false;
			if (col.gameObject.CompareTag("Platform") || col.collider.gameObject.CompareTag("DPlatform")){
				hitSpot	= new Vector3 (col.contacts[0].point.x,col.contacts[0].point.y,0f);
				hitNormal = new Vector3 (col.contacts[0].normal.x,col.contacts[0].normal.y,0f);
				setAngle = Vector3.Angle(Vector3.right,hitNormal);
				setSpot = hitSpot - Vector3.Normalize(hitNormal)*iceSheetOffset;

				if (col.contacts[0].normal.y<0){
					setAngle = -setAngle;
				}
				setAngle -= 90f;
				
				frozenIce = Instantiate (Resources.Load ("Prefabs/Effects/FrozenIce"),setSpot,Quaternion.Euler(0f,0f,setAngle)) as GameObject;
				frozenIce.transform.localScale = new Vector3(2,1,1);
				//done = true;
			}
			StartCoroutine(FreezeIt());
		}
	}

	public IEnumerator FreezeIt() {
		Instantiate ( Resources.Load ("Prefabs/Effects/Frozesplosion"),transform.position,Quaternion.identity);
		freezeTimer = timeFrozen;
		allFrozen = Physics2D.OverlapCircleAll (transform.position, exRadius);
		allGameObjects = new GameObject[allFrozen.Length];
		
		i = 0;
		foreach (Collider2D coller in allFrozen){
			allGameObjects[i] = coller.gameObject;
			i++;
		}
		
		i = 0;
		foreach (Collider2D fro in allFrozen){
			itsAPlayer = false;
			bPos = transform.position;
			thingPos = fro.transform.position;
			freDir = Vector3.Normalize(thingPos - bPos);
			
			dist2frozeSploder = Vector3.Distance(bPos,thingPos); //center to center distance
			
			RaycastHit2D[] rayThings = Physics2D.RaycastAll (bPos,freDir,dist2frozeSploder);
			foreach (RaycastHit2D rayThing in rayThings){
				if (rayThing.collider.gameObject == fro.gameObject){
					hitPointThing = rayThing.point; //world space point
					break;
				}
			}
			RaycastHit2D[] rayExps = Physics2D.RaycastAll (thingPos,-freDir,dist2frozeSploder);
			foreach (RaycastHit2D rayExp in rayExps){
				if (rayExp.collider.gameObject == this.gameObject){
					hitPointFro = rayExp.point; //world space point
					break;
				}
			}
			
			dist2frozeSploder = Vector3.Distance(hitPointFro,hitPointThing); //edge to edge distance
			
			if (icecle){
				froMultiplier = 1f-Mathf.Exp (-1.25f*dist2frozeSploder);
			}
			else{
				froMultiplier = 1f-Mathf.Exp (-.6f*dist2frozeSploder);
			}
			damage = Mathf.RoundToInt ( damageBaseline * (1f-froMultiplier) );
			
			if (fro.GetComponent<GetDamage>()){ //this protects against double damage to gameobjects with a circle and a box collider (and triple to those also with a polygon collider!)
				j = 0;
				makeItHurt = true;
				while ( j < i ){
					if (allGameObjects[i] == allGameObjects[j]){
						makeItHurt = false;
						break;
					}
					j++;
				}
				
				if (makeItHurt){
					getDamage = fro.GetComponent<GetDamage>();
					StartCoroutine(getDamage.SendDamage(damage, weaponBlockScript.blockType));
				}
			}
			
			if (fro.sharedMaterial){ //store its starting name, bounciness and friction for later use
				GatherAllKnowledge know = GameObject.Find ("theOmniscient").GetComponent<GatherAllKnowledge> ();
				k=0;
				
				while (k<know.numMats) {
					if (fro.sharedMaterial.name.Contains (know.allNames[k]) ){
						startingName = know.allNames[k];
						startingBounce = know.allBounciness[k];
						startingFriction = know.allFriction[k];		
						break;
					}
					k++;
				}
				
				froFriction = froMultiplier * startingFriction;
				froBounce = froMultiplier * startingBounce;
				
				if (fro.transform.parent){
					if (fro.transform.parent.parent){
						if (fro.transform.parent.parent.CompareTag("Player")){
							player = fro.transform.parent.parent.GetComponent<Player>();
							itsAPlayer = true;
						}
					}
					else{
						if (fro.transform.parent.CompareTag("Player")){
							player = fro.transform.parent.GetComponent<Player>();
							itsAPlayer = true;
						}
						else{
							if (fro.CompareTag("Player")){
								player = fro.GetComponent<Player>();
								itsAPlayer = true;
							}
						}
					}
				}
				else{
					if (fro.CompareTag("Player")){
						player = fro.GetComponent<Player>();
						itsAPlayer = true;
					}
				}

				if (itsAPlayer){
					if (!player.onMyKnees){
						startingJumpSpeed = player.startingJumpSpeed;
						startingRunningSpeed = player.startingRunningSpeed;
						startingMaxVelocity = player.startingMaxVelocity;
					}
					else{
						startingJumpSpeed = player.startingJumpSpeed * .5f;
						startingRunningSpeed = player.startingRunningSpeed * .5f;
						startingMaxVelocity = player.startingMaxVelocity * .5f;
					}
					
					froJumpSpeed = froMultiplier * startingJumpSpeed;
					froRunningSpeed = froMultiplier * startingRunningSpeed;
					froMaxVelocity = froMultiplier * startingMaxVelocity;
					
					player.jumpSpeed = froJumpSpeed;
					player.runningSpeed = froRunningSpeed;
					player.maxVelocity.x = froMaxVelocity;
				}
				
				if (!fro.GetComponent<Unfreeze>()){ // if it doesn't already have the unfreeze, check to add
				
					if (fro.transform.parent){ //if there is a parent 

						if (fro.transform.parent.parent){ //if there is a grandparent!

							fro.transform.parent.parent.gameObject.AddComponent<Unfreeze>();
							unfreeze = fro.transform.parent.parent.GetComponent<Unfreeze>();
							unfreeze.startingFriction = startingFriction;
							unfreeze.startingBounce = startingBounce;
							unfreeze.mydickiscold = true;

							fro.transform.parent.gameObject.AddComponent<Unfreeze>();
							unfreeze = fro.transform.parent.GetComponent<Unfreeze>();
							unfreeze.startingFriction = startingFriction;
							unfreeze.startingBounce = startingBounce;
							unfreeze.mydickiscold = true;

							fro.transform.gameObject.AddComponent<Unfreeze>();
							unfreeze = fro.GetComponent<Unfreeze>();
							unfreeze.startingFriction = startingFriction;
							unfreeze.startingBounce = startingBounce;
							unfreeze.mydickiscold = true;

							foreach (Collider2D col in fro.transform.parent.parent.GetComponents<Collider2D>()){ //slip up the grandparentparent colliders
								startingEnabled = col.enabled;
								col.enabled = false;
								col.sharedMaterial = GetMaterial (froFriction,froBounce,startingName);
								col.enabled = true;
								col.enabled = startingEnabled;
							}
							foreach (Collider2D col in fro.transform.parent.GetComponents<Collider2D>()){//and the parents
								startingEnabled = col.enabled;
								col.enabled = false;
								col.sharedMaterial = GetMaterial (froFriction,froBounce,startingName);
								col.enabled = true;
								col.enabled = startingEnabled;
							}
							foreach (Collider2D col in fro.GetComponents<Collider2D>()){ //and the thing
								startingEnabled = col.enabled;
								col.enabled = false;
								col.sharedMaterial = GetMaterial (froFriction,froBounce,startingName);
								col.enabled = true;
								col.enabled = startingEnabled;
							}
						}
						else{ //ok it's not on the grandparent, so we should add one to the parent
							fro.transform.parent.gameObject.AddComponent<Unfreeze>();
							unfreeze = fro.transform.parent.GetComponent<Unfreeze>();
							unfreeze.startingFriction = startingFriction;
							unfreeze.startingBounce = startingBounce;
							unfreeze.mydickiscold = true;
							
							fro.transform.gameObject.AddComponent<Unfreeze>();
							unfreeze = fro.GetComponent<Unfreeze>();
							unfreeze.startingFriction = startingFriction;
							unfreeze.startingBounce = startingBounce;
							unfreeze.mydickiscold = true;
						
							foreach (Collider2D col in fro.transform.parent.GetComponents<Collider2D>()){//and the parents
								startingEnabled = col.enabled;
								col.enabled = false;
								col.sharedMaterial = GetMaterial (froFriction,froBounce,startingName);
								col.enabled = true;
								col.enabled = startingEnabled;
							}
							foreach (Collider2D col in fro.GetComponents<Collider2D>()){ //and the thing
								startingEnabled = col.enabled;
								col.enabled = false;
								col.sharedMaterial = GetMaterial (froFriction,froBounce,startingName);
								col.enabled = true;
								col.enabled = startingEnabled;
							}
						}
					}
					else{ //so it's not on this gameobject, and it's there's no parent. add it
						fro.transform.gameObject.AddComponent<Unfreeze>();
						unfreeze = fro.GetComponent<Unfreeze>();
						unfreeze.startingFriction = startingFriction;
						unfreeze.startingBounce = startingBounce;
						unfreeze.mydickiscold = true;

						foreach (Collider2D col in fro.GetComponents<Collider2D>()){ //and the thing
							startingEnabled = col.enabled;
							col.enabled = false;
							col.sharedMaterial = GetMaterial (froFriction,froBounce,startingName);
							col.enabled = true;
							col.enabled = startingEnabled;
						}
					}
				}
				else{ //so it was here all along
					unfreeze = fro.GetComponent<Unfreeze>();
					unfreeze.mydickiscold = true;
					if (fro.transform.parent){ //if there is a parent 
						unfreeze = fro.transform.parent.GetComponent<Unfreeze>();
						unfreeze.mydickiscold = true;
						if (fro.transform.parent.parent){ //if there is a grandparent!
							unfreeze = fro.transform.parent.parent.GetComponent<Unfreeze>();
							unfreeze.mydickiscold = true;
						}
					}

				}
			}
			i++;
			
		}
		yield return null;
		Destroy(this.gameObject);

	}

	public PhysicsMaterial2D GetMaterial(float froFriction, float froBounce, string startingName){
		materialCache[froFriction] = Instantiate (collider2D.sharedMaterial) as PhysicsMaterial2D;
		materialCache[froFriction].name = startingName+" "+froFriction.ToString();
		materialCache[froFriction].friction = froFriction;
		materialCache[froFriction].bounciness = froBounce;
		return materialCache[froFriction];
	}
					
}