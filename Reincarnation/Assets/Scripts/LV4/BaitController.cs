using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitController : MonoBehaviour
{
    public bool isAllure;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            isAllure = true;
            //Invoke("DestoryObjects", 3f);
        }
    }

    /*void DestoryObjects()
    {
        isAllure = false;
        Destroy(gameObject);
    }*/
}
