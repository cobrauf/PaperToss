using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{

    public static void SetEasyBestScore(int score)
    {
        PlayerPrefs.SetInt("easy_best_score", score);
        UpdateUnlockable();
    }

    public static int GetEasyBestScore()
    {
        return PlayerPrefs.GetInt("easy_best_score");
    }

    public static void SetHardBestScore(int score)
    {
        PlayerPrefs.SetInt("hard_best_score", score);
        UpdateUnlockable();
    }

    public static int GetHardBestScore()
    {
        return PlayerPrefs.GetInt("hard_best_score");
    }

    public static int GetUnlockablesReached()//return the # of unlockable that has been unlocked
    {
        return PlayerPrefs.GetInt("hard_unlockable_num");
    }

    public static void UpdateUnlockable()//check if high score is enough to unlock items
    {
        int hardHighScore = PlayerPrefs.GetInt("hard_best_score");

        if (hardHighScore >= 30)
        {
            PlayerPrefs.SetInt("hard_unlockable_num", 4);
        }
        else if (hardHighScore >= 25)
        {
            PlayerPrefs.SetInt("hard_unlockable_num", 3);
        }
        else if (hardHighScore >= 15)
        {
            PlayerPrefs.SetInt("hard_unlockable_num", 2);
        }
        else if (hardHighScore >= 10)
        {
            PlayerPrefs.SetInt("hard_unlockable_num", 1);
        }
        else
        {
            PlayerPrefs.SetInt("hard_unlockable_num", 0);
        }
    }

    public static int[] CheckIfUnlockPlayed()
    {
        int[] items = new int[5];

        items[0] = 0;//ignore
        items[1] = PlayerPrefs.GetInt("unlockable_1_played");//0 = not played, 1=played
        items[2] = PlayerPrefs.GetInt("unlockable_2_played");
        items[3] = PlayerPrefs.GetInt("unlockable_3_played");
        items[4] = PlayerPrefs.GetInt("unlockable_4_played");

        return items;
    }

    public static void SetUnlockablePlayed(int num)
    {
        //set animations played this way in case player unlocks multiple items
        switch (num)
        {
            case 1:
                PlayerPrefs.SetInt("unlockable_1_played", 1);
                break;
            case 2:
                PlayerPrefs.SetInt("unlockable_2_played", 1);
                PlayerPrefs.SetInt("unlockable_1_played", 1);
                break;
            case 3:
                PlayerPrefs.SetInt("unlockable_3_played", 1);
                PlayerPrefs.SetInt("unlockable_2_played", 1);
                PlayerPrefs.SetInt("unlockable_1_played", 1);
                break;
            case 4:
                PlayerPrefs.SetInt("unlockable_4_played", 1);
                PlayerPrefs.SetInt("unlockable_3_played", 1);
                PlayerPrefs.SetInt("unlockable_2_played", 1);
                PlayerPrefs.SetInt("unlockable_1_played", 1);
                break;
            default:
                break;
        }
    }

    public static void ResetUnlockablePlayed()//for testing purposes
    {
        PlayerPrefs.SetInt("unlockable_4_played", 0);
        PlayerPrefs.SetInt("unlockable_3_played", 0);
        PlayerPrefs.SetInt("unlockable_2_played", 0);
        PlayerPrefs.SetInt("unlockable_1_played", 0);
    }

    #region Examples-------------------------------
    const string MASTER_VOLUME_KEY = "master_volume";

    const string LEVEL_KEY = "level_unlocked_";

    public static void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    public static void UnlockLevel(int level)
    {
        PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(), 1);//using 1 as true		
    }

    public static bool IsLevelUnlocked(int level)
    {

        int boolInt = PlayerPrefs.GetInt(LEVEL_KEY + level.ToString());//return either 0 or 1		

        if (boolInt == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

}

