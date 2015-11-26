using UnityEngine;
using System.Collections;

public class TelekineticBlockController : MonoBehaviour {

	public GameObject telekineticBlock;
	public WeaponDetectorScript weaponDetectorScript;
	public TelekineticBlock telekineticBlockScript;

	public int controllerID;
	public int wepDSID;

	// Use this for initialization
	void Awake () {
		switch(wepDSID){
		case 0:
			weaponDetectorScript = GameObject.Find ("WeaponDetector1").GetComponent<WeaponDetectorScript>();
			break;
		case 1:
			weaponDetectorScript = GameObject.Find ("WeaponDetector2").GetComponent<WeaponDetectorScript>();
			break;
		}
	}

	void OnDestroy(){
		if (!telekineticBlockScript.merging) {
			weaponDetectorScript.teleDisconnect.Play ();
		}
		if (telekineticBlock) {
			telekineticBlock.GetComponent<Animator>().SetInteger("AnimState",1);
			if (telekineticBlockScript.orbiting && !telekineticBlockScript.merging) {
				telekineticBlockScript.orbiting = false;
				telekineticBlock.rigidbody2D.gravityScale = 1;
			}
		}

	}

}
