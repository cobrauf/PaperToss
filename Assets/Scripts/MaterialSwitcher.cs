using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{

    public Material[] trashCanMats;
    public GameObject trashCan;

    private int currentMatCount, totalMatCount;

    void Start()
    {
        totalMatCount = trashCanMats.Length;
    }   

    public void MatNext()
    {
        int skinNumReached = CheckBinSkinReached();

        currentMatCount++;
        if (currentMatCount >= skinNumReached)
        {
            currentMatCount = 0;
        }
        trashCan.GetComponent<Renderer>().material = trashCanMats[currentMatCount];
        SoundManager.instance.PlayButtonSFX();
    }

    public void MatPrev()
    {
        int skinNumReached = CheckBinSkinReached();

        currentMatCount--;
        if (currentMatCount < 0)
        {
            currentMatCount = skinNumReached - 1;
        }
        trashCan.GetComponent<Renderer>().material = trashCanMats[currentMatCount];
        SoundManager.instance.PlayButtonSFX();
    }
    

    int CheckBinSkinReached()
    {
        int unlockableReached = PlayerPrefsManager.GetUnlockablesReached();

        switch (unlockableReached)
        {
            case 0:
                return 2;//returns # of skins unlocked; default is 2
            case 1:
                return 2;
            case 2:
                return 6;
            case 3:
                return 6;
            case 4:
                return 10;
            default:
                return 2;
        }
    }

}
