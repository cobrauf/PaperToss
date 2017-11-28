using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

	public static MainMenuManager instance;

	private Animator menuAnimator;
	public Canvas inGameCanvas;
    public GameObject miniTrashCan, title;
    public GameObject settingsGO, moreButtons;
    public Canvas unlockablesCanvas, menuCanvas, titleCanvas;


    void Awake()
	{
        instance = this;
    }

	void Start () {
		menuAnimator = GetComponent<Animator>();
		inGameCanvas.enabled = false;
	}

	#region Events
	void OnEnable()
	{
		EventsManager.OnMainMenuEvent += ShowMainMenu;
		EventsManager.OnPlayModeEnterEvent += ShowInGameMenu;
        EventsManager.OnScenePlacedEvent += DisableSettingsBtns;
        EventsManager.OnGameStartEvent += DisableSettingsBtns;
        EventsManager.OnGameResetEvent += DisableSettingsBtns;
        EventsManager.OnSceneResetEvent += DisableSettingsBtns;
    }

	void OnDisable()
	{
		EventsManager.OnMainMenuEvent -= ShowMainMenu;
		EventsManager.OnPlayModeEnterEvent -= ShowInGameMenu;
        EventsManager.OnScenePlacedEvent -= DisableSettingsBtns;
        EventsManager.OnGameStartEvent -= DisableSettingsBtns;
        EventsManager.OnGameResetEvent -= DisableSettingsBtns;
        EventsManager.OnSceneResetEvent -= DisableSettingsBtns;

    }

	void ShowMainMenu()
	{
		menuAnimator.SetBool("HideButtons", false);
		inGameCanvas.enabled = false;
        miniTrashCan.SetActive(true);
    }

    void ShowInGameMenu()
	{
		inGameCanvas.enabled = true;
        settingsGO.SetActive(false);
    }

    void DisableSettingsBtns()
    {
        settingsGO.SetActive(false);
    }
    #endregion



    public void EnterPlayMode()
	{
		EventsManager.PlayModeEnter();
		menuAnimator.SetBool("HideButtons", true);        
        miniTrashCan.SetActive(false);
        SoundManager.instance.PlayButtonSFX();
        SoundManager.instance.PlayWhooshSFX();
	}

    public void ShowSettingsGO()
    {
        settingsGO.SetActive(true);
    }

    public void ShowMoreButtons()
    {
        if (!moreButtons.activeInHierarchy)
        {
            moreButtons.SetActive(true);
        }
        else
        {
            moreButtons.SetActive(false);
        }
        SoundManager.instance.PlayButtonSFX();
    }  

}
