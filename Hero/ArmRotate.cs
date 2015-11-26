using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller))]
[RequireComponent (typeof (QuantizeAngles))]
public class ArmRotate : MonoBehaviour {

	public GameObject character;
	public GameObject shield;
	public GameObject hammer;
	public GameObject tractorBeam;

	public PolygonCollider2D shieldCol;
	public PolygonCollider2D polyCol;
	public PolygonCollider2D tractorBeamCol;
	public BoxCollider2D hammerCol;

	public QuantizeAngles quantizeAnglesScript;
	public Controller controller;
	public WeaponDetectorScript weaponDetectorScript;
	public Shield shieldScript;
	public Player playerScript;
	public Hammer hammerScript;

	public SpriteRenderer armSprite;
	public SpriteRenderer shieldSprite;
	public SpriteRenderer hammerSprite;
	
	public JointMotor2D jMoney;

	public Transform tractorBeamTrans;
	public Transform shieldTrans;
	public Transform hammerTrans;

	public Quaternion target;

	public float[] handAngles;
	public float rotDir; //-1 = counterclockwise, 1=clockwise
	public float entryAngle;
	public float realArmAng;
	public float rotoSpeed;
	public float startingRotoSpeed;
	public float shieldMass;
	public float bodyMass;
	public float hammerMass;
	public float gunMass;
	public float totalMass;

	public int i;

	public bool clockwise;
	public bool enter;
	public bool swap;
	public bool first;
	public bool left;
	public bool stopped;

	// Update is called once per frame

	void Awake() {
		rotoSpeed = 20f;
		startingRotoSpeed = rotoSpeed;

		shieldMass = .5f;
		bodyMass = .5f;
		hammerMass = .5f;
		gunMass = .5f;
		totalMass = 1f;

		if (gameObject.name == "Arm1") {
			character = GameObject.Find("Hero1");
			tractorBeam = GameObject.Find("TractorBeam1");
			weaponDetectorScript = GameObject.Find("WeaponDetector1").GetComponent<WeaponDetectorScript>();
			shield = GameObject.Find("Shield1");
			hammer = GameObject.Find("Hammer1");
		}
		else{
			character = GameObject.Find("Hero2");
			tractorBeam = GameObject.Find("TractorBeam2");
			weaponDetectorScript = GameObject.Find("WeaponDetector2").GetComponent<WeaponDetectorScript>();
			shield = GameObject.Find("Shield2");
			hammer = GameObject.Find("Hammer2");
		}
		tractorBeamTrans = tractorBeam.transform;
		tractorBeamCol = tractorBeam.GetComponent<PolygonCollider2D>();

		shieldCol = shield.GetComponent<PolygonCollider2D> ();
		shieldTrans = shield.transform;
		shieldScript = shield.GetComponent<Shield>();
		shieldSprite = shield.GetComponent<SpriteRenderer>();

		hammerCol = hammer.GetComponent<BoxCollider2D> ();
		hammerTrans = hammer.transform;
		hammerSprite = hammer.GetComponent<SpriteRenderer> ();
		hammerScript = hammer.GetComponent<Hammer>();

		armSprite = GetComponent<SpriteRenderer> ();
		quantizeAnglesScript = GetComponent<QuantizeAngles> ();
		controller = character.GetComponent<Controller> ();
		playerScript = character.GetComponent<Player>();
		first = false;

		handAngles = new float[]{
			90f,
			45f,
			0f,
			-45f,
			-90f
		};

	}

	void Update () {
		if (playerScript.alive){
			enter = true;
			if (weaponDetectorScript.pushing || hammerScript.smacking || hammerScript.smashing || hammerScript.cocking){
				enter = false;
			}
			if (enter){
				transform.rotation = Quaternion.Euler(0f,0f,quantizeAnglesScript.HalfTrackJacket(controller.armAng));
				shieldTrans.rotation = transform.rotation;
				hammerTrans.rotation = Quaternion.Euler(0f,0f,transform.rotation.eulerAngles.z-90f);
			}
		}
	}

	public IEnumerator SwitchHands(){
		if (playerScript.alive && !hammerScript.smacking && !hammerScript.smashing && !hammerScript.cocking && !hammerScript.recovering){
			switch (controller.weaponState){
			case 0: //Gun Hide, Shield Show
				//Gun Hide
				controller.weaponState = 1;
				controller.leftShoot = 0;
				controller.rightShoot = 0;

				armSprite.enabled = false;
				weaponDetectorScript.collider2D.enabled = false;
				tractorBeamCol.enabled = false;

				foreach (GameObject wep in weaponDetectorScript.storedWeapons){
					if (wep){
						wep.GetComponent<SpriteRenderer>().enabled = false;
					}
				}

				//Shield Show
				StartCoroutine(shieldScript.GrowShield());
				shieldSprite.enabled = true;
				shieldCol.enabled = true;

				//Handle Masses
				character.rigidbody2D.mass = bodyMass; 
				shield.rigidbody2D.mass = shieldMass;
				hammer.rigidbody2D.mass = 0f;

				break;
			case 1: //Shield Hide, Hammer Show
				controller.weaponState = 2;

				//Shield Hide
				shieldSprite.enabled = false;
				shieldCol.enabled = false;

				StartCoroutine(shieldScript.GoStill());

				//Hammer Show
				hammerSprite.enabled = true;

				//Handle Masses
				character.rigidbody2D.mass = bodyMass;
				shield.rigidbody2D.mass = 0f;
				hammer.rigidbody2D.mass = hammerMass;
				break;
			case 2: //Hammer Away, Gun Out
				controller.weaponState = 0;

				//Hammer Away
				hammerSprite.enabled = false;

				//Gun Show
				armSprite.enabled = true;
				weaponDetectorScript.collider2D.enabled = true;
				tractorBeamCol.enabled = true;

				foreach (GameObject wep in weaponDetectorScript.storedWeapons){
					if (wep){
						wep.GetComponent<SpriteRenderer>().enabled = true;
					}
				}

				//Handle Masses
				character.rigidbody2D.mass = totalMass;
				shield.rigidbody2D.mass = 0f;
				hammer.rigidbody2D.mass = 0f;
				break;
			}
		}
		yield return null;
	}
}