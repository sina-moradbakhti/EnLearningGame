using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct UIElements{
    public Image CharacterSpeechCloud;
    public TMP_Text CharacterSpeechText;
    public Button SpeakerButton;
    public Button ContinueButton;
    public Image Brush;
    public RectTransform ClearPanel;
    public Button ExitButton;
    public Button RetryButton;
    public RectTransform MicrophonePanel;
    public Image ResultImage;
}

[System.Serializable]
public struct GameSetting
{
    public float startDelay;
    public AudioClip levelMusic;
}

public class GameManager : MonoBehaviour
{
    public UIElements UIElements;
    public GameSetting gameSetting;
    public GameObject[] LetterParts;
    public AudioClip ClearSound;
    private string BeginningGuideString = "Hello friends!​\nMagic Chef Laiya needs Help from you!";

    private GuideControllerScript GuideController;

    private void Awake()
    {
        SetInitialReferences();
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(gameSetting.startDelay);
        if (!string.IsNullOrEmpty(BeginningGuideString))
        {
            GuideController.StartGuid(BeginningGuideString, 0.1f, MoveOnFirstSequence(), true);
        }
        else
        {
            ListeningSequence firstSeq = this.GetComponent<ListeningSequence>();
            firstSeq.StartSequence();
        }
    }

    IEnumerator MoveOnFirstSequence(){
        yield return new WaitForSeconds(1f);
        ListeningSequence firstSeq = this.GetComponent<ListeningSequence>();
        firstSeq.StartSequence();
    }

    public void RestartGame()
    {
        ScenesManager scenesManager = GameObject.FindObjectOfType<ScenesManager>();
        scenesManager.LoadLevelByName(SceneManager.GetActiveScene().name);
    }

    private void SetInitialReferences()
    {
        GuideController = GameObject.FindObjectOfType<GuideControllerScript>();

        if (gameSetting.levelMusic != null && MusicManager.musicManager.music != gameSetting.levelMusic)
        {
            MusicManager.musicManager.music = gameSetting.levelMusic;
            MusicManager.musicManager.PlayMusic();
        }
        MusicManager.musicManager.ChangeMusicVolume(0.4f, 0.75f);
    }

}
