using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class ListeningSequence : MonoBehaviour
{
    private GameManager manager;
    private AnimationsController animations;
    private GuideControllerScript GuideController;
    private AudioSource audioSource;

    public AudioClip PronunciationAudio;
    public AudioClip CountdownAudio;
    public int RepeatCount = 3;
    public float RepeatDelay = 1f;
    public float CountDownDelay = 1f;
    public GameObject[] CountDownObjects;
    public Sprite SpeakerOnSprite;
    public Sprite SpeakerOffSprite;

    public int countDownStartNumber { get; set; }

    private string sequenceGuideStrings = "1,2,3 하면\n마법의 주문이\n들려요!";
    private bool takeSpeaker = false;
    private int repeatCount;


    private void Awake()
    {
        SetInitialReferences();
    }

    private void Update()
    {
        if (takeSpeaker)
        {
            if(countDownStartNumber == 0)
            {
                animations.Animators.LaiAnimator.SetBool("listen", true);
                takeSpeaker = false;
            }
        }
    }

    public void StartSequence()
    {
        this.GetComponent<AudioSource>().mute = false;
        GuideController.StartGuid(sequenceGuideStrings , 0.1f,SpellTheLetter(),true);
    }

    IEnumerator SpellTheLetter()
    {
        countDownStartNumber = 3;
        yield return new WaitForSeconds(1f);
        MusicManager.musicManager.ChangeMusicVolume(0f, 0.25f);
        audioSource.clip = CountdownAudio;
        audioSource.Play();
        takeSpeaker = true;
        StartCoroutine(CountDown(PronounceAlphabet()));
    }

    public void CountDownFunction(IEnumerator enumerator , bool turnMusicDown)
    {
        if(turnMusicDown)
            MusicManager.musicManager.ChangeMusicVolume(0f, 0.25f);

        audioSource.clip = CountdownAudio;
        audioSource.Play();
        StartCoroutine(CountDown(enumerator));
    }

    public IEnumerator CountDown(IEnumerator enumerator)
    {
        for (int i = 0; i < CountDownObjects.Length; i++)
        {
            if(i == countDownStartNumber - 1)
            {
                CountDownObjects[i].SetActive(true);
                animations.Animators.CountDownAnimator.Play("CountDown_"+countDownStartNumber+"_popUp");
            }
            else
            {
                CountDownObjects[i].SetActive(false);
            }
        }
        countDownStartNumber -= 1;

        yield return new WaitForSeconds(CountDownDelay);

        if (countDownStartNumber > 0)
            StartCoroutine(CountDown(enumerator));
        else
        {
            foreach(GameObject obj in CountDownObjects)
            {
                obj.SetActive(false);
            }
            repeatCount = RepeatCount;
            yield return new WaitForSeconds(1f);
            StartCoroutine(enumerator);
        }
    }

    IEnumerator PronounceAlphabet()
    {
        animations.Animators.SpeakerAnimator.SetTrigger("OpenSpeaker");
        audioSource.clip = PronunciationAudio;
        audioSource.Play();
        repeatCount -= 1;

        if (repeatCount > 0)
        {
            yield return new WaitForSeconds(RepeatDelay);
            StartCoroutine(PronounceAlphabet());
        }
        else
        {
            Invoke("StopSequence",1f);
        }
    }

    private void StopSequence()
    {
        animations.Animators.LaiAnimator.SetBool("listen", false);
        //DeactivateSpeaker();
        GuideController.Reset();
        MusicManager.musicManager.ChangeMusicVolume(0.32f, 0.25f);
        Invoke("MoveOnSecondSequence", 1f);
    }

    private void MoveOnSecondSequence()
    {
        GuideController.Reset();
        ReadingSequence secondSeq = this.GetComponent<ReadingSequence>();
        secondSeq.StartSequence();
    }

    private void ActivateSpeaker()
    {
        animations.Animators.SpeakerCloudAnimation.Play("SpeakerActivateAnim");
    }

    private void DeactivateSpeaker()
    {
        animations.Animators.SpeakerCloudAnimation.Play("SpeakerDeactivateAnim");
    }

    private void SetInitialReferences()
    {
        manager = this.GetComponent<GameManager>();
        animations = this.GetComponent<AnimationsController>();
        GuideController = GameObject.FindObjectOfType<GuideControllerScript>();
        audioSource = this.GetComponent<AudioSource>();
        MusicManager.musicManager = GameObject.FindObjectOfType<MusicManager>();
    }

}
