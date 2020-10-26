using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideControllerScript : MonoBehaviour
{
    private bool typeGuide = false;
    private bool reset = true;
    private float typeSpeed;
    private string guideText;
    private int charIndex;
    private float firstSpeed;
    IEnumerator MoveOnEnumerator = null;

    private GameManager manager;
    private AnimationsController animations;

    void Awake()
    {
        SetInitialReferences();
    }

    private void Update()
    {
        TypeGuides();
    }

    public void StartGuid(string guideText, float typeSpeed, IEnumerator MoveOnEnumerator, bool resetGuideAfterType)
    {
        this.guideText = guideText;
        this.typeSpeed = typeSpeed;
        this.MoveOnEnumerator = MoveOnEnumerator;
        reset = resetGuideAfterType;
        firstSpeed = typeSpeed;
        charIndex = 0;
        manager.UIElements.CharacterSpeechCloud.gameObject.SetActive(true);
        animations.Animators.SpeakerCloudAnimation.SetBool("showGuide", true);
        Invoke("activeTypeGuide", 0.5f);
    }

    private void activeTypeGuide()
    {
        typeGuide = true;
        animations.Animators.LaiAnimator.SetBool("explain", true);
        animations.Animators.BekiAnimator.SetBool("explain", true);
    }

    private void TypeGuides()
    {
        if (typeGuide)
        {
            typeSpeed -= Time.deltaTime;
            if (typeSpeed <= 0 && (charIndex < guideText.Length))
            {
                manager.UIElements.CharacterSpeechText.text += guideText[charIndex];
                charIndex += 1;
                typeSpeed = firstSpeed;
            }
            else if (charIndex >= guideText.Length)
            {
                typeGuide = false;
                if (reset)
                {
                    Invoke("Reset", 1f);
                }
                else
                {
                    if (MoveOnEnumerator == null) return;
                    else
                        StartCoroutine(MoveOnEnumerator);
                }
            }
        }
    }

    public void Reset()
    {
        animations.Animators.LaiAnimator.SetBool("explain", false);
        animations.Animators.BekiAnimator.SetBool("explain", false);
        animations.Animators.SpeakerCloudAnimation.SetBool("showGuide", false);
        manager.UIElements.CharacterSpeechText.text = "";
        if (MoveOnEnumerator == null) return;
        else
            StartCoroutine(MoveOnEnumerator);
    }

    private void SetInitialReferences()
    {
        manager = this.GetComponent<GameManager>();
        animations = this.GetComponent<AnimationsController>();
    }

}
