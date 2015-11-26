using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Animator healthAnimator;
	public AnimationState[] healthAnimationStates;
	public GameObject character;
	public Player player;

	public Vector3 setSpot;

	public float time2DrainFull;
	public float time2Refill;
	public float refillTime;
	public float drainTime;
	public float animationSpeed;
	public float playSpeed;
	public float lastDamage;

	public int[] damages;
	public int i;
	public int lastHealth;
	public int checkDamage;
	public int damQ;
	public int damCount;
	public int curDamCount;

	public bool running;

	void Awake(){
		if (gameObject.name == "HealthBar1") {
			character = GameObject.Find ("Hero1");
			setSpot = new Vector3 (-2f, -1.5f, 0f);
		}
		else{
			character = GameObject.Find ("Hero2");
			setSpot = new Vector3 (2f, -1.5f, 0f);
		}
		player = character.GetComponent<Player> ();

		i = 0;

		healthAnimator = GetComponent<Animator> ();
		healthAnimator.SetInteger ("AnimState", 0);
		healthAnimator.speed = 0;

		drainTime = .25f;
		refillTime = 2f;

		time2DrainFull = 3.417f; //based on animation length
		time2Refill = 3.417f;
		running = false;
		damQ = 0;
		damCount = 0;
		curDamCount = 0;
		damages = new int[100];

		transform.position = setSpot;
	}

	public IEnumerator AnimateHealth(int damageReceived){
		damages [damCount] = damageReceived;
		damQ += 1;
		damCount += 1;
		while(running){
			yield return new WaitForEndOfFrame();
		}

		while (damQ>0){
			running = true;
			healthAnimator.SetInteger("AnimState",0);
			lastDamage = Mathf.Floor(damages[curDamCount]+.1f);
			animationSpeed = (time2DrainFull / drainTime) * (lastDamage / 100f);
			healthAnimator.speed = animationSpeed;
			yield return new WaitForSeconds(drainTime);
			curDamCount += 1;
			running = false;
			damQ -= 1;
			healthAnimator.speed = 0;
		}

		yield return null;
	}

	public IEnumerator RebirthHealth(){
		damages = new int[100];
		damCount = 0;
		curDamCount = 0;
		running = true;
		healthAnimator.SetInteger("AnimState",1);
		animationSpeed = time2Refill / refillTime;
		healthAnimator.speed = animationSpeed;
		yield return new WaitForSeconds(refillTime);
		running = false;
		healthAnimator.SetInteger("AnimState",0);
		healthAnimator.speed = 0f;
	}

	void Update(){
		playSpeed = healthAnimator.speed;
		transform.position = setSpot;
	}


}
