using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Animators
{
    public Animator LaiAnimator;
    public Animator BekiAnimator;
    public Animator SpeakerCloudAnimation;
    public Animator SpeakerAnimator;
    public Animator CountDownAnimator;
    public Animator BrushAnimator;
    public Animator MicAnimator;
}
public class AnimationsController : MonoBehaviour
{
    public Animators Animators;
}
