using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour {

	public delegate void SceneEventsHandler();

	public static event SceneEventsHandler OnMainMenuEvent;
	public static event SceneEventsHandler OnPlayModeEnterEvent;
	public static event SceneEventsHandler OnPlayModeExitEvent;//isn't this the same as on MainMenuevent?.....unresolved

	public static event SceneEventsHandler OnScenePlacedEvent;
	public static event SceneEventsHandler OnSceneResetEvent;

	public static event SceneEventsHandler OnGameStartEvent;
	public static event SceneEventsHandler OnGameOverEvent;
	public static event SceneEventsHandler OnGameResetEvent;

    public static event SceneEventsHandler OnFanOnEvent;
    public static event SceneEventsHandler OnFireEvent;
    public static event SceneEventsHandler OnFireOffEvent;


    public static void MainMenu()
	{
		if (OnMainMenuEvent != null)
		{
			OnMainMenuEvent();
		}
	}

	public static void PlayModeEnter()
	{
		if (OnPlayModeEnterEvent != null)
		{
			OnPlayModeEnterEvent();
		}
	}

	public static void PlayModeExit()
	{
		if (OnPlayModeExitEvent != null)
		{
			OnPlayModeExitEvent();
		}
	}

	public static void ScenePlaced () {
		if (OnScenePlacedEvent != null) {
			OnScenePlacedEvent();
		}
	}

	public static void GameStart () {
		if (OnGameStartEvent != null) {
			OnGameStartEvent();
		}
	}

	public static void GameOver () {
		if (OnGameOverEvent != null) {
			OnGameOverEvent();
		}
	}

	public static void GameReset () {
		if (OnGameResetEvent != null) {
			OnGameResetEvent();
            //Debug.Log("game reset called");
		}
	}

	public static void SceneReset () {
		if (OnSceneResetEvent != null) {
			OnSceneResetEvent();
		}
	}

    public static void FanOn()
    {
        if (OnFanOnEvent != null)
        {
            OnFanOnEvent();
        }
    }
    public static void OnFire()
    {
        if (OnFireEvent != null)
        {
            OnFireEvent();
        }
    }

    public static void FireOff()
    {
        if (OnFireOffEvent != null)
        {
            OnFireOffEvent();
        }
    }
}
