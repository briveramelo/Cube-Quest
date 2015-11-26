using UnityEngine;
using System.Collections;

public class NeutralSuper : MonoBehaviour {

	public bool lastFrameProjectile;

	public float breakTimer;
	public float time2Break;
	private string weaponBlockPath;
	public GameObject block;
	public GameObject explo;
	public int numBlocks;
	public SpawnNeutralBlocks neutralBlockSpawner;
	public GameObject blockSpawner;
	public WeaponBlockScript weaponBlockScript;
	public Vector3 crossDir;
	public Vector3[] spots;
	public float separationDistance;
	public int i;
	public Vector2 vel;

	// Use this for initialization
	void Awake () {
		time2Break = .03f;
		weaponBlockPath = "Prefabs/WeaponBlocks/NeutralBlock";
		numBlocks = 3;
		separationDistance = .2f;
		i = 0;

		if (GameObject.Find ("BlockSpawner")){
			blockSpawner = GameObject.Find ("BlockSpawner");
			neutralBlockSpawner = blockSpawner.GetComponent<SpawnNeutralBlocks>();
		}

	}

	public IEnumerator BreakUp(){
		yield return new WaitForSeconds (time2Break);

		vel = rigidbody2D.velocity;
		crossDir = separationDistance * Vector3.Normalize ( Vector3.Cross (new Vector3 (vel.x,vel.y, 0) , Vector3.forward )) ;

		spots = new Vector3[]{
			transform.position - crossDir,
			transform.position,
			transform.position + crossDir
		};

		while (i<3){
			block = Instantiate ( Resources.Load (weaponBlockPath) , spots[i] , Quaternion.identity) as GameObject;
			weaponBlockScript = block.GetComponent<WeaponBlockScript>();
			StartCoroutine(weaponBlockScript.Project());
			block.transform.localScale = Vector3.one * 3f;
			block.rigidbody2D.velocity = vel;
			block.rigidbody2D.gravityScale = 1f;
			i++;
		}

		explo = Instantiate ( Resources.Load("Prefabs/Effects/Explosion"),this.transform.position,this.transform.rotation) as GameObject;
		explo.transform.localScale = Vector3.one*5;
		Destroy(this.gameObject);

	}
}
