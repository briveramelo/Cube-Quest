using UnityEngine;
using System.Collections;

public class GetDamage : MonoBehaviour {

	//this script serves as a repository for other harmful objects to dump their damage here,
	//and then for the character script to pick it up from here

	public int characterID;
	public int count;
	public int visDam;

	private Player player;
	private MiniCopterBlock miniCopterBlock;
	private TankBlock tankBlock;
	private JumpBlock jumpBlock;
	private TelekineticBlock telekineticBlock;
	private SuperTelekineticBlock superTelekineticBlock;
	public HealthBar healthBar;
	private DestructablePlatform destructablePlatform;
	private TurretBlock turretBlock;
	private SuperTurretBlock superTurretBlock;
	public PistonButton pistonButton;


	// Use this for initialization
	void Awake () {
		count = 0;

		if (gameObject.name == "Hero1") {
			characterID = 0;
			player = GetComponent<Player> ();
		}
		else if (gameObject.name == "Hero2"){
			characterID = 1;
			player = GetComponent<Player> ();
		}
		else if (GetComponent<MiniCopterBlock> ()) {
			characterID = 2;
			miniCopterBlock = GetComponent<MiniCopterBlock> ();
		}
		else if (gameObject.GetComponent<TankBlock> ()) {
			characterID = 3;
			tankBlock = GetComponent<TankBlock> ();
		}
		else if (GetComponent<JumpBlock> ()) {
			characterID = 4;
			jumpBlock = GetComponent<JumpBlock> ();
		} 
		else if (GetComponent<TelekineticBlock> ()) {
			characterID = 5;
			telekineticBlock = GetComponent<TelekineticBlock> ();
		} 
		else if (GetComponent<SuperTelekineticBlock> ()) {
			characterID = 6;
			superTelekineticBlock = GetComponent<SuperTelekineticBlock> ();
		} 
		else if (GetComponent<DestructablePlatform>()){
			characterID = 7;
			destructablePlatform = GetComponent<DestructablePlatform>();
		}
		else if (GetComponent<TurretBlock>()){
			characterID = 8;
			turretBlock = GetComponent<TurretBlock> ();
		}
		else if (GetComponent<SuperTurretBlock>()){
			characterID = 9;
			superTurretBlock = GetComponent<SuperTurretBlock> ();
		}
		else if (GetComponent<PistonButton>()){
			characterID = 10;
			pistonButton = GetComponent<PistonButton> ();
		}
		else{
			characterID = 100;
		}
	}

	public IEnumerator SendDamage(int damage,int blockType){
		count += 1;
		visDam = damage;
		//Start Damage coroutine in effected gameobject
		//so we have to check who has this instance of the GetDamage script
		switch (characterID) {
		case 0:
			StartCoroutine(player.Damage(damage));
			break;
		
		case 1:
			StartCoroutine(player.Damage(damage));
			break;

		case 2:
			StartCoroutine(miniCopterBlock.Damage(damage));
			break;

		case 3:
			StartCoroutine(tankBlock.Damage(damage));
			break;

		case 4:
			StartCoroutine(jumpBlock.Damage(damage));
			break;

		case 5:
			StartCoroutine(telekineticBlock.Damage(damage));
			break;

		case 6:
			StartCoroutine(superTelekineticBlock.Damage(damage));
			break;

		case 7:
			StartCoroutine(destructablePlatform.Damage(damage,blockType));
			break;
		case 8:
			StartCoroutine(turretBlock.Damage(damage));
			break;
		case 9:
			StartCoroutine(superTurretBlock.Damage(damage));
			break;
		case 10:
			StartCoroutine(pistonButton.TriggerLaunch(damage,blockType));
			break;
		}

		yield return new WaitForEndOfFrame();
	}

}
