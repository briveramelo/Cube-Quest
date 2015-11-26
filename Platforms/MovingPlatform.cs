using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]

public class MovingPlatform : MonoBehaviour {

	public float xMaxDist;
	public float yMaxDist;
	public float xSpeedmax;
	public float ySpeedmax;
	public float xOffset;
	public float yOffset;
	
	private float xSpeed;
	private float ySpeed;
	private Vector2 vel2;


	// Use this for initialization
	void Awake () {
		rigidbody2D.fixedAngle = true;
		rigidbody2D.mass = 1000f;
		rigidbody2D.gravityScale = 0f;
	}
	
	void Update(){	
		if (xMaxDist!=0f){
			xSpeed = Mathf.Cos(Time.realtimeSinceStartup * xSpeedmax / xMaxDist + xOffset*Mathf.PI) * xSpeedmax;
		}
		else{
			xSpeed = 0f;
		}
		if (yMaxDist!=0f){
			ySpeed = Mathf.Cos(Time.realtimeSinceStartup * ySpeedmax / yMaxDist + yOffset*Mathf.PI) * ySpeedmax;
		}
		else{
			ySpeed = 0f;
		}
		vel2 = new Vector2 (xSpeed,ySpeed);
		if (rigidbody2D){
			rigidbody2D.velocity = vel2;
		}
	}

}
