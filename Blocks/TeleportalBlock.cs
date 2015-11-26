using UnityEngine;
using System.Collections;

public class TeleportalBlock : MonoBehaviour {


	public AudioSource teleportSound;
	public AudioSource teleportPreppingSound;

	public TeleportalBlock[] teleportalBlocks;
	public TeleportalBlock[] allTeleportalBlocksInTheLand;
	public GameObject buddyPortal;
	public TeleportalBlock buddyPortalScript;
	public Rigidbody2D rigidB;
	public GatherAllKnowledge ourMaster;
	public Animator teleAnimator;

	public Vector3 moveDir;
	public float moveSpeed;
	public float waitingGameTime;
	public float time2Set;
	public float startingGscale;
	public float newPan;
	public float panSpeed;

	public int totalTickets;
	public int ticketNum;
	public int i;
	public int j;
	public int[] ourJ;
	public int q;
	public int portalNum;
	public int buddyPortalNum;
	public int teleState;

	public bool phase1;
	public bool phase2;
	public bool openDoor;
	public bool set;
	public bool comeOnIn;
	public bool activatedPortal;

	// Use this for initialization
	void Awake () {
		phase1 = false;
		phase2 = false;
		openDoor = true;
		activatedPortal = false;
		set = false;
		comeOnIn = true;
		ourJ = new int[3];
		teleAnimator = GetComponent<Animator> ();
		teleAnimator.SetInteger("AnimState",0);
		teleportPreppingSound.volume = .2f;
		teleState = 0;
		panSpeed = .4f;
		i = 0;
		q = 0;
		buddyPortalNum = 0;
		moveSpeed = .2f;
		waitingGameTime = 2f;
		time2Set = 1f;

		totalTickets = 3;
		ticketNum = 0;

		ourMaster = GameObject.Find ("theOmniscient").GetComponent<GatherAllKnowledge> ();
		teleportPreppingSound.pan = 0;

	}

	//comes in without gravity
	public IEnumerator Phase1(){
		portalNum = ourMaster.portalNum;
		ourMaster.portalNum++;
		phase1 = true;
		yield return new WaitForSeconds (time2Set);
		set = true;
		rigidbody2D.velocity = Vector2.zero;
		teleportalBlocks = GameObject.FindObjectsOfType<TeleportalBlock> ();
		if (phase1){
			foreach (TeleportalBlock portal in teleportalBlocks) {
				if (portal.portalNum != portalNum && portal.buddyPortalNum==0 && portal.set){
					buddyPortal = portal.gameObject;
					buddyPortalScript = portal;
					buddyPortalScript.buddyPortal = this.gameObject;
					buddyPortalScript.buddyPortalScript = this;
					buddyPortalNum = buddyPortalScript.portalNum;
					buddyPortalScript.buddyPortalNum = portalNum;
					StartCoroutine (buddyPortalScript.Phase2 ());
					StartCoroutine (Phase2());
					teleState = 2;
					buddyPortalScript.teleState = 1;
					StartCoroutine (CounterSyncedPortalAnimation());
					break;
				}
			}
			yield return null;
		}
	}

	public IEnumerator Phase2(){
		phase1 = false;
		yield return null;
		while (!set) {
			yield return null;
		}
		phase2 = true;
		collider2D.isTrigger = true;
		Destroy (rigidbody2D);
	}
	
	public IEnumerator WaitingGame(){
		yield return new WaitForSeconds(waitingGameTime);
		openDoor = true;
	}

	public IEnumerator BuddyWaitingGame(int Jcount){
		yield return new WaitForSeconds (waitingGameTime);
		if (openDoor) {
			buddyPortalScript.openDoor = true;
		}
		ourMaster.youShallNotPass [Jcount] = false;
	}

	public IEnumerator ActivatePortal(){
		activatedPortal = true;
		teleAnimator.SetInteger ("AnimState", 3);
		buddyPortalScript.teleAnimator.SetInteger ("AnimState", 3);
		teleportPreppingSound.Play ();
		StartCoroutine (activatedPortalPanningSounds ());
		yield return null;
	}

