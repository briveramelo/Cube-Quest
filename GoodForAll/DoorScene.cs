using UnityEngine;
using System.Collections;

public class DoorScene : MonoBehaviour {

	public AudioSource winDitty;
	public Animator doorAnimator;
	public SpriteRenderer levelComplete;
	public bool open;
	public bool appear;

	public GatherAllKnowledge allKnow;

	void Awake(){
		doorAnimator = GetComponent<Animator> ();
		if (appear) {
			doorAnimator.SetInteger("AnimState",0);
			doorAnimator.speed = 0f;
		}
		else{
			doorAnimator.SetInteger("AnimState",1);
			doorAnimator.speed = 1f;
		}
		if (name == "Door_End") {
			levelComplete = transform.parent.FindChild ("LevelComplete").GetComponent<SpriteRenderer> (); 
		}
		allKnow = GameObject.Find ("theOmniscient").GetComponent<GatherAllKnowledge>();
	}

	void OnTriggerStay2D(Collider2D col){
		if (!open){
			if (col.GetComponent<Controller>()) {
				open = true;
				col.GetComponent<Controller>().doorSceneScript = this;
			}
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (open){
			if (col.GetComponent<Controller>()) {
				open = false;
				col.GetComponent<Controller>().doorSceneScript = null;
			}
		}
	}

	public IEnumerator OpenTheDoor(){
		doorAnimator.SetInteger("AnimState",1);
		doorAnimator.speed = 1f;
		yield return null;
	}

	public IEnumerator PeaceOut(){
		if (name == "Door_End") {
			StartCoroutine (LevelEnd());
		}
		else{
			if (name == "NeutralRoom"){
				allKnow.neutralRoom = true;
			}
			else if (name == "ExplosiveRoom"){
				allKnow.neutralRoom = true;
				allKnow.explosiveRoom = true;
			}
			else if (name == "HeliRoom"){
				allKnow.neutralRoom = true;
				allKnow.explosiveRoom = true;
				allKnow.heliRoom = true;
			}
			else if (name == "TeleportalRoom"){
				allKnow.neutralRoom = true;
				allKnow.explosiveRoom = true;
				allKnow.heliRoom = true;
				allKnow.teleportalRoom = true;
			}
			else if (name == "BonusRoom"){

			}
			Application.LoadLevel (name);
		}

		yield return null;
	}

	public IEnumerator LevelEnd(){
		if (!winDitty.isPlaying && open){
			winDitty.Play();
			levelComplete.enabled = true;
			Time.timeScale = .1f;
			while (winDitty.isPlaying){
				yield return null;
			}
			Caching.CleanCache();
			Application.Quit();
			//UnityEditor.EditorApplication.isPlaying = false;
		}
		yield return null;
	}
}
