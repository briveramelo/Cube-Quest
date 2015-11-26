using UnityEngine;
using System.Collections;

public class CameraFollowPlayer1 : MonoBehaviour {


	private GameObject character;

	private Player player;
	
	private Vector3 startSpot;
	private Vector3 shift;

	public Vector2 currentPlayerPos;
	public Vector2 lastPlayerPos;

	private Vector2 playerVel;
	private Vector2 viewDir;
	private Vector2 cameraVel;


	private float xSwitchTol;
	private float startX;
	private float vertDistAway;
	public float defaultVertDistAway;
	private float defaultHorDistAway;
	private float time2Move;
	public float yVel;
	public float xVel;
	private float minVel;
	private float sizeMult;
	public float sizeTarget;
	public float defaultSize;
	public float startingDefaultSize;
	public float startingDefaultVertDistAway;
	private float distAway;
	private float cameraVelFactorX;
	private float cameraVelFactorY;
	private float cameraVelFactorDefault;
	private float cameraVelFactorSwitch;
	public float yTarget;
	public float xTarget;
	private float xDistAway;
	private float yDistAway;
	public float lastYDist;
	public float lastXDist;
	private float xCap;
	private float playerVelMag;

	public int nowDir;

	public bool shake;
	public bool switching;


	// Use this for initialization
	void Awake () {
		shake = false;

		xSwitchTol = 0.4f;


		defaultVertDistAway = .65f;
		startingDefaultVertDistAway = defaultVertDistAway;
		defaultHorDistAway = .75f;

		minVel = 3f;
		sizeMult = .1f;
		xCap = 2.75f;

		cameraVel = Vector2.zero;
		cameraVelFactorDefault = 10f;
		cameraVelFactorSwitch = 2f;
		cameraVelFactorX = 10f;
		cameraVelFactorY = 10f;
		
		defaultSize = 2f;
		startingDefaultSize = defaultSize;
		sizeTarget = defaultSize;

		character = GameObject.Find ("Hero1");
		player = character.GetComponent<Player> ();

		transform.position = currentPlayerPos;
		camera.orthographicSize = defaultSize;

		nowDir = 1;
	}
	
	// Update is called once per frame
	void Update () {
		currentPlayerPos = character.transform.position;
		playerVel = character.rigidbody2D.velocity;
		playerVelMag = Vector2.Distance (playerVel, Vector2.zero);
		lastXDist = lastPlayerPos.x - currentPlayerPos.x;

		///Y SETTING START

		yTarget = currentPlayerPos.y + defaultVertDistAway;
		yDistAway = yTarget - transform.position.y;
		/// Y SETTING END


		///X SETTING START
		if ((nowDir == 1 && currentPlayerPos.x > lastPlayerPos.x) || (nowDir == -1 && currentPlayerPos.x < lastPlayerPos.x)) {
			lastPlayerPos.x = currentPlayerPos.x;
		}

		if (lastXDist > xSwitchTol){
			nowDir = -1;
			StartCoroutine (SwitchSides());
		}
		else if (lastXDist<-xSwitchTol){
			nowDir = 1;
			StartCoroutine (SwitchSides());
		}

		xTarget = currentPlayerPos.x + nowDir * defaultHorDistAway;
		xDistAway = xTarget - transform.position.x;
		///X SETTING END


		//Final location setting



		Debug.DrawLine(currentPlayerPos,new Vector3 (xTarget,yTarget,0f));

		//if (Mathf.Abs (xDistAway)>.1f){
			xVel = xDistAway * cameraVelFactorX;// + Mathf.Sign(xDistAway)*2f;
		//}
		//else{
		//	xVel = Mathf.MoveTowards(xVel, xDistAway * cameraVelFactorX,1f);
		//}

		//if (Mathf.Abs (yDistAway) > .1f) {
			yVel = yDistAway * cameraVelFactorY;// + Mathf.Sign (yDistAway) * 2f;
		//}
		//else{
		//	yVel = Mathf.MoveTowards(yVel, yDistAway * cameraVelFactorY,1f);
		//}



		cameraVel = new Vector2 (xVel, yVel);

		sizeTarget = defaultSize + sizeMult * (playerVelMag-minVel);
		if (sizeTarget<defaultSize || !player.alive){
			sizeTarget = defaultSize;
		}

		camera.orthographicSize = Mathf.MoveTowards (camera.orthographicSize, sizeTarget, Time.deltaTime);
		rigidbody2D.velocity = cameraVel;
	}

	public IEnumerator SwitchSides(){
		cameraVelFactorX = cameraVelFactorSwitch;
		while (Mathf.Abs (playerVel.x) < xCap && cameraVelFactorX<cameraVelFactorDefault) {
			cameraVelFactorX = Mathf.MoveTowards(cameraVelFactorX,cameraVelFactorDefault,0.03f);
			yield return null;
		}
		cameraVelFactorX = cameraVelFactorDefault;
		yield return null;	
	}

	public IEnumerator Shake(){
		StartCoroutine (ShakeIt ());
		yield return new WaitForSeconds (.1f);
		shake = false;
	}

	public IEnumerator ShakeIt(){
		shake = true;
		startSpot = this.transform.position;
		while (shake) {
			shift = new Vector3( Random.insideUnitCircle.x,Random.insideUnitCircle.y,0) * .05f;
			this.transform.position = startSpot + shift;
			yield return null;
		}
		yield return null;
	}
}
