using UnityEngine;
using System.Collections;

public class AcidBlock : MonoBehaviour {

	private string acidPoolString;
	private int i;
	private int iMax;
	private GameObject[] acidPool;
	private float[] spewAngles;
	private float splooshSpeed;

	void Awake () {

		i = 0;
		iMax = 3;
		spewAngles = new float[]{10f,0f,-10f};
		acidPool = new GameObject[iMax];
		acidPoolString = "Prefabs/WeaponBlocks/AcidPool";
		splooshSpeed = 150f;

	}

	void OnCollisionEnter2D(Collision2D col){
		if (!col.collider.isTrigger){
			while (i<iMax){
				acidPool[i] = Instantiate ( Resources.Load (acidPoolString), gameObject.transform.position , Quaternion.identity ) as GameObject;
				acidPool[i].rigidbody2D.AddForce(splooshSpeed * Vector3.Normalize(new Vector3( -Mathf.Sin(spewAngles[i] * Mathf.Deg2Rad) , Mathf.Cos(spewAngles[i] * Mathf.Deg2Rad) , 0f)));
				i++;
			}
			
			Destroy (this.gameObject);;
		}
	}
}
