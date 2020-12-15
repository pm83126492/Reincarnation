using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag2 : Drag
{
    public GameObject MirrorPosition;
    Vector3 RetPosition;

    public static int MirrorCrackNumber;

    protected override void Awake()
    {
        base.Awake();
        RetPosition = transform.localPosition;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.touchCount > 0)
        {
           /* Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Ended:
                    Debug.Log(Mathf.Abs(transform.localPosition.x - MirrorPosition.transform.localPosition.x));
                    Debug.Log(Mathf.Abs(transform.localPosition.y - MirrorPosition.transform.localPosition.y));
                    if (Mathf.Abs(transform.localPosition.x - MirrorPosition.transform.localPosition.x) <= 30f&& 
                        Mathf.Abs(transform.localPosition.y - MirrorPosition.transform.localPosition.y) <= 30f)
                    {
                        transform.localPosition = new Vector3(MirrorPosition.transform.localPosition.x, MirrorPosition.transform.localPosition.y, MirrorPosition.transform.localPosition.z);
                        MirrorCrackNumber += 1;
                    }
                    else
                    {
                        transform.localPosition = RetPosition;
                    }
                    break;
            }*/
        }
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
         Debug.Log(Mathf.Abs(transform.localPosition.x - MirrorPosition.transform.localPosition.x));
         Debug.Log(Mathf.Abs(transform.localPosition.y - MirrorPosition.transform.localPosition.y));
        if (Mathf.Abs(transform.localPosition.x - MirrorPosition.transform.localPosition.x) <= 40f &&
            Mathf.Abs(transform.localPosition.y - MirrorPosition.transform.localPosition.y) <= 40f)
        {
            transform.localPosition = new Vector3(MirrorPosition.transform.localPosition.x, MirrorPosition.transform.localPosition.y+30, MirrorPosition.transform.localPosition.z);
            MirrorCrackNumber += 1;
        }
        else
        {
            transform.localPosition = RetPosition;
        }
    }
}
