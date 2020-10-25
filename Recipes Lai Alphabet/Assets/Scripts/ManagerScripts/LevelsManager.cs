using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{

    public Button[] alphabetsLevels;
    public AudioClip menuMusic;

    void Start()
    {
        SetMenuMusic();
        int currentLevelIndex;
        if (PlayerPrefs.HasKey("CurrentLevelIndex"))
        {
            currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex");
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevelIndex" , 1);
            currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex");
        }

        UnlockLevels(currentLevelIndex);
    }

    private void UnlockLevels(int currentLevelIndex)
    {
        for (int i = 0; i < alphabetsLevels.Length; i++)
        {
            if ((i + 1) <= currentLevelIndex) 
                alphabetsLevels[i].interactable = true;
            else    
                alphabetsLevels[i].interactable = false;
        }
    }

    private void SetMenuMusic()
    {
        MusicManager.musicManager.music = menuMusic;
        MusicManager.musicManager.PlayMusic();
        MusicManager.musicManager.ChangeMusicVolume(0.4f, 0.75f);
    }

}
