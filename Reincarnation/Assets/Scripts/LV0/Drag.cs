using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    protected float deltaX, deltaY;
    protected bool moveAllowed = false;
    protected bool thisColTouched = false;
    protected Rigidbody2D rb;

    protected bool isMoving;

    protected bool isFinish;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    protected virtual void Update()
    {
        Physics2D.IgnoreLayerCollision(9, 5);
        Physics2D.IgnoreLayerCollision(8, 5);
      
        if (isMoving&&!isFinish)
        {
            if (moveAllowed && thisColTouched)
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(mousePos);
                rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isFinish)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(mousePos);

            if (GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                this.transform.SetAsLastSibling();
                thisColTouched = true;
                moveAllowed = true;
                rb.isKinematic = false;
                deltaX = touchPos.x - transform.position.x;
                deltaY = touchPos.y - transform.position.y;
                isMoving = true;
            }
        }
    }

    protected virtual void OnMouseUp()
    {
        if (!isFinish)
        {
            moveAllowed = true;
            thisColTouched = false;
            rb.isKinematic = true;
            isMoving = false;
        }
    }
}
