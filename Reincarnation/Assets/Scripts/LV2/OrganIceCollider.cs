using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganIceCollider : MonoBehaviour
{
    public ParticleSystem DustEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            DustEffect.Play();
        }
    }

}
