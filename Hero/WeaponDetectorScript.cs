using UnityEngine;
using System.Collections;
using System.Linq;

public class WeaponDetectorScript : MonoBehaviour {

	public GameObject[] storedWeapons;
	public GameObject blockSpawner;
	public GameObject blockInSlot;
	public GameObject superTelekineticBlock;
	public GameObject character;
	public GameObject superBlock;

	public Transform arm;

	public Collider2D[] slotCheck;


	public WeaponBlockScript[] allWeapons;
	public WeaponBlockScript[] activatedBlocks;
	public MiniCopterBlock[] allCopters;
	public SuperTelekineticBlock superTelekineticBlockScript;
	public TelekineticBlock telekineticBlockScript;
	public TelekineticBlock[] telekineticBlockScripts;

	public TractorBeam tractorBeam;
	public ChargeDisplay chargeDisplay;
	public Player player;
	public Controller controller;
	public WeaponBlockScript weaponBlockScript;
	public MiniCopterBlock miniCopterBlock;
	public CameraFollowPlayer1 cameraFollowPlayer1;
	public CameraBattleArena cameraBattleArena;

	public AudioSource chargeNoise;
	public AudioSource shootLow;
	public AudioSource shootMed;
	public AudioSource shootHigh;
	public AudioSource teleDisconnect;
	public AudioSource toggleWeapons;

	private string superBlockPath;
	private string superNeutralBlockPath;
	private string superExplosiveBlockPath;
	private string superFrozesplosiveBlockPath;
	private string superTelekineticBlockPath;
	private string superTurretBlockPath;

	public Vector3[] slots;
	public Vector3 fDir;
	public Vector3 kDir;
	public Vector3 aPos;
	public Vector3 hPos;
	public Vector3 slotSpot4SuperBlock;

	public float baselineKnockback;
	public float knockbackFactor;
	public float charger;
	public float lastFrameTrigger;
	public float thisFrameTrigger;
	public float lowThresh;
	public float highThresh;
	public float baselineFire;
	public float fireSpeed;
	public float leftShoot;
	public float gravScale;

	public bool nowProjectile; //check if the block has been fired
	public bool occupied; //check if this weapon detector is occupied
	public bool lockstatus;
	public bool wasOccupied;
	public bool c1;
	public bool c2;
	public bool waiting;
	public bool checking;
	public bool playToggle;
	public bool scale;
	public bool shooting;
	public bool pushing;
	public bool combinator;

	public int[] blockTypes;
	public int slotNum;
	public int j;
	public int i;
	public int k;
	public int c;
	public int p;
	public int l;
	public int f;
	public int numCops;
	public int wepDSID;
	public int launchCount;
	public int numBlocks;

	public Vector3 slot1;
	public Vector3 slot2;
	public Vector3 slot3;

	void Awake () {
		wasOccupied = false;

		if (GameObject.Find ("BlockSpawner")){
			blockSpawner = GameObject.Find ("BlockSpawner");
		}
		launchCount = 0;
		f = 0;
		lowThresh = 10f;
		highThresh = 90f;
		knockbackFactor = 4f;
		baselineKnockback = 20f;

		occupied = false;
		nowProjectile = false;
		waiting = false;
		blockTypes = new int[] {0,0,0,0};
		storedWeapons = new GameObject[] {null,null,null,null};

		superNeutralBlockPath = "Prefabs/WeaponBlocks/SuperNeutralBlock";
		superExplosiveBlockPath = "Prefabs/WeaponBlocks/SuperExplosiveBlock";
		superFrozesplosiveBlockPath = "Prefabs/WeaponBlocks/SuperFreezeBlock";
		superTelekineticBlockPath = "Prefabs/WeaponBlocks/SuperTelekineticBlock";
		superTurretBlockPath = "Prefabs/WeaponBlocks/SuperTurretBlock";

		if (gameObject.name == "WeaponDetector1"){
			chargeDisplay = GameObject.Find ("ChargeBar1").GetComponent<ChargeDisplay> ();
			character = GameObject.Find ("Hero1");

			arm = GameObject.Find ("Arm1").transform;
			tractorBeam = GameObject.Find ("TractorBeam1").GetComponent<TractorBeam>();
			wepDSID = 1;
		}
		else {
			chargeDisplay = GameObject.Find ("ChargeBar2").GetComponent<ChargeDisplay> ();
			character = GameObject.Find ("Hero2");

			arm = GameObject.Find ("Arm2").transform;
			tractorBeam = GameObject.Find ("TractorBeam2").GetComponent<TractorBeam>();
			wepDSID = 2;
		}
		player = character.GetComponent<Player> ();
		controller = character.GetComponent<Controller> ();

		if (GameObject.Find ("MainCamera").GetComponent<CameraFollowPlayer1> ().enabled){
			c1 = true;
			c2 = false;
			cameraFollowPlayer1 = GameObject.Find ("MainCamera").GetComponent<CameraFollowPlayer1> ();
		}
		else {
			c1 = false;
			c2 = true;
			cameraBattleArena = GameObject.Find ("MainCamera").GetComponent<CameraBattleArena> ();
		}
		baselineFire = 300f;
		fireSpeed = 12f;
		StartCoroutine (PeriodicCatalog ());
	}

