using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanRB : MonoBehaviour
{

    public Transform DropPos;

    private Quaternion initialRotation;
    private Vector3 initialPos;
    private Rigidbody rb;
    private Collider rbCol;
    private float timer;
    private bool initialPSPlayed;
    private bool justGotHit;
    private const float ROTATION_THRESHOLD = 10f;
    private const float POSITION_THRESHOLD = 0.01f;


    void Start()
    {
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
    }

    void OnDisable()
    {
        EventsManager.OnScenePlacedEvent -= DropCan;
        EventsManager.OnGameResetEvent -= DropCan;
    }

    void DropCan()
    {
        transform.localPosition = DropPos.localPosition;
        timer = 0f;
        initialPSPlayed = false;
    }
    #endregion

    void Update()
    {
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
}
