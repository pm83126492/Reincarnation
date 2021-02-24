using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seal_2Rotation : MonoBehaviour
{
    public float RotationZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //鏡子上封印旋轉
        transform.Rotate(new Vector3(0f, 0f, RotationZ));
    }
}