	public IEnumerator PeriodicCatalog(){
		StartCoroutine(CatalogWeapons());
		yield return new WaitForSeconds (1f);
		StartCoroutine (PeriodicCatalog ());
	}

	public IEnumerator DestroyHeld(){
		foreach (GameObject weapon in storedWeapons) {
			Destroy(weapon);
		}
		yield return null;
	}

	public IEnumerator DestroyBlockInHand(){
		yield return StartCoroutine(CatalogWeapons ());
		if (storedWeapons [0]) {
			Destroy(storedWeapons[0]);
		}
		yield return StartCoroutine(CatalogWeapons ());
	}
	
	public IEnumerator Distract(){
		tractorBeam.attract = false;
		yield return null;
	}
	
	public IEnumerator ReAttract(){
		if (player.alive && !occupied){
			tractorBeam.attract = true;
		}
		yield return null;
	}

	public IEnumerator CombineWeapons(){
		if (combinator){
			yield return StartCoroutine (CatalogWeapons ());
			if (blockTypes[1] == blockTypes[2] && blockTypes[1] == blockTypes[3] && (blockTypes[1] == 1 || blockTypes[1] ==2 || blockTypes[1] ==3 || blockTypes[1] == 7 || blockTypes[1] == 11)){
				if (occupied){
					slotSpot4SuperBlock = slot3; //Check me for changes to slot Spots and slots and so on
					lockstatus = false;
					slotNum = 4;
				}
				else{
					slotSpot4SuperBlock = hPos;
					lockstatus = true;
					slotNum = 1;
				}

				switch (blockTypes[1]){
				case 1:
					superBlockPath = superNeutralBlockPath;
					break;
				case 2:
					superBlockPath = superExplosiveBlockPath;
					break;
				case 3:
					superBlockPath = superFrozesplosiveBlockPath;
					break;
				case 7:
					superBlockPath = superTelekineticBlockPath;
					break;
				case 11:
					superBlockPath = superTurretBlockPath;
					break;
				}

				superBlock = Instantiate ( Resources.Load (superBlockPath),slotSpot4SuperBlock,Quaternion.identity) as GameObject;
				weaponBlockScript = superBlock.GetComponent<WeaponBlockScript>();
				weaponBlockScript.wepDSID = wepDSID;
				weaponBlockScript.toggleCount = slotNum;
				if (lockstatus){
					StartCoroutine(weaponBlockScript.LockIn());
				}
				superBlock.collider2D.isTrigger = true;

				i=1;
				while ( i<storedWeapons.Length ){
					Destroy (storedWeapons[i]);
					i++;
				}
			}
		}
		yield return StartCoroutine (CatalogWeapons ());
	}


