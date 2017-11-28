using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanRB : MonoBehaviour {

    public Transform DropPos;
    private Quaternion initialRotation;

    private Vector3 initialPos;
    private Rigidbody rb;
    private Collider rbCol;
    private float timer;
    private bool initialPSPlayed, justGotHit;
    private const float ROTATION_THRESHOLD = 10f;
    private const float POSITION_THRESHOLD = 0.01f;



    void Start () {
        rb = GetComponent<Rigidbody>();
        rbCol = GetComponent<Collider>();
        initialRotation = transform.localRotation;
        initialPos = transform.localPosition;
    }

    #region Events
    void OnEnable()
    {        
        EventsManager.OnScenePlacedEvent += DropCan;
        EventsManager.OnGameResetEvent += DropCan;
        EventsManager.OnGameStartEvent += ResetVariable;

    }

    void OnDisable()
    {        
        EventsManager.OnScenePlacedEvent -= DropCan;
        EventsManager.OnGameResetEvent -= DropCan;
        EventsManager.OnGameStartEvent -= ResetVariable;

    }

    void DropCan()
    {
        transform.localPosition = DropPos.localPosition;
        timer = 0f;
        initialPSPlayed = false;
    }

    void ResetVariable ()
    {
        //justGotHit = false;
        //rbCol.enabled = true;
        //rb.useGravity = true;
    }
#endregion

    void Update () {
        timer += Time.deltaTime;

        //clamps trash can to upright rotation but allow some movement
        if (transform.localEulerAngles.z > ROTATION_THRESHOLD || transform.localEulerAngles.z < -ROTATION_THRESHOLD)
        {
            transform.localRotation = initialRotation;
        }
        if (transform.localEulerAngles.x > ROTATION_THRESHOLD || transform.localEulerAngles.x < -ROTATION_THRESHOLD)
        {
            transform.localRotation = initialRotation;
        }
        //clamps trash can to initial position
        if (transform.localPosition.x > POSITION_THRESHOLD || transform.localEulerAngles.x < -POSITION_THRESHOLD)
        {
            transform.localPosition = initialPos;
        }
        if (transform.localPosition.z > POSITION_THRESHOLD || transform.localEulerAngles.z < -POSITION_THRESHOLD)
        {
            transform.localPosition = initialPos;
        }
    }


    void OnCollisionEnter(Collision col)
    {
        //add a cooldown to being hit by ball
        //if (col.gameObject.layer == 8 && !justGotHit)
        //{
        //    justGotHit = true;
        //    rbCol.enabled = false;
        //    rb.useGravity = false;
        //    rb.isKinematic = true;
        //    StartCoroutine(CoolDownCR());
        //}

        //for initial drop-------------
        if (timer > 2f)
        {
            return;
        }
        if (!initialPSPlayed && GameManager.instance.scenePlaced)
        {
            initialPSPlayed = true;
            PSManager.instance.PlayPaperSplashSmall();
        }

        SoundManager.instance.PlayTrashHit(rb.velocity.magnitude * 5f);          
        
    }

    //IEnumerator CoolDownCR()
    //{
    //    yield return new WaitForSeconds(1f);
    //    justGotHit = false;
    //    rbCol.enabled = true;
    //    rb.useGravity = true;
    //    rb.isKinematic = false;

    //}
}
