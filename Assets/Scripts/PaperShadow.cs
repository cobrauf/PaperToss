using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperShadow : MonoBehaviour {

	public GameObject paperBall;
	public PaperBall _paperBall;
	private Vector3 offSet = new Vector3 (0f, 0.5f, 0f);   

	void LateUpdate()
	{
		if (paperBall != null) {
			transform.LookAt (paperBall.transform.position);
			transform.position = paperBall.transform.position + offSet;
		} else {
			Destroy (gameObject);
		}

		if (_paperBall.hasScored) {
			Destroy (gameObject);
		}        
	
	}

}
