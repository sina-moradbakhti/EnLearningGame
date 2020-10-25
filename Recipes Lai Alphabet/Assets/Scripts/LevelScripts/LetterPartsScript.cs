using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPartsScript : MonoBehaviour
{
    WritingSequence writing;

    private void Awake()
    {
        writing = GameObject.FindObjectOfType<WritingSequence>();
    }
    
    private void OnMouseOver()
    {
        writing.ClearPart(this.gameObject);
    }

}
