using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour
{
    public EdgeCollider2D[] edgeCollider2D;
    public static int ColliderNumber;
    // Start is called before the first frame update
    void Start()
    {
        ColliderNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DrawCollider"))
        {
            ColliderNumber += 1;
        }
    }
}
