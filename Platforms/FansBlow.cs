using UnityEngine;
using System.Collections;

public class FansBlow : MonoBehaviour {

	private float blowForce;
	private float xDir;
	private float yDir;
	private string[] puffName;
	private Vector2 blowDir;
	private Vector3 startSpot;
	private Transform puffSpot;

	// Use this for initialization
	void Awake () {
		blowForce = 1f;
		puffName = new string[]{
			"Prefabs/Effects/Puff",
			"Prefabs/Effects/Smoke",
			"Prefabs/Effects/Smoke2",
		};
		puffSpot = transform;
		StartCoroutine (Puff ());
	}

	public IEnumerator Puff(){
		int i = Mathf.RoundToInt(Random.Range (0,20) * .1f);
		Instantiate (Resources.Load (puffName[i]), puffSpot.position, Quaternion.identity);
		float timer = Random.Range (20, 40) * .1f;
		yield return new WaitForSeconds (timer);
		StartCoroutine (Puff ());
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.rigidbody2D){
			if (!col.CompareTag("DPlatform")){
				xDir = -Mathf.Sin (transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
				yDir = Mathf.Cos (transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
				blowDir = new Vector2 (xDir,yDir);
				col.rigidbody2D.AddForce(blowDir * blowForce);
			}
		}
	}
}
