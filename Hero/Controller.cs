using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public WeaponDetectorScript weaponDetectorScript;
	public SuperFreezeBlock[] superFreezeBlockScripts;
	public SuperTelekineticBlock[] superTelekineticBlockScripts; 
	public ChargeDisplay chargeDisplay;	
	public Player player;
	public GameObject blockSpawner;
	public GameObject enemySpawner;
	public ArmRotate armRotate;
	public Shield shieldScript;
	public Hammer hammerScript;
	public DoorScene doorSceneScript;
	public TrollToll trollScript;

	private SpriteRenderer controlsImage;

	public AudioSource pauseSound;
	public AudioSource unpauseSound;
	
	public string rightTrigger;
	public string leftTrigger;
	public string start;
	public string rightBumper;
	public string leftBumper;
	public string leftHorStick;
	public string leftHorStickLeft;
	public string leftHorStickRight;
	public string leftHorStickUp;
	public string leftHorStickDown;
	public string leftVerStick;
	public string rightHorStick;
	public string rightVerStick;
	public string yBut;
	public string xBut;
	public string aBut;
	public string bBut;
	public string lBut;
	public string dBut;
	public string rBut;
	public string uBut;
	public string ljBut;
	public string rjBut;
	public string playerName;
	public string switchBut;
	public string resetBut;

	public Vector3 armVec;

	private float delayTimerToggleWeapon;
	private float time2WaitToggleWeapon;
	private float delayTimerCombine;
	private float time2WaitCombine;
	public float leftShoot;
	public float lastLeftShoot;
	public float rightShoot;
	public float lastRightShoot;
	private float timer;
	public float horAx;
	private float lastHorAx;
	public float vertAx;
	public float leftAxSize;
	public float horAx2;
	public float vertAx2;
	public float armAng;
	public float delayTimer;
	public float time2Wait;
	public float delayTimerPause;
	public float time2WaitPause;

	public int weaponState;
	
	public bool goAheadToggleWeapon;
	public bool lastFrameToggleWeapon;
	public bool pleaseShoot;
	public bool lBumper;
	public bool goAheadCombine;
	public bool lastFrameCombine;
	public bool jump;
	public bool pause;
	public bool rBumper;
	public bool goAheadPause;
	public bool goAhead;
	public bool triggerCheckL;
	public bool triggerCheckR;
	public bool spawnBlocks;
	public bool spawnEnemies;
	public bool keyboard;
	public bool suited;
	public bool heavyLifter;

	// Use this for initialization
	void Awake () {
		horAx = 0;
		vertAx = 0;
		weaponState = 0;

		armVec = Vector3.zero;
		pause = false;
		timer = 0f;
		pleaseShoot = false;
		keyboard = false;

		spawnBlocks = false;
		spawnEnemies = false;

		controlsImage = GameObject.Find ("ControlsImage").GetComponent<SpriteRenderer> ();

		if (GameObject.Find ("BlockSpawner")){
			spawnBlocks = true;
			blockSpawner = GameObject.Find ("BlockSpawner");
		}

		if (GameObject.Find ("EnemySpawner")){
			spawnEnemies = true;
			enemySpawner = GameObject.Find("EnemySpawner");
		}

		if (gameObject.name == "Hero1"){
			chargeDisplay = GameObject.Find ("ChargeBar1").GetComponent<ChargeDisplay> ();
			weaponDetectorScript = GameObject.Find ("WeaponDetector1").GetComponent<WeaponDetectorScript> ();
			player = GameObject.Find ("Hero1").GetComponent<Player> ();
			shieldScript = GameObject.Find("Shield1").GetComponent<Shield>();
			hammerScript = GameObject.Find("Hammer1").GetComponent<Hammer>();
			armRotate = GameObject.Find("Arm1").GetComponent<ArmRotate>();
			playerName = "Hero1";


			start = "P1Start";
			rightTrigger = "P1RightTrigger";
			leftTrigger = "P1LeftTrigger";
			rightBumper = "P1ToggleWeapons";
			leftBumper = "P1CombineWeapons";
			leftHorStick = "P1MoveHor";
			leftVerStick = "P1MoveVer";
			rightHorStick = "P1AimHor";
			rightVerStick = "P1AimVer";
			yBut = "P1Y";
			xBut = "P1X";
			aBut = "P1A";
			bBut = "P1B";
			lBut = "P1L";
			dBut = "P1D";
			rBut = "P1R";
			uBut = "P1U";
			rjBut = "P1RJ";
			ljBut = "P1LJ";
			switchBut = "Switch";
			resetBut = "Reset";
		}
		else {
			chargeDisplay = GameObject.Find ("ChargeBar2").GetComponent<ChargeDisplay> ();
			weaponDetectorScript = GameObject.Find ("WeaponDetector2").GetComponent<WeaponDetectorScript> ();
			player = GameObject.Find ("Hero2").GetComponent<Player> ();
			shieldScript = GameObject.Find("Shield2").GetComponent<Shield>();
			hammerScript = GameObject.Find("Hammer2").GetComponent<Hammer>();
			armRotate = GameObject.Find("Arm2").GetComponent<ArmRotate>();
			playerName = "Hero2";

			rightTrigger = "P2RightTrigger";
			leftTrigger = "P2LeftTrigger";
			start = "P2Start";
			rightBumper = "P2ToggleWeapons";
			yBut = "P2Jump";
			leftBumper = "P2CombineWeapons";
			leftHorStick = "P2MoveHor";
			leftVerStick = "P2MoveVer";
			rightHorStick = "P2AimHor";
			rightVerStick = "P2AimVer";
			xBut = "P2X";
			aBut = "P2A";
			bBut = "P2B";
			lBut = "P2L";
			dBut = "P2D";
			rBut = "P2R";
			uBut = "P2U";
			rjBut = "P2RJ";
			ljBut = "P2LJ";
			resetBut = "Reset";
		}
	}
	/// <summary>
	/// Things you can always do
	/// </summary>
	void Pause (){
		if (Input.GetButtonDown (start)) {
			pause = !pause;
			if (pause){
				if (weaponDetectorScript.chargeNoise.isPlaying){
					weaponDetectorScript.chargeNoise.Pause();
				}
				pauseSound.Play ();
				Time.timeScale = 0;
				controlsImage.enabled = true;
			}
			else {
				if (weaponDetectorScript.chargeNoise.time>0 && rightShoot>0){
					weaponDetectorScript.chargeNoise.Play();
				}
				else{
					weaponDetectorScript.chargeNoise.Stop();
				}
				unpauseSound.Play ();
				Time.timeScale = 1;
				controlsImage.enabled = false;
			}
		}
	}

	/// <summary>
	/// Gun Things
	/// </summary>
	public float RightTrigger(){
		rightShoot = (Input.GetAxisRaw (rightTrigger) + 1f) / 2f; //returns 0-1
		if (rightShoot == 0.5f) {
			rightShoot = 0f;
		}
		//press right trigger and...
		if (weaponState == 0 && weaponDetectorScript.occupied && rightShoot>0f && lastRightShoot==0f) {
			StartCoroutine(weaponDetectorScript.PlayCharge());
			StartCoroutine(chargeDisplay.StartCharger());
		}
		//release right trigger and...
		else if (weaponState == 0 && weaponDetectorScript.occupied && rightShoot==0f && lastRightShoot>0f && weaponDetectorScript.wasOccupied){
			StartCoroutine(weaponDetectorScript.LaunchOverIt());
		}
		else if (weaponState == 1 && rightShoot>0f && lastRightShoot==0f){
			StartCoroutine(shieldScript.PushAway());
		}
		else if (weaponState == 2 && rightShoot>0f && lastRightShoot==0f){
			StartCoroutine(hammerScript.Cocking(1));
		}

		lastRightShoot = rightShoot;
		return rightShoot;
	}

	public float LeftTrigger(){
		leftShoot = (Input.GetAxisRaw (leftTrigger) + 1f) / 2f; //returns 0-1
		if (leftShoot == 0.5f) {
			leftShoot = 0f;
		}
		
		if (leftShoot>0f && lastLeftShoot==0f) {
			if (weaponState == 0){
				StartCoroutine(weaponDetectorScript.ReAttract());
				superTelekineticBlockScripts = null;
				superTelekineticBlockScripts = GameObject.FindObjectsOfType<SuperTelekineticBlock>();
				foreach (SuperTelekineticBlock tele in superTelekineticBlockScripts){
					if (tele.wepDSID == weaponDetectorScript.wepDSID){	
						StartCoroutine(tele.DoubleTriggerCheck());
					}
				}
			}
			//StartCoroutine(chargeDisplay.StartCharger());
		}
		else if (leftShoot==0f && lastLeftShoot>0f){
			StartCoroutine(weaponDetectorScript.Distract());
			//StartCoroutine(chargeDisplay.StopCharger());
		}
		
		lastLeftShoot = leftShoot;	
		return leftShoot;

	}

	void GunToggleWeapons(){
		if (Input.GetButtonDown (rightBumper)) {
			StartCoroutine(weaponDetectorScript.ToggleWeapons());
			StartCoroutine(chargeDisplay.StopCharger());
			StartCoroutine(weaponDetectorScript.StopChargeNoise());
		}
	}

	void GunCombineWeapons(){
		if (Input.GetButtonDown (leftBumper)) {
			StartCoroutine(weaponDetectorScript.CombineWeapons());
		}
	}

	/*void GunDestroyBlockInHand(){
		if (Input.GetButtonDown (dBut)) {
			StartCoroutine(weaponDetectorScript.DestroyBlockInHand());
		}
	}*/

	void GunDetonate(){
		if (Input.GetButtonDown (xBut)) {
			if (playerName=="Hero1"){
				superFreezeBlockScripts = new SuperFreezeBlock[]{null};
				superFreezeBlockScripts = GameObject.FindObjectsOfType<SuperFreezeBlock>();
				foreach (SuperFreezeBlock supFB in superFreezeBlockScripts){
					if (supFB.wepDSID == 1 && !supFB.frozen){
						StartCoroutine(supFB.ColdAsIce());
					}
				}
			}
			else if (playerName=="Hero2"){
				superFreezeBlockScripts = new SuperFreezeBlock[]{null};
				superFreezeBlockScripts = GameObject.FindObjectsOfType<SuperFreezeBlock>();
				foreach (SuperFreezeBlock supFB in superFreezeBlockScripts){
					if (supFB.wepDSID == 2 && !supFB.frozen){
						StartCoroutine(supFB.ColdAsIce());
					}
				}			
			}
		}
	}

	/// <summary>
	/// Shield Things
	/// </summary>
	
	/*void ShieldPush(){
		if (Input.GetButtonDown (xBut)) {
			StartCoroutine(shieldScript.PushAway());
		}
	}
	*/
	/// <summary>
	/// Hammer Things
	/// </summary>
	/*
	void HammerSmack(){
		if (Input.GetButtonDown (xBut)) {
			StartCoroutine(hammerScript.Cocking(1));
		}
	}

	void HammerSmash(){
		if (Input.GetButtonDown (bBut)) {
			StartCoroutine(hammerScript.Cocking(2));
		}
	}*/
	
	/// <summary>
	/// Things you can always do when alive
	/// </summary>
	void ShieldGunSword(){
		if (Input.GetButtonDown (yBut)) {
			StartCoroutine(armRotate.SwitchHands());
		}
	}

	void Crouch(){
		if (Input.GetButtonDown (dBut) && !player.onMyKnees) {
			StartCoroutine(player.Crouch());
		}
		else if (Input.GetButtonDown (uBut) && player.onMyKnees){
			StartCoroutine(player.ReverseCrouch());
		}
	}

	void Jump(){
		if (Input.GetButtonDown (aBut)) {
			StartCoroutine(player.TriggerJump());
		}
	}

	void ExitThroughTheBlockShop(){
		if (Input.GetButtonDown (xBut)) {
			if (doorSceneScript){
				StartCoroutine(doorSceneScript.PeaceOut());
			}
		}
	}



	void ResetButton(){
		if (Input.GetButtonDown (resetBut)) {
			StartCoroutine(ResetLevel ());
		}
	}

	void SuitUp(){
		if (heavyLifter) {
			if (Input.GetButtonDown(bBut)){
				if (!suited){
					player.rigidbody2D.mass = 2f;
				}
				else{
					player.rigidbody2D.mass = 1f;
				}
				suited = !suited;
			}
		}
	}

	public IEnumerator ResetLevel(){
		Application.LoadLevel (Application.loadedLevelName);
		yield return null;
	}

	void SwitchKeyboardController(){
		if (Input.GetButtonDown(switchBut)){
			if (keyboard){
				keyboard = false;
				start = "P1Start";
				rightTrigger = "P1RightTrigger";
				leftTrigger = "P1LeftTrigger";
				rightBumper = "P1ToggleWeapons";
				leftBumper = "P1CombineWeapons";
				leftHorStick = "P1MoveHor";
				leftVerStick = "P1MoveVer";
				rightHorStick = "P1AimHor";
				rightVerStick = "P1AimVer";
				yBut = "P1Y";
				xBut = "P1X";
				aBut = "P1A";
				bBut = "P1B";
				lBut = "P1L";
				dBut = "P1D";
				rBut = "P1R";
				uBut = "P1U";
				rjBut = "P1RJ";
				ljBut = "P1LJ";
			}
			else{
				keyboard = true;
				rightTrigger = "ComRightTrigger";
				leftTrigger = "ComLeftTrigger";
				start = "ComStart";
				rightBumper = "ComToggleWeapons";
				leftBumper = "ComCombineWeapons";
				leftHorStickLeft = "ComMoveLeft";
				leftHorStickRight = "ComMoveRight"; 
				leftHorStickUp = "ComMoveUp";
				leftHorStickDown = "ComMoveDown";
				leftVerStick = "ComMoveVer";
				rightHorStick = "ComAimHor";
				rightVerStick = "ComAimVer";
				yBut = "ComY";
				xBut = "ComX";
				aBut = "ComA";
				bBut = "ComB";
				lBut = "ComL";
				dBut = "ComD";
				rBut = "ComR";
				uBut = "ComU";
				rjBut = "ComRJ";
				ljBut = "ComLJ";
			}
		}
	}

	void Enter(){
		if (Input.GetButtonDown(xBut)){
			if (trollScript){
				StartCoroutine(trollScript.OpenShop());
				player.rigidbody2D.mass = 10000000f;
				player.rigidbody2D.velocity = Vector2.zero;
			}
		}
	}

	void Leave(){
		if (Input.GetButtonDown(bBut)){
			StartCoroutine(trollScript.CloseShop());
			player.rigidbody2D.mass = 1f;
		}
	}

	void ChooseItem(){
		if (Input.GetButtonDown(aBut)){
			StartCoroutine(trollScript.CheckForCash());
		}
	}

	void NextItem(){
		if ((horAx >= .8f && lastHorAx < .8f) || Input.GetButtonDown(rBut)) {
			StartCoroutine (trollScript.NextItem(1));
		}
		else if ((horAx <= -.8f && lastHorAx > -.8f) || Input.GetButtonDown(lBut)) {
			StartCoroutine (trollScript.NextItem(-1));
		}
	}

	void LateUpdate(){
		lastHorAx = horAx;
	}

	// Update is called once per frame
	void Update () {
		ResetButton ();
		if (!pause){
			SwitchKeyboardController();
			if (!keyboard){
				//moving
				horAx = Input.GetAxisRaw (leftHorStick);
				vertAx = Input.GetAxisRaw (leftVerStick);
				leftAxSize = Vector2.Distance(Vector2.zero, new Vector2(horAx,vertAx));
				//aiming
				horAx2 = Input.GetAxisRaw (rightHorStick);
				vertAx2 = Input.GetAxisRaw (rightVerStick);
			}
			else{
				//moving
				if ((Input.GetButton(leftHorStickLeft) && Input.GetButton(leftHorStickRight)) || (!Input.GetButton(leftHorStickLeft) && !Input.GetButton(leftHorStickRight)) ){
					horAx = 0f;
				}
				else if (Input.GetButton(leftHorStickLeft)){
					horAx = -1f;
				}
				else if (Input.GetButton(leftHorStickRight)){
					horAx = 1f;
				}

				if ((Input.GetButton(leftHorStickUp) && Input.GetButton(leftHorStickDown)) || (!Input.GetButton(leftHorStickUp) && !Input.GetButton(leftHorStickDown)) ){
					vertAx = 0f;
				}
				else if (Input.GetButton(leftHorStickDown)){
					vertAx = -1f;
				}
				else if (Input.GetButton(leftHorStickUp)){
					vertAx = 1f;
				}

				leftAxSize = Vector2.Distance(Vector2.zero, new Vector2(horAx,vertAx));
				//aiming
				horAx2 = Input.GetAxisRaw (rightHorStick);
				vertAx2 = Input.GetAxisRaw (rightVerStick);
			}

			armVec = Vector3.Normalize (new Vector3(horAx, vertAx, 0));
			if (Mathf.Abs (horAx) + Mathf.Abs (vertAx)>.1f && !hammerScript.cocking && !hammerScript.smashing && !hammerScript.smacking && !hammerScript.recovering){
				armAng = Mathf.Atan2 (armVec.y,armVec.x) * Mathf.Rad2Deg;
			}

			if (player.buyingWares){
				Leave();
				NextItem();
				ChooseItem();
			}
			else if (player.alive){
				Jump ();
				//ShieldGunSword ();
				Crouch ();
				ExitThroughTheBlockShop();
				rightShoot = RightTrigger ();
				leftShoot = LeftTrigger ();
				SuitUp();
				Enter();

				switch (weaponState){
				case 0: //GUN
					GunCombineWeapons ();
					GunToggleWeapons ();
					GunDetonate ();
					break;
				case 1: //SHIELD
					break;
				case 2: //HAMMER
					break;
				}
			}


			/*
			//spawning
			if (spawnBlocks){
				//SpawnA();
				SpawnB();
				SpawnX();
				SpawnU();
			}

			if (spawnEnemies){
				SpawnL();
				SpawnR();
			}
*/
		}
		//pausing
		Pause ();
	}

	/*
	void OnGUI(){
		if (pause){
			//Set the GUIStyle style to be label
			GUIStyle menuStyle = GUI.skin.GetStyle ("label");
			
			//Set the style font size to increase and decrease over time
			menuStyle.fontSize = (int)(80.0f + 10.0f * Mathf.Sin (Time.realtimeSinceStartup * 2f));
			if (timer < 5f){
				GUI.contentColor = Color.Lerp (Color.red,Color.blue,timer/5);
				timer+=0.017f;
			}
			else if(timer > 5f && timer<10f){
				GUI.contentColor = Color.Lerp (Color.blue,Color.green,(timer-5)/5);
				timer+=0.017f;
			}
			else if (timer >10f && timer<15f){
				GUI.contentColor = Color.Lerp (Color.green,Color.red,(timer-10)/5);
				timer+=0.017f;
			}
			else{
				timer = 0;
			}
			//Create a label and display with the current settings
			GUI.Label (new Rect (190, 110, 700, 1000), "It's PAusEd BABBYYYY",menuStyle);
		}
	}
	*/
	/*void SpawnA(){
		if (Input.GetButtonDown (aBut)) {
			if (playerName=="Hero1"){
				spawnNeutralBlocks.spawnP1 = true;
			}
			else if (playerName=="Hero2"){
				spawnNeutralBlocks.spawnP2 = true;
			}
		}
	}

	void SpawnX(){
		if (Input.GetButtonDown (xBut)) {
			if (playerName=="Hero1"){
				spawnExplosiveBlocks.spawnP1 = true;
			}
			else if (playerName=="Hero2"){
				spawnExplosiveBlocks.spawnP2 = true;
			}
		}
	}*/
	
	/*void SpawnB(){
		if (Input.GetButtonDown (bBut)) {
			if (playerName=="Hero1"){
				spawnFreezeBlocks.spawnP1 = true;
			}
			else if (playerName=="Hero2"){
				spawnFreezeBlocks.spawnP2 = true;
			}
		}
	}

	void SpawnL(){
		if (Input.GetButtonDown (lBut)) {
			if (playerName=="Hero1"){
				spawnMiniCopterBlocks.spawnP1 = true;
			}
			else if (playerName=="Hero2"){
				spawnMiniCopterBlocks.spawnP2 = true;
			}
		}
	}
	
	void SpawnR(){
		if (Input.GetButtonDown (rBut)) {
			if (playerName=="Hero1"){
				spawnTurretBlocks.spawnP1 = true;
			}
			else if (playerName=="Hero2"){
				spawnTurretBlocks.spawnP2 = true;
			}
		}
	}

	void SpawnU(){
		if (Input.GetButtonDown (uBut)) {
			if (playerName=="Hero1"){
				spawnTelekineticBlocks.spawnP1 = true;
			}
			else if (playerName=="Hero2"){
				spawnTelekineticBlocks.spawnP2 = true;
			}
		}
	}

	void SpawnTank(){
		if (Input.GetButtonDown (rBut)) {
			spawnTankBlocks.spawn = true;
		}
	}
	*/

}
