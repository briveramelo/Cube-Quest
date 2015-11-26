using UnityEngine;
using System.Collections;

public class MoveToHere : MonoBehaviour {

	public Transform[] transforms;
	private Vector3[] spots;
	public bool moving;
	private int i;
	private int max;
	private float distAway;
	private float distThresh;
	public float moveSpeed;
	public bool cycle;
	private bool reversing;
	public float slowDistThresh;
	public float pauseTime;
	public float preSpinPauseTime;
	public float rotAngle;
	public bool spinner;
	private float newAngle;
	private float angAway;
	private float angThresh;
	public float spinSpeed;
	public bool startMoving;
	private Quaternion targetAngle;
	private bool spinning;

	// Use this for initialization
	void Awake () {
		transforms = GetComponentsInChildren<Transform> ();
		i = 0;
		spots = new Vector3[transforms.Length];
		foreach (Transform tran in transforms) {
			spots[i] = tran.position;
			i++;
		}
		max = transforms.Length-1;
		moving = false;
		reversing = false;
		i = 0;
		distThresh = .05f;
		angThresh = .01f;
		newAngle = transform.rotation.eulerAngles.z;
		if (startMoving){
			StartCoroutine (StartItUp());
		}
	}

	public IEnumerator StartItUp(){
		yield return new WaitForSeconds (1f);
		StartCoroutine (MoveAround ());
	}

	public IEnumerator SpinCycle(){
		spinning = true;
		rigidbody2D.fixedAngle = false;
		newAngle += rotAngle;
		angAway = rotAngle;
		targetAngle = Quaternion.Euler (0f, 0f, newAngle);
		while (angAway>angThresh){
			angAway = Quaternion.Angle (transform.rotation,targetAngle);
			transform.rotation = Quaternion.Lerp (transform.rotation,targetAngle,spinSpeed * Time.deltaTime);
			yield return null;
		}
		transform.rotation = targetAngle;
		spinning = false;
		rigidbody2D.fixedAngle = true;
		yield return null;
	}

	public IEnumerator MoveAround(){
		if (!moving){
			moving = true;
			while (moving) {
				distAway = Vector2.Distance(transform.position,spots[i]);
				if (distAway<slowDistThresh){
					rigidbody2D.velocity = Vector3.Normalize(-transform.position+spots[i]) * moveSpeed/10 * distAway/slowDistThresh;
				}
				else{
					rigidbody2D.velocity = Vector3.Normalize(-transform.position+spots[i]) * moveSpeed/10;
				}
				if (distAway<=distThresh){
					transform.position = spots[i];
					rigidbody2D.velocity = Vector2.zero;
					if (spinner){
						yield return new WaitForSeconds (preSpinPauseTime);
						StartCoroutine (SpinCycle());
						while (spinning){
							yield return null;
						}
					}
					yield return new WaitForSeconds(pauseTime);
					if (cycle){
						i++;
						if (i>max){
							i=0;
						}
					}
					else{
						if (!reversing){
							i++;
							if (i>max){
								i=max-1;
								reversing = true;
							}
						}
						else{
							i--;
							if (i<0){
								i=1;
								reversing = false;
							}
						}
					}
				}
				yield return null;
			}
		}
		yield return null;
	}

}
