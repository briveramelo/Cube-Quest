using UnityEngine;
using System.Collections;

public class SuperTurretBlock : TurretBlock {

	public QuantizeAngles quantizeAnglesScript;

	public Quaternion targetRot;

	public Transform enTran;

	public Vector3 gunScale;
	public Vector3 line2En;
	public Vector3 line2Tip;

	public float[] turretAngles;
	public float ang2En;
	public float angTip2En;
	public float targetAng;
	public float rotationZ;

	public bool looking;

	//0 = start (disappear/appear)
	//1 = still (stay / flash)
	//2 = shine (sheen)
	//3 = fire (yellow light)
	
	
	void Awake(){
		turretGun = transform.FindChild ("SuperTurret_Gun").gameObject;
		turretGun.SetActive(false);
		tipTran = turretGun.transform.FindChild ("SuperTurretTip");
		barrelTran = turretGun.transform.FindChild ("SuperTurretBarrel");
		blockAnimator = GetComponent<Animator> ();
		gunAnimator = turretGun.GetComponent<Animator> ();
		blockAnimator.SetInteger ("AnimState", -1);

		bulletPath = "Prefabs/EnemyProjectiles/Tur_Sup_Bullet";
		quantizeAnglesScript = GetComponent<QuantizeAngles> ();
		weaponBlockScript = GetComponent<WeaponBlockScript> ();

		startingHealth = 50;
		currentHealth = startingHealth;

		setOffset = .045f;


		delayShoot = 2f;
		fireForce = 35f;
		burstCount = 10;
		timebtShots = .15f;
		bulletCount = 40;
		maxInt = 5;
		superTurretBlockScript = this;
		
		mounted = false;
		looking = false;
		super = true;

		boxCol = GetComponent<BoxCollider2D> ();
		polyCol = GetComponent<PolygonCollider2D>();
		polyCol.enabled = false;
		
	}

	public IEnumerator SuperStick(){
		boxCol.center = Vector2.zero;
		zeroLine = Vector3.Normalize(tipTran.position - barrelTran.position);
		polyCol.enabled = true;
		turretGun.GetComponent<SpriteRenderer>().sortingLayerID = 1;
		yield return null;
	}
	
	public IEnumerator Track(){ //movement
		looking = true;
		while (enTran) {
			line2En = Vector3.Normalize(enTran.position - barrelTran.position);
			line2Tip = Vector3.Normalize(tipTran.position - barrelTran.position);
			ang2En = Vector3.Angle(zeroLine,line2En);
			angTip2En = Vector3.Angle(line2Tip,line2En);
			targetAng = -ang2En; //totally works
			gunScale = turretGun.transform.localScale;
			turretGun.transform.rotation = Quaternion.Euler(0f,0f,quantizeAnglesScript.FullTrackJacket(targetAng));
			rotationZ = turretGun.transform.rotation.eulerAngles.z;

			if ((rotationZ<270f && rotationZ>=90f) || (rotationZ<-90f && rotationZ>-270f)){
				turretGun.transform.localScale = new Vector3(gunScale.x,-1f,gunScale.z);
			}
			else if ((rotationZ>=270f && rotationZ<=360f)  || (rotationZ>-90f && rotationZ<=90f)){
				turretGun.transform.localScale = new Vector3(gunScale.x,1f,gunScale.z);;
			}
			yield return null;
		}
	}

	void OnTriggerStay2D(Collider2D coller){ //Circle of enemy detection
		if (!enTran && mounted){
			if (coller.gameObject.GetComponent<GetDamage>() && !coller.gameObject.CompareTag("Turret") && !coller.gameObject.CompareTag("DPlatform")){
				if (coller.name.Contains("Hero1")){
					enTran = GameObject.Find("Arm1").transform;
				}
				else if (coller.name.Contains("Hero2")){
					enTran = GameObject.Find("Arm2").transform;
				}
				else{
					enTran = coller.transform;
				}
				StartCoroutine (Track());
			}
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (enTran) {
			enTran = null;
			looking = false;
		}
	}
}