using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDetection : MonoBehaviour {

    public static bool isDetectingScene { get; private set; }
    private int updateCount;

    void Update()
    {
        updateCount++;
        if (updateCount < 60)
        {
            return;
        }

        updateCount = 0;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            isDetectingScene = true;
        }
        else
        {
            isDetectingScene = false;
        }
    }
}
