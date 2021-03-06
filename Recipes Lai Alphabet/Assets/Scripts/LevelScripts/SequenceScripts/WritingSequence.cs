﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WritingSequence : MonoBehaviour
{
    private GuideControllerScript GuideController;
    private GameManager manager;
    private AnimationsController animations;

    public char LetterToDraw;
    public GameObject PlayerBrush;
    public DrawGuid[] drawGuids;
    public Sprite starCompleteSprite;
    public AudioClip dingSound;

    private bool PlayerCanDraw = false;
    private bool clear = false;

    GameObject PartToClear;
    public DrawGuid currentGuid { set; get; }
    GameObject startPart;
    GameObject endPart;
    int guideToActivate = 0;
    int partToClearIndex;

    private void Start()
    {
        SetInitialReferences();
    }

    private void Update()
    {
        if (clear)
        {
            ShowWinPanel();
        }
    }

    public void StartSequence()
    {
        string message = "마법의 붓을\n불려봐요!";
        GuideController.StartGuid(message,0.1f, ShowBrush(), true);
    }

    IEnumerator ShowBrush()
    {
        yield return new WaitForSeconds(1f);
        manager.UIElements.Brush.gameObject.SetActive(true);
        animations.Animators.BrushAnimator.Play("BrushAppear");
    }

    public void DrawOrderMessage()
    {
        string message = "마법의 붓이\n그리는 그림을\n잘 보세요";
        GuideController.StartGuid(message, 0.1f, DrawTheLetter(), true);
    }

    IEnumerator DrawTheLetter()
    {
        yield return new WaitForSeconds(0.5f);
        animations.Animators.BrushAnimator.Play("Brush_"+ LetterToDraw);
    }

    public void PlayerDrawTime()
    {
        string message = "3,2,1 하면\n마법의 붓으로\n그려봐요";
        GuideController.StartGuid(message, 0.1f, CountDown(), true);
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.1f);
        ListeningSequence listening = this.GetComponent<ListeningSequence>();
        listening.countDownStartNumber = 3;
        listening.CountDownFunction(ActivateDrawingBrush(), true);
    }

    IEnumerator ActivateDrawingBrush()
    {
        MusicManager.musicManager.ChangeMusicVolume(0.32f, 0.25f);
        animations.Animators.LaiAnimator.SetBool("cheer", true);
        animations.Animators.BekiAnimator.SetBool("cheer", true);

        foreach (GameObject part in manager.LetterParts)
        {
            part.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        partToClearIndex = manager.LetterParts.Length - 1;
        PartToClear = manager.LetterParts[partToClearIndex];
        ActivateGuide(guideToActivate);

        PlayerBrush.SetActive(true);
        PlayerBrush.transform.position = PartToClear.transform.position;
        PlayerCanDraw = true;
    }

    public GameObject guidePointToClear;
    int guidePointIndex = 0;
    private void ActivateGuide(int guideIndex)
    {
        currentGuid = drawGuids[guideIndex];
        currentGuid.GuideObject.SetActive(true);

        foreach (DrawGuid drawGuid in drawGuids)
        {
            if(drawGuid.GuideObject != currentGuid.GuideObject)
            {
                drawGuid.GuideObject.SetActive(false);
            }
        }

        if(currentGuid.GuideStars.Length > 1)
        {
            currentGuid.GuideStars[1].GetComponent<Image>().sprite = starCompleteSprite;
        }
        else
            currentGuid.GuideStars[0].GetComponent<Image>().sprite = starCompleteSprite;

        if(currentGuid.GuidePoints.Length > 0)
        {
            guidePointIndex = 0;
            guidePointToClear = currentGuid.GuidePoints[guidePointIndex];
        }
        else guidePointIndex = -1;


        if (currentGuid.GuideParts.Length > 1)
        {
            startPart = currentGuid.GuideParts[0];
        }
        else
        {
            startPart = null;
        }
        endPart = currentGuid.GuideParts[currentGuid.GuideParts.Length - 1];
    }

    public void MoveOnGuidePoints()
    {
        if(guidePointIndex >= 0)
        {
            guidePointIndex++;
            if (guidePointIndex < currentGuid.GuidePoints.Length)
                guidePointToClear = currentGuid.GuidePoints[guidePointIndex];
        }
    }

    public void ClearPart(GameObject part)
    {
        if(part == PartToClear)
        {
            PlayerBrush playerBrush = GameObject.FindObjectOfType<PlayerBrush>();

            if (PlayerCanDraw && playerBrush.BrushSelected)
            {
                PartToClear.SetActive(false);

                if(PartToClear != endPart)
                {
                    if(startPart != null)
                    {
                        if (PartToClear == startPart)
                        {
                            GameObject star = currentGuid.GuideStars[0];
                            star.GetComponent<Animator>().Play("Star_Disappear");
                            //star.GetComponent<Image>().sprite = starCompleteSprite;
                            //star.SetActive(false);
                            star.AddComponent<AudioSource>();
                            star.GetComponent<AudioSource>().loop = false;
                            star.GetComponent<AudioSource>().clip = dingSound;
                            star.GetComponent<AudioSource>().Play();
                        }
                    }

                    if (partToClearIndex > -1)
                    {
                        partToClearIndex -= 1;
                        if (partToClearIndex >= 0)
                            PartToClear = manager.LetterParts[partToClearIndex];
                    }
                }
                else
                {
                    GameObject star;
                    if (currentGuid.GuideStars.Length > 1)
                        star = currentGuid.GuideStars[1];
                    else
                        star = currentGuid.GuideStars[0];

                    star.GetComponent<Animator>().Play("Star_Disappear");
                    //star.GetComponent<Image>().sprite = starCompleteSprite;
                    star.AddComponent<AudioSource>();
                    star.GetComponent<AudioSource>().loop = false;
                    star.GetComponent<AudioSource>().clip = dingSound;
                    star.GetComponent<AudioSource>().Play();
                    Invoke("ChangeGuide", 0.75f);
                }
            }
        }
    }

    private void ChangeGuide()
    {
        guideToActivate += 1;

        if (guideToActivate < drawGuids.Length)
            ActivateGuide(guideToActivate);
        else
        {
            currentGuid.GuideObject.SetActive(false);
            PlayerBrush.SetActive(false);
            clear = true;
        }

        if (partToClearIndex > -1)
        {
            partToClearIndex -= 1;
            if (partToClearIndex >= 0)
                PartToClear = manager.LetterParts[partToClearIndex];
        }
    }

    private void ShowWinPanel()
    {
        manager.UIElements.WinPanel.gameObject.SetActive(true);

        this.GetComponent<AudioSource>().clip = manager.ClearSound;
        this.GetComponent<AudioSource>().loop = false;
        this.GetComponent<AudioSource>().Play();
        UpdatePrefs();
        clear = false;
    }

    private void UpdatePrefs()
    {
        string levelLetter = LetterToDraw.ToString();

        if(levelLetter == levelLetter.ToUpper())
        {
            int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex");
            int levelBuildIndex = SceneManager.GetActiveScene().buildIndex - 26;

            int newLevelIndex = currentLevelIndex + 1;

            if (levelBuildIndex == newLevelIndex && newLevelIndex < 27)
            {
                PlayerPrefs.SetString("SmallLevelToActive", levelLetter + "_small");
            }
            Invoke("MoveToSmallLetter", 1f);
        }
        else if (levelLetter == levelLetter.ToLower())
        {
            PlayerPrefs.DeleteKey("SmallLevelToActive");
            int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex");
            int levelBuildIndex = SceneManager.GetActiveScene().buildIndex;

            int newLevelIndex = currentLevelIndex + 1;

            if(levelBuildIndex == newLevelIndex && newLevelIndex < 27)
            {
                PlayerPrefs.SetInt("CurrentLevelIndex",newLevelIndex);
            }

            Invoke("MoveToMainMenu", 1f);
        }
    }
    
    private void MoveToSmallLetter()
    {
        manager.UIElements.WinPanel.gameObject.SetActive(false);
        ScenesManager scenesManager = GameObject.FindObjectOfType<ScenesManager>();
        scenesManager.GoToSmallLetterLevel();
    }

    private void MoveToMainMenu()
    {
        manager.UIElements.WinPanel.gameObject.SetActive(false);
        ScenesManager scenesManager = GameObject.FindObjectOfType<ScenesManager>();
        scenesManager.LoadLevelByName("MainMenu");
    }

    private void SetInitialReferences()
    {
        SetGuidStarsAndPoints();
        GuideController = this.GetComponent<GuideControllerScript>();
        manager = this.GetComponent<GameManager>();
        animations = this.GetComponent<AnimationsController>();
    }

    private void SetGuidStarsAndPoints()
    {
        foreach (DrawGuid guid in drawGuids)
        {
            Animator[] stars = guid.GuideObject.GetComponentsInChildren<Animator>();
            guid.GuideStars = new GameObject[stars.Length];
            for (int i = 0; i < stars.Length; i++)
            {
                guid.GuideStars[i] = stars[i].gameObject;
            }

            GuidePointsScript[] guidePoints = guid.GuideObject.GetComponentsInChildren<GuidePointsScript>();
            guid.GuidePoints = new GameObject[guidePoints.Length];
            if(guidePoints.Length > 0)
            {
                for (int i = 0; i < guidePoints.Length; i++)
                {
                    guid.GuidePoints[i] = guidePoints[i].gameObject;
                }
            }
        }
    }
}

[System.Serializable]
public class DrawGuid
{
    public GameObject GuideObject;
    public GameObject[] GuideParts;
    public GameObject[] GuideStars { get; set; }
    public GameObject[] GuidePoints { get; set; }
}
