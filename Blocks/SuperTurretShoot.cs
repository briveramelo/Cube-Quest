using UnityEngine;
using System.Collections;

public class SuperTurretShoot : MonoBehaviour {

	public SuperTurretBlock superTurretBlockScript;

	// Use this for initialization
	void Awake () {
		superTurretBlockScript = transform.parent.GetComponent<SuperTurretBlock> ();
	}

	void OnTriggerStay2D(Collider2D col){
		if (superTurretBlockScript.enTran && !superTurretBlockScript.shooting) {
			if (col.transform == superTurretBlockScript.enTran) {
				StartCoroutine(superTurretBlockScript.BurstFire());
			}
		}
	}

}
