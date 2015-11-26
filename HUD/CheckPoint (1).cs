using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	private Player player;

	public GameObject currentCheckPoint;
	public Vector3 currentCheckPointSpot;

	private int i;

	void Awake(){
		player = GameObject.Find ("Hero1").GetComponent<Player> ();
		currentCheckPoint = GameObject.Find ("CheckPoint1");
		currentCheckPointSpot = currentCheckPoint.transform.position;
	}

	public IEnumerator ResetToCheckPoint(){
		yield return new WaitForSeconds(1f);
		player.deathSpot = currentCheckPointSpot;
	}

}
