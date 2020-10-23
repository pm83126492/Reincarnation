using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.forward * y);
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * x);
    }
}
