using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
    public RectTransform scrollPanelTrans;//panel to be scrolled
    public RectTransform centerTrans;//center point to be compared to so we know which button to center
    public Button[] btnsArray;
    public float lerpSpeed;

    private float[] btnDistanceArray; //holds the distances of each btn for calc
    private bool isDragging;
    private int btnConstDistance; //distance between each button
    private int minBtnNum;//closes btn to the center

    void Start()
    {
        btnDistanceArray = new float[btnsArray.Length]; //declaring the array

        //calc the distance of each btn by measuring the first and sec btn distance
        btnConstDistance = (int)Mathf.Abs(btnsArray[1].GetComponent<RectTransform>().anchoredPosition.x - btnsArray[0].GetComponent<RectTransform>().anchoredPosition.x);
    }

    void Update()
    {
        //figure out distance of each btn to center
        for (int i = 0; i < btnDistanceArray.Length; i++)
        {
            btnDistanceArray[i] = Mathf.Abs(centerTrans.transform.position.x - btnsArray[i].transform.position.x);
        }
        float minDistance = Mathf.Min(btnDistanceArray);

        //assign the closes btn
        for (int a = 0; a < btnsArray.Length; a++)
        {
            if (minDistance == btnDistanceArray[a])
            {
                minBtnNum = a;
            }
        }

        if (!isDragging)
        {
            LerpToBtn(minBtnNum * -btnConstDistance);
        }

    }

    void LerpToBtn(int position)
    {
        float newX = Mathf.Lerp(scrollPanelTrans.anchoredPosition.x, position, Time.deltaTime * lerpSpeed);
        Vector2 newPos = new Vector2(newX, scrollPanelTrans.anchoredPosition.y);

        scrollPanelTrans.anchoredPosition = newPos;
    }

    public void StartDragging()
    {
        isDragging = true;
    }

    public void EndDragging()
    {
        isDragging = false;
    }
}
