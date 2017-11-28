using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour {

	private Image fadePanel;
	private bool fadeOutTriggered;
	private float delay = 2f;

	void Start () {
		fadePanel = GetComponent<Image> ();
		fadePanel.enabled = false;
#if UNITY_EDITOR

#else
        StartCoroutine (CrossFadeInCR ());
#endif
    }

    void OnEnable () {
		//EventsManager.OnPlayModeEnterEvent += CrossFadeIn;
		//EventsManager.OnGameResetEvent += CrossFadeIn;
	}

	void OnDisable () {
        //EventsManager.OnPlayModeEnterEvent -= CrossFadeIn;
        //EventsManager.OnGameResetEvent += CrossFadeIn;
    }

    void CrossFadeIn () {
		StartCoroutine (CrossFadeInCR ());
	}

	//void GameOverSequence () {
	//	StartCoroutine (GameOverCR ());
	//}

	IEnumerator CrossFadeInCR () {
		fadePanel.enabled = true;
		fadePanel.canvasRenderer.SetAlpha (1f);
		fadePanel.CrossFadeAlpha (0f, delay, false);
		yield return new WaitForSeconds (delay);
		fadePanel.enabled = false;
	}    

    //IEnumerator GameOverCR () {
    //	yield return new WaitForSeconds (2f);
    //	fadePanel.canvasRenderer.SetAlpha (0.1f);
    //	fadePanel.enabled = true;
    //	fadePanel.CrossFadeAlpha (1f, delay, false);
    //	yield return new WaitForSeconds (delay);
    //	fadePanel.enabled = false;
    //	EventsManager.GameReset ();
    //}


}