	public IEnumerator CatalogWeapons(){
		blockTypes = new int[] {0,0,0,0};
		storedWeapons = new GameObject[] {null,null,null,null};
		j = 1;
		numBlocks = 0;
		while (j<5){
			slotCheck = Physics2D.OverlapCircleAll (character.transform.position,3f);
			//Physics2D.OverlapPointNonAlloc(slot,slotCheck);
			foreach (Collider2D block in slotCheck) {
				if (block){
					if (block.GetComponent<WeaponBlockScript>()){
						weaponBlockScript = block.GetComponent<WeaponBlockScript>();
						if (weaponBlockScript.wepDSID == wepDSID){	
							if ( weaponBlockScript.toggleCount == j ) {
								blockTypes[j-1] = weaponBlockScript.blockType;
								storedWeapons[j-1] = block.gameObject;
								numBlocks++;
							}
						}
					}
					else if (block.GetComponent<MiniCopterBlock>()){
						miniCopterBlock = block.GetComponent<MiniCopterBlock>();
						if (miniCopterBlock.wepDSID == wepDSID){
							if (miniCopterBlock.toggleCount == j) {
								blockTypes[j-1] = miniCopterBlock.blockType;
								storedWeapons[j-1] = block.gameObject;
								numBlocks++;
							}
						}
					}
				}
				else{
					blockTypes[j-1] = -1;
					storedWeapons[j-1] = null;
				}
			}
			j++;
		}
		StartCoroutine (GravityMonitor ());

		if (!storedWeapons [0]) {
			occupied = false;
		}
		else {
			occupied = true;
			blockInSlot = storedWeapons[0];
		}
		yield return null;
	}

	public IEnumerator TeleBlockCheck(){
		l = 0;
		telekineticBlockScripts = null;

		telekineticBlockScripts = GameObject.FindObjectsOfType<TelekineticBlock> ();
		foreach (TelekineticBlock tele in telekineticBlockScripts){
			if (tele.GetComponent<WeaponBlockScript>().wepDSID == wepDSID) {
				tele.holdCount = l;
				if (tele.holdCount>2){
					StartCoroutine(tele.FallApart());
				}
				l++;
			}
		}
		yield return null;
	}
	
	public IEnumerator GravityMonitor(){
		k = 0;
		numCops = 0;
		while (k<blockTypes.Length) {
			if (blockTypes[k]==-1){ //this is blockType of the miniCopterBlock
				numCops += 1;
			}
			k++;
		}
		gravScale = 1f - (numCops/3f) ;
		if (gravScale <= 0) {
			gravScale = 0;
			player.copterHovering = true;
		}
		else{
			player.copterHovering = false;
		}
		character.rigidbody2D.gravityScale = gravScale;
		if (player.teleHovering) {
			character.rigidbody2D.gravityScale = 0;
		}
		yield return null;
	}

	public IEnumerator ToggleWeapons(){
		playToggle = false;
		yield return StartCoroutine (CatalogWeapons ());
		foreach (GameObject block in storedWeapons) {
			if (block){
				playToggle = true;
				if (block.GetComponent<WeaponBlockScript>()){
					weaponBlockScript = block.GetComponent<WeaponBlockScript>();
					weaponBlockScript.lockedin = false;
					switch (weaponBlockScript.toggleCount){
					case 1:
						weaponBlockScript.toggleCount = 2;
						block.transform.position = slot1;

						break;
					case 2:
						weaponBlockScript.toggleCount = 3;
						block.transform.position = slot2;
						break;
					case 3:
						weaponBlockScript.toggleCount = 4;
						block.transform.position = slot3;
						break;
					case 4:
						weaponBlockScript.toggleCount = 1;
						StartCoroutine(weaponBlockScript.LockIn());
						blockInSlot = block;
						block.transform.position = hPos;
						break;
					}
				}
				else if (block.GetComponent<MiniCopterBlock>()){
					miniCopterBlock = block.GetComponent<MiniCopterBlock>();
					miniCopterBlock.lockedin = false;
					switch (miniCopterBlock.toggleCount){
					case 1:
						miniCopterBlock.toggleCount = 2;
						block.transform.position = slot1;
						break;
					case 2:
						miniCopterBlock.toggleCount = 3;
						block.transform.position = slot2;
						break;
					case 3:
						miniCopterBlock.toggleCount = 4;
						block.transform.position = slot3;
						break;
					case 4:
						miniCopterBlock.toggleCount = 1;
						StartCoroutine(miniCopterBlock.LockIn());
						blockInSlot = block;
						block.transform.position = hPos;
						break;
					}
				}
			}
		}
		if (playToggle){
			toggleWeapons.Play();
		}
		yield return StartCoroutine (CatalogWeapons ());
	}

