using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLV4 : Player
{
    public GameObject WaterPS;
    protected override void Start()
    {
        base.Start();
    }

    protected override void MobileTouch()
    {
        base.MobileTouch();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Instantiate(WaterPS, transform.position, transform.rotation);
        }
    }
}
