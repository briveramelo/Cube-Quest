using UnityEngine;
using System.Collections;

public class Teleportation : MonoBehaviour {

	private Vector3 endSpot;
	public GameObject[] secretWalls;
	public bool block;
	public bool player;

	// Use this for initialization
	void Awake () {
		endSpot = transform.FindChild ("OutPut").FindChild ("Spot").position;
		secretWalls = GameObject.FindGameObjectsWithTag ("SecretWall");
	}
	
	void OnCollisionEnter2D(Collision2D col){
		if (block) {
			foreach (GameObject secretWall in secretWalls){
				Physics2D.IgnoreCollision(col.collider.collider2D,secretWall.collider2D);
			}
			//14 is the secret platform layer
		}
		if (block || player) {
			col.transform.position = endSpot;
		}
	}
}
