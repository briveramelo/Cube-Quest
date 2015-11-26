using UnityEngine;
using System.Collections;

public class CameraBattleArena : MonoBehaviour {

	public AudioSource musicJams;

	private GameObject char1;
	public GameObject char2;
	private Transform charTran1;
	private Transform charTran2;
	private Transform tranny;

	private Vector4 arenaBounds;

	public Vector3 viewTarget;
	public Vector3 middleGround;
	public Vector3 shift;
	public Vector3 startSpot;

	private float sizeMult;
	private float moveCameraSpeed;
	public float sizeTarget;
	private float defaultSize;
	private float maxSize;
	private float startingZ;
	public float halfHeight;
	public float aspect;
	
	public bool leftCheck;
	public bool rightCheck;
	public bool upCheck;
	public bool downCheck;
	private bool restart;
	public bool shake;
	
	// Use this for initialization
	void Awake () {
		shake = false;

		sizeMult = .15f;
		
		moveCameraSpeed = 2f;

		defaultSize = 2f;
		maxSize = 3.5f;

		if (GameObject.Find ("Hero1")) {
			char1 = GameObject.Find ("Hero1");
			charTran1 = char1.transform;
		}
		if (GameObject.Find ("Hero2")){
			char2 = GameObject.Find ("Hero2");
			charTran2 = char2.GetComponent<Transform> ();
		}

		//will be the other character eventually
		tranny = GetComponent<Transform> ();
		startingZ = tranny.position.z;

		if (Application.loadedLevelName == "BattleArena") {
			//arenaBounds = new Vector4(-6.2f,6.2f,-4f,4f);//-x,+x,-y,+y
		}
		else{
			//arenaBounds = new Vector4(-6.2f,6.2f,-4f,4f);//-x,+x,-y,+y
		}
		arenaBounds = new Vector4(-6.2f,6.2f,-4f,4f);//-x,+x,-y,+y

		if (char1 && char2){
			if (charTran1.position.x > charTran2.position.x) {
				middleGround = (charTran2.position+charTran1.position)/2f;
			}
			else{
				middleGround = (charTran1.position+charTran2.position)/2f;
			}
			if(charTran1.position.y > charTran2.position.y){
				middleGround = new Vector3( middleGround.x,(charTran1.position.y+charTran2.position.y)/2f,startingZ);
			}
			else{
				middleGround = new Vector3( middleGround.x,(charTran2.position.y+charTran1.position.y)/2f,startingZ);
			}
			sizeTarget = defaultSize+sizeMult*(Vector3.Distance(charTran1.position,charTran2.position));
			if (sizeTarget>maxSize) {
				sizeTarget = maxSize;
			}
			camera.orthographicSize = sizeTarget;
			aspect = camera.aspect;
			halfHeight = camera.orthographicSize;
			viewTarget = middleGround;

			if (viewTarget.x < (arenaBounds.x + halfHeight * aspect) ){//checking and correcting leftside camerabounds
				viewTarget = new Vector3 (arenaBounds.x + halfHeight * aspect, viewTarget.y, viewTarget.z);
			}
			else if (viewTarget.x > (arenaBounds.y - halfHeight * aspect) ){//checking and correcting rightside camerabounds
				viewTarget = new Vector3 (arenaBounds.y - halfHeight * aspect, viewTarget.y, viewTarget.z);
			}
			
			if (viewTarget.y < (arenaBounds.z+halfHeight)){//checking and correcting bottomside camerabounds
				viewTarget = new Vector3 (viewTarget.x, arenaBounds.z + halfHeight, viewTarget.z);
			}
			else if (viewTarget.y > (arenaBounds.w-halfHeight)){//checking and correcting topside camerabounds
				viewTarget = new Vector3 (viewTarget.x, arenaBounds.w - halfHeight, viewTarget.z);
			}

			tranny.position = viewTarget;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (char1 && char2){
			if (charTran1.position.x > charTran2.position.x) {
				middleGround = (charTran2.position+charTran1.position)/2f;
			}
			else{
				middleGround = (charTran1.position+charTran2.position)/2f;
			}
			if(charTran1.position.y > charTran2.position.y){
				middleGround = new Vector3( middleGround.x,(charTran1.position.y+charTran2.position.y)/2f,startingZ);
			}
			else{
				middleGround = new Vector3( middleGround.x,(charTran2.position.y+charTran1.position.y)/2f,startingZ);
			}

			sizeTarget = defaultSize+sizeMult*(Vector3.Distance(charTran1.position,charTran2.position));
			if (sizeTarget>maxSize) {
				sizeTarget = maxSize;
			}
			camera.orthographicSize = Mathf.Lerp (camera.orthographicSize, sizeTarget, Time.deltaTime);
			aspect = camera.aspect;
			halfHeight = camera.orthographicSize;

			viewTarget = middleGround;

			if (viewTarget.x < (arenaBounds.x + halfHeight * aspect) ){//checking and correcting leftside camerabounds
				viewTarget = new Vector3 (arenaBounds.x + halfHeight * aspect, viewTarget.y, viewTarget.z);
			}
			else if (viewTarget.x > (arenaBounds.y - halfHeight * aspect) ){//checking and correcting rightside camerabounds
				viewTarget = new Vector3 (arenaBounds.y - halfHeight * aspect, viewTarget.y, viewTarget.z);
			}

			if (viewTarget.y < (arenaBounds.z+halfHeight)){//checking and correcting bottomside camerabounds
				viewTarget = new Vector3 (viewTarget.x, arenaBounds.z + halfHeight, viewTarget.z);
			}
			else if (viewTarget.y > (arenaBounds.w-halfHeight)){//checking and correcting topside camerabounds
				viewTarget = new Vector3 (viewTarget.x, arenaBounds.w - halfHeight, viewTarget.z);
			}

			if (!shake){	
				tranny.position = Vector3.Lerp (tranny.position, viewTarget, Time.deltaTime*moveCameraSpeed);
			}
			else{
				ShakeIt ();
			}
		}
	}
	
	void ShakeIt(){
		shift = new Vector3( Random.insideUnitCircle.x, Random.insideUnitCircle.y,0f) * .05f;
		this.transform.position = startSpot + shift;
	}
	
	public IEnumerator Shake(){
		shake = true;
		startSpot = this.transform.position;
		yield return new WaitForSeconds (.1f);
		shake = false;
	}
}
