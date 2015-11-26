using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	private GameObject character;
	private GameObject pointsObj;
	private GameObject livesObj;
	private GameObject coinObj;
	private GameObject coinsImage;
	private GameObject livesImage;
	private GameObject GUICamera;

	private GatherAllKnowledge allKnow;

	public TextMesh livesMesh;
	public TextMesh killsMesh;
	public TextMesh pointsMesh;
	public TextMesh coinMesh;

	public string livesString; 
	public string killedString;
	public string healthString;
	public string pointsString;
	public string coinsString;

	public Vector3 pointPosition;
	public Vector3 setSpot;
	public Vector3 livesPosition;
	public Vector3 coinPosition;
	public Vector3 livesImagePosition;
	public Vector3 coinImagePosition;

	public Color pointsColor;
	public Color livesColor;
	public Color coinColor;
	
	private float alpha;
	public float showKillsTimer;
	public float time2ShowKills;

	public int lives;
	public int numKilled;
	public int numCoins;
	public int points;

	public bool showKills;
	public bool keepingShowing;
	
	void Awake(){
		if (GameObject.Find ("GUICamera")) {
			GUICamera = GameObject.Find ("GUICamera");
			GUICamera.transform.position = Vector3.zero;
		}

		if (gameObject.name == "HUD1") {
			livesObj = GameObject.Find ("Lives1");
			coinObj = GameObject.Find ("Coins1");
			pointsObj = GameObject.Find ("Points1");
			character = GameObject.Find ("Hero1");
			livesImage = GameObject.Find("LivesImage1");
			coinsImage = GameObject.Find("CoinsImage1");

			killsMesh = GameObject.Find ("Kills1").GetComponent<TextMesh> ();

			livesPosition = new Vector3(-1.75f,-1.75f,0);
			livesImagePosition = new Vector3(-1.95f,-1.85f,0);

			coinPosition = new Vector3(-2.2f,-1.75f,0);
			coinImagePosition = new Vector3(-2.35f,-1.75f,0);

		}
		else{
			livesObj = GameObject.Find ("Lives2");
			coinObj = GameObject.Find ("Coins2");
			pointsObj = GameObject.Find ("Points2");
			character = GameObject.Find ("Hero2");
			livesImage = GameObject.Find("LivesImage2");
			coinsImage = GameObject.Find("CoinsImage2");

			killsMesh = GameObject.Find ("Kills2").GetComponent<TextMesh> ();

			livesPosition = new Vector3(1.75f,-1.75f,0);
			livesImagePosition = new Vector3(1.95f,-2f,0);
			
			coinPosition = new Vector3(2.2f,-1.85f,0);
			coinImagePosition = new Vector3(2.35f,-1.75f,0);
		}
		livesMesh = livesObj.GetComponent<TextMesh> ();
		coinMesh = coinObj.GetComponent<TextMesh>();
		pointsMesh = pointsObj.GetComponent<TextMesh>();


		allKnow = GameObject.Find ("theOmniscient").GetComponent<GatherAllKnowledge> ();
		lives = allKnow.lives;

		time2ShowKills = 2f;
		showKillsTimer = time2ShowKills;
		showKills = false;
	
		setSpot = new Vector3 (0f, 0f, 0f);
		transform.position = setSpot;
		livesObj.transform.position = livesPosition;
		coinObj.transform.position = coinPosition;
		livesImage.transform.position = livesImagePosition;
		coinsImage.transform.position = coinImagePosition;

	}

	void OnGUI(){
		lives = allKnow.lives;
		numCoins = allKnow.numCoins;
		livesString = "x"+lives.ToString ();//+" Lives";
		//killedString = numKilled.ToString ()+" Killed";
		coinsString = "x"+numCoins.ToString ();

		points = numKilled * 10;
		pointsString = "+"+points.ToString ();

		if (showKills) {
			showKillsTimer = time2ShowKills;
			keepingShowing = true;
			showKills = false;
			alpha = 1f;
		}

		if (keepingShowing){
			showKillsTimer -= Time.deltaTime;

			pointPosition = character.transform.position + Vector3.up * .8f * (1f + ((time2ShowKills - showKillsTimer)/time2ShowKills));

			pointsObj.transform.position = pointPosition;
			alpha = Mathf.Lerp (alpha,0,Time.deltaTime);
			pointsColor = new Color(1f,1f,1f,alpha);


			if (showKillsTimer<=0){
				keepingShowing = false;
			}
		}
		else{
			pointsColor = Color.clear;
		}

		livesColor = Color.white;
		livesMesh.text = livesString;
		livesMesh.color = livesColor;
		livesMesh.transform.position = livesPosition;
		//killsMesh.text = killedString;

		pointsMesh.text = pointsString;
		pointsMesh.color = pointsColor;

		coinColor = Color.white;
		coinMesh.text = coinsString;
		coinMesh.color = coinColor;


	}
	
}
