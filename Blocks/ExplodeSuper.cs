using UnityEngine;
using System.Collections;
using System.Linq;

public class ExplodeSuper : MonoBehaviour {

	public AudioSource bigBoom;
	public AudioSource pull;

	public WeaponBlockScript weaponBlockScript;
	private Animator animator;

	public GetDamage getDamage;
	public Collider2D[] allObjects;
	public GameObject[] allGameObjects;

	private string[] dontPullStrings;
	private string[] dontBlowStrings;
	private string[] weaponStrings;

	private Vector2 hitPointThing;
	private Vector2 hitPointExp;
	public Vector2 pullDir;
	public Vector2 exDir;

	public float dist2exploder;
	public float exMultiplier;
	public float exForce;
	public float exForceBaseline;
	public float damageBaseline;
	public float pullForce;
	private float exRadius;
	public float pullSecs;
	public float triggerTimer;
	public float pulRadius;

	public int damage;
	public int i;
	public int j;

	public bool getCozy;
	public bool makeItHurt;
	public bool enter;

	void Awake() {

		weaponBlockScript = GetComponent<WeaponBlockScript> ();
		pulRadius = 7f;
		exRadius = 7f;
		pullSecs = 1.5f;
		pullForce = 600f;
		exForceBaseline = 1500f;
		damageBaseline = 20f;
		getCozy = false;

		dontPullStrings = new string[] {"Floor", "Wall","Arm1","WeaponDetector1","Arm2","WeaponDetector2", "TractorBeam1","TractorBeam2","Shield1","Shield2"}; //gameobject names
		dontBlowStrings = new string[] {"Floor", "Wall","Arm1","WeaponDetector1","Arm2","WeaponDetector2","TractorBeam1","TractorBeam2","Shield1","Shield2"}; //gameobject names
		weaponStrings = new string[] {"SuperNeutral","SuperExploder","Neutral","Exploder","Frozesploder","MiniCopter","Telekinetic"}; //gameobject tags
	}
	
	//explode some time after this block is fired

	public IEnumerator GetIntimate(){
		getCozy = true;
		StartCoroutine (PullMeIn ());
		yield return new WaitForSeconds (pullSecs);
		getCozy = false;
	}
	
	public IEnumerator PullMeIn() {
		while (getCozy){
			allObjects = Physics2D.OverlapCircleAll (transform.position,pulRadius);
			foreach (Collider2D thing in allObjects){
				enter = true;
				//don't pull if you shouldn't pull (pull if you should)
				if (!dontPullStrings.Contains(thing.name) && thing.gameObject!=gameObject && thing.rigidbody2D){
					if ((thing.gameObject.name == "Hero1" && weaponBlockScript.wepDSID==1) || (thing.gameObject.name == "Hero2" && weaponBlockScript.wepDSID==2)){
						enter = false;
					}
					else if (thing.gameObject.GetComponent<SuperTelekineticBlock>()){
						if (thing.gameObject.GetComponent<SuperTelekineticBlock>().teleHovering){
							enter = false;
						}
					}
					if (enter){
						pullDir = Vector3.Normalize(transform.position - thing.transform.position);

						//make sure locked in blocks aren't effected (for some reason they are)
						if (weaponStrings.Contains (thing.tag)){
							
							if (thing.GetComponent<WeaponBlockScript>()){
								if (thing.GetComponent<WeaponBlockScript>().toggleCount<=0){
									thing.rigidbody2D.AddForce(pullDir * pullForce * Time.deltaTime);
									
									if (Mathf.Sign (thing.rigidbody2D.velocity.x) != Mathf.Sign (pullDir.x)){
										thing.rigidbody2D.velocity = new Vector2(0f,thing.rigidbody2D.velocity.y);
									}
									if (Mathf.Sign (thing.rigidbody2D.velocity.y) != Mathf.Sign (pullDir.y)){
										thing.rigidbody2D.velocity = new Vector2(thing.rigidbody2D.velocity.x,0f);
									} 
								}
							}
							
							else if (thing.GetComponent<MiniCopterBlock>()){
								if (thing.GetComponent<MiniCopterBlock>().toggleCount<=0){
									thing.rigidbody2D.AddForce(pullDir * pullForce * Time.deltaTime);
									
									if (Mathf.Sign (thing.rigidbody2D.velocity.x) != Mathf.Sign (pullDir.x)){
										thing.rigidbody2D.velocity = new Vector2(0f,thing.rigidbody2D.velocity.y);
									}
									if (Mathf.Sign (thing.rigidbody2D.velocity.y) != Mathf.Sign (pullDir.y)){
										thing.rigidbody2D.velocity = new Vector2(thing.rigidbody2D.velocity.x,0f);
									} 
								}
							}
						}
						
						else{
							thing.rigidbody2D.AddForce(pullDir * pullForce * Time.deltaTime);
							
							if (Mathf.Sign (thing.rigidbody2D.velocity.x) != Mathf.Sign (pullDir.x)){
								thing.rigidbody2D.velocity = new Vector2(0f,thing.rigidbody2D.velocity.y);
							}
							if (Mathf.Sign (thing.rigidbody2D.velocity.y) != Mathf.Sign (pullDir.y)){
								thing.rigidbody2D.velocity = new Vector2(thing.rigidbody2D.velocity.x,0f);
							} 
						}
					}
				}
			}
			yield return new WaitForEndOfFrame();
		}
		StartCoroutine (BlowYourLoad ());
	}

	public IEnumerator BlowYourLoad(){
		yield return new WaitForEndOfFrame ();
		
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
				Vector3 bPos = transform.position;
				Vector3 thingPos = thing.transform.position;
				Vector3 exDir = Vector3.Normalize(thingPos - bPos);
				
				float dist2exploder = Vector3.Distance(bPos,thingPos);
				
				/*RaycastHit2D[] rayThings = Physics2D.RaycastAll (bPos,exDir,dist2exploder);
				foreach (RaycastHit2D rayThing in rayThings){
					if (rayThing.collider.gameObject == thing.gameObject){
						hitPointThing = rayThing.point;
						break;
					}
				}
				RaycastHit2D[] rayExps = Physics2D.RaycastAll (thingPos,-exDir,dist2exploder);
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
						StartCoroutine(getDamage.SendDamage(damage, weaponBlockScript.blockType));
						if (thing.rigidbody2D){
							thing.rigidbody2D.AddForce(exDir * exForce);
						}
					}
				}
				//only addforce to things that are not locked in blocks/copters

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