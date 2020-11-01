using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrush : MonoBehaviour
{

    public bool BrushSelected = false;

    private WritingSequence writingSequence;
    private bool moveWithMouse = false;

    private void Start()
    {
        writingSequence = GameObject.FindObjectOfType<WritingSequence>();
    }

    private void Update()
    {
        if (moveWithMouse)
        {
            BrushSelected = true;
            MoveWithMouse();
        }
        else
        {
            BrushSelected = false;
            ReturnToFirstPos();
        }

////#if UNITY_EDITOR
//        if (Input.GetMouseButton(0))
//        {
//            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            mousePos.z = 50f;
//            Vector3 brushPos = this.transform.Find("GrabPoint").position;
//            brushPos.z = 50f;

//            float distance = Vector2.Distance(brushPos ,mousePos);

//            //print(distance);

//            if (distance <= 1.6f)
//            {
//                moveWithMouse = true;
//            }
//        }
//        else
//        {
//            moveWithMouse = false;
//        }
////#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            mousePos.z = 50f;
            Vector3 brushPos = this.transform.Find("GrabPoint").position;
            brushPos.z = 50f;

            float distance = Vector2.Distance(brushPos, mousePos);

            print(distance);

            if (distance <= 1.6f)
            {
                moveWithMouse = true;
            }
        }
        else
        {
            moveWithMouse = false;
        }
#endif

    }

    public void MoveWithMouse()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 50f;
        this.transform.position = newPos;
    }

    public void ReturnToFirstPos()
    {
        Vector3 newPos = writingSequence.currentGuid.GuideStars[0].transform.position;
        this.transform.position = newPos;
    }
}
