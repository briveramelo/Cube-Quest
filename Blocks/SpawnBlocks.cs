using UnityEngine;
using System.Collections;

public class SpawnBlocks : MonoBehaviour {

	private GameObject char1;
	private GameObject char2;
	private GameObject block;

	private Animator tubeAnimator;
	
	private PolygonCollider2D polyCol;

	private Transform tubeTip;

	private Vector3 respawnDir;
	private Vector2 respawnSpot;

	public string weaponBlockPath;
	private string spawnerName;
	
	public float shootForce;
	
	private bool delaying;
	
	// Use this for initialization
	void Awake () {
		spawnerName = gameObject.name;

		if (spawnerName.Contains ("Exp")) {
			weaponBlockPath = "Prefabs/WeaponBlocks/ExplosiveBlock";
		}
		else if (spawnerName.Contains ("Fre")) {
			weaponBlockPath = "Prefabs/WeaponBlocks/FreezeBlock";
		}
		else if (spawnerName.Contains ("Neu")) {
			weaponBlockPath = "Prefabs/WeaponBlocks/NeutralBlock";
		}
		else if (spawnerName.Contains ("Telek")) {
			weaponBlockPath = "Prefabs/WeaponBlocks/TelekineticBlock";
		}
		else if (spawnerName.Contains ("Telep")) {
			weaponBlockPath = "Prefabs/WeaponBlocks/TeleportalBlock";
		}
		else if (spawnerName.Contains ("Tur")) {
			weaponBlockPath = "Prefabs/WeaponBlocks/TurretBlock";
		}
		else if (spawnerName.Contains ("Heli")) {
			weaponBlockPath = "Prefabs/Enemies/MiniCopterBlock";
		}

		shootForce = 25f;
		polyCol = GetComponent<PolygonCollider2D> ();
		tubeAnimator = GetComponent<Animator> ();
		
		if (polyCol) {
			tubeTip = transform.GetChild (0);
			tubeAnimator.SetInteger ("AnimState", 0); //still animation
			tubeAnimator.speed = 1;
		}

		delaying = false;
		
		if (GameObject.Find ("Hero1")){
			char1 = GameObject.Find ("Hero1");
		}

		if (GameObject.Find ("Hero2")){
			char2 = GameObject.Find ("Hero2");
		}
	}

	public IEnumerator SpawnIt(){
		tubeAnimator.SetInteger ("AnimState", 1); //poop animation
		respawnSpot = transform.position;
		block = Instantiate (Resources.Load (weaponBlockPath), respawnSpot, Quaternion.Euler(0f,0f,transform.rotation.eulerAngles.z)) as GameObject;
		Physics2D.IgnoreCollision (polyCol, block.collider2D,true);

		respawnDir = -Vector3.Normalize(transform.position - tubeTip.position);
		yield return new WaitForSeconds (.3f);
		tubeAnimator.SetInteger ("AnimState", 0); //still animation
		block.rigidbody2D.AddForce(respawnDir * shootForce); 
		yield return new WaitForSeconds (.3f);
		Physics2D.IgnoreCollision (polyCol, block.collider2D,false);
	}	
	
	public IEnumerator PoopDelay(){
		delaying = true;
		yield return new WaitForSeconds (1f);
		delaying = false;
	}
	
	void OnCollisionEnter2D(Collision2D col){
		if (!delaying){
			if (col.gameObject == char1 || col.gameObject == char2) {
				StartCoroutine(PoopDelay());
				StartCoroutine(SpawnIt());
			}
		}
	}

	void OnCollisionExit2D(Collision2D coller){
		if (coller.gameObject == block) {
			block.collider2D.enabled = true;
		}
	}
}
