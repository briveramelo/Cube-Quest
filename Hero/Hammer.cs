using UnityEngine;
using System.Collections;

public class Hammer : MonoBehaviour {

	public GameObject character;
	public Transform hammerTip;
	public Transform armHingePoint;
	
	public Player player;
	public ArmRotate armRotate;
	public Controller controller;
	public GetDamage getDamageScript;

	public Quaternion targetRotAngle;
	
	public WeaponDetectorScript weaponDetectorScript;
	public BoxCollider2D hammerCol;
	public Collider2D[] playerCols;

	public AudioSource growthHormone;
	public AudioSource smack;
	public AudioSource smackHit;
	public AudioSource smash;
	public AudioSource smashHit;

	public Vector3 hitDir;
	public Vector3 hamVel;
	public Vector3 lastPos;


	public float cockSmackTime;
	public float cockSmashTime;
	public float smackTime;
	public float smashTime;
	public float cockSpeed;
	public float cockSmackSpeed;
	public float cockSmashSpeed;
	public float swingSpeed;
	public float swingSmackSpeed;
	public float swingSmashSpeed;
	public float angleShift;
	public float relY;
	public float smashRecoverTime;

	public int speed;
	public int smackDamage;
	public int smashDamage;
	public int damage;
	public int reason; //reason for entering the timer function
	public int weaponType;

	public bool smacking;
	public bool smashing;
	public bool cocking;
	public bool recovering;
	
	// Use this for initialization
	void Awake () {

		weaponType = -3;
		swingSmackSpeed = 20f;
		swingSmashSpeed = 25f;

		cockSmashSpeed = .5f;
		cockSmackSpeed = .8f;

		cockSmackTime = 1f/8f;
		smackTime = 1f/5.5f; //(3x per second)

		cockSmashTime = 1f/1f;
		smashTime = 1.5f/1f; //(1x per second)

		smashRecoverTime = 1f;
			
		smackDamage = 5;
		smashDamage = 10;

		smacking = false;
		smashing = false;
		cocking = false;

		hammerCol = GetComponent<BoxCollider2D> ();
		hammerCol.enabled = false;

		GetComponent<SpriteRenderer> ().enabled = false;

		rigidbody2D.centerOfMass = Vector2.zero;
		rigidbody2D.mass = 0f;
		
		if (gameObject.name == "Hammer1"){
			weaponDetectorScript = GameObject.Find("WeaponDetector1").GetComponent<WeaponDetectorScript>();
			character = GameObject.Find("Hero1");
			armRotate = GameObject.Find("Arm1").GetComponent<ArmRotate>();
			hammerTip = GameObject.Find ("HammerTip1").transform;
		}
		else {
			weaponDetectorScript = GameObject.Find("WeaponDetector2").GetComponent<WeaponDetectorScript>();
			character = GameObject.Find("Hero2");
			armRotate = GameObject.Find("Arm2").GetComponent<ArmRotate>();
			hammerTip = GameObject.Find ("HammerTip2").transform;
		}
		player = character.GetComponent<Player> ();
		armHingePoint = transform;
		controller = character.GetComponent<Controller> ();

		playerCols = player.GetComponents<Collider2D> ();

		foreach (Collider2D col in playerCols) {
			Physics2D.IgnoreCollision(hammerCol,col);
		}

	}

	public IEnumerator Timer(int reason){
		switch (reason) {
		case 1: //cockSmacking
			yield return new WaitForSeconds (cockSmackTime);
			StartCoroutine(Timer (3));
			break;
		case 2: //cockSmashing
			yield return new WaitForSeconds (cockSmashTime);
			StartCoroutine(Timer (4));
			break;
		case 3: //smacking
			cocking = false;
			smacking = true;
			StartCoroutine (SwingAway (reason));
			yield return new WaitForSeconds (smackTime);
			smacking = false;
			break;
		case 4: //smashing
			cocking = false;
			smashing = true;
			StartCoroutine (SwingAway (reason));
			yield return new WaitForSeconds (smashTime);
			smashing = false;
			break;
		case 5:
			hammerCol.enabled = false;
			break;
		case 6:
			hammerCol.enabled = false;
			recovering = true;
			yield return new WaitForSeconds (smashRecoverTime);
			recovering = false;
			break;
		}

		yield return null;
	}

	public IEnumerator Cocking(int reason){
		if (!cocking && !smashing && !smacking){
			relY = hammerTip.position.y - armHingePoint.position.y;
			if ((player.transform.localScale.x>0 && relY>0) || (player.transform.localScale.x<0 && relY<0)){ //if facing right and hammer up
				angleShift = -90f;
			}
			else{
				angleShift = 90f;
			}

			cocking = true;
			StartCoroutine (Timer (reason));

			switch (reason) {
			case 1:
				cockSpeed = cockSmackSpeed;
				swingSpeed = swingSmackSpeed;
				break;
			case 2:
				cockSpeed = cockSmashSpeed;
				swingSpeed = swingSmashSpeed;
				break;
			}

			while (cocking){
				targetRotAngle = Quaternion.Euler (0f,0f,transform.rotation.eulerAngles.z-angleShift);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotAngle, Time.deltaTime * cockSpeed);
				yield return null;
			}
		}
		yield return null;
	}

	public IEnumerator SwingAway(int reason){
		hammerCol.enabled = true;

		while (smacking || smashing){
			targetRotAngle = Quaternion.Euler(0f,0f,transform.rotation.eulerAngles.z+angleShift);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotAngle, Time.deltaTime * swingSpeed);
			controller.armAng = transform.rotation.eulerAngles.z+90f;
			yield return null;
		}


		switch (reason){
		case 3:
			StartCoroutine (Timer (5));
			break;
		case 4:
			StartCoroutine (Timer (6));
			break;
		}
		//hammerCol.enabled = false;
		yield return null;
	}



	void OnTriggerEnter2D(Collider2D col){
		if (smacking || smashing) {
			if (col.gameObject.GetComponent<GetDamage>() && !col.gameObject.CompareTag("Player")){
				getDamageScript = col.gameObject.GetComponent<GetDamage>();
				if (smacking){
					damage = smackDamage;
				}
				else{
					damage = smackDamage;
				}
				
				StartCoroutine(getDamageScript.SendDamage(damage,weaponType));
			}
		}
	}

}