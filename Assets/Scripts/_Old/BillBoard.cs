using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour {

		private GameObject playerCam;

		void Start()
		{
			playerCam = Camera.main.gameObject;
		}

		private void Update()
		{
		this.transform.LookAt(transform.position - playerCam.transform.position);
		//or this.transform.LookAt(playerCam.transform.position);
		}

}
