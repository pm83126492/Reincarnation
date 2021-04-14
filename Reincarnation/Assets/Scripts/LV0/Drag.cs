using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    protected float deltaX, deltaY;
    Vector3 LastTouchPos;

    protected bool moveAllowed = false;
    protected bool thisColTouched = false;
    protected bool isMoving;
    protected bool isFinish;
    bool CanPlayAudio;

    protected Rigidbody2D rb;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        Physics2D.IgnoreLayerCollision(9, 5);
        Physics2D.IgnoreLayerCollision(8, 5);
    }

    protected virtual void Update()
    {
        if (isMoving&&!isFinish)
        {
            if (moveAllowed && thisColTouched)
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(mousePos);
                rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                if (LastTouchPos != touchPos)
                {
                    CanPlayAudio = true;
                }
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
            LastTouchPos = touchPos;
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
            CanPlayAudio = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wood"))
        {
            if (thisColTouched&& CanPlayAudio)
            {
                AudioManager.Instance.PlaySource("Wood","0");
            }
        }
    }
}
