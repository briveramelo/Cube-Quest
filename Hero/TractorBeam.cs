using UnityEngine;
using System.Collections;

public class TractorBeam : MonoBehaviour {

	public WeaponBlockScript weaponBlockScript;
	public Transform weaponDetectorTransform;
	public WeaponDetectorScript weaponDetectorScript;
	public bool attract;
	public float pullForce;
	public float pullForceBaseline;
	public float dist;
	public Vector3 aDir;

	// Use this for initialization
	void Awake () {
		if (transform.parent.name == "Arm1"){
			weaponDetectorTransform = GameObject.Find ("WeaponDetector1").transform;
			weaponDetectorScript = GameObject.Find ("WeaponDetector1").GetComponent<WeaponDetectorScript>();
		}
		else if (transform.parent.name == "Arm2"){
			weaponDetectorTransform = GameObject.Find ("WeaponDetector2").transform;
			weaponDetectorScript = GameObject.Find ("WeaponDetector2").GetComponent<WeaponDetectorScript>();
		}
		attract = false;
		pullForceBaseline = 500f;
	}

	void OnTriggerStay2D(Collider2D block){
		if (attract && block.GetComponent<WeaponBlockScript>()){
			weaponBlockScript = block.GetComponent<WeaponBlockScript>();
			if (weaponBlockScript.toggleCount<1 && !weaponBlockScript.nowProjectile){
				aDir = Vector3.Normalize(weaponDetectorTransform.position - block.transform.position);
				dist = Vector3.Distance(weaponDetectorTransform.position,block.transform.position);
				pullForce = Mathf.Exp(-dist) * pullForceBaseline;
				block.rigidbody2D.AddForce(aDir*pullForce*Time.deltaTime);
			}
		}
	}
	
}
