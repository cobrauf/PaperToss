using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.HelloAR;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

    #region Variables
    //object references
    public Light sceneLight;
    public bool scenePlaced, gameStarted, inPlayMode;
    public enum Difficulty { Easy, Hard };
    public static Difficulty gameMode = Difficulty.Easy;
    public static int score, endDisplayScore, currentMissCount, chainCount;
    public static bool isOnFire;

    //fan speed levels
    public static int fanLevel1Threshold = 3;
    public static int fanLevel2Threshold = 6;
    public static int fanLevel3Threshold = 9;

    private int lightUpdateCount;
    private const int NUM_MISSES_ALLOWED = 5;
    private bool fanTurnedOn;
    #endregion


    void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}	

	#region Events
	void OnEnable () {		
		EventsManager.OnScenePlacedEvent += ScenePlacedActions;
		EventsManager.OnPlayModeEnterEvent += PlayModeEnterActions;
		EventsManager.OnPlayModeExitEvent += PlayModeExitActions;
		EventsManager.OnGameStartEvent += GameStartActions;
		EventsManager.OnGameOverEvent += GameOverActions;
		EventsManager.OnGameResetEvent += GameResetActions;
		EventsManager.OnSceneResetEvent += SceneResetActions;
	}

	void OnDisable () {
		EventsManager.OnScenePlacedEvent -= ScenePlacedActions;
		EventsManager.OnPlayModeEnterEvent -= PlayModeEnterActions;
		EventsManager.OnPlayModeExitEvent -= PlayModeExitActions;
		EventsManager.OnGameStartEvent -= GameStartActions;
		EventsManager.OnGameOverEvent -= GameOverActions;
		EventsManager.OnGameResetEvent -= GameResetActions;
		EventsManager.OnSceneResetEvent -= SceneResetActions;
	}

	void ScenePlacedActions() {
		scenePlaced = true;
        currentMissCount = NUM_MISSES_ALLOWED;
        score = 0;
        chainCount = 0;
    }

    void PlayModeEnterActions()
    {
        inPlayMode = true;
    }

    void PlayModeExitActions()
    {
        inPlayMode = false;
    }

    void GameStartActions()
	{
		gameStarted = true;
        score = 0;
        chainCount = 0;
        endDisplayScore = 0;
        fanTurnedOn = false;
    }

    void GameOverActions () {
		gameStarted = false;
        fanTurnedOn = false;
        chainCount = 0;
    }

    void GameResetActions ()
    {
        currentMissCount = NUM_MISSES_ALLOWED;
        fanTurnedOn = false;
    }   

	void SceneResetActions () {
		gameStarted = false;
		scenePlaced = false;
        isOnFire = false;
        chainCount = 0;
	}
	#endregion

	void Update () {

        //adjust light source intensity according to global light estimation       
        if (lightUpdateCount++ > 60f)
        {
            lightUpdateCount = 0;
            sceneLight.intensity = 0.5f * Shader.GetGlobalFloat("_GlobalLightEstimation");
        }

        //calls game over if run out of misses
        if (currentMissCount <= 0 && gameStarted)
        {
            EventsManager.GameOver();
        }

        //turns fan on 
        if (score >= fanLevel1Threshold && !fanTurnedOn)
        {
            EventsManager.FanOn();
            fanTurnedOn = true;
        }

        //turn on fire mode
        if (chainCount >=3 && !isOnFire)
        {
            EventsManager.OnFire();
            isOnFire = true;
        }

        #region Testing----------------------
        if (Input.GetKeyDown(KeyCode.G))
        {
            EventsManager.GameOver();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefsManager.SetEasyBestScore(0);
            PlayerPrefsManager.SetHardBestScore(0);
            PlayerPrefsManager.ResetUnlockablePlayed();
            SoundManager.instance.PlayButtonSFX();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            score += 10;
            endDisplayScore += 10;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameMode = Difficulty.Easy;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            EventsManager.OnFire();
            isOnFire = true;
        }
        #endregion

    }

    public void IncrementScore()
    {
        if (isOnFire)
        {
            score += 2;
        } else
        {
            score++;
        }
        endDisplayScore = score;
        InGameMenuManager.instance.Scored();
        chainCount++;
        InGameMenuManager.instance.TriggerChainTextAnim();
    }

    public void RegisterMiss ()
    {
        currentMissCount--;
        SoundManager.instance.PlayMissSFX();
        HitPanel.instance.MissFlash();
        chainCount = 0;
        if (isOnFire)
        {
            EventsManager.FireOff();
            isOnFire = false;
        }
    }

    public void OnFireTest ()
    {
        EventsManager.OnFire();
        isOnFire = true;
    }

    public void tenTest()
    {
        score += 10;
        endDisplayScore += 10;
    }

}
