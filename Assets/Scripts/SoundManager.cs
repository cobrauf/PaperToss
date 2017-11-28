using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    public AudioSource musicPlayer;
    public AudioSource soundPlayer;
    public AudioClip trashHit, coin, otherHit, toss, splash, paperFlutter, fanFaded, buttonPress,
        miss, whoosh, aww, fireIgnite, fireBall, amazing, bestHighScore, itemUnlocked, rubberduckSFX, canSFX;

    private bool muted;
    public Image muteImage;

    void Awake()
    {
        instance = this;
    }

    #region Events
    void OnEnable()
    {
        EventsManager.OnPlayModeEnterEvent += PlayModeEnterActions;
        EventsManager.OnFireEvent += PlayOnFireSFX;
        EventsManager.OnFireOffEvent += PlayAwwSFX;
    }

    void OnDisable()
    {
        EventsManager.OnPlayModeEnterEvent -= PlayModeEnterActions;
        EventsManager.OnFireEvent -= PlayOnFireSFX;
        EventsManager.OnFireOffEvent -= PlayAwwSFX;
    }

    void PlayModeEnterActions()
    {
        StartCoroutine(FadeMusic());
    }

    IEnumerator FadeMusic()
    {
        while (musicPlayer.volume > 0.1f)
        {
            musicPlayer.volume -= 0.1f;
            yield return new WaitForSeconds(1f);
        }
        musicPlayer.Stop();
    }
    #endregion

    public void PlayCoinSFX()
    {
        soundPlayer.PlayOneShot(coin, 0.3f);
        // soundPlayer.PlayOneShot(splash, 1f);
    }

    public void PlayTrashHit(float rbSpeed)
    {
        if (rbSpeed > 0.2f)
        {
            soundPlayer.PlayOneShot(trashHit, rbSpeed / 3);
        }
    }

    public void PlayOtherHit(float rbSpeed)
    {
        if (rbSpeed > 0.2f)
        {
            soundPlayer.PlayOneShot(otherHit, rbSpeed / 5);
        }
    }
    public void PlayRubberDuckHit(float rbSpeed)
    {
        if (rbSpeed > 1.2f)
        {
            soundPlayer.PlayOneShot(rubberduckSFX, rbSpeed / 5);
        }
    }
    public void PlayCanHit(float rbSpeed)
    {
        if (rbSpeed > 0.5f)
        {
            soundPlayer.PlayOneShot(canSFX, rbSpeed / 3);
        }
    }

    public void PlayTossSFX()
    {
        soundPlayer.PlayOneShot(toss, 1f);
    }

    public void PlayPaperWindSFX()
    {
        soundPlayer.PlayOneShot(paperFlutter, 0.15f);
        soundPlayer.PlayOneShot(fanFaded, 0.1f);
    }

    public void PlayButtonSFX()
    {
        soundPlayer.PlayOneShot(buttonPress, 1f);
    }

    public void PlayMissSFX()
    {
        soundPlayer.PlayOneShot(miss, 1f);
    }

    public void PlayWhooshSFX()
    {
        soundPlayer.PlayOneShot(whoosh, 0.5f);
    }

    public void PlayAwwSFX()
    {
        soundPlayer.PlayOneShot(aww, 0.3f);
    }

    public void PlayFireIgnite()
    {
        soundPlayer.PlayOneShot(fireIgnite, 1f);
    }

    public void PlayFireBallSFX()
    {
        soundPlayer.PlayOneShot(fireBall, 0.2f);
    }

    void PlayOnFireSFX()
    {
        soundPlayer.PlayOneShot(amazing, 1f);
    }

    public void PlayBeatHighScoreSFX()
    {
        soundPlayer.PlayOneShot(bestHighScore, 1f);
    }

    public void PlayItemUnlockedSFX()
    {
        soundPlayer.PlayOneShot(itemUnlocked, 1f);
    }


    public void MuteToggle()
    {
        if (!muted)
        {
            soundPlayer.volume = 0f;
            musicPlayer.volume = 0f;
            muted = true;
            muteImage.enabled = true;
        }
        else
        {
            soundPlayer.volume = 1f;
            musicPlayer.volume = 1f;
            muted = false;
            muteImage.enabled = false;
            PlayButtonSFX();
        }
    }


}
