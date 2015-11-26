using UnityEngine;
using System.Collections;
using System.Linq;

public class Explode : MonoBehaviour {

	public WeaponBlockScript weaponBlockScript;

	public AudioSource bigBoom;
	public AudioSource pull;

	public GetDamage getDamage;

	public GameObject[] allGameObjects;
	public Collider2D[] allObjects;

	public RaycastHit2D[] rayExps;
	public RaycastHit2D[] rayThings;

	private string[] dontBlowStrings;

	public Vector3 hitPointThing;
	public Vector3 hitPointExp;
	public Vector3 bPos;
	public Vector3 thingPos;
	public Vector3 exDir;

	public Vector2 pullDir;
	
	public float time2Blow;
	public float triggerTimer;
	public float dist2exploder;
	public float exMultiplier;
	public float exForce;
	public float exForceBaseline;
	public float damageBaseline;
	private float exRadius;

	public int damage;
	public int i;
	public int j;
	public int blows;

	public bool play;
	public bool horny;
	public bool makeItHurt;
	
	void Awake() {
		weaponBlockScript = GetComponent<WeaponBlockScript> ();
		blows = 0;
		i = 0;
		j = 0;
		exRadius = 7f;
		time2Blow = 1f;
		play = true;
		horny = true;
		this.enabled = true;
		exForceBaseline = 900f;
		damageBaseline = 20f;
		dontBlowStrings = new string[] {
			"Arm1",
			"Arm2",
			"WeaponDetector1",
			"WeaponDetector2",
			"TractorBeam1",
			"TractorBeam2",
			"Shield1",
			"Shield2"
		};
	}
	
	//explode some time after this block is fired

	public IEnumerator BlowMe(){
		yield return new WaitForSeconds (time2Blow);
		
		allObjects = Physics2D.OverlapCircleAll (transform.position,exRadius);
		allGameObjects = new GameObject[allObjects.Length];
		
		i = 0;
		foreach (Collider2D coller in allObjects){
			allGameObjects[i] = coller.gameObject;
			i++;
		}
		
		i = 0;
		foreach (Collider2D thing in allObjects){
			if (!dontBlowStrings.Contains(thing.name) && thing.gameObject!=gameObject){

				bPos = transform.position;
				thingPos = thing.transform.position;
				exDir = Vector3.Normalize(thingPos - bPos);
				
				dist2exploder = Vector3.Distance(bPos,thingPos);
				
				/*rayThings = Physics2D.RaycastAll (bPos,exDir,dist2exploder);
				foreach (RaycastHit2D rayThing in rayThings){
					if (rayThing.collider.gameObject == thing.gameObject){
						hitPointThing = rayThing.point;
						break;
					}
				}
				rayExps = Physics2D.RaycastAll (thingPos,-exDir,dist2exploder);
				foreach (RaycastHit2D rayExp in rayExps){
					if (rayExp.collider.gameObject == this.gameObject){
						hitPointExp = rayExp.point;
						break;
					}
				}
				
				dist2exploder = Vector3.Distance(hitPointExp,hitPointThing);
				*/
				if (dist2exploder<0){
					dist2exploder=0;
				}
				
				exMultiplier = Mathf.Exp(-.6f * dist2exploder);
				
				exForce = exForceBaseline * exMultiplier;
				damage = Mathf.RoundToInt ( damageBaseline * exMultiplier );
				
				if (thing.GetComponent<GetDamage>()){
					j = 0;
					makeItHurt = true;
					while ( j < i ){
						if (allGameObjects[i] == allGameObjects[j]){
							makeItHurt = false;
						}
						j++;
					}
					
					if (makeItHurt){
						getDamage = thing.GetComponent<GetDamage>();
						StartCoroutine(getDamage.SendDamage(damage,weaponBlockScript.blockType));
						if (thing.rigidbody2D){
							thing.rigidbody2D.AddForce(exDir * exForce);
						}
					}
				}
				else{
					if (thing.rigidbody2D){
						thing.rigidbody2D.AddForce(exDir * exForce);
					}
				}
			}
			i++;
		}
		GameObject expo = Instantiate ( Resources.Load("Prefabs/Effects/Explosion"),this.transform.position,this.transform.rotation) as GameObject;
		expo.transform.localScale = Vector3.one * 25;
		Destroy (this.gameObject);
	}

}