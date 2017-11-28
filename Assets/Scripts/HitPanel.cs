using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPanel: MonoBehaviour {

    public static HitPanel instance;

	private Image fadePanel;
	private float delay = 0.5f;

	void Start () {
        instance = this;
		fadePanel = GetComponent<Image> ();
		fadePanel.enabled = false;
	}

	void OnEnable () {
		//EventsManager.OnGameOverEvent += FlashScreen;
	}

	void OnDisable () {
		//EventsManager.OnGameOverEvent -= FlashScreen;
	}


	public void MissFlash () {
		StartCoroutine (FlashScreenCR ());
	}

	IEnumerator FlashScreenCR () {
        fadePanel.enabled = true;
        fadePanel.canvasRenderer.SetAlpha (0.8f);
		fadePanel.CrossFadeAlpha (0.1f, delay, false);
		yield return new WaitForSeconds (delay);
		fadePanel.enabled = false;
	}
}
