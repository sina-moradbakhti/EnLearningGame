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
        MusicManager.musicManager.ChangeMusicVolume(0f,0.75f);
        StartCoroutine(PlayTransition(name));
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
