using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRotation : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 v = (target.position - transform.position).normalized;
        transform.right = v;
    }
}
