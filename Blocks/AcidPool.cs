using UnityEngine;
using System.Collections;

public class AcidPool : MonoBehaviour {
	
	private bool freshStart;
	public bool sticky;
	public bool bubbling;
	public Vector2[] newVertex;

	private Vector2 lengths;
	public PolygonCollider2D polyCol;
	public int i;
	public int[] k;
	public int numPoints;
	public float sloshSpeed;
	public float[] magnitudes;

	void Awake () {
		freshStart = true;
		polyCol = GetComponent<PolygonCollider2D> ();
		i = 0;
		sloshSpeed = 1f;
		lengths = new Vector2 (.1f, .2f);
		bubbling = false;
		numPoints = polyCol.points.Length;
		newVertex = new Vector2[numPoints];
		magnitudes = new float[numPoints];
		k = new int[numPoints];

		foreach (Vector2 vertex in polyCol.points){
			newVertex[i] = vertex;
			magnitudes[i] = vertex.magnitude;
			i++;
		}
		
		polyCol.pathCount = numPoints;
		StartCoroutine (Bubbly ());
	}

	public IEnumerator Bubbly(){
		bubbling = true;
		while (freshStart) {
			i=0;
			foreach (Vector2 vertex in polyCol.points){
				magnitudes[i] = vertex.magnitude;
				k[i] = 1;
				if (magnitudes[i]<lengths.x){
					k[i]=1; //grow
				}
				else if (magnitudes[i]>lengths.y){
					k[i]=-1; //shrink
				}

				if (k[i]==1){
					newVertex[i] = vertex*1.1f;
				}
				else{
					newVertex[i] = vertex*0.9f;
				}
				polyCol.SetPath(i,newVertex);
				//polyCol.points[i] = newVertex[i];
				i++;
			}
			yield return null;
		}
		bubbling = false;
		yield return null;
	}

	public IEnumerator GetSticky(){
		rigidbody2D.velocity = Vector2.zero;
		sticky = true;
		yield return null;
	}

	void OnTriggerStay2D(Collider2D col){
		if (!sticky && !col.isTrigger) {
			StartCoroutine(GetSticky());
		}
	}
}