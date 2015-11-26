using UnityEngine;
using System.Collections;

public class InstantDeath : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.name == "Hero1") {
			Player player = col.gameObject.GetComponent<Player>();
			if (player.alive){
				StartCoroutine(player.Death());
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collera){
		if (collera.gameObject.name == "Hero1") {
			Player player = collera.gameObject.GetComponent<Player>();
			if (player.alive){
				StartCoroutine(player.Death());
			}
		}
	}
}
