using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float smooth = 1.5f;

	private Transform player;
	private Vector3 relCameraPos; 	//this is the camera position relative to the player

	private float relCameraPosMag;	//this store the potention camera position, when the player is obstacled
	private Vector3 newPos; 	//use to lerp between this new position and old position.

	void Awake(){
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;

		relCameraPos = transform.position - player.position;
		relCameraPosMag = relCameraPos.magnitude - 0.5f;	//magnitude is the |V|

	}

	void FixedUpdate(){		//although is not a rigid body , but it follows a rigidbody object.
		Vector3 standardPos = player.position + relCameraPos;
		Vector3 abovePos = player.position + Vector3.up * relCameraPosMag;
		Vector3[] checkPoints = new Vector3[5];		//the potential pos of camera 

		checkPoints [0] = standardPos;
		checkPoints [1] = Vector3.Lerp (standardPos, abovePos, 0.25f);
		checkPoints [2] = Vector3.Lerp (standardPos, abovePos, 0.5f);
		checkPoints [3] = Vector3.Lerp (standardPos, abovePos, 0.75f);
		checkPoints [4] = abovePos;

		//loop through them to see which one can work.
		for (int i = 0; i < checkPoints.Length; i++) {
			if(ViewingPosCheck(checkPoints[i]))
			{
				break;
			}
		}
		//move the camera to the correct position
		transform.position = Vector3.Lerp (transform.position, newPos, smooth * Time.deltaTime);
		//make the camera look at the correct direction. smoothly catch up with the player.
	}

	//to find if the camera can see the player
	bool ViewingPosCheck(Vector3 checkPos){
		RaycastHit hit;

		if (Physics.Raycast (checkPos, player.position - checkPos, out hit, relCameraPosMag)) {
			if(hit.transform != player){	//if the raycast hit something not the player, return false
				return false;
			}		
		}
		newPos = checkPos;
		return true;
	}
}
