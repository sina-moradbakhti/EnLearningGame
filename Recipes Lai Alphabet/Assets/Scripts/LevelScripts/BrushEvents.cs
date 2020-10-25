using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushEvents : MonoBehaviour
{

    WritingSequence WritingSequence;
    GameManager manager;

    private int partIndex;

    private void Awake()
    {
        WritingSequence = GameObject.FindObjectOfType<WritingSequence>();
        manager = GameObject.FindObjectOfType<GameManager>();
        partIndex = manager.LetterParts.Length - 1;
    }

    public void MessageAfterPopupEvent()
    {
        WritingSequence.DrawOrderMessage();
    }

    public void DisAppear()
    {
        this.GetComponent<Animator>().Play("BrushDisappear");
        WritingSequence.PlayerDrawTime();
    }
    
    public void ClearPart()
    {
        manager.LetterParts[partIndex].SetActive(false);
        partIndex--;
    }

    public void DeActivateBrush()
    {
        this.gameObject.SetActive(false);
    }  
}
