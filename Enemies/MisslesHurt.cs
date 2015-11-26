using UnityEngine;
using System.Collections;

public class MisslesHurt : MonoBehaviour {
	
	public GameObject hitObject;
	public int missleDamage;
	public int blockType;
	private GetDamage getDamage;

	void Awake(){
		missleDamage = 20;
		blockType = -5;
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		hitObject = collision.collider.gameObject;

		if (hitObject.GetComponent<GetDamage>()){
			getDamage = hitObject.GetComponent<GetDamage>();
			getDamage.StartCoroutine(getDamage.SendDamage(missleDamage,blockType));
			Destroy (this.gameObject);
		}
	}

	
}
