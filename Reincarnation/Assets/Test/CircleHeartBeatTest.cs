using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHeartBeatTest : MonoBehaviour
{
    private Transform CircleTransform;
    public float range;
    public float rangeMax;
    public float rangeSpeed;
    // Start is called before the first frame update
    void Start()
    {
        CircleTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        range += rangeSpeed * Time.deltaTime;
        if (range > rangeMax)
        {
            range = 150;
        }
        CircleTransform.localScale = new Vector3(range, range);
    }
}
