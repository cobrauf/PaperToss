using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

	public static ObjectPool instance;

	public GameObject pipeTopPrefab;
	private List<GameObject> pipeTopList = new List<GameObject> ();
	private int pipeTopTotal = 10;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	void Start () {
		CreatePipes (pipeTopPrefab, pipeTopTotal, pipeTopList);
	}

	void CreatePipes(GameObject prefab, int total, List<GameObject> GOList) {
		Vector3 prefabLocation = prefab.transform.position;
		Vector3 prefabScale = prefab.transform.localScale;
		Quaternion prefabRotation = prefab.transform.rotation;
		for (int i = 0; i < total; i++) {
			GameObject GO = Instantiate (prefab, prefabLocation, prefabRotation) as GameObject;
			GO.transform.SetParent (transform);
			GO.transform.localScale = prefabScale;
			GOList.Add (GO);
			GO.SetActive (false);//don't deactivate
		}
	}

	public GameObject GetStoredObject (string name) {

		switch (name) {
		case "Pipe Top":
			for (int i = 0; i < pipeTopList.Count; i++) {
				if (!pipeTopList [i].activeInHierarchy) {
					return pipeTopList [i];
				}
			}
			break;
		}
		return null;
	}

}
