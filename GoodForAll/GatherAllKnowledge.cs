using UnityEngine;
using System.Collections;

public class GatherAllKnowledge : MonoBehaviour {

	public PhysicsMaterial2D[] allSharedMaterials;
	public float[] allFriction;
	public float[] allBounciness;
	public string[] allNames;
	public int i;
	public int j;
	public int k;
	public int numMats;
	public int numTelekinetic;
	public int numSuperTelekinetic;
	public Rigidbody2D[] portalBlacklist;
	public bool[] youShallNotPass;
	public int portalNum;
	public int numCoins;
	public int lives;
	public int currentHealth;

	public bool jetSet;
	public bool neutralRoom;
	public bool explosiveRoom;
	public bool heliRoom;
	public bool teleportalRoom; 
	public ShowCaseRoomManager showcaseRoomManager;
	public HUD hud;
	public Player player;
	public int l; 
	public int p;

	void OnLevelWasLoaded(){
		k++;
		StartCoroutine (UpdateHUD ());
		StartCoroutine (SendStatus ());
	}


	public IEnumerator UpdateHUD(){
		hud = GameObject.Find ("HUD1").GetComponent<HUD> ();
		player = GameObject.Find ("Hero1").GetComponent<Player> ();
		hud.numCoins = numCoins;
		hud.lives = lives;
		player.lives = lives;
		player.currentHealth = currentHealth;
		player.jetSet = jetSet;
		yield return null;
	}

	public IEnumerator SendStatus(){
		if (GameObject.Find ("LevelManager")){
			showcaseRoomManager = GameObject.Find ("LevelManager").GetComponent<ShowCaseRoomManager>();
			showcaseRoomManager.neutralRoom = neutralRoom;
			showcaseRoomManager.explosiveRoom = explosiveRoom;
			showcaseRoomManager.heliRoom = heliRoom;
			showcaseRoomManager.teleportalRoom = teleportalRoom;
			StartCoroutine (showcaseRoomManager.CheckRoomStatus());
		}
		yield return null;
	}

	// Use this for initialization
	void Awake () {
		l = GameObject.FindGameObjectsWithTag ("know").Length;
		if (k < 1 && l>1) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad (this.gameObject);
		numTelekinetic = 0;
		numSuperTelekinetic = 0;
		allSharedMaterials = Resources.LoadAll<PhysicsMaterial2D> ("Prefabs/SharedMaterials");
		numMats = allSharedMaterials.Length;
		allNames = new string[numMats];
		allFriction = new float[numMats];
		allBounciness = new float[numMats];

		portalBlacklist = new Rigidbody2D[100];
		youShallNotPass = new bool[100];
		portalNum = 1;

		lives = 99;
		currentHealth = 100;

		Application.targetFrameRate = 60;

		i=0;
		j = 0;
		while ( i < numMats ){
			allNames[i] = allSharedMaterials[i].name;
			allFriction[i] = allSharedMaterials[i].friction;
			allBounciness[i] = allSharedMaterials[i].bounciness;
			i++;
		}
		StartCoroutine (UpdateHUD ());

	}
	
}
