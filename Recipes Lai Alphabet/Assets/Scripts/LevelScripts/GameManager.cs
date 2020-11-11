using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if PLATFORM_IOS
using UnityEngine.iOS;
#endif

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

[System.Serializable]
public struct UIElements{
    public Image CharacterSpeechCloud;
    public TMP_Text CharacterSpeechText;
    public Button SpeakerButton;
    public Image Brush;
    public RectTransform ClearPanel;
    public RectTransform WinPanel;
    public RectTransform MicrophonePanel;
    public RectTransform ConnectionCheckPanel;
    public Image ResultImage;
}

[System.Serializable]
public struct GameSetting
{
    public bool CanSkip;
    public float startDelay { get; set; }
    public AudioClip levelMusic { get; set; }
}

public class GameManager : MonoBehaviour
{
    public UIElements UIElements;
    public GameSetting gameSetting;
    public GameObject[] LetterParts;
    public AudioClip ClearSound;

    private GuideControllerScript GuideController;

    private string BeginningGuideString = "안녕 친구들!​\n마법 요리사 라이야\n친구들의 도움이\n필요해";

    private void Awake()
    {
        CheckPermissions();
        SetInitialReferences();
    }

    void CheckPermissions()
    {
#if PLATFORM_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Application.RequestUserAuthorization(UserAuthorization.Microphone);
        }
#endif

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
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

    void SkipSection()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            if (gameSetting.CanSkip)
            {
                GuideController.Skip();
            }
        }
    }

    private void SetInitialReferences()
    {
        gameSetting.levelMusic = Resources.Load<AudioClip>("Level_BackgroundMusic");
        gameSetting.startDelay = 0.5f;

        GuideController = GameObject.FindObjectOfType<GuideControllerScript>();

        Button exitBtn = GameObject.Find("Exit_Btn").GetComponent<Button>();
        exitBtn.onClick.AddListener(() => {
            ScenesManager scenesManager = GameObject.FindObjectOfType<ScenesManager>();
            scenesManager.LoadLevelByName("MainMenu");
        });

        if (gameSetting.levelMusic != null && MusicManager.musicManager.music != gameSetting.levelMusic)
        {
            MusicManager.musicManager.music = gameSetting.levelMusic;
            MusicManager.musicManager.PlayMusic();
        }
        MusicManager.musicManager.ChangeMusicVolume(0.32f, 0.75f);
    }

}
