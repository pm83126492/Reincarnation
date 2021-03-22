using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingEye : MonoBehaviour
{
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Target.position.x * 0.016f,transform.position.y,transform.position.z);
    }
}
