   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

    public bool playDisappearAnimation;
    public Animator LevelTransitionAnimator;
    public float transitionLength;

    private void Start()
    {
        if (playDisappearAnimation)
        {
            LevelTransitionAnimator.Play("Appear");
        }
    }

    public void LoadLevelByName(string name)
    {
        MusicManager.musicManager.ChangeMusicVolume(0f, 0.75f);

        if (name == "MainMenu")
        {
            PlayerBrush playerBrush = GameObject.FindObjectOfType<PlayerBrush>();
            if(playerBrush != null && playerBrush.gameObject.activeInHierarchy)
            {
                playerBrush.gameObject.SetActive(false);
            }
            StartCoroutine(PlayTransition(name));
        }

        if (SceneManager.GetActiveScene().name ==  "MainMenu")
        {
            string[] levelNames = name.Split("@".ToCharArray());
            string capitalLevelName = levelNames[0];
            string smallLevelName = levelNames[1];

            if (PlayerPrefs.HasKey("SmallLevelToActive"))
            {
                if (smallLevelName == PlayerPrefs.GetString("SmallLevelToActive"))
                {
                    StartCoroutine(PlayTransition(smallLevelName));
                }
                else
                {
                    StartCoroutine(PlayTransition(capitalLevelName));
                }
            }
            else
            {
                StartCoroutine(PlayTransition(capitalLevelName));
            }
        }
    }

    public void ReloadScene()
    {
        string levelName = SceneManager.GetActiveScene().name;
        StartCoroutine(PlayTransition(levelName));
    }

    public void GoToSmallLetterLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        string[] splitName = levelName.Split("_".ToCharArray());
        string newName = splitName[0] + "_Small";
        StartCoroutine(PlayTransition(newName));
    }

    IEnumerator PlayTransition(string levelName)
    {
        LevelTransitionAnimator.Play("Disappear");
        yield return new WaitForSeconds(transitionLength);
        SceneManager.LoadScene(levelName);
    }
}
