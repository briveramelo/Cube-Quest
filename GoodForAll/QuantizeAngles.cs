using UnityEngine;
using System.Collections;

public class QuantizeAngles : MonoBehaviour {
	
	public float[] quantizedAngles;
	public float quantizedAngle;

	void Start(){
		quantizedAngles = new float[]{
			180f,
			135f,
			90f,
			45f,
			0f,
			-45f,
			-90f,
			-135f,
			-180f
		};
	}

	//assumes pointing right is the default at 0, up is 90 and so on

	public float HalfTrackJacket(float rawAngle){
		if ((rawAngle>67.5f && rawAngle<112.5f) || (rawAngle<-247.5f && rawAngle>-292.5f)){
			quantizedAngle = quantizedAngles[2];
		}
		else if ((rawAngle>22.5f && rawAngle<67.5f) || (rawAngle>112.5f && rawAngle<157.5f) || (rawAngle>-337.5f && rawAngle<-292.5f) || (rawAngle>-247.5f && rawAngle<-202.5f)){
			quantizedAngle = quantizedAngles[3];
		}
		else if ((rawAngle>-22.5f && rawAngle<22.5f) || (rawAngle>157.5f && rawAngle<202.5f) || (rawAngle>-202.5f && rawAngle<-157.5f) || (rawAngle>337.5f && rawAngle<382.5f)){
			quantizedAngle = quantizedAngles[4];
		}
		else if ((rawAngle<-22.5f && rawAngle>-67.5f) || (rawAngle>-157.5f && rawAngle<-112.5f) || (rawAngle>292.5f && rawAngle<337.5f) || (rawAngle>202.5f && rawAngle<247.5f)) {
			quantizedAngle = quantizedAngles[5];
		}
		else if ((rawAngle<-67.5f && rawAngle>-112.5f) || (rawAngle>247.5f && rawAngle<292.5f)){
			quantizedAngle = quantizedAngles[6];
		}
		return quantizedAngle;
	}

	public float FullTrackJacket(float rawAngle){

		//left 180
		if ((rawAngle>157.5f && rawAngle<202.5f) || (rawAngle>-202.5f && rawAngle<-157.5f)){
			quantizedAngle = quantizedAngles[0];
		}

		//up left 135
		else if ((rawAngle>112.5f && rawAngle<157.5f) || (rawAngle>-247.5f && rawAngle<-202.5f)){
			quantizedAngle = quantizedAngles[1];
		}

		//above 90
		if ((rawAngle>67.5f && rawAngle<112.5f) || (rawAngle<-247.5f && rawAngle>-292.5f)){
			quantizedAngle = quantizedAngles[2];
		}

		//up right 45
		else if ((rawAngle>22.5f && rawAngle<67.5f) ||  (rawAngle>-337.5f && rawAngle<-292.5f)){
			quantizedAngle = quantizedAngles[3];
		}

		//right 0
		else if ((rawAngle>-22.5f && rawAngle<22.5f) || (rawAngle>337.5f && rawAngle<382.5f)){
			quantizedAngle = quantizedAngles[4];
		}

		//down right -45
		else if ((rawAngle<-22.5f && rawAngle>-67.5f) || (rawAngle>292.5f && rawAngle<337.5f)) {
			quantizedAngle = quantizedAngles[5];
		}

		//down -90
		else if ((rawAngle<-67.5f && rawAngle>-112.5f) || (rawAngle>247.5f && rawAngle<292.5f)){
			quantizedAngle = quantizedAngles[6];
		}

		//down left -135
		else if ((rawAngle>-157.5f && rawAngle<-112.5f) || (rawAngle>202.5f && rawAngle<247.5f)){
			quantizedAngle = quantizedAngles[7];
		}


		return quantizedAngle;
	}
}
