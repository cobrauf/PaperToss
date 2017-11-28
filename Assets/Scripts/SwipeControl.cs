using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControl : MonoBehaviour {

	public static SwipeControl instance;

    #region Variables
    public GameObject[] throwObjArray;
    public GameObject paperBall;

    public GameObject[] miniThrowObjArray;
    public GameObject miniPaperBall;

    public GameObject contourBall;
    public Transform shootPos;

    private bool isDraging;
    private Vector2 startTouch, swipeDelta;
    private float deltaX, deltaY;
    private float deltaTime;
    public static float powerFactor = 3f;
    public static float xValueFactor = 20f;
    public bool hasWind, hasWindActivated;
    private bool firstShotLaunched;

    private bool canShoot;
    public float force;
    private Rigidbody ballRB;
    private List<GameObject> ballList = new List<GameObject>();

    public float menuPower;
    #endregion


    void Awake () {
        instance = this;
	}

	void Start () {
		miniPaperBall.SetActive (false);
        InvokeRepeating("MainMenuShoot", 1f, 2.5f);
	}

	#region Events
	void OnEnable () {
		EventsManager.OnScenePlacedEvent += ScenePlacedActions;
        EventsManager.OnPlayModeEnterEvent += PlayModeEnterActions;
        EventsManager.OnGameOverEvent += GameOverActions;
        EventsManager.OnGameResetEvent += GameResetActions;
        EventsManager.OnSceneResetEvent += SceneResetActions;
        EventsManager.OnFanOnEvent += FanOnActions;       
    }

    void OnDisable () {
		EventsManager.OnScenePlacedEvent -= ScenePlacedActions;
        EventsManager.OnPlayModeEnterEvent -= PlayModeEnterActions;
        EventsManager.OnGameOverEvent -= GameOverActions;
        EventsManager.OnGameResetEvent -= GameResetActions;
        EventsManager.OnSceneResetEvent -= SceneResetActions;
        EventsManager.OnFanOnEvent -= FanOnActions;        
    }

    void ScenePlacedActions () {
		canShoot = true;
		firstShotLaunched = false;
	}

    void PlayModeEnterActions()
    {
        CancelInvoke("MainMenuShoot");
    }

    void GameOverActions ()
    {
        StopAllCoroutines();
        canShoot = false;
        hasWind = false;
    }

    void GameResetActions()
    {
        //destroy all balls
        foreach (GameObject go in ballList)
        {
            Destroy(go);
        }      
        ballList.Clear();
        canShoot = true;
        firstShotLaunched = false;
    }

    void SceneResetActions () {
		foreach (GameObject go in ballList) {
			Destroy (go);
		}		
		ballList.Clear ();
		canShoot = false;
		firstShotLaunched = false;
        hasWind = false;

        StopAllCoroutines();
	}

    void FanOnActions ()
    {
        hasWind = true;
    }    
    #endregion

    void Update()
    {
        if (canShoot && !InGameMenuManager.instance.tooClose)
        {
            if (!miniPaperBall.activeInHierarchy)
            {
                miniPaperBall.SetActive(true);
                if (GameManager.isOnFire)
                {
                    PSManager.instance.PlayMiniFire();
                    SoundManager.instance.PlayFireBallSFX();
                }
            }           
        }
        else
        {
            miniPaperBall.SetActive(false);
        }

        #region Ball swipe controls
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //if first touch, set isDraging and mark start pos
                isDraging = true;
                deltaTime = Time.time;
                startTouch = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                //else if touching ended or canceled + magnitude > threshold + swipe direction is up, shoot then reset
                if (swipeDelta.magnitude > 100 && swipeDelta.y > 0)
                {
                    deltaX = swipeDelta.x;
                    deltaTime = Time.time - deltaTime;
                    ShootBall(swipeDelta.magnitude, deltaX, deltaTime);
                }
                Reset();
            }
        }

        //calculate distance every update if isDraging
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touchCount > 0)
            {
                swipeDelta = Input.GetTouch(0).position - startTouch;
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            float tempSpeed = Random.Range(500f, 1000f);
            ShootBall(tempSpeed, 0f, 0.2f);
        }
        #endregion
    }

    void Reset () {
		startTouch = swipeDelta = Vector2.zero;
		deltaX = deltaY = 0f;
		isDraging = false;
	}

    #region Launch/Shoot paper ball
    void ShootBall(float magnitude, float xValue, float timeValue)
    {
        /// shoot ball using the swipe magnitude and delta in swipe x
        /// attach blob shadow to it
        /// also assign a windforce to affect trajectory
#if UNITY_EDITOR
        UnityShoot(magnitude, xValue, timeValue);
        SwipeSlider.instance.ShowSwipeSlider(magnitude, timeValue);
        return;
#endif

        if (!canShoot)
        {
            return;
        }
        if (InGameMenuManager.instance.tooClose)//if too close to trash can, don't shoot
        {
            return;
        }

        if (!firstShotLaunched)
        {
            firstShotLaunched = true;
            EventsManager.GameStart();
        }

        GameObject ballObj = Instantiate(paperBall, shootPos.position, shootPos.rotation);       
        PaperBall _paperBall = ballObj.GetComponent<PaperBall>();       
        ballRB = ballObj.GetComponent<Rigidbody>();

        if (GameManager.isOnFire)
        {
            _paperBall.fire.Play();
        }

        //normalize values using an arbitrary multiplier
        float xValueNorm = xValue / xValueFactor;
        float powerNorm = Mathf.Sqrt((magnitude / timeValue)) * powerFactor;

        ballObj.transform.Rotate(0f, xValueNorm, 0f);
        ballRB.AddForce(ballObj.transform.forward * powerNorm);
        ballRB.AddTorque(Camera.main.transform.right * 500f);

        SoundManager.instance.PlayTossSFX();

        ballList.Add(ballObj);
        Destroy(ballObj, 30f);

        StopCoroutine(ShootCoolDownCR());
        StartCoroutine(ShootCoolDownCR());
        canShoot = false;
        SwipeSlider.instance.ShowSwipeSlider(magnitude, timeValue);
    }

    void UnityShoot (float magnitude, float xValue, float timeValue)
    {

        if (!canShoot)
        {
            return;
        }

        if (!firstShotLaunched)
        {
            firstShotLaunched = true;
            EventsManager.GameStart();
        }
        GameObject ballObj = Instantiate(paperBall, shootPos.position, shootPos.rotation);       
        PaperBall _paperBall = ballObj.GetComponent<PaperBall>();      
        ballRB = ballObj.GetComponent<Rigidbody>();

        if (GameManager.isOnFire)
        {
            _paperBall.fire.Play();
        }

        //normalize values using an arbitrary multiplier
        float xValueNorm = xValue / xValueFactor;
        float powerNorm = Mathf.Sqrt((magnitude / timeValue)) * powerFactor;

        ballObj.transform.Rotate(0f, xValueNorm, 0f);
        ballRB.AddForce(ballObj.transform.forward * powerNorm);
        ballRB.AddTorque(Camera.main.transform.right * 500f);

        SoundManager.instance.PlayTossSFX();

        ballList.Add(ballObj);
        Destroy(ballObj, 30f);

        StopCoroutine(ShootCoolDownCR());
        StartCoroutine(ShootCoolDownCR());
        canShoot = false;
    }
    #endregion

    void MainMenuShoot()    {  

        GameObject ballObj = Instantiate(contourBall, shootPos.position, shootPos.rotation);   
        Rigidbody ballRB = ballObj.GetComponent<Rigidbody>();

        menuPower = Random.Range(75f, 150f);

        ballRB.AddForce(ballObj.transform.forward * menuPower);
        ballRB.AddTorque(Camera.main.transform.right * 500f);
        SoundManager.instance.PlayTossSFX();
        Destroy(ballObj, .75f);       
    }

    #region Cooldown and set fan/wind speed
    IEnumerator ShootCoolDownCR()
    {
        yield return new WaitForSeconds(2f);
        canShoot = true;       

        if (!hasWind)
        {
            PaperBall.windForce = 0f;
            if (hasWindActivated)
            {
                //once hasWind triggers, we will note that it has activated and re run this
                PaperBall.windForce = SetFanSpeed();
                if (PaperBall.windForce != 0f)
                {
                    PSManager.instance.PlayWindSplashPS();
                    SoundManager.instance.PlayPaperWindSFX();
                }
                InGameMenuManager.instance.PlayFan();
            }
        }
        else
        {
            hasWindActivated = true;
            //set next wind force   
            PaperBall.windForce = SetFanSpeed();
            if (PaperBall.windForce != 0f)
            {
                PSManager.instance.PlayWindSplashPS();
                SoundManager.instance.PlayPaperWindSFX();
            }
            InGameMenuManager.instance.PlayFan();
        }
    }

    float SetFanSpeed ()
    {
        //adds a random to disable wind
      
        int windRandom = Random.Range(0, 3);
        if (windRandom == 0)
        {
            hasWind = false;
            Debug.Log("randome wind 0");
            return 0f;
        }
        else
        {
            hasWind = true;
        }       

        //sets a random sign pos or neg
        int randomNum = Random.Range(0, 2);
        int sign;
        if (randomNum == 0)
        {
            sign = -1;
        } else
        {
            sign = 1;
        }

        float fanSpeed, minSpeed, maxSpeed;        
        
        if (GameManager.score >= GameManager.fanLevel1Threshold && GameManager.score < GameManager.fanLevel2Threshold)
        {
            minSpeed = 0.5f;
            maxSpeed = 1f;
            fanSpeed = sign * Random.Range(minSpeed, maxSpeed);
        }
        else if (GameManager.score >= GameManager.fanLevel2Threshold && GameManager.score < GameManager.fanLevel3Threshold)
        {
            minSpeed = 1f;
            maxSpeed = 1.5f;
            fanSpeed = sign * Random.Range(minSpeed, maxSpeed);
        }
        else if (GameManager.score >= GameManager.fanLevel3Threshold)
        {
            minSpeed = 1.5f;
            maxSpeed = 2.5f;
            fanSpeed = sign * Random.Range(minSpeed, maxSpeed);
        }
        else
        {
            fanSpeed = 0f;//should not occur
        }

        return fanSpeed;
    }
    #endregion

    //used to count # of misses remaining
    public int NumBallsLaunched { get { return ballList.Count; } }

}
