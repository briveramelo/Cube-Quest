using UnityEngine;
using System.Collections;

public class TriggerDoor : MonoBehaviour {

	public DoorScene doorScene;
	public AudioSource doorOpenBloop;

	void Awake(){
		doorScene = transform.GetComponentInChildren<DoorScene> ();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.name == "Hero1" || col.name == "Hero2") {
			if (doorScene.doorAnimator){
				if (doorScene.doorAnimator.speed==0){
					StartCoroutine(doorScene.OpenTheDoor());
					doorOpenBloop.Play();
				}
			}
		}
	}
}
