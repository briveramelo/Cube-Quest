using UnityEngine;
using System.Collections;

public class SuperTelekineticBlockController : MonoBehaviour {

	//does rotation, push and pull

	public GameObject character;
	public WeaponDetectorScript weaponDetectorScript;
	public Controller controller;
	public Player player;
	public SuperTelekineticBlock superTelekineticBlockScript;

	public GameObject superTelekineticBlock;
	public GameObject[] telekineticBlocks;
	public TelekineticBlock[] telekineticBlockScripts;
	public TelekineticBlockController[] telekineticBlockControllerScripts;

	public ArmRotate armRotate;

	public Quaternion bodyTarg;

	public Vector3 pointDir;
	public Vector3 moveDir;
	public Vector3 targBodyVec;
	public Vector3 pullDir;
	public Vector3 holdSpot;

	public float pullForce;
	public float targRotAng;
	public float maxMoveSpeed;
	public float moveForce;
	public float maxAway;
	public float minAway;
	public float newHoldRadius;
	public float change;
	public float[] timeCheck;
	public float angleTol;
	public float triggerDelay;
	public float rotoSpeed;
	public float corRotoSpeed;
	public float aimTol;
	public float startingAng;
	public float holdAwayRadius;
	public float[] sepDists;
	public float sepDistTol;
	public float brakeForce;
	public float brakeDistanceFactor;
	public float brakeSpeedFactor;
	public float vel;
	public float maxVelocity;
	public float pullFactor;

	public int controllerID;
	public int i;
	public int wepDSID;

	public bool rotating;
	public bool recovering;
	public bool pushing;
	public bool teleHovering;
	public bool separated;
	public bool clockwise;
	
	// Use this for initialization
	void Awake () {




	}




	
	void OnDestroy(){

		player.teleHovering = false;
	}
}
