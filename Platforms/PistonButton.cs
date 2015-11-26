using UnityEngine;
using System.Collections;
[RequireComponent(typeof(GetDamage))]

public class PistonButton : MonoBehaviour {

	public PistonPlatform pistonPlatform;
	public AudioSource buttonClick;
	public AudioSource buttonLaunch;
	public MoveToHere moveToHere;

	public int[] buttonType;
	public int triggerDamage;

	private float pushTime;
	private float height;
	private float force;
	public float forceTol;
	public bool oneAndDone;
	public bool hideSwitch;
	public Transform HideSwitch;
	public bool triggerPlatformToMove;
	public Transform PlatformToMove;
	public bool triggerPlatformToLaunch;
	public Transform PlatformToLaunch;

	private bool pushedIn;
	private Animator buttonAnimator;
	private int i;


	private SpriteRenderer[] switchSprites;
	private Collider2D[] switchColliders;
	private BoxCollider2D[] thisCollider;

	// Use this for initialization
	void Awake () {
		buttonType = new int[]{0,0,0};
		pushTime = 1f;
		buttonAnimator = GetComponent<Animator> ();

		if (name.Contains("Exp_Stan")){
			buttonType[0] = 2;
		}
		else if (name.Contains("Fro_Stan")){
			buttonType[0] = 3;
		}
		//else if (name.Contains("Neut_Sup")){
		//	buttonType[0] = 4;
		//}
		else if (name.Contains("Exp_Sup")){
			buttonType[0] = 5;
		}
		else if (name.Contains("Fro_Sup")){
			buttonType[0] = 6;
		}
		else if (name.Contains("TeleK_Stan")){
			buttonType[0] = 7;
		}
		else if (name.Contains("TeleK_Sup")){
			buttonType[0] = 8;
		}
		else if (name.Contains("Acid_Stan")){
			buttonType[0] = 9;
		}
		/*else if (name.Contains("Acid_Sup")){
			buttonType = 10;
		}*/
		else if (name.Contains("Tur_Stan")){
			buttonType[0] = 11;
		}
		else if (name.Contains("Tur_Sup")){
			buttonType[0] = 12;
		}
		else if (name.Contains("TeleP_Stan")){
			buttonType[0] = 13;
		}
		//else if (name.Contains("TeleP_Sup")){
		//	buttonType = 14;
		//}
		else if (name.Contains("Shock_Stan")){
			buttonType[0] = 15;
		}
		else if (name.Contains("Ham")){
			buttonType[0] = -3;
		}
		else if (name.Contains("Jump")){
			buttonType[0] = -2;
		}
		else if (name.Contains("Copter")){
			buttonType[0] = -1;
		}
		else if (name.Contains("Missle")){
			buttonType[0] = -5;
		}
		else { //neutral block
			buttonType[0] = 1;
			buttonType[1] = -3;
		}



		if (hideSwitch){
			switchSprites = HideSwitch.GetComponentsInChildren<SpriteRenderer>();
			switchColliders = HideSwitch.GetComponentsInChildren<Collider2D>();
		}
		if (triggerPlatformToMove){
			moveToHere = PlatformToMove.GetComponentInChildren<MoveToHere>();
		}
		else if (triggerPlatformToLaunch){
			PlatformToMove.GetComponentInChildren<PistonPlatform>();
		}
	
		i=0;
		thisCollider = GetComponents<BoxCollider2D> ();
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.rigidbody2D) {
			force = Mathf.Abs (Vector2.Dot (col.contacts [0].normal, col.relativeVelocity) * col.gameObject.rigidbody2D.mass);
			if (!pushedIn && force>forceTol && col.contacts[0].point.y > transform.position.y){
				StartCoroutine (PushIn ());
			}
		}
	}

	public IEnumerator TriggerLaunch(int damage, int blockType){
		if (damage > triggerDamage && triggerDamage>0) {
			foreach (int num in buttonType) {
				if (num == blockType){
					StartCoroutine (PushIn());
					break;
				}
			}
		}
		yield return null;
	}

	public IEnumerator PushIn(){
				
		if (triggerPlatformToMove){
			StartCoroutine (moveToHere.MoveAround());
		}
		if (triggerPlatformToLaunch){
			StartCoroutine (pistonPlatform.Launch());
			if (!pistonPlatform.moving){
				buttonLaunch.Play ();
			}
			else{
				buttonClick.Play();
			}
		}
		if (hideSwitch){
			buttonLaunch.Play ();
			StartCoroutine (Switch());
		}

		pushedIn = true;
		buttonAnimator.SetInteger ("AnimState", 1); //Start Flattening Animation
		thisCollider [0].enabled = false;
		yield return new WaitForSeconds (.5f);
		StartCoroutine (PushOut ());
		yield return null;
	}

	public IEnumerator Switch(){
		if (hideSwitch){
			foreach (Collider2D col in switchColliders) {
				if (!(col == thisCollider[0] || col == thisCollider[1])){
					col.enabled = !col.enabled;
				}
			}

			foreach (SpriteRenderer sprite in switchSprites) {
				if (sprite != GetComponent<SpriteRenderer>()){
					sprite.enabled = !sprite.enabled;
				}
			}
		}

		while (buttonLaunch.isPlaying) {
			yield return null;
		}
	}

	void OnTriggerStay2D(Collider2D coltan){
		Vector2 moveDir = new Vector2 (-Mathf.Sin (transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos (transform.rotation.eulerAngles.z * Mathf.Deg2Rad));

		if (coltan.name == "Hero1") {
			coltan.rigidbody2D.AddForce (moveDir * 10f);
		}
		else if (coltan.rigidbody2D) {
			coltan.rigidbody2D.AddForce (moveDir * 15f );
		}
	}

	public IEnumerator PushOut(){
		if (!oneAndDone) {
			pushedIn = false;
			buttonAnimator.SetInteger ("AnimState", 0); //Start Repoofening Animation
			thisCollider [0].enabled = true;
			thisCollider [0].isTrigger = true;
			yield return new WaitForSeconds (.5f);
			thisCollider [0].isTrigger = false;
		}
		yield return null;
	}

}
