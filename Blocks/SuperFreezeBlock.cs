using UnityEngine;
using System.Collections;

public class SuperFreezeBlock : MonoBehaviour {

	public int wepDSID;
	public WeaponBlockScript weaponBlockScript;
	public BoxCollider2D[] boxCols;

	public Freeze freezeScript;
	public bool frozen;
	public float freezeTime;
	public float dropTime;
	public string freezeBlockString;
	public GameObject frozenIce;
	public GameObject icecle;
	public float iceOffset;
	public float icecleOffset;
	public int icecleDamage;

	// Use this for initialization
	void Awake () {
		weaponBlockScript = GetComponent<WeaponBlockScript> ();
		boxCols = GetComponents<BoxCollider2D> ();
		frozen = false;
		freezeTime = 11f;
		dropTime = 2.5f;
		iceOffset = .08f;
		icecleOffset = -.17f;
		icecleDamage = 20;
		freezeBlockString = "Prefabs/WeaponBlocks/FreezeBlock";
		boxCols [0].enabled = true;
		boxCols [1].enabled = false;
	}

	public IEnumerator OhMyGodImMelting(){
		yield return new WaitForSeconds (freezeTime);
		Destroy (this.gameObject);
	}

	public IEnumerator DroppingIceBombs(){
		yield return new WaitForSeconds (dropTime);
		icecle = Instantiate (Resources.Load (freezeBlockString), transform.position + Vector3.up*icecleOffset, Quaternion.identity) as GameObject;
		icecle.rigidbody2D.gravityScale = 1;
		freezeScript = icecle.GetComponent<Freeze>();
		Physics2D.IgnoreCollision(icecle.collider2D,boxCols[1]);
		freezeScript.damageBaseline = icecleDamage;
		freezeScript.icecle = true;
		freezeScript.nowProjectile = true;
		freezeScript.weaponBlockScript.nowProjectile = true;
		StartCoroutine(DroppingIceBombs());
	}

	public IEnumerator ColdAsIce(){ // triggered when the player hits the A button or something
		if (!frozen){
			frozen = true;

			frozenIce = Instantiate (Resources.Load ("Prefabs/Effects/FrozenIce"),transform.position + Vector3.up * iceOffset ,Quaternion.identity) as GameObject;
			//frozenIce.transform.localScale = new Vector3(2,1,1);

			Destroy (rigidbody2D);
			transform.rotation = Quaternion.identity;
			Destroy (boxCols[0]);
			boxCols [1].enabled = true;
			gameObject.tag = "Platform";
			StartCoroutine (OhMyGodImMelting());
			StartCoroutine (DroppingIceBombs());
		}
		yield return null;
	}
}
