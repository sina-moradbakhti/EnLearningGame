using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.Android;

public class ReadingSequence : MonoBehaviour
{
    GameManager manager;
    GuideControllerScript GuideControllerScript;
    AnimationsController animations;
    ListeningSequence listening;


    private bool micConnected = false;
    private bool takeMicrophone = false;
    private int minFreq;
    private int maxFreq;
    private AudioSource source;
    private AudioClip clip;

    private void Awake()
    {
        SetInitialReferences();
    }

    void Start()
    {
        source = this.GetComponent<AudioSource>();
        //CheckForMicrophine();
    }

    public void StartSequence()
    {
        string message = "friends!​\nMagic spell\nDid you listen well?";
        GuideControllerScript.StartGuid(message,0.1f, FirstGuide(), true);
    }

    IEnumerator FirstGuide()
    {
        yield return new WaitForSeconds(1f);
        string message = "Magic alphabet\nTo find\nYou have to shout a spell";
        GuideControllerScript.StartGuid(message, 0.1f, ReadyToCountDown(), true);
    }

    IEnumerator ReadyToCountDown()
    {
        yield return new WaitForSeconds(1f);
        string message = "If you do 3,2,1\nOf magic spells\nShout out!​";
        GuideControllerScript.StartGuid(message, 0.1f, CountDown(), true);
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.5f);
        listening.countDownStartNumber = 3;
        takeMicrophone = true;
        listening.CountDownFunction(ListenToPlayer(), true);
    }

    IEnumerator ListenToPlayer()
    {
        //manager.UIElements.MicrophonePanel.gameObject.SetActive(true);
        //animations.Animators.MicAnimator.SetBool("Start", true);
        yield return new WaitForSeconds(0.5f);
        //animations.Animators.MicAnimator.SetBool("Recording", true);
        //animations.Animators.MicAnimator.SetBool("Start", false);
        StartRecording();
    }

    private void StartRecording()
    {
        /*if (micConnected && !Microphone.IsRecording(null))
        {
            clip = Microphone.Start(null, true, 20, maxFreq);
            source.clip = clip;
        }*/

        Invoke("StopRecording", 3f);
    }

    public void StopRecording()
    {
        animations.Animators.LaiAnimator.SetBool("speak", false);
        MusicManager.musicManager.ChangeMusicVolume(0.4f, 0.25f);

        //if (micConnected && Microphone.IsRecording(null))
        //{
            //animations.Animators.MicAnimator.SetBool("Recording", false);
            Microphone.End(null);
            StartCoroutine(AISpellingCheck(clip));
            //source.Play();
        //}
    }

    IEnumerator AISpellingCheck(AudioClip clip)
    {
        float testResult;
        //Do the API job
        testResult = 0.8f/*UnityEngine.Random.Range(0.1f, 1)*/;
        yield return new WaitForSeconds(1f);

        ShowResult(testResult);
    }

    bool showResult = false;
    bool clearResult = false;
    float result;

    private void Update()
    {
        if (showResult)
        {
            float fillAmount = manager.UIElements.ResultImage.fillAmount;
            if(fillAmount < result)
            {
                manager.UIElements.ResultImage.fillAmount += 0.5f * Time.deltaTime;
            }
            else
            {
                if (result >= 1f) SpellingSuccess();
                else SpellingFailed();
                showResult = false;
            }
        }

        if (clearResult)
        {
            float fillAmount = manager.UIElements.ResultImage.fillAmount;
            if (fillAmount > 0)
            {
                manager.UIElements.ResultImage.fillAmount -= 0.5f * Time.deltaTime;
            }
            else
            {
                clearResult = false;
            }
        }

        if (takeMicrophone)
        {
            if (listening.countDownStartNumber == 0)
            {
                animations.Animators.LaiAnimator.SetBool("speak", true);
                takeMicrophone = false;
            }
        }
    }

    public void ShowResult(float result)
    {
        if(result >= 0.5)
        {
            this.result = 1;
        }
        else
        {
            this.result = 0.7f;
        }
        showResult = true;
    }

    private void SpellingSuccess()
    {
        animations.Animators.LaiAnimator.SetBool("cheer", true);
        animations.Animators.BekiAnimator.SetBool("cheer", true);
        manager.UIElements.ClearPanel.gameObject.SetActive(true);
        this.GetComponent<AudioSource>().clip = manager.ClearSound;
        this.GetComponent<AudioSource>().loop = false;
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(MoveToWritingSequence());
    }

    IEnumerator MoveToWritingSequence()
    {
        clearResult = true;
        yield return new WaitForSeconds(2.5f);
        manager.UIElements.ClearPanel.gameObject.SetActive(false);
        WritingSequence writingSequence = this.GetComponent<WritingSequence>();
        writingSequence.StartSequence();
        manager.UIElements.ResultImage.gameObject.SetActive(false);
        animations.Animators.LaiAnimator.SetBool("cheer", false);
        animations.Animators.BekiAnimator.SetBool("cheer", false);
    }

    private void SpellingFailed()
    {
        manager.UIElements.MicrophonePanel.gameObject.SetActive(false);
        string message = "Don't be disappointed\nLet's start again!​\nGo with Rai~";
        GuideControllerScript.StartGuid(message, 0.1f, ListenAgain(), true);
    }

    private IEnumerator ListenAgain()
    {
        clearResult = true;
        yield return new WaitForSeconds(2f);
        ListeningSequence firstSeq = this.GetComponent<ListeningSequence>();
        firstSeq.StartSequence();
    }

    private void CheckForMicrophine()
    {
        if (Microphone.devices.Length > 0)
        {
            micConnected = true;
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

            if (minFreq == 0 && maxFreq == 0)
            {
                maxFreq = 44100;
            }

        }
        else
        {
            print("No microphone detected!");
        }
    }

    private void SetInitialReferences()
    {
        manager = this.GetComponent<GameManager>();
        GuideControllerScript = this.GetComponent<GuideControllerScript>();
        animations = this.GetComponent<AnimationsController>();
        listening = this.GetComponent<ListeningSequence>();
    }
}
