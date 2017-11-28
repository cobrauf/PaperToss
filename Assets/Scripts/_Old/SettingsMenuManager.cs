using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.HelloAR;

public class SettingsMenuManager: MonoBehaviour {

	public static SettingsMenuManager instance;

	public HelloARController helloARController;
	public GameObject startText;


    //........testing variables
    public Text powerText, xValueText, lightText;


	void Awake () {
        instance = this;
    }


	public void ResetScene () {
		EventsManager.SceneReset ();
		StartCoroutine (ResetSceneCR ());
        SoundManager.instance.PlayButtonSFX();
	}

	IEnumerator ResetSceneCR () {		
		yield return new WaitForSeconds (0.1f);
		//helloARController.sceneParent.SetActive (false);
		//GameManager.instance.scenePlaced = false;
	}

	public void Pause () {
		if (Time.timeScale > 0f) {
			Time.timeScale = 0f;
		} else {
			Time.timeScale = 1f;
		}
	}

	#region Testing Settings
	public void PowerUp()
	{
		SwipeControl.powerFactor += 0.5f;
	}

	public void PowerDown()
	{
		SwipeControl.powerFactor -= 0.5f;
	}

	public void xValueUp()
	{
		SwipeControl.xValueFactor += 10f;
	}

	public void xValueDown()
	{
		SwipeControl.xValueFactor -= 10f;
	}

	public void WindOnOff()
	{
		SwipeControl.instance.hasWind = !SwipeControl.instance.hasWind;
	}

	public void GameToggle()
	{
		if (GameManager.instance.gameStarted)
		{
			EventsManager.GameOver();
			Debug.Log("game Over trigger");
		}
		else
		{
			EventsManager.GameStart();
			Debug.Log("game start trigger");
		}
	}

	#endregion

	void Update () {		

		powerText.text = "" + SwipeControl.powerFactor;
		xValueText.text = "" + SwipeControl.xValueFactor;
        lightText.text = "" + Shader.GetGlobalFloat("_GlobalLightEstimation");

    }
	
}