	public IEnumerator PlayCharge(){
		if (occupied){
			chargeNoise.Play ();
			wasOccupied = true;
		}

		yield return null;
	}

	public IEnumerator LaunchOverIt(){
		StartCoroutine (CatalogWeapons ());
		StartCoroutine (StopChargeNoise ());
		StartCoroutine (chargeDisplay.StopCharger());
		if (blockInSlot){
			if (chargeDisplay.charger!=0 & !waiting){
				//TRIGGER RELEASE BUNDLE// (see below)
				waiting = true;
				launchCount++;
				StartCoroutine (ReleaseNoise ());
				StartCoroutine (KickBack ());
				StartCoroutine (InitialPassThrough());
				if (c1){
					StartCoroutine (cameraFollowPlayer1.Shake());
				}
				else{
					StartCoroutine (cameraBattleArena.Shake());
				}

				//things that happen to ALL fired weapons

				blockInSlot.rigidbody2D.WakeUp();
				if (blockTypes[0]!=7 && blockTypes[0]!=8 && blockTypes[0]!=13){ //telekineticblocks should float
					blockInSlot.rigidbody2D.gravityScale = 1f;
				}
				if (blockTypes[0]!=11 && blockTypes[0]!=12){ //turretblocks should stay small
					//blockInSlot.transform.localScale = Vector3.one * 3f;
				}
				charger = chargeDisplay.charger;
				blockInSlot.rigidbody2D.velocity = Vector2.zero;
				yield return new WaitForEndOfFrame();
				blockInSlot.rigidbody2D.AddForce(fDir * (baselineFire + fireSpeed * charger));
				blockInSlot.collider2D.isTrigger = false;
				occupied = false;
				wasOccupied = false;

				//Special things that those special blocks do
				if (blockInSlot.GetComponent<WeaponBlockScript> ()) {
					weaponBlockScript = blockInSlot.GetComponent<WeaponBlockScript> ();
					StartCoroutine(weaponBlockScript.Project());
				}
			
				switch (blockTypes[0]){
				case 2:
					Explode explode = blockInSlot.GetComponent<Explode>();
					StartCoroutine(explode.BlowMe());
					break;
				case 3:
					Freeze freezeScript = blockInSlot.GetComponent<Freeze>();
					freezeScript.nowProjectile = true;
					break;
				case 4:
					NeutralSuper neutralSuper = blockInSlot.GetComponent<NeutralSuper>();
					StartCoroutine(neutralSuper.BreakUp());
					break;
				case 5:
					ExplodeSuper explodeSuper = blockInSlot.GetComponent<ExplodeSuper>();
					StartCoroutine(explodeSuper.GetIntimate());
					break;
				case 6: // super freeze
					SuperFreezeBlock freezeSuper = blockInSlot.GetComponent<SuperFreezeBlock>();
					freezeSuper.wepDSID = wepDSID;
					break;
				case 7: //telekinetic block
					telekineticBlockScript = blockInSlot.GetComponent<TelekineticBlock>();
					occupied = true;
					wasOccupied = true;

					telekineticBlockScript.weaponDetectorScript = this;
					telekineticBlockScript.heroSpot = arm;
					telekineticBlockScript.player = player;
					telekineticBlockScript.wepDSID = wepDSID;
					telekineticBlockScript.GetComponent<WeaponBlockScript>().wepDSID = wepDSID;

					StartCoroutine (TeleBlockCheck());
					StartCoroutine (telekineticBlockScript.Orbit());
					StartCoroutine (telekineticBlockScript.StartingOff());
					break;
				case 8: //super telekinetic block
					superTelekineticBlockScript = blockInSlot.GetComponent<SuperTelekineticBlock>();
					superTelekineticBlockScript.weaponDetectorScript = this;
					superTelekineticBlockScript.character = character;
					superTelekineticBlockScript.wepDSID = wepDSID;
					superTelekineticBlockScript.GetComponent<WeaponBlockScript> ().wepDSID = wepDSID;

					GameObject expo = Instantiate ( Resources.Load("Prefabs/Effects/Telesplosion"),hPos,Quaternion.identity) as GameObject;
					expo.transform.localScale = Vector3.one;

					StartCoroutine (superTelekineticBlockScript.Welcome());
					break;
				case -1:
					MiniCopterBlock miniCopterBlock = blockInSlot.GetComponent<MiniCopterBlock>();
					StartCoroutine(miniCopterBlock.BeFree());
					break;
				case 11: //turret block
					//blockInSlot.transform.localScale = Vector3.one;
					break;
				case 12: //super turret block
					//blockInSlot.transform.localScale = Vector3.one;
					break;
				case 13:
					TeleportalBlock teleB = blockInSlot.GetComponent<TeleportalBlock>();
					StartCoroutine(teleB.Phase1());
					break;
				case 15:
					ShockBlock shockb = blockInSlot.GetComponent<ShockBlock>();
					StartCoroutine(shockb.FlipTheSwitch());
					break;
				}

				StartCoroutine(CatalogWeapons());
				yield return new WaitForSeconds(0.01f);
				waiting = false;
				//StartCoroutine (ToggleWeapons());
				yield return null;
			}
		}
	}

