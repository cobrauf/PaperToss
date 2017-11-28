using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjSwitcher : MonoBehaviour {

    private int throwObjNum = 0;
    private int totalNumObjs;

	// Use this for initialization
	void Start () {
        InitialSettings();
        totalNumObjs = SwipeControl.instance.throwObjArray.Length;
	}

    void InitialSettings ()
    {
        SwipeControl.instance.paperBall = SwipeControl.instance.throwObjArray[0];
        SwipeControl.instance.miniPaperBall = SwipeControl.instance.miniThrowObjArray[0];
        SwipeControl.instance.miniPaperBall.SetActive(true);
        PSManager.instance.miniFire = PSManager.instance.miniFireArray[0];
        PSManager.instance.miniFireChild1 = PSManager.instance.miniFireChild1Array[0];
        PSManager.instance.miniFireChild2 = PSManager.instance.miniFireChild2Array[0];
    }
	
	

    public void NextThrowObj ()
    {
        //unlocks throw objects according to  unlockable reached
        int unlockableReached = PlayerPrefsManager.GetUnlockablesReached();
        int numObjsAvailable = 0;
        
        switch (unlockableReached)
        {
            case 0:
                numObjsAvailable = 1;
                break;
            case 1:
                numObjsAvailable = 2;
                break;
            case 2:
                numObjsAvailable = 2;
                break;
            case 3:
                numObjsAvailable = 3;
                break;
            case 4:
                numObjsAvailable = 3;
                break;
            default:
                numObjsAvailable = 1;
                break;
        }

        //cycles through unlocked throwable objects
        throwObjNum++;
        if (throwObjNum >= numObjsAvailable)
        {
            throwObjNum = 0;
        }
        SwipeControl.instance.paperBall = SwipeControl.instance.throwObjArray[throwObjNum];

        //activate corresponding mini ball
        SwipeControl.instance.miniPaperBall.SetActive(false);
        SwipeControl.instance.miniPaperBall = SwipeControl.instance.miniThrowObjArray[throwObjNum];
        SwipeControl.instance.miniPaperBall.SetActive(true);

        //assign corresponding mini fire
        PSManager.instance.miniFire = PSManager.instance.miniFireArray[throwObjNum];
        PSManager.instance.miniFireChild1 = PSManager.instance.miniFireChild1Array[throwObjNum];
        PSManager.instance.miniFireChild2 = PSManager.instance.miniFireChild2Array[throwObjNum];

        SoundManager.instance.PlayButtonSFX();

    }  
}
