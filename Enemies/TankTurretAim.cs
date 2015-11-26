using UnityEngine;
using System.Collections;

public class TankTurretAim : MonoBehaviour {

	public GameObject player;

	public Vector3 pPos;
	public Vector3 aDir;
	public float turAng;

	public Quaternion target;
	public float rotoSpeed;


	// Use this for initialization
	void Awake () {
		player = GameObject.Find ("Hero");
		rotoSpeed = 20f;
	}
	
	// Update is called once per frame
	void Update () {
		aDir = Vector3.Normalize (player.transform.position-transform.position);

		turAng = 180f + Mathf.Atan2 (aDir.y, aDir.x) * Mathf.Rad2Deg;

		target = Quaternion.Euler(0,0,turAng);
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotoSpeed);
	}
}
