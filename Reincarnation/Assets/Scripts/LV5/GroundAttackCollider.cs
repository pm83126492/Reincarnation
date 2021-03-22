using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttackCollider : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        Invoke("OpenCollider", 1f);
    }

    void OpenCollider()
    {
        boxCollider2D.enabled = true;
    }
}
