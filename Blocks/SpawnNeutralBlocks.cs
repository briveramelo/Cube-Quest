using UnityEngine;
using System.Collections;

public class SpawnNeutralBlocks : MonoBehaviour {
	public GameObject char1;
	public GameObject char2;
	public Animator tubeAnimator;
	
	public PolygonCollider2D polyCol;
	public CircleCollider2D weaponDet1;
	public CircleCollider2D weaponDet2;
	
	public Transform charTran1;
	public Transform charTran2;
	public Transform tubeTip;
	
	public Vector4 spawnBounds;
	
	public Vector2 respawnSpot;
	public Vector2 respawnSpotP1;
	public Vector2 respawnSpotP2;
	public Vector3 respawnDir;
	
	public string weaponBlockPath;
	
	public float respawnX;
	public float respawnY;	
	public float time2Respawn;
	public float time2RespawnP1;
	public float time2RespawnP2;
	public float shootForce;
	
	public int spawnQ;
	public int spawnP1Q;
	public int spawnP2Q;
	
	public bool running;
	public bool runningP1;
	public bool runningP2;
	
	public bool spawn;
	public bool spawnP1;
	public bool spawnP2;
	public bool delaying;
	
	// Use this for initialization
	void Awake () {
		time2Respawn = 1.5f;
		spawnBounds = new Vector4 (-4,4,-3,3);
		
		respawnSpot = new Vector2 ( .1f * Random.Range (spawnBounds.x*10f, spawnBounds.y*10f) , .1f * Random.Range (spawnBounds.z*10f, spawnBounds.w*10f) );
		weaponBlockPath = "Prefabs/WeaponBlocks/NeutralBlock";
		spawn = false;
		spawnP1 = false;
		spawnP2 = false;
		
		spawnQ = 0;
		spawnP1Q = 0;
		spawnP2Q = 0;
		
		shootForce = 25f;
		polyCol = GetComponent<PolygonCollider2D> ();
		tubeAnimator = GetComponent<Animator> ();
		
		if (polyCol) {
			tubeTip = transform.GetChild (0);
			tubeAnimator.SetInteger ("AnimState", 0); //still animation
			tubeAnimator.speed = 1;
		}
		
		running = false;
		runningP1 = false;
		runningP2 = false;
		delaying = false;
		
		if (GameObject.Find ("Hero1")){
			char1 = GameObject.Find ("Hero1");
			charTran1 = char1.transform;
			weaponDet1 = GameObject.Find("WeaponDetector1").GetComponent<CircleCollider2D>();
		}
		else{
			charTran1 = this.transform;
		}
		
		if (GameObject.Find ("Hero2")){
			char2 = GameObject.Find ("Hero2");
			charTran2 = char2.transform;
			weaponDet2 = GameObject.Find("WeaponDetector2").GetComponent<CircleCollider2D>();
		}
		else{
			charTran2 = this.transform;
		}
		
		
	}
	
	void Update(){
		if (spawn) {
			spawnQ += 1;
			StartCoroutine(SpawnIt());
		}
		else if (spawnP1){
			spawnP1Q += 1;
			StartCoroutine(SpawnItP1());
		}
		else if (spawnP2){
			spawnP2Q += 1;
			StartCoroutine(SpawnItP2());
		}
	}
	
	public IEnumerator SpawnItP1(){
		spawnP1 = false;
		
		while (runningP1){
			yield return new WaitForEndOfFrame();
		}
		
		while (spawnP1Q>0){
			runningP1 = true;
			yield return new WaitForSeconds(time2Respawn);
			runningP1 = false;
			respawnSpotP1 = charTran1.position + Vector3.up*.5f;
			Instantiate (Resources.Load (weaponBlockPath), respawnSpotP1, Quaternion.identity);
			spawnP1Q -= 1;
		}
		yield return null;
	}
	
	public IEnumerator SpawnItP2(){
		spawnP2 = false;
		
		while (runningP2){
			yield return new WaitForEndOfFrame();
		}
		
		while (spawnP2Q>0){
			runningP2 = true;
			yield return new WaitForSeconds(time2Respawn);
			runningP2 = false;
			respawnSpotP2 = charTran2.position + Vector3.up*.5f;;
			Instantiate (Resources.Load (weaponBlockPath), respawnSpotP2, Quaternion.identity);
			spawnP2Q -= 1;
		}
		yield return null;
	}
	
	
	public IEnumerator SpawnIt(){
		tubeAnimator.SetInteger ("AnimState", 1); //poop animation
		respawnSpot = transform.position;
		GameObject block = Instantiate (Resources.Load (weaponBlockPath), respawnSpot, Quaternion.Euler(0f,0f,transform.rotation.eulerAngles.z)) as GameObject;
		block.collider2D.enabled = false;
		
		respawnDir = -Vector3.Normalize(transform.position - tubeTip.position);
		yield return new WaitForSeconds (.3f);
		tubeAnimator.SetInteger ("AnimState", 0); //still animation
		block.rigidbody2D.AddForce(respawnDir * shootForce); 
		yield return new WaitForSeconds (.4f);
		block.collider2D.enabled = true;
	}	
	
	public IEnumerator DelayOfShame(){
		delaying = true;
		yield return new WaitForSeconds (1f);
		delaying = false;
	}
	
	void OnCollisionEnter2D(Collision2D col){
		if (!delaying){
			if (col.gameObject == char1 || col.gameObject == char2) {
				StartCoroutine(DelayOfShame());
				StartCoroutine(SpawnIt());
			}
		}
	}
}
