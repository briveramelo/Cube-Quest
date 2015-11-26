using UnityEngine;
using System.Collections;

public class ShowCaseRoomManager : MonoBehaviour {

	public bool neutralRoom;
	public bool explosiveRoom;
	public bool heliRoom;
	public bool teleportalRoom;

	private GameObject neutralTube;
	private GameObject explosiveTube;
	private GameObject heliTube;
	private GameObject teleportalTube;

	private Animator neutralDoorAnim;
	private Animator explosiveDoorAnim;
	private Animator heliDoorAnim;
	private Animator teleportalDoorAnim;

	private GameObject character;
	private Vector3 spot1;
	private Vector3 spot2;
	private Vector3 spot3;
	private Vector3 spot4;

	// Use this for initialization
	void Awake () {
		neutralTube = GameObject.Find ("NeutralTube");
		explosiveTube = GameObject.Find ("ExplosiveTube");
		heliTube = GameObject.Find ("HeliTube");
		teleportalTube = GameObject.Find ("TeleportalTube");
		character = GameObject.Find ("Hero1");

		neutralDoorAnim = GameObject.Find ("NeutralRoom").GetComponent<Animator> ();
		explosiveDoorAnim = GameObject.Find ("ExplosiveRoom").GetComponent<Animator> ();
		heliDoorAnim = GameObject.Find ("HeliRoom").GetComponent<Animator> ();
		teleportalDoorAnim = GameObject.Find ("TeleportalRoom").GetComponent<Animator> ();


		spot1 = GameObject.Find("CheckPoint1").transform.position;
		spot2 = GameObject.Find("CheckPoint2").transform.position;
		spot3 = GameObject.Find("CheckPoint3").transform.position;
		spot4 = GameObject.Find("CheckPoint4").transform.position;
		StartCoroutine (CheckRoomStatus ());
	}

	public IEnumerator NeutralClear(){
		neutralTube.GetComponent<SpriteRenderer>().enabled = true;
		neutralTube.GetComponent<PolygonCollider2D>().enabled = true;
		neutralDoorAnim.SetInteger ("AnimState", 1);
		neutralDoorAnim.speed = 1;
		character.transform.position = spot1;
		yield return null;
	}

	public IEnumerator ExplosiveClear(){
		explosiveTube.GetComponent<SpriteRenderer>().enabled = true;
		explosiveTube.GetComponent<PolygonCollider2D>().enabled = true;
		explosiveDoorAnim.SetInteger ("AnimState", 1);
		explosiveDoorAnim.speed = 1;
		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("DPlatform");
		foreach (GameObject brick in bricks) {
			if (brick.name == "Destructible_Platform_Neutral"){
				Destroy(brick);
			}
		}
		Destroy (GameObject.Find("CheckPoint1"));
		character.transform.position = spot2;
		yield return null;
	}

	public IEnumerator HeliClear(){
		heliTube.GetComponent<SpriteRenderer>().enabled = true;
		heliTube.GetComponent<PolygonCollider2D>().enabled = true;
		heliDoorAnim.SetInteger ("AnimState", 1);
		heliDoorAnim.speed = 1;
		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("DPlatform");
		foreach (GameObject brick in bricks) {
			if (brick.name == "Destructible_Platform_Explosive_Standard"){
				Destroy(brick);
			}
		}
		Destroy (GameObject.Find("CheckPoint1"));
		Destroy (GameObject.Find("CheckPoint2"));
		character.transform.position = spot3;
		yield return null;
	}

	public IEnumerator TeleportalClear(){
		teleportalTube.GetComponent<SpriteRenderer>().enabled = true;
		teleportalTube.GetComponent<PolygonCollider2D>().enabled = true;
		teleportalDoorAnim.SetInteger ("AnimState", 1);
		teleportalDoorAnim.speed = 1;
		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("DPlatform");
		foreach (GameObject brick in bricks) {
			Destroy(brick);
		}
		Destroy (GameObject.Find("CheckPoint1"));
		Destroy (GameObject.Find("CheckPoint2"));
		Destroy (GameObject.Find("CheckPoint3"));
		character.transform.position = spot4;
		yield return null;
	}

	public IEnumerator CheckRoomStatus(){
		if (Application.loadedLevelName == "HomeRoom"){
			if (neutralRoom) {
				StartCoroutine (NeutralClear());
			}
			if (explosiveRoom) {
				StartCoroutine(ExplosiveClear());
			}
			if (heliRoom) {
				StartCoroutine(HeliClear());
			}
			if (teleportalRoom) {
				StartCoroutine(TeleportalClear());
			}
		}
		yield return null;
	}
}
