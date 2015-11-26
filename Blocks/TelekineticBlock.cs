using UnityEngine;
using System.Collections;

public class TelekineticBlock : MonoBehaviour {

	//does orbital protection, welcome explosive poof,

	public Transform heroSpot;

	public WeaponBlockScript weaponBlockScript;
	public WeaponDetectorScript weaponDetectorScript;
	public Controller controller;
	public Player player;

	public Transform wepDTran;

	public RaycastHit2D[] rayThings;
	public RaycastHit2D[] rayExps;
	public RaycastHit2D[] lines;
	
	public string characterName;

	public Quaternion targetRot;

	public Vector3 hitPointThing;
	public Vector3 hitPointExp;
	public Vector3 crossDir;
	public Vector3 bPos;
	public Vector3 orbVec;
	public Vector3 orbSpot;
	public Vector3 pullDir;
	public Vector3 startDir;

	public Vector2 maxFloatVelocity;
	public Vector2 velocity;

	public float directionCheck;
	public float vel;
	public float maxVelocity;

	public float[] orbitSpeeds;
	public float orbAng;
	public float pullInForce;
	public float pullFactor;
	public float brakeSpeedFactor;
	public float brakeDistanceFactor;
	public float brakeForce;
	public float orbDist2Spot;
	public float orbDist2Player;
	public float targetAng;
	public float reverseVel;
	public float[] loseRadii;

	public float[] orbitDists;

	public int startingHealth;
	public float projectionTol;
	public float pushFactor;
	public Vector3 pushDir;
	public float pushOutForce;
	public int currentHealth;

	public bool orbiting;
	public bool clockwise;
	public bool running;
	public bool merging;
	public bool startingOff;

	public int i;
	public int j;
	public int p;
	public int k;
	public int l;
	public int holdCount;
	public int numTelCon;
	public int wepDSID;



	// Use this for initialization
	void Awake () {

		startingHealth = 50;
		currentHealth = startingHealth;
		pushOutForce = 5000f;
		projectionTol = 0.9f;
		orbitDists = new float[]{.7f,1.1f,1.5f,1.9f};
		loseRadii = new float[] {0f,0f,0f,0f};
		orbitSpeeds = new float[] {0f,0f,0f,0f};

		l = 0;
		foreach (float o in orbitDists) {
			loseRadii[l] = o * 3f;
			orbitSpeeds[l] = 1/o;
			l++;
		}
		running = false;
		merging = false;
		maxVelocity = 4f;
		reverseVel = .1f;
		weaponBlockScript = GetComponent<WeaponBlockScript> ();
		maxFloatVelocity = new Vector2 (3f, 3f);
		holdCount = 0;
		brakeForce = 1500f;
		pullInForce = 5000f;
	
		orbiting = false;
		GameObject.Find ("theOmniscient").GetComponent<GatherAllKnowledge> ().numTelekinetic ++;
	}

	public IEnumerator Damage(int damageReceived){
		
		//include something to animate future healthbar
		if (currentHealth - damageReceived < 0 ){
			damageReceived = currentHealth;
		}
		
		currentHealth -= damageReceived;
		
		if (currentHealth <= 0) {
			orbiting = false;
			yield return null;
			Destroy(this.gameObject);
		}
		
		yield return null;
		
	}

	void OnCollisionStay2D(Collision2D col){
		if (orbiting){
			vel = Vector2.Distance(Vector2.zero,rigidbody2D.velocity);
			velocity = Vector3.Normalize(rigidbody2D.velocity);
			bPos = transform.position;
			directionCheck = Vector3.Distance(Vector3.Project(pullDir,velocity),Vector3.zero);
			if (directionCheck<projectionTol && !running) {
				StartCoroutine(ReverseDirection ());
			}
		}
	}

	public IEnumerator ReverseDirection(){
		running = true;
		clockwise = !clockwise;
		//orbitSpeed = reversingOrbitSpeed;

		if (clockwise){
			targetAng = orbAng+90f;
		}
		else{
			targetAng = orbAng-90f;
		}
		bPos = transform.position;
		orbVec = Vector3.Normalize(bPos-heroSpot.position);
		orbAng = Mathf.Atan2 (orbVec.y, orbVec.x) * Mathf.Rad2Deg;
		orbSpot = heroSpot.position + orbVec * orbitDists[holdCount];

		yield return new WaitForSeconds (1.5f);
		//orbitSpeed = startingOrbitSpeed;
		running = false;
	}

	public IEnumerator StartingOff(){
		startingOff = true;
		yield return new WaitForSeconds (.5f);
		startingOff = false;
	}

	public IEnumerator Orbit(){
		orbiting = true;
		orbVec = weaponDetectorScript.fDir;
		orbAng = Mathf.Atan2 (orbVec.y, orbVec.x) * Mathf.Rad2Deg;
		while (orbiting) {
			vel = Vector2.Distance(Vector2.zero,rigidbody2D.velocity);
			bPos = transform.position;

			if (!startingOff){
				if (clockwise){
					targetAng = orbAng+90f;
				}
				else{
					targetAng = orbAng-90f;
				}

				orbAng = Mathf.LerpAngle(orbAng,targetAng,Time.deltaTime*orbitSpeeds[holdCount]);
			}

			orbVec = Vector3.Normalize(new Vector2 ( Mathf.Cos(orbAng * Mathf.Deg2Rad),Mathf.Sin(orbAng * Mathf.Deg2Rad)));
			orbSpot = heroSpot.position + orbVec * orbitDists[holdCount];
			Debug.DrawLine(heroSpot.position,orbSpot);

			pullDir = Vector3.Normalize(orbSpot - bPos);

			orbDist2Spot = Vector3.Distance(bPos,orbSpot);
			orbDist2Player = Vector3.Distance(bPos,heroSpot.position);

			brakeSpeedFactor = vel/maxVelocity; //0-1
			brakeDistanceFactor = 1-orbDist2Spot/loseRadii[holdCount];
			pullFactor = orbDist2Spot/loseRadii[holdCount];//0-1
			
			if (orbDist2Player>loseRadii[holdCount] || !player.alive){
				StartCoroutine(FallApart());
			}

			pushDir = Vector3.Normalize(bPos-heroSpot.position);
			pushFactor = 1-orbDist2Player/orbitDists[holdCount];
			if (pushFactor<-1){
				pushFactor = -1;
			}

			rigidbody2D.AddForce(-Vector2.ClampMagnitude(rigidbody2D.velocity,1f) * brakeSpeedFactor * brakeDistanceFactor * brakeForce * Time.deltaTime);
			if (vel<maxVelocity){
				rigidbody2D.AddForce(pullDir * pullInForce * pullFactor * Time.deltaTime);
				rigidbody2D.AddForce(pushDir * pushOutForce * pushFactor * Time.deltaTime);
			}
			yield return null;
		}
	}
	
	public IEnumerator FallApart(){
		orbiting = false;
		rigidbody2D.gravityScale = 1f;
		GetComponent<Animator>().SetInteger("AnimState",1);
		weaponDetectorScript.teleDisconnect.Play ();
		yield return null;
	}
	
	public IEnumerator SelfDestruct(){
		yield return new WaitForSeconds (90f);
		Destroy (this.gameObject);
	}

}
