using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{

    public Button[] alphabetsLevels;
    public AudioClip menuMusic;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

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
        string LetterToChangeToSmall;
        if (PlayerPrefs.HasKey("SmallLevelToActive"))
        {
            string[] smallLevelToActive = PlayerPrefs.GetString("SmallLevelToActive").Split("_".ToCharArray());
            LetterToChangeToSmall = smallLevelToActive[0];

            foreach (Button levelBtn in alphabetsLevels)
            {
                if (levelBtn.name == LetterToChangeToSmall)
                {
                    levelBtn.image.sprite = Resources.Load<Sprite>("SmallLettersButtonIcons/" + LetterToChangeToSmall.ToLower());

                    SpriteState spriteState = new SpriteState();
                    spriteState.pressedSprite = Resources.Load<Sprite>("SmallLettersButtonIcons/clicked/" + LetterToChangeToSmall.ToLower());
                    levelBtn.spriteState = spriteState;
                }
            }
        }

        for (int i = 0; i < alphabetsLevels.Length; i++)
        {
            if ((i + 1) <= currentLevelIndex)
            {
                alphabetsLevels[i].interactable = true;
            }
            else
            {
                alphabetsLevels[i].interactable = false;
            }
        }
    }

    private void SetMenuMusic()
    {
        MusicManager.musicManager.music = menuMusic;
        MusicManager.musicManager.PlayMusic();
        MusicManager.musicManager.ChangeMusicVolume(0.4f, 0.75f);
    }

}
