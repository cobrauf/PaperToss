using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSManager : MonoBehaviour {

    public static PSManager instance;

    public ParticleSystem paperSplashSmall;
    public ParticleSystem dustPuff;

    [Header("Paper Splash and Child PS")]
    public ParticleSystem paperSplashPS;
    public ParticleSystem paperSplashPSChild1;
    public ParticleSystem paperSplashPSChild2;
    public ParticleSystem paperSplashPSChild3;

    [Header("Wind Splash and Child PS")]
    public GameObject windSplashGO;
    public ParticleSystem windSplashPS;
    public ParticleSystem windSplashPSChild1;
    public ParticleSystem windSplashPSChild2;
    public ParticleSystem windSplashPSChild3;

    [Header("Wind Splash Positions")]
    public Transform windSplashPosRight;
    public Transform windSplashPosLeft;    

    [Header("Trash Can Fire")]
    public ParticleSystem trashFire;
    public ParticleSystem trashFireChild1;
    public ParticleSystem trashFireChild2;
    public ParticleSystem trashFireChild3;
    public ParticleSystem trashFireChild4;
    public ParticleSystem trashFireChild5;

    [Header("Mini Fire Arrays")]
    public ParticleSystem[] miniFireArray;
    public ParticleSystem[] miniFireChild1Array;
    public ParticleSystem[] miniFireChild2Array;
    [Header("Mini Paper Ball Fire")]
    public ParticleSystem miniFire;
    public ParticleSystem miniFireChild1;
    public ParticleSystem miniFireChild2;

    void Awake ()
    {
        instance = this;
    }

    #region Events
    void OnEnable()
    {
        EventsManager.OnFireEvent += PlayMiniFire;
        EventsManager.OnFireOffEvent += StopMiniFire;
    }

    void OnDisable()
    {
        EventsManager.OnFireEvent -= PlayMiniFire;
        EventsManager.OnFireOffEvent -= StopMiniFire;
    }   

    #endregion

    public void PlayPaperSplashSmall ()
    {
        paperSplashSmall.Play();
    }
        
    public void PlayPaperSplashPS()    {
        //float adjustedWindForce = PaperBall.windForce * -2;

        //var fol1 = paperSplashPSChild1.forceOverLifetime;
        //var fol2 = paperSplashPSChild2.forceOverLifetime;
        //var fol3 = paperSplashPSChild3.forceOverLifetime;

        //fol1.x = fol2.x = fol3.x = adjustedWindForce;
        paperSplashPS.Play();
    }

    public void PlayWindSplashPS ()
    {
        if (PaperBall.windForce < 0)
        {
            windSplashGO.transform.localPosition = windSplashPosRight.localPosition;
        } else
        {
            windSplashGO.transform.localPosition = windSplashPosLeft.localPosition;
        }

        float adjustedWindForce = PaperBall.windForce * 2;

        var vol1 = windSplashPSChild1.velocityOverLifetime;
        var vol2 = windSplashPSChild2.velocityOverLifetime;
        var vol3 = windSplashPSChild3.velocityOverLifetime;

        vol1.x = vol2.x = vol3.x = adjustedWindForce;
        windSplashPS.Play();
    }

    public void PlayDustPuffPS (Vector3 pos)
    {
        Instantiate(dustPuff, pos, Quaternion.LookRotation(Camera.main.transform.position));
    }

    public void PlayMiniFire()
    {
        var vol = miniFireChild1.velocityOverLifetime;
        var fol = miniFireChild2.forceOverLifetime;

        if (PaperBall.windForce <= -0.5f)
        {
            vol.x = -0.05f;
            fol.x = -0.2f;
        }
        else if (PaperBall.windForce >= 0.5f)
        {
            vol.x = 0.05f;
            fol.x = 0.2f;
        } else
        {
            vol.x = 0f;
            fol.x = 0f;
        }

        miniFire.Play();
    }

    void StopMiniFire()
    {
        miniFire.Stop();
    }

    public void PlayTrashCanFire ()
    {
       // var vol1 = trashFireChild1.velocityOverLifetime;//doesn't look good
        var vol2 = trashFireChild2.velocityOverLifetime;
        var vol3 = trashFireChild3.velocityOverLifetime;
        var fol4 = trashFireChild4.forceOverLifetime;
        var fol5 = trashFireChild5.forceOverLifetime;

        if (PaperBall.windForce <= -0.5f)
        {
            vol2.x = vol3.x = 0.3f;
            fol4.x = fol5.x = 0.3f;
        }
        else if (PaperBall.windForce >= 0.5f)
        {
            vol2.x = vol3.x = -0.3f;
            fol4.x = fol5.x = -0.3f;
        } else
        {
            vol2.x = vol3.x = fol4.x = fol5.x = 0f;
        }

        trashFire.Play();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayTrashCanFire();
        }
    }


}
