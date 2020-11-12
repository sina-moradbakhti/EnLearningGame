using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ReadingSequence : MonoBehaviour
{
    GameManager manager;
    GuideControllerScript GuideControllerScript;
    AnimationsController animations;
    ListeningSequence listening;
    WritingSequence writing;

    private bool micConnected = false;
    private bool takeMicrophone = false;
    private int minFreq;
    private int maxFreq;
    private AudioSource source;
    private AudioClip clip;
    private AudioClip failureSound;

    bool showResult = false;
    bool clearResult = false;
    int tryCount = 3;
    float result;

    private void Awake()
    {
        SetInitialReferences();
    }

    void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (showResult)
        {
            float fillAmount = manager.UIElements.ResultImage.fillAmount;
            if (fillAmount < result)
            {
                manager.UIElements.ResultImage.fillAmount += 1f * Time.deltaTime;
            }
            else
            {
                if (result >= 1f) SpellingSuccess();
                else {
                    SpellingFailed();
                    tryCount -= 1;
                } 
                showResult = false;
            }
        }

        if (clearResult)
        {
            float fillAmount = manager.UIElements.ResultImage.fillAmount;
            if (fillAmount > 0)
            {
                manager.UIElements.ResultImage.fillAmount -= 1f * Time.deltaTime;
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
    public void StartSequence()
    {
        CheckForMicrophine();
        string message = "친구들!​\n마법의 주문을\n잘 들었나요?";
        GuideControllerScript.StartGuid(message,0.1f, FirstGuide(), true);
    }

    IEnumerator FirstGuide()
    {
        yield return new WaitForSeconds(1f);
        string message = "마법의 알파벳을\n찾기 위해서는\n주문을 외쳐야 해요";
        GuideControllerScript.StartGuid(message, 0.1f, ReadyToCountDown(), true);
    }

    IEnumerator ReadyToCountDown()
    {
        yield return new WaitForSeconds(1f);
        string message = "3,2,1 하면\n마법의 주문의\n외쳐봐요!​";
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
        if (micConnected && !Microphone.IsRecording(null))
        {
            clip = Microphone.Start(null, true, 2, maxFreq);
            source.clip = clip;
        }

        Invoke("StopRecording", 2f);
    }

    private void StopRecording()
    {
        animations.Animators.LaiAnimator.SetBool("speak", false);

        if (micConnected && Microphone.IsRecording(null))
        {
            Microphone.End(null);
            SaveWav.Save("SpellingAudio", clip);
            CheckForConnection();
        }
    }

    public void CheckForConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (!manager.UIElements.ConnectionCheckPanel.gameObject.activeInHierarchy)
            {
                manager.UIElements.ConnectionCheckPanel.gameObject.SetActive(true);
            }
        }
        else
        {
            if (manager.UIElements.ConnectionCheckPanel.gameObject.activeInHierarchy)
            {
                manager.UIElements.ConnectionCheckPanel.gameObject.SetActive(false);
            }

            StartCoroutine(AISpellingCheck());
        }
    }

    IEnumerator AISpellingCheck()
    {
        yield return new WaitForSeconds(1f);
        string url = "https://dl1.youtubot.co.kr/studioBong/save.php";
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();

        string path = Path.Combine(Application.persistentDataPath,"SpellingAudio.wav");
        byte[] AudioBytes = File.ReadAllBytes(path);

        form.Add(new MultipartFormDataSection("sbval", writing.LetterToDraw.ToString().ToLower()));
        form.Add(new MultipartFormFileSection("sbfile", AudioBytes, "SpellingAudio.wav", "audio/wav"));

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            //www.timeout = 4;
            yield return www.SendWebRequest();
            if (!www.isNetworkError)
            {
                MusicManager.musicManager.ChangeMusicVolume(0.32f, 0.25f);
                float result = Convert.ToInt32(www.downloadHandler.text);
                ShowResult(result);
            }
            else
            {
                ShowResult(0);
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
        manager.UIElements.WinPanel.gameObject.SetActive(true);
        //animations.Animators.LaiAnimator.SetBool("cheer", true);
        //animations.Animators.BekiAnimator.SetBool("cheer", true);
        //manager.UIElements.ClearPanel.gameObject.SetActive(true);
        this.GetComponent<AudioSource>().clip = manager.ClearSound;
        this.GetComponent<AudioSource>().loop = false;
        this.GetComponent<AudioSource>().volume = 1;
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(MoveToWritingSequence());
    }

    IEnumerator MoveToWritingSequence()
    {
        clearResult = true;
        yield return new WaitForSeconds(2.5f);
        manager.UIElements.WinPanel.gameObject.SetActive(false);
        manager.UIElements.ClearPanel.gameObject.SetActive(false);
        writing.StartSequence();
        manager.UIElements.ResultImage.gameObject.SetActive(false);
        animations.Animators.LaiAnimator.SetBool("cheer", false);
        animations.Animators.BekiAnimator.SetBool("cheer", false);
    }

    private void SpellingFailed()
    {
        MusicManager.musicManager.ChangeMusicVolume(0f, 0.85f);
        this.GetComponent<AudioSource>().clip = failureSound;
        this.GetComponent<AudioSource>().loop = false;
        this.GetComponent<AudioSource>().volume = 1;
        this.GetComponent<AudioSource>().Play();
        manager.UIElements.MicrophonePanel.gameObject.SetActive(false);

        if(tryCount < 0)
        {
            string message = "실망하지 마세요\n다시 시작해 봐요!\n라이와 함께 GO~";
            GuideControllerScript.StartGuid(message, 0.1f, ListenAgain(), true);
        }
        else
        {
            StartCoroutine(ListenAgain());
        }
    }

    private IEnumerator ListenAgain()
    {
        clearResult = true;
        yield return new WaitForSeconds(0.7f);
        this.GetComponent<AudioSource>().clip = null;
        MusicManager.musicManager.ChangeMusicVolume(0.32f, 0.85f);

        if (tryCount < 0)
            StartSequence();
        else writing.StartSequence();
    }

    private void CheckForMicrophine()
    {
        if (Microphone.devices.Length > 0)
        {
            micConnected = true;
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

            if (minFreq == 0 && maxFreq == 0)
            {
                maxFreq = 84100;
            }

        }
        else
        {
            print("No microphone detected!");
        }
    }

    private void SetInitialReferences()
    {
        failureSound = Resources.Load<AudioClip>("Speak_Failure");
        manager = this.GetComponent<GameManager>();
        GuideControllerScript = this.GetComponent<GuideControllerScript>();
        animations = this.GetComponent<AnimationsController>();
        listening = this.GetComponent<ListeningSequence>();
        writing = this.GetComponent<WritingSequence>();
    }
}
