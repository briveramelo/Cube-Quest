using UnityEngine;
using System.Collections;

public class TrollToll : MonoBehaviour {

	private bool open;
	public bool buyOn;
	private bool ready2Buy;
	private Player player;
	private WeaponDetectorScript weaponDetectorScript;
	private Controller controller;
	private GameObject character;
	private GUIStyle menuStyle;
	private GUIStyle choiceStyle;
	private GUIStyle moneyStyle;
	private SpriteRenderer sign;
	private SpriteRenderer jetpack;
	private SpriteRenderer leadVest;
	private SpriteRenderer blockombinator;
	private Transform highlighterTransform;
	private SpriteRenderer highlighter;
	private SpriteRenderer[] items; 
	private int spotLight;
	private Vector3[] highlighterSpots;
	private GatherAllKnowledge allKnow;
	private int[] costBank;
	private bool[] purchased;
	private int numPurchased;
	private int i;
	private SpriteRenderer[] costs;
	public AudioSource buySound;
	public AudioSource moveHighlighter;
	public AudioSource openSound;
	public AudioSource closeSound;
	private float pauseTime;

	// Use this for initialization
	void Awake () {
		character = GameObject.Find ("Hero1");
		player = character.GetComponent<Player> ();
		controller = character.GetComponent<Controller> ();
		weaponDetectorScript = GameObject.Find ("WeaponDetector1").GetComponent<WeaponDetectorScript> ();

		sign = transform.GetChild(0).GetComponent<SpriteRenderer> ();
		highlighter = transform.GetChild (1).GetComponent<SpriteRenderer> ();
		highlighterTransform = highlighter.transform;
		highlighterSpots = new Vector3[]{
			new Vector3 (-2f, -1f, 0f),
			new Vector3 (0f, -1f, 0f),
			new Vector3 (2f, -1f, 0f)
		};
		costBank = new int[]{
			25,
			50,
			100
		};
		items = new SpriteRenderer[]{
			GameObject.Find ("leadVest").GetComponent<SpriteRenderer> (),
			GameObject.Find ("jetpack").GetComponent<SpriteRenderer> (),
			GameObject.Find ("blockombinator").GetComponent<SpriteRenderer> ()
		};
		costs = new SpriteRenderer[]{
			GameObject.Find ("25").GetComponent<SpriteRenderer> (),
			GameObject.Find ("50").GetComponent<SpriteRenderer> (),
			GameObject.Find ("100").GetComponent<SpriteRenderer> ()
		};
		while (i<items.Length) {
			items[i].transform.position = highlighterSpots[i];
			costs[i].transform.position = highlighterSpots[i]-Vector3.up*.3f;
			i++;
		}
		purchased = new bool[costBank.Length];
		allKnow = GameObject.Find ("theOmniscient").GetComponent<GatherAllKnowledge>();
		sign.transform.position = new Vector3 (0f,1.8f,0f);
		sign.gameObject.SetActive (false);
		highlighter.transform.position = highlighterSpots [0];
		pauseTime = .28f;
	}
	
	// Update is called once per frame
	void OnTriggerStay2D(Collider2D col){
		if (!open){
			if (col.GetComponent<Controller>()) {
				open = true;
				col.GetComponent<Controller>().trollScript = this;
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D col){
		if (open){
			if (col.GetComponent<Controller>()) {
				open = false;
				col.GetComponent<Controller>().trollScript = null;
			}
		}
	}

	public IEnumerator MakeTheTrade(){
		allKnow.numCoins -= costBank [spotLight];
		switch (spotLight) {
		case 0:
			controller.heavyLifter = true;
			break;
		case 1:
			player.jetSet = true;
			break;
		case 2:
			weaponDetectorScript.combinator = true;
			break;
		}
		costs[spotLight].enabled = false;
		purchased [spotLight] = true;
		buySound.Stop ();
		buySound.Play ();

		if (numPurchased == purchased.Length-1) {
			StartCoroutine (CloseShop());
		}

		yield return null;
	}

	public IEnumerator CheckForCash(){
		if (!purchased[spotLight] && allKnow.numCoins>=costBank[spotLight]){
			StartCoroutine (MakeTheTrade());
		}
		yield return null;
	}

	public IEnumerator CloseShop(){
		closeSound.Stop ();
		closeSound.Play ();
		yield return new WaitForSeconds (pauseTime);
		player.buyingWares = false;
		sign.gameObject.SetActive(false);
		highlighter.enabled = false;
		yield return null;
	}

	public IEnumerator OpenShop(){
		numPurchased = 0;
		foreach (bool purchase in purchased) {
			if (purchase){
				numPurchased++;
			}
		}
		if (numPurchased<purchased.Length){
			openSound.Stop ();
			openSound.Play ();
			yield return new WaitForSeconds (pauseTime);
			player.buyingWares = true;
			sign.gameObject.SetActive(true);
			highlighter.enabled = true;
		}
		yield return null;
	}

	public IEnumerator NextItem(int dir){
		moveHighlighter.Stop ();
		moveHighlighter.Play ();

		switch (dir) {
		case -1:
			spotLight--;
			break;
		case 1:
			spotLight++;
			break;
		}

		if (spotLight > 2) {
			spotLight = 0;
		}
		else if (spotLight <0){
			spotLight = 2;
		}
		highlighterTransform.position = highlighterSpots[spotLight];
		yield return null;
	}
}
