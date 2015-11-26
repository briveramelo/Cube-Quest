using UnityEngine;
using System.Collections;
using System.Linq;

public class TurretBlock : MonoBehaviour {

	public GameObject bullet;
	public GameObject turretGun;

	public SuperTurretBlock superTurretBlockScript;
	public SmallBulletsHurt smallBulletsHurtScript;
	public WeaponBlockScript weaponBlockScript;

	public Animator blockAnimator;
	public Animator gunAnimator;

	public AudioSource shot1;
	public AudioSource shot2;
	public AudioSource shot3;
	public AudioSource shot4;
	public AudioSource shot5;

	public PolygonCollider2D polyCol;
	public BoxCollider2D boxCol;

	public Transform tipTran;
	public Transform barrelTran;

	public Vector3 contactPoint;
	public Vector3 setPosition;
	public Vector3 turretSpot;
	public Vector3 fDir;
	public Vector3 fDirHold;
	public Vector3 hitNormal;
	public Vector3 setSpot;
	public Vector3 zeroLine;

	public Vector2 newBoxCenter1;
	public Vector2 newBoxCenter2;

	public string bulletPath;

	public float[] angleDif;
	public float delayShoot;
	public float fireForce;
	public float timebtShots;
	public float shootAngle;
	public float setOffset;
	public float contactAngle;
	public float platformAngle;
	public float shineDelay;
	public float setAngle;
	public float xDif;
	public float yDif;
	public float xyDif;
	public float visRad;


	public int playNum;
	public int burstCount;
	public int i;
	public int j;	
	public int f;
	public int upDown;
	public int maxInt;
	public int bulletCount;
	public int currentHealth;
	public int startingHealth;

	public bool done;
	public bool shooting;
	public bool mounted;
	public bool super;

	//0 = start (disappear/appear)
	//1 = still (stay / flash)
	//2 = shine (sheen)
	//3 = fire (yellow light)


	void Awake(){
		visRad = 2f;
		shineDelay = 16f * 0.083f;
		setOffset = .05f;

		startingHealth = 30;
		currentHealth = startingHealth;

		turretGun = transform.FindChild ("Turret_Gun").gameObject;
		turretGun.SetActive(false);
		weaponBlockScript = GetComponent<WeaponBlockScript> ();
		blockAnimator = GetComponent<Animator> ();
		gunAnimator = turretGun.GetComponent<Animator> ();
		boxCol = GetComponent<BoxCollider2D> ();

		newBoxCenter1 = new Vector2 (0.05f, -.055f);
		newBoxCenter2 = new Vector2 (0f, 0.04f);
		done = false;
		delayShoot = 2f;
		blockAnimator.SetInteger ("AnimState", -1);

		fireForce = 35f;
		burstCount = 10;
		timebtShots = .15f;
		bulletCount = 40;
		maxInt = 5;

		tipTran = transform.FindChild ("TurretTip");
		barrelTran = transform.FindChild ("TurretBarrel");
		bulletPath = "Prefabs/EnemyProjectiles/Tur_Stan_Bullet";
	}

	public IEnumerator Damage(int damageReceived){
		
		//include something to animate future healthbar
		if (currentHealth - damageReceived < 0 ){
			damageReceived = currentHealth;
		}
		
		currentHealth -= damageReceived;
		
		if (currentHealth <= 0) {
			StopCoroutine(BurstFire());
			StopCoroutine(SelfDestruct());
			StopCoroutine(Shine());
			Destroy(this.gameObject);
		}
		
		yield return null;
		
	}

	public IEnumerator BurstFire(){
		shooting = true;
		yield return new WaitForSeconds (delayShoot);
		i=0;
		while (i<=burstCount){
			blockAnimator.SetInteger("AnimState",3); //fire
			gunAnimator.SetInteger("AnimState",3); //fire
			if ((i/2f)==Mathf.Floor(i/2f)){ //if even
				upDown = 1;
			}
			else{
				upDown = -1;
			}
			j=i;
			if (j>maxInt){
				j=maxInt;
			}
			fDir = tipTran.position - barrelTran.position;
			fDir = Vector3.Normalize (fDir);
			shootAngle = Vector3.Angle(Vector3.right,fDir);
			shootAngle += upDown * Random.Range(0,100) * .02f * (j-1);
			fDirHold = new Vector3( Mathf.Cos(shootAngle * Mathf.Deg2Rad), Mathf.Sin(shootAngle * Mathf.Deg2Rad),0f);

			if (transform.eulerAngles.z>=180f || transform.eulerAngles.z<=-180f){
				fDirHold *= -1; 
			}
			turretSpot = tipTran.position;
			playNum = Random.Range(1,5);
			switch (playNum){
			case 1:
				shot1.Play();
				break;
			case 2:
				shot2.Play();
				break;
			case 3:
				shot3.Play();
				break;
			case 4:
				shot4.Play();
				break;
			case 5:
				shot5.Play();
				break;
			}
			bullet = Instantiate (Resources.Load (bulletPath), turretSpot, Quaternion.identity) as GameObject;
			smallBulletsHurtScript = bullet.GetComponent<SmallBulletsHurt>();
			if (super){
				smallBulletsHurtScript.blockType = 12;
			}
			else{
				smallBulletsHurtScript.blockType = 11;
			}
			bullet.transform.localScale *= 3;
			bullet.rigidbody2D.AddForce (fDirHold * fireForce);
			i++;
			bulletCount--;
			yield return new WaitForSeconds(timebtShots);
		}
		blockAnimator.SetInteger("AnimState",1); //still
		gunAnimator.SetInteger("AnimState",1); //still
		if (bulletCount <= 0) {
			StartCoroutine(SelfDestruct());
		}
		shooting = false;
		if (!super) {
			StartCoroutine (BurstFire ());
		}
	}

	public IEnumerator SelfDestruct(){
		yield return new WaitForSeconds (.5f);
		Destroy (this.gameObject);
	}

	public IEnumerator Shine(){
		gunAnimator.SetInteger ("AnimState", 2);
		yield return new WaitForSeconds (shineDelay);
		blockAnimator.SetInteger ("AnimState", 2);
		yield return null;
		blockAnimator.SetInteger ("AnimState", 1);
	}

	void OnDrawGizmos(){
		Gizmos.DrawLine (contactPoint, contactPoint + hitNormal);
	}

	void OnCollisionEnter2D(Collision2D col){
		if (weaponBlockScript.nowProjectile && !done && !mounted) {
			if (col.collider.gameObject.CompareTag("Platform") || col.collider.gameObject.CompareTag("DPlatform")){
				mounted = true;
				hitNormal = new Vector3 (col.contacts[0].normal.x,col.contacts[0].normal.y,0f);
				setAngle = Vector3.Angle(Vector3.right,hitNormal);
				setSpot = transform.position - Vector3.Normalize(hitNormal)*setOffset;

				if (col.contacts[0].normal.y<0){
					setAngle = -setAngle;
				}
				setAngle -= 90f;


				if (super){
					StartCoroutine(superTurretBlockScript.SuperStick());
				}
				else{
					StartCoroutine(BurstFire());
				}

				transform.rotation = Quaternion.Euler(0f,0f,setAngle);
				transform.position = setSpot;

				turretGun.SetActive(true);
				GetComponent<SpriteRenderer>().sortingLayerID = -1;
				Destroy (rigidbody2D);
				blockAnimator.SetInteger("AnimState",0); //disappear
				gunAnimator.SetInteger("AnimState",0); //still
				StartCoroutine (Shine());
			}
		}
	}
}
