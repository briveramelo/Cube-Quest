using UnityEngine;
using System.Collections;

public class DestructablePlatform : MonoBehaviour {

	public GameObject crumber;
	public Animator platAnimator;
	
	public Vector3 shrinkScale;

	public string crumbPath;

	public float crumbTorque;
	public float forceToLife;

	public int[] damageStates;
	public int startingHealth;
	public int currentHealth;
	public int crumbs;
	public int lastHealth;
	public int numCrum;
	public int[] platType;

	public bool doDamage;

	void Awake(){
		platType = new int[25];

		startingHealth = 20;
		currentHealth = startingHealth;
		lastHealth = currentHealth;
		platAnimator = GetComponent<Animator> ();
		platAnimator.SetInteger ("AnimState", 1);
		damageStates = new int[]{
			20,
			15,
			10,
			5,
			0
		};
		crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		crumbTorque = 5f;
		forceToLife = 5f;
		shrinkScale = Vector3.one * .8f;

		if (name.Contains("Explosive_Standard")){
			platType[0] = 2;
			platType[1] = 5;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}
		else if (name.Contains("Freeze_Standard")){
			platType[0] = 3;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}
		//else if (name.Contains("Neut_Sup")){
		//	platType[0] = 4;
		//	crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		//}
		else if (name.Contains("Explosive_Super")){
			platType[0] = 5;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}
		else if (name.Contains("Freeze_Super")){
			platType[0] = 6;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}
		else if (name.Contains("Telekinetic_Standard")){
			platType[0] = 7;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}
		else if (name.Contains("Telekinetic_Super")){
			platType[0] = 8;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}
		else if (name.Contains("Acid_Standard")){
			platType[0] = 9;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}
		/*else if (name.Contains("Acid_Sup")){
			platType = 10;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}*/
		else if (name.Contains("Turret_Standard")){
			platType[0] = 11;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}
		else if (name.Contains("Turret_Super")){
			platType[0] = 12;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}
		//else if (name.Contains("Teleportal_Standard")){
		//	platType[0] = 13;
		//	crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		//}
		//else if (name.Contains("Teleportal_Super")){
		//	platType = 14;
		//	crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		//}
		else if (name.Contains("Shock_Standard")){
			platType[0] = 15;
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}
		else if (name.Contains("Hammer")){
			platType[0] = -3;
		}
		else if (name.Contains("JumperBlock")){
			platType[0] = -2;
		}
		else if (name.Contains("CopterBlock")){
			platType[0] = -1;
		}
		else if (name.Contains("Missle")){
			platType[0] = -5;
		}
		else if (name.Contains("Crumble")){
			platType[0] = -6;
		}
		else { //neutral block
			platType = new int[]{
				-5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
			};
			crumbPath = "Prefabs/Platforms/Destructible/Crumb_Neutral";
		}

	}
	
	public IEnumerator Damage(int damageReceived,int blockType){
		doDamage = false;

		foreach (int num in platType) {
			if (num == blockType){
				doDamage = true;
				break;
			}
		}
		if (doDamage){

			//include something to animate future healthbar
			if (currentHealth - damageReceived < 0 ){
				damageReceived = currentHealth;
			}
			
			currentHealth -= damageReceived;

			if (currentHealth<=damageStates[4]) { //0
				if (lastHealth>=damageStates[0]){
					StartCoroutine (SpawnCrumbs(5));
				}
				else if (lastHealth>=damageStates[1]){
					StartCoroutine (SpawnCrumbs(4));
				}
				else if (lastHealth>=damageStates[2]){
					StartCoroutine (SpawnCrumbs(3));
				}
				else if (lastHealth>=damageStates[3]){
					StartCoroutine (SpawnCrumbs(2));
				}
				else{
					StartCoroutine (SpawnCrumbs(1));
				}
				Destroy(this.gameObject);
			}
			else if (currentHealth<=damageStates[3]) { //5
				if (lastHealth>=damageStates[0]){
					platAnimator.SetInteger ("AnimState", 5);
					StartCoroutine (SpawnCrumbs(4));
				}
				else if (lastHealth>=damageStates[1]){
					platAnimator.SetInteger ("AnimState", 5);
					StartCoroutine (SpawnCrumbs(3));
				}
				else if (lastHealth>=damageStates[2]){
					platAnimator.SetInteger ("AnimState", 5);
					StartCoroutine (SpawnCrumbs(2));
				}
				else{
					platAnimator.SetInteger ("AnimState", 5);
					StartCoroutine (SpawnCrumbs(1));
				}
			}
			else if (currentHealth<=damageStates[2]) { //10
				if (lastHealth>=damageStates[0]){
					platAnimator.SetInteger ("AnimState", 4);
					StartCoroutine (SpawnCrumbs(3));
				}
				else if (lastHealth>=damageStates[1]){
					platAnimator.SetInteger ("AnimState", 4);
					StartCoroutine (SpawnCrumbs(2));
				}
				else{
					platAnimator.SetInteger ("AnimState", 4);
					StartCoroutine (SpawnCrumbs(1));
				}
			}
			else if (currentHealth<=damageStates[1]) { //15
				if (lastHealth>=damageStates[0]){
					platAnimator.SetInteger ("AnimState", 3);
					StartCoroutine (SpawnCrumbs(2));
				}
				else{
					platAnimator.SetInteger ("AnimState", 3);
					StartCoroutine (SpawnCrumbs(1));
				}
			}
			else if (currentHealth<=damageStates[0]) { //20
				if (lastHealth>=damageStates[0]){
					platAnimator.SetInteger ("AnimState", 2);
					StartCoroutine (SpawnCrumbs(1));
				}			
			}

			lastHealth = currentHealth;
		}
		yield return null;
		
	}

	public IEnumerator SpawnCrumbs(int numCrum){
		while (numCrum>0){
			crumber = Instantiate (Resources.Load (crumbPath),transform.position, Quaternion.identity) as GameObject;
			crumber.transform.localScale = shrinkScale;
			crumber.rigidbody2D.AddTorque(crumbTorque);
			crumber.rigidbody2D.AddForce(forceToLife * Vector2.up);
			numCrum--;
		}
		yield return null;
	}
}