	//RELEASE BUNDLE START//

	public IEnumerator ReleaseNoise(){
		chargeNoise.Stop ();
		if (chargeDisplay.charger<=lowThresh){
			shootLow.Play ();
		}
		else if (chargeDisplay.charger>lowThresh && chargeDisplay.charger<=highThresh){
			shootMed.Play ();
		}
		else{
			shootHigh.Play();
		}
		
		yield return null;
	}

	public IEnumerator StopChargeNoise(){
		chargeNoise.Stop ();
		yield return null;
	}
	
	public IEnumerator InitialPassThrough(){
		if (blockInSlot){
			int startLayerBlock = blockInSlot.layer;
			int startLayerPlayer = character.layer;
			blockInSlot.layer = 9;
			character.layer = 13;

			Physics2D.IgnoreLayerCollision (9, 13, true);
			yield return new WaitForSeconds (.1f);
			Physics2D.IgnoreLayerCollision (9, 13, false);
			if (blockInSlot) {
				blockInSlot.layer = startLayerBlock;
			}
			character.layer = startLayerPlayer;
		}
	}

	public IEnumerator KickBack(){
		kDir = -fDir;
		character.rigidbody2D.AddForce (kDir * (baselineKnockback + chargeDisplay.charger * knockbackFactor) ); //ranges between 20 + (0-100) * (4) so 20-420
		yield return null;
	}
	//RELEASE BUNDLE END//

	void OnTriggerStay2D(Collider2D col){
		if (!occupied){
			if ( col.GetComponent<WeaponBlockScript>() ) { 
				weaponBlockScript = col.GetComponent<WeaponBlockScript>();
				if (!weaponBlockScript.nowProjectile && weaponBlockScript.toggleCount<1){
					blockInSlot = col.gameObject;
					occupied = true;
					weaponBlockScript.wepDSID = wepDSID; 
					StartCoroutine(weaponBlockScript.LockIn());
					StartCoroutine(CatalogWeapons());
				}
					
			}
			else if ( col.GetComponent<MiniCopterBlock>() ){
				miniCopterBlock = col.GetComponent<MiniCopterBlock>();
				if (!miniCopterBlock.recovering && miniCopterBlock.toggleCount<1){
					blockInSlot = col.gameObject;
					occupied = true;
					miniCopterBlock.wepDSID = wepDSID;
					StartCoroutine(miniCopterBlock.LockIn());
					StartCoroutine(CatalogWeapons());
				}
			}
		}
	}

	void Update() {
		hPos = transform.position;
		aPos = arm.position;
		fDir = Vector3.Normalize (hPos - aPos);
		slot1 = character.transform.position + new Vector3 (.15f * -character.transform.localScale.x, 0.05f, 0f);
		slot2 = character.transform.position + new Vector3 (.15f * -character.transform.localScale.x, 0.15f, 0f);
		slot3 = character.transform.position + new Vector3 (.15f * -character.transform.localScale.x, 0.25f, 0f);
	}
}