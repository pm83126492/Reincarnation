using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttackCollider : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    // Start is called before the first frame update
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;
        Invoke("OpenCollider", 1f);
    }

    void OpenCollider()
    {
        polygonCollider.enabled = true;
    }
}
