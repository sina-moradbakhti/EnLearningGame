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

    private void Update()
    {

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 50f;
            Vector3 brushPos = this.transform.position;
            brushPos.z = 50f;

            float distance = Vector2.Distance(brushPos, mousePos);

            if (distance <= 0.5)
            {
                writing.ClearPart(this.gameObject);
            }
        }
#endif

#if (PLATFORM_IOS || PLATFORM_ANDROID)
            if (Input.touchCount > 0)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                mousePos.z = 50f;
                Vector3 brushPos = this.transform.position;
                brushPos.z = 50f;

                float distance = Vector2.Distance(brushPos, mousePos);

                if (distance <= 0.5)
                {
                writing.ClearPart(this.gameObject);
                }
            }
#endif

    }


    private void OnMouseOver()
    {
        writing.ClearPart(this.gameObject);
    }

}