	public IEnumerator activatedPortalPanningSounds(){
		bool up = true;
		newPan = 0;
		while (activatedPortal) {
			if (up){
				newPan = Mathf.Lerp(teleportPreppingSound.pan,2,Time.deltaTime * panSpeed);
				if (newPan>=1){
					newPan = 1;
					up = false;
				}
			}
			else{
				newPan = Mathf.Lerp(teleportPreppingSound.pan,-2,Time.deltaTime * panSpeed);
				if (newPan<=-1){
					newPan = -1;
					up = true;
				}
			}
			teleportPreppingSound.pan = newPan;

			yield return new WaitForSeconds(.1f);
		}

	}

	public IEnumerator CounterSyncedPortalAnimation(){
		activatedPortal = false;
		teleAnimator.SetInteger ("AnimState", teleState);
		buddyPortalScript.teleAnimator.SetInteger ("AnimState", buddyPortalScript.teleState);
		yield return null;
	}

	public IEnumerator EnterTheWormhole(Rigidbody2D rig, int Jmoney){
		StartCoroutine (ActivatePortal ());
		openDoor = false;
		buddyPortalScript.openDoor = false;
		startingGscale = rig.gravityScale;
		rig.gravityScale = 0f;
		StartCoroutine (WaitingGame ());

		while (!openDoor && rig) {
			if (!activatedPortal){//if it's not activated, activate it
				StartCoroutine(ActivatePortal());
			}
			moveDir = Vector3.Normalize(transform.position - rig.transform.position);
			rig.velocity = moveDir * moveSpeed;
			yield return null;
		}
		StartCoroutine(CounterSyncedPortalAnimation());
		teleportPreppingSound.Stop ();

		if (rig){
			rig.gravityScale = startingGscale;
			ticketNum++;
			buddyPortalScript.ticketNum = ticketNum;
			rig.position = buddyPortal.transform.position;
			StartCoroutine (BuddyWaitingGame (Jmoney));
			teleportSound.Play();
		}
		else{
			ourMaster.youShallNotPass [Jmoney] = false;
			buddyPortalScript.openDoor = true;
			q--;
			buddyPortalScript.q--;
		}
		
		if (ticketNum >= totalTickets) {
			Instantiate (Resources.Load ("Prefabs/Effects/TeleportalCloseSound"),transform.position,Quaternion.identity);
			Destroy(this.gameObject);
			Destroy(buddyPortal);
		}
		yield return null;
	}

	void OnTriggerStay2D(Collider2D col){
		rigidB = null;
		if (phase2) {
			if (col.rigidbody2D && !col.isTrigger){
				rigidB = col.rigidbody2D;
			}
			else if (col.transform.parent && !col.isTrigger){
				if (col.transform.parent.rigidbody2D){
					rigidB = col.transform.parent.rigidbody2D;
				}
				else if (col.transform.parent.parent){
					if (col.transform.parent.parent.rigidbody2D){
						rigidB = col.transform.parent.parent.rigidbody2D;
					}
				}
			}

			if (rigidB){
				comeOnIn = true;
				j=0;
				foreach (Rigidbody2D rigger in ourMaster.portalBlacklist){
					if (rigger && rigger==rigidB && ourMaster.youShallNotPass[j]){
						comeOnIn = false;
						break;
					}
					j++;
				}
			}
			else{
				comeOnIn = false;
			}

			if (col.gameObject == buddyPortal){
				comeOnIn = false;
			}

			if (comeOnIn && q<3){
				ourJ[q]=ourMaster.j;
				buddyPortalScript.ourJ[q]=ourMaster.j;

				ourMaster.portalBlacklist[ourMaster.j] = rigidB;
				ourMaster.youShallNotPass[ourMaster.j] = true;

				q++;
				buddyPortalScript.q++;
				StartCoroutine(EnterTheWormhole(rigidB,ourMaster.j));
				ourMaster.j++;

			}
		}
	}

	void OnDestroy(){
		j = 0;
		while (j<3){
			ourMaster.youShallNotPass [ourJ[j]] = false;
			j++;
		}
		ourMaster.youShallNotPass [ourJ[j-1]+1] = false;
		StopAllCoroutines ();
		//rig.gravityScale = startingGscale;

		//crazy animation shit
	}

}
