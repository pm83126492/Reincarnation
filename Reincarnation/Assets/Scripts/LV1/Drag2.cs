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
    }

    protected override void OnMouseUp()
    {
        if (!isFinish)
        {
            base.OnMouseUp();
            //Debug.Log(Mathf.Abs(transform.localPosition.x - MirrorPosition.transform.localPosition.x));
           // Debug.Log(Mathf.Abs(transform.localPosition.y - MirrorPosition.transform.localPosition.y));
            if (Mathf.Abs(transform.localPosition.x - MirrorPosition.transform.localPosition.x) <= 60f &&
                Mathf.Abs(transform.localPosition.y - MirrorPosition.transform.localPosition.y) <= 60f)
            {
                transform.localPosition = new Vector3(MirrorPosition.transform.localPosition.x, MirrorPosition.transform.localPosition.y + 30, MirrorPosition.transform.localPosition.z);
                MirrorCrackNumber += 1;
                isFinish = true;
            }
            else
            {
                transform.localPosition = RetPosition;
            }
        }
    }
}
