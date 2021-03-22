using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tonadolce : MonoBehaviour
{
    public float Speed = 1;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        this.transform.position += this.transform.forward * Speed * Time.fixedDeltaTime;
    }

}
