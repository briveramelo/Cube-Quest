using UnityEngine;
using System.Collections;

public class Circulate : MonoBehaviour {

	private Vector3 startPosition;
	private Vector3 centerPos;
	public float rotationSpeed;

	void Awake(){
		startPosition = transform.position;
		centerPos = -transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		transform.RotateAround (startPosition + centerPos, Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
