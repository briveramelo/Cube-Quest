using UnityEngine;
using System.Collections;

public class DeathZone: MonoBehaviour {

	public Player player;

	// Use this for initialization
	void Awake () {
		player = GameObject.Find ("Hero1").GetComponent<Player>();
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.name == "Hero1") {
			if (player.alive){
				StartCoroutine(player.Death());
			}
		}
		else if(col.GetComponent<Rigidbody2D>()){
			if (!col.GetComponent<TeleportalBlock>()){
			    Destroy(col.gameObject);
			}
		}
	}

}
