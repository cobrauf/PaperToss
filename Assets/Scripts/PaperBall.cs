using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBall : MonoBehaviour
{

    public ParticleSystem fire;
    public bool hasScored;
    public static float windForce;

    private Rigidbody ballRB;
    private MeshCollider meshCol;
    private BoxCollider boxCol;

    private bool missRegistered;
    private bool CRfired;
    private bool dustPlayed;
    private float timer = 0f;
    private Color currentPaperColor;
    private Renderer paperRend;

    void Start()
    {
        ballRB = GetComponent<Rigidbody>();
        meshCol = GetComponent<MeshCollider>();
        boxCol = GetComponent<BoxCollider>();
        paperRend = GetComponent<Renderer>();        
        currentPaperColor = paperRend.material.color;
    }

    #region Events
    void OnEnable()
    {
        EventsManager.OnSceneResetEvent += ResetWind;
        EventsManager.OnGameOverEvent += ResetWind;
    }

    void OnDisable()
    {
        EventsManager.OnSceneResetEvent -= ResetWind;
        EventsManager.OnGameOverEvent -= ResetWind;
    }

    void ResetWind()
    {
        windForce = 0f;
    }

    #endregion

    void FixedUpdate()
    {
        if (!hasScored)
        {
            ballRB.AddForce(Camera.main.transform.right * windForce);
        }
        timer += Time.deltaTime;
    }

    //plays sounds according to velocity of ball; if timer > 3, don't play
    void OnCollisionEnter(Collision col)
    {
        if (timer > 3f)
        {
            return;
        }

        if (GameManager.isOnFire)
        {
            StartCoroutine(ChangePaperBallColor());
        }

        //if hit trash can RB, disable collision 
        if (col.gameObject.layer == 12)
        {
            ballRB.isKinematic = true;
            ballRB.detectCollisions = false;
            ballRB.useGravity = false;
            meshCol.enabled = false;
            boxCol.enabled = false;
        }

        //play trash can SFX
        if (col.gameObject.CompareTag("Trash Can"))
        {
            SoundManager.instance.PlayTrashHit(ballRB.velocity.magnitude);
            PlayObjSFX();
            return;
        }

        //play floor sound and dust puff
        if (col.gameObject.CompareTag("Floor"))
        {
            if (!missRegistered && !hasScored)
            {
                GameManager.instance.RegisterMiss();
                missRegistered = true;
            }
            if (!dustPlayed && !hasScored)
            {
                dustPlayed = true;
                PSManager.instance.PlayDustPuffPS(col.contacts[0].point);
            }
        }
        PlayObjSFX();
    }

    //play sound according to ball object launched
    void PlayObjSFX()
    {
        switch (tag)
        {
            case "Paperball":
                SoundManager.instance.PlayOtherHit(ballRB.velocity.magnitude);
                break;
            case "Rubberduck":
                SoundManager.instance.PlayRubberDuckHit(ballRB.velocity.magnitude);
                break;
            case "Beer Can":
                SoundManager.instance.PlayCanHit(ballRB.velocity.magnitude);
                break;
            default:
                SoundManager.instance.PlayOtherHit(ballRB.velocity.magnitude);
                break;
        }
    }

    //scoring
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Score"))
        {
            StartCoroutine(TurnOffRB());
            if (!hasScored)
            {
                hasScored = true;
                SoundManager.instance.PlayCoinSFX();
                GameManager.instance.IncrementScore();
                if (GameManager.isOnFire)
                {
                    PSManager.instance.PlayTrashCanFire();
                    SoundManager.instance.PlayFireIgnite();
                }
                else
                {
                    PSManager.instance.PlayPaperSplashPS();
                }
            }
        }

        //if goes out of bounds, destroy; and if has not scored, register a miss
        if (other.gameObject.CompareTag("Kill Floor"))
        {
            if (!missRegistered && !hasScored)
            {
                GameManager.instance.RegisterMiss();
                missRegistered = true;
            }
            Destroy(this.gameObject);
        }
    }

    IEnumerator TurnOffRB()
    {
        yield return new WaitForSeconds(2f);
        ballRB.isKinematic = true;
        ballRB.detectCollisions = false;
        ballRB.useGravity = false;
        meshCol.enabled = false;
        boxCol.enabled = false;
    }

    IEnumerator ChangePaperBallColor()
    {
        while (currentPaperColor.r > 0.3f)
        {
            yield return new WaitForSeconds(0.05f);
            currentPaperColor.r -= 0.03f;
            currentPaperColor.g -= 0.03f;
            currentPaperColor.b -= 0.03f;
            paperRend.material.color = currentPaperColor;
        }
    }

}
