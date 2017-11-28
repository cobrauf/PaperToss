using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeSlider : MonoBehaviour
{
    public static SwipeSlider instance;

    public Slider swipeSlider;
    public Image sliderBackground, fillImage;
    public Text speedText;

    private float maxSpeed = 15f;//to normalize swipe speed against
    private float delay = 0.001f;
    private float fillrate = 0.02f;
    private float delaySpeedText = 0.0005f;
    private float displaySpeedIncRate = 0.2f;

    private float sliderFill;
    //private Coroutine sliderCRVar, speedTextCRVar;
    private float displaySpeed;

    public float adjustment = 1f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        swipeSlider.enabled = false;
        sliderBackground.enabled = false;
        fillImage.enabled = false;
        speedText.enabled = false;
    }

    #region Events
    void OnEnable()
    {
        EventsManager.OnSceneResetEvent += DisableSlider;
    }

    void OnDisable()
    {
        EventsManager.OnSceneResetEvent -= DisableSlider;
    }

    void DisableSlider()
    {
        swipeSlider.enabled = false;
        sliderBackground.enabled = false;
        speedText.enabled = false;
        fillImage.enabled = false;
    }
    #endregion

    
    public void ShowSwipeSlider(float swipeMagnitude, float deltaTime)
    {
        float swipeSpeed = swipeMagnitude / deltaTime;
        Debug.Log("speed = " + swipeSpeed);
        //if (sliderCRVar != null)
        //{
        //    StopCoroutine(sliderCRVar);
        //}

        //if (speedTextCRVar != null)
        //{
        //    StopCoroutine(speedTextCRVar);
        //}
        StopAllCoroutines();
        StartCoroutine(ShowSwipeSliderCR(swipeSpeed));
        StartCoroutine(ShowSpeedTextCR(swipeSpeed));
        //sliderCRVar = StartCoroutine(ShowSwipeSliderCR(swipeSpeed));
        //speedTextCRVar = StartCoroutine(ShowSpeedTextCR(swipeSpeed));
    }

    IEnumerator ShowSwipeSliderCR(float swipeSpeed)
    {
        swipeSlider.enabled = true;
        sliderBackground.enabled = true;
        fillImage.enabled = true;
        speedText.enabled = true;

        //Debug.Log("swipeSpeed" + swipeSpeed);
        float percentFill = swipeSpeed * adjustment / maxSpeed;
        swipeSlider.value = 0f;
        while (swipeSlider.value < percentFill)
        {
            swipeSlider.value += fillrate;
            yield return new WaitForSeconds(delay);
        }
        //sliderCRVar = null;
        yield return new WaitForSeconds(1.5f);
        swipeSlider.enabled = false;
        sliderBackground.enabled = false;
        speedText.enabled = false;
        fillImage.enabled = false;
        //yield return null;
    }

    IEnumerator ShowSpeedTextCR(float swipeSpeed)
    {
        displaySpeed = 0f;
        while (displaySpeed < swipeSpeed * adjustment)
        {
            displaySpeed += displaySpeedIncRate;
            speedText.text = "Speed:\n" + displaySpeed.ToString("F1");
            yield return new WaitForSeconds(delaySpeedText);
        }
        //speedTextCRVar = null;
        //yield return null;
    }

}
