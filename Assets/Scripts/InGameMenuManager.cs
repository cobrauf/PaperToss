using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour
{

    public static InGameMenuManager instance;

    #region Variables
    //Object references
    public Animator scoreAnimator, scoreAnimator2, sceneCanvasAnimator;
    public SimpleHelvetica simpleHelvetica_Score;
    public GameObject tooCloseText3D, easyText3D, normalText3D, hardText3D, tooClose2Text3D, swipeUpTextGO; //scoreText3D scoreText3D2

    public Text placeSceneText, menuScoreText, bestScoreText, modeText, missesRemainText, fanTextRight, fanTextLeft, chainText, beatHighScoreText, itemUnlockedText;
    public GameObject trashCan, restartButton, paperIcons, resetSceneButton, fanGORight, fanGOLeft, bonusGO, scorecardGO;
    public Canvas inGameCanvas, scoreCanvas;
    public Image leftFanImage, rightFanImage;

    public bool tooClose;
    private bool isSwipeUpTextEnabled;//used to keep track of swipe up text, so it works with too close warning
    private bool scoreCRRunning;//for score animation CR
    private bool didUnlock;//for  checking if player unlocked item during scorecard
    private string scoreText3DString;
    private int bestScore, maxCombo;
    private Animator inGameAnimator;
    public Animator scorecardAnimator, chainTextAnimator, onFireAnimator, beatHighScoreAnimator, itemUnlockedAnimator, unlockableBtnAnimator;


    //Difficulty distances
    private float easyDistance = 2.25f;
    private float hardDistance = 3f;

    //Game Over score Card
    public Text endModeText, endScoreText, endBestScoreText, maxComboText;
    #endregion


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitialSettings();
    }

    void InitialSettings()
    {
        //swipeUpText.enabled = false;
        swipeUpTextGO.SetActive(false);
        placeSceneText.enabled = false;
        menuScoreText.enabled = false;
        bestScoreText.enabled = false;
        modeText.enabled = false;
        paperIcons.SetActive(false);
        tooCloseText3D.SetActive(false);
        easyText3D.SetActive(false);
        normalText3D.SetActive(false);
        hardText3D.SetActive(false);
        tooClose2Text3D.SetActive(false);
        bestScore = 0;
        maxCombo = 0;
        restartButton.SetActive(false);
        inGameAnimator = GetComponent<Animator>();
        rightFanImage.enabled = false;
        leftFanImage.enabled = false;
        bonusGO.SetActive(false);
        fanGORight.SetActive(false);
        fanGOLeft.SetActive(false);
        resetSceneButton.SetActive(false);
        scorecardGO.SetActive(false);
    }

    #region Events
    void OnEnable()
    {
        EventsManager.OnPlayModeEnterEvent += PlayModeEnterActions;
        EventsManager.OnScenePlacedEvent += ScenePlacedActions;
        EventsManager.OnGameStartEvent += GameStartActions;
        EventsManager.OnGameOverEvent += GameOverActions;
        EventsManager.OnGameResetEvent += GameResetActions;
        EventsManager.OnSceneResetEvent += SceneResetActions;
        EventsManager.OnFireEvent += FireOnActions;
        EventsManager.OnFireOffEvent += FireOffActions;
    }

    void OnDisable()
    {
        EventsManager.OnPlayModeEnterEvent -= PlayModeEnterActions;
        EventsManager.OnScenePlacedEvent -= ScenePlacedActions;
        EventsManager.OnGameStartEvent -= GameStartActions;
        EventsManager.OnGameOverEvent -= GameOverActions;
        EventsManager.OnGameResetEvent -= GameResetActions;
        EventsManager.OnSceneResetEvent -= SceneResetActions;
        EventsManager.OnFireEvent -= FireOnActions;
        EventsManager.OnFireOffEvent -= FireOffActions;
    }

    void ScenePlacedActions()
    {
        swipeUpTextGO.SetActive(true);
        isSwipeUpTextEnabled = true;
        placeSceneText.enabled = false;
        easyText3D.SetActive(false);
        normalText3D.SetActive(false);
        hardText3D.SetActive(false);
        tooClose2Text3D.SetActive(false);
        resetSceneButton.SetActive(true);
    }

    void SceneResetActions()
    {
        swipeUpTextGO.SetActive(false);
        isSwipeUpTextEnabled = false;
        placeSceneText.enabled = true;
        menuScoreText.enabled = false;
        bestScoreText.enabled = false;
        modeText.enabled = false;
        paperIcons.SetActive(false);
        restartButton.SetActive(false);
        bonusGO.SetActive(false);
        fanGORight.SetActive(false);
        fanGOLeft.SetActive(false);
        leftFanImage.enabled = false;
        rightFanImage.enabled = false;
        chainText.enabled = false;
        resetSceneButton.SetActive(false);
        inGameCanvas.enabled = true;
        scorecardAnimator.SetBool("HideScoreCard", true);

        StopAllCoroutines();
    }

    void GameStartActions()
    {
        swipeUpTextGO.SetActive(false);
        isSwipeUpTextEnabled = false;
        menuScoreText.enabled = true;
        bestScoreText.enabled = true;
        modeText.enabled = true;
        paperIcons.SetActive(true);
        scorecardAnimator.SetBool("HideScoreCard", true);
        chainText.enabled = true;
        maxCombo = 0;

        //gets correct best score according to difficulty (game mode)
        if (GameManager.gameMode == GameManager.Difficulty.Hard)
        {
            bestScore = PlayerPrefsManager.GetHardBestScore();
        }
        else
        {
            bestScore = PlayerPrefsManager.GetEasyBestScore();
        }
        bestScoreText.text = "Best: " + bestScore;
    }

    void PlayModeEnterActions()
    {
        placeSceneText.enabled = true;
        scorecardGO.SetActive(true);
    }

    void GameOverActions()
    {
        menuScoreText.enabled = false;
        bestScoreText.enabled = false;
        modeText.enabled = false;
        paperIcons.SetActive(false);
        rightFanImage.enabled = false;
        leftFanImage.enabled = false;
        fanGORight.SetActive(false);
        fanGOLeft.SetActive(false);
        bonusGO.SetActive(false);
        //resetSceneButton.SetActive(false);

        ShowScoreCard();
    }

    void GameResetActions()
    {
        restartButton.SetActive(false);
        swipeUpTextGO.SetActive(true);
        isSwipeUpTextEnabled = true;
        scorecardAnimator.SetBool("HideScoreCard", true);
        SoundManager.instance.PlayWhooshSFX();
        resetSceneButton.SetActive(true);
    }

    void FireOnActions()
    {
        bonusGO.SetActive(true);
        PlayOnFireAnim();
    }

    void FireOffActions()
    {
        bonusGO.SetActive(false);
    }
    #endregion


    void Update()
    {
        #region In Game UI elements
        menuScoreText.text = "Score: " + GameManager.score;
        modeText.text = "Mode: " + GameManager.gameMode;
        MissesCrossOut();

        if (GameManager.chainCount > maxCombo)
        {
            maxCombo = GameManager.chainCount;
        }

        fanTextRight.text = Mathf.Abs(PaperBall.windForce).ToString("F2");
        fanTextLeft.text = Mathf.Abs(PaperBall.windForce).ToString("F2");

        if (GameManager.chainCount > 0)
        {
            chainText.text = "Combo: " + GameManager.chainCount;
        }
        else
        {
            chainText.text = "";
        }
        #endregion

        #region Set Difficulty Mode
        if (GameManager.instance.inPlayMode && !GameManager.instance.scenePlaced)
        {
            if (Vector3.Distance(Camera.main.transform.position, trashCan.transform.position) < easyDistance)
            {
                easyText3D.SetActive(false);
                normalText3D.SetActive(false);
                hardText3D.SetActive(false);
                tooClose2Text3D.SetActive(true);
            }
            if (Vector3.Distance(Camera.main.transform.position, trashCan.transform.position) >= easyDistance && Vector3.Distance(Camera.main.transform.position, trashCan.transform.position) < hardDistance)
            {
                easyText3D.SetActive(true);
                normalText3D.SetActive(false);
                hardText3D.SetActive(false);
                tooClose2Text3D.SetActive(false);
                GameManager.gameMode = GameManager.Difficulty.Easy;
            }
            if (Vector3.Distance(Camera.main.transform.position, trashCan.transform.position) >= hardDistance)
            {
                easyText3D.SetActive(false);
                normalText3D.SetActive(false);
                hardText3D.SetActive(true);
                tooClose2Text3D.SetActive(false);
                GameManager.gameMode = GameManager.Difficulty.Hard;
            }
        }
        #endregion

        #region Distance warning too close to trash can
        float distanceToTriggerTooClose;
        if (GameManager.gameMode == GameManager.Difficulty.Hard)
        {
            distanceToTriggerTooClose = hardDistance;
        }
        else
        {
            distanceToTriggerTooClose = easyDistance;
        }

        if (Vector3.Distance(Camera.main.transform.position, trashCan.transform.position) < distanceToTriggerTooClose && GameManager.instance.scenePlaced)
        {
            tooCloseText3D.SetActive(true);
            tooClose = true;
            swipeUpTextGO.SetActive(false);
        }
        else
        {
            tooCloseText3D.SetActive(false);
            tooClose = false;
            swipeUpTextGO.SetActive(isSwipeUpTextEnabled);
        }
        #endregion

    }

    #region Button Press functions - Restart game, Reset Scene, isOnFire
    //public void ToggleSettings()
    //{
    //    if (GameManager.isOnFire)
    //    {
    //        EventsManager.FireOff();
    //        GameManager.isOnFire = false;
    //    }
    //    else
    //    {
    //        EventsManager.OnFire();
    //        GameManager.isOnFire = true;
    //    }
    //}

    public void Restart()
    {
        EventsManager.GameReset();
        SoundManager.instance.PlayButtonSFX();
    }

    public void ResetScene()
    {
        EventsManager.SceneReset();
        SoundManager.instance.PlayButtonSFX();
    }
    #endregion

    #region Scoring animation, miss cross out text

    public void Scored()
    {
        scoreCanvas.enabled = true;
        if (GameManager.isOnFire)
        {
            sceneCanvasAnimator.SetTrigger("Score2Trigger");
        }
        else
        {
            sceneCanvasAnimator.SetTrigger("Score1Trigger");
        }
        StartCoroutine(ScoreAnimationCR());
    }

    IEnumerator ScoreAnimationCR()
    {
        yield return new WaitForSeconds(1f);
        scoreCanvas.enabled = false;
    }

    void MissesCrossOut()
    {
        switch (GameManager.currentMissCount)
        {
            case 5:
                missesRemainText.text = "";
                break;
            case 4:
                missesRemainText.text = "X";
                break;
            case 3:
                missesRemainText.text = "X X";
                break;
            case 2:
                missesRemainText.text = "X X X";
                break;
            case 1:
                missesRemainText.text = "X X X X";
                break;
            case 0:
                missesRemainText.text = "X X X X X";
                break;
            default:
                missesRemainText.text = "";
                break;
        }
    }
    #endregion

    #region Play Fan
    public void PlayFan()
    {
        if (PaperBall.windForce <= -0.5f)
        {
            rightFanImage.enabled = true;
            leftFanImage.enabled = false;
            inGameAnimator.SetInteger("FanSpinInt", 1);

            fanGORight.SetActive(true);
            fanGOLeft.SetActive(false);

        }
        else if (PaperBall.windForce >= 0.5f)
        {
            leftFanImage.enabled = true;
            rightFanImage.enabled = false;
            inGameAnimator.SetInteger("FanSpinInt", -1);

            fanGORight.SetActive(false);
            fanGOLeft.SetActive(true);
        }
        else
        {
            fanGORight.SetActive(false);
            fanGOLeft.SetActive(false);
            leftFanImage.enabled = false;
            rightFanImage.enabled = false;
        }
    }

    IEnumerator StopFan()
    {
        yield return new WaitForSeconds(2f);
        inGameAnimator.SetInteger("FanSpinInt", 0);
        leftFanImage.enabled = false;
        rightFanImage.enabled = false;
    }
    #endregion


    #region ScoreCard Animation
    void ShowScoreCard()
    {
        StartCoroutine(ShowScoreCardCR());
    }

    IEnumerator ShowScoreCardCR()
    {
        yield return new WaitForSeconds(1.5f);
        //if beat high Score--------------------------
        if (GameManager.score > bestScore)
        {
            #region Set high score in player prefs
            if (GameManager.gameMode == GameManager.Difficulty.Hard)
            {
                PlayerPrefsManager.SetHardBestScore(GameManager.score);
            }
            else
            {
                PlayerPrefsManager.SetEasyBestScore(GameManager.score);
            }
            #endregion

            endScoreText.text = "Score: " + GameManager.endDisplayScore;
            endModeText.text = "Mode: " + GameManager.gameMode;
            endBestScoreText.text = "Best: " + bestScore;
            maxComboText.text = "Max Combo: " + maxCombo;
            beatHighScoreText.text = "";
            itemUnlockedText.text = "";
            scorecardAnimator.SetBool("HideScoreCard", false);
            SoundManager.instance.PlayWhooshSFX();

            //play beat high score animation (for both easy and hard mode)
            yield return new WaitForSeconds(1f);
            beatHighScoreText.text = "New High Score!";
            bestScore = GameManager.score;
            endBestScoreText.text = "Best: " + bestScore;
            beatHighScoreAnimator.SetTrigger("HighScoreTrigger");
            SoundManager.instance.PlayBeatHighScoreSFX();

            //check unlockable item and play animation
            didUnlock = false;
            yield return StartCoroutine(ItemUnlockAnimation());

            //show restart button
            yield return new WaitForSeconds(1.5f);
            restartButton.SetActive(true);
            MainMenuManager.instance.ShowSettingsGO();
            if (didUnlock)
            {
                unlockableBtnAnimator.SetTrigger("UnlockableBtnTrigger");
            }
            didUnlock = false;
        }

        //if not beat high score---------------------------
        else
        {
            endScoreText.text = "Score: " + GameManager.endDisplayScore;
            endModeText.text = "Mode: " + GameManager.gameMode;
            endBestScoreText.text = "Best: " + bestScore;
            maxComboText.text = "Max Combo: " + maxCombo;
            beatHighScoreText.text = "";
            itemUnlockedText.text = "";
            restartButton.SetActive(true);
            scorecardAnimator.SetBool("HideScoreCard", false);
            SoundManager.instance.PlayWhooshSFX();
            MainMenuManager.instance.ShowSettingsGO();
        }
    }

    IEnumerator ItemUnlockAnimation()
    {
        int[] itemUnlockPlayed = new int[5];
        itemUnlockPlayed = PlayerPrefsManager.CheckIfUnlockPlayed();

        if (GameManager.gameMode == GameManager.Difficulty.Easy)
        {
            itemUnlockedText.text = "Try Hard Mode to unlock items\n(Reposition bin further away)";
            yield break;//exit coroutine
        }

        if (bestScore >= 30)
        {
            if (itemUnlockPlayed[4] == 0)//if animation hasn't been played yet for item #3
            {
                yield return new WaitForSeconds(1f);
                itemUnlockedText.text = "Reward #4 Unlocked!";
                itemUnlockedAnimator.SetTrigger("ItemUnlockedTrigger");
                SoundManager.instance.PlayItemUnlockedSFX();
                PlayerPrefsManager.SetUnlockablePlayed(4);
                didUnlock = true;
            }
        }
        else if (bestScore >= 25)
        {
            if (itemUnlockPlayed[3] == 0)//if animation hasn't been played yet for item #3
            {
                yield return new WaitForSeconds(1f);
                itemUnlockedText.text = "Reward #3 Unlocked!";
                itemUnlockedAnimator.SetTrigger("ItemUnlockedTrigger");
                SoundManager.instance.PlayItemUnlockedSFX();
                PlayerPrefsManager.SetUnlockablePlayed(3);
                didUnlock = true;
            }
            else
            {
                itemUnlockedText.text = "Score 30+ on Hard Mode to Unlock Next Item";
            }
        }
        else if (bestScore >= 15)
        {
            if (itemUnlockPlayed[2] == 0)//if animation hasn't been played yet for item #2
            {
                yield return new WaitForSeconds(1f);
                itemUnlockedText.text = "Reward #2 Unlocked!";
                itemUnlockedAnimator.SetTrigger("ItemUnlockedTrigger");
                SoundManager.instance.PlayItemUnlockedSFX();
                PlayerPrefsManager.SetUnlockablePlayed(2);
                didUnlock = true;
            }
            else
            {
                itemUnlockedText.text = "Score 25+ on Hard Mode to Unlock Next Item";
            }
        }
        else if (bestScore >= 10)
        {
            if (itemUnlockPlayed[1] == 0)//if animation hasn't been played yet for item #1
            {
                yield return new WaitForSeconds(1f);
                itemUnlockedText.text = "Reward #1 Unlocked!";
                itemUnlockedAnimator.SetTrigger("ItemUnlockedTrigger");
                SoundManager.instance.PlayItemUnlockedSFX();
                PlayerPrefsManager.SetUnlockablePlayed(1);
                didUnlock = true;
            }
            else
            {
                itemUnlockedText.text = "Score 15+ on Hard Mode to Unlock Next Item";
            }
        }
        else
        {
            itemUnlockedText.text = "Score 10+ on Hard Mode to Unlock Next Item";
        }
        yield return null;
    }
    #endregion

    public void TriggerChainTextAnim()
    {
        chainTextAnimator.SetTrigger("ChainTextTrigger");
    }

    void PlayOnFireAnim()
    {
        onFireAnimator.SetTrigger("OnFireTrigger");
    }
}
