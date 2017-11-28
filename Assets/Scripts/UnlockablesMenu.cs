using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockablesMenu : MonoBehaviour
{
    public static UnlockablesMenu instance;

    public Canvas unlockablesCanvas, menuCanvas, titleCanvas;
    public Button[] unlockablesBtns;
    public Image[] lockBackgroundImgs, lockImgs, itemImgs;
    public Text[] btnTexts, itemTexts;
    public GameObject scorecardGO, switchBallBtn, restartBtn;
    public Animator scorecardAnimator;


    void Start()
    {
        instance = this;
        switchBallBtn.SetActive(false);
        unlockablesCanvas.enabled = false;
    }

    #region Events
    void OnEnable()
    {
        EventsManager.OnScenePlacedEvent += EnableSwitchBallBtn;
        EventsManager.OnGameResetEvent += EnableSwitchBallBtn;
        EventsManager.OnGameOverEvent += DisableSwitchBallBtn;
        EventsManager.OnSceneResetEvent += DisableSwitchBallBtn;
    }

    void OnDisable()
    {
        EventsManager.OnScenePlacedEvent -= EnableSwitchBallBtn;
        EventsManager.OnGameResetEvent -= EnableSwitchBallBtn;
        EventsManager.OnGameOverEvent -= DisableSwitchBallBtn;
        EventsManager.OnSceneResetEvent -= DisableSwitchBallBtn;
    }

    void EnableSwitchBallBtn()
    {
        int unlockablesReached = PlayerPrefsManager.GetUnlockablesReached();
        if (unlockablesReached > 0)
        {
            switchBallBtn.SetActive(true);
        }
        else
        {
            switchBallBtn.SetActive(false);

        }
    }
    void DisableSwitchBallBtn()
    {
        switchBallBtn.SetActive(false);
    }
    #endregion


    //each time the unlockable menu is called we run through the list again
    public void CheckUnlockables()
    {
        PlayerPrefsManager.UpdateUnlockable();
        int unlockablesReached = PlayerPrefsManager.GetUnlockablesReached();

        for (int i = 0; i < unlockablesBtns.Length; i++)
        {
            if (i + 1 > unlockablesReached)
            {
                unlockablesBtns[i].interactable = false;
                lockImgs[i].enabled = true;
                //itemImgs[i].enabled = false;
                itemTexts[i].enabled = false;
                lockBackgroundImgs[i].enabled = true;
            }
            else
            {
                unlockablesBtns[i].interactable = true;
                lockImgs[i].enabled = false;
                EnableItem(unlockablesBtns[i], itemImgs[i], btnTexts[i], itemTexts[i], lockBackgroundImgs[i]);
            }
        }
    }

    private void EnableItem(Button button, Image image, Text btnText, Text itemText, Image background)
    {
        switch (button.tag)
        {
            case "Unlockable 1":
                image.enabled = true;
                btnText.text = "Reward #1\n\nUnlocked!";
                itemText.enabled = true;
                background.enabled = false;
                break;
            case "Unlockable 2":
                image.enabled = true;
                btnText.text = "Reward #2\n\nUnlocked!";
                itemText.enabled = true;
                background.enabled = false;
                break;
            case "Unlockable 3":
                image.enabled = true;
                btnText.text = "Reward #3\n\nUnlocked!";
                itemText.enabled = true;
                background.enabled = false;
                break;
            case "Unlockable 4":
                image.enabled = true;
                btnText.text = "Reward #4\n\nUnlocked!";
                itemText.enabled = true;
                background.enabled = false;
                break;
            default:
                break;
        }
    }


    public void ShowUnlockables()
    {
        unlockablesCanvas.enabled = true;
        menuCanvas.enabled = false;
        titleCanvas.enabled = false;
        scorecardGO.SetActive(false);
        CheckUnlockables();
        SoundManager.instance.PlayButtonSFX();

        if (GameManager.instance.inPlayMode)//for some reason on phone couldn't get this button to go away by disabling canvas
        {
            restartBtn.SetActive(false);
        }
    }

    public void ExitUnlockables()
    {
        unlockablesCanvas.enabled = false;
        titleCanvas.enabled = true;
        menuCanvas.enabled = true;
        SoundManager.instance.PlayButtonSFX();

        if (GameManager.instance.inPlayMode)
        {
            scorecardGO.SetActive(true);
            restartBtn.SetActive(true);
            scorecardAnimator.SetBool("HideScoreCard", false);
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefsManager.SetEasyBestScore(0);
        PlayerPrefsManager.SetHardBestScore(0);
        PlayerPrefsManager.UpdateUnlockable();
        PlayerPrefsManager.ResetUnlockablePlayed();
        SoundManager.instance.PlayButtonSFX();
    }
}
