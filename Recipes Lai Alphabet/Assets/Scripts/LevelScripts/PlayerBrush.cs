using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrush : MonoBehaviour
{

    public bool BrushSelected = false;

    private Vector3 firstLocation;
    private bool moveWithMouse = false;

    private void Start()
    {
        firstLocation = this.transform.position;
    }


    private void OnMouseDown()
    {
        moveWithMouse = true;
    }

    private void OnMouseUp()
    {
        moveWithMouse = false;
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
    }

    public void MoveWithMouse()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        this.transform.position = newPos;
    }

    public void ReturnToFirstPos()
    {
        this.transform.position = firstLocation;
    }
}
