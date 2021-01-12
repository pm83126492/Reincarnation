using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorDontCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(5, 10);
        Physics2D.IgnoreLayerCollision(9, 10);
        StartCoroutine(MovePosition());
    }


    IEnumerator MovePosition()
    {
        yield return new WaitForSeconds(20f);
        transform.position = new Vector3(100f, 0, 0);
    }
}
