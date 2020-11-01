using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Diagnostics.Eventing.Reader;

[System.Serializable]
public struct UIElements{
    public Image CharacterSpeechCloud;
    public TMP_Text CharacterSpeechText;
    public Button SpeakerButton;
    public Image Brush;
    public RectTransform ClearPanel;
    public RectTransform WinPanel;
    public RectTransform MicrophonePanel;
    public Image ResultImage;
}

[System.Serializable]
public struct GameSetting
{
    public bool CanSkip;
    public float startDelay { get; set; }
    public AudioClip levelMusic;
}

public class GameManager : MonoBehaviour
{
    public UIElements UIElements;
    public GameSetting gameSetting;
    public GameObject[] LetterParts;
    public AudioClip ClearSound;

    private GuideControllerScript GuideController;

    private string BeginningGuideString = "안녕 친구들!​\n마법 요리사 라이야\n친구들의 도움이\n필요해";

    private List<IEnumerator> enumerators = new List<IEnumerator>();
    private IEnumerator currentEnumerator;

    private void Awake()
    {
        enumerators.Add(StartGame());
        enumerators.Add(MoveOnFirstSequence());

        gameSetting.startDelay = 0.5f;
        SetInitialReferences();
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        SkipSection();
    }

    IEnumerator StartGame()
    {
        currentEnumerator = StartGame();
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
        currentEnumerator = MoveOnFirstSequence();
        yield return new WaitForSeconds(1f);
        ListeningSequence firstSeq = this.GetComponent<ListeningSequence>();
        firstSeq.StartSequence();
    }

    void SkipSection()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (gameSetting.CanSkip)
            {
                GuideController.Skip();
            }
        }
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
