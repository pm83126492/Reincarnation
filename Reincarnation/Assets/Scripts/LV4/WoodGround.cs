using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodGround : MonoBehaviour
{
    private Rigidbody2D boxrigidbody;
    private BoxCollider2D boxCollider;
    public bool isFallingWater;
    bool CanMove;

    public PhysicsMaterial2D NoFriction;

    float RotationTime;
    // Start is called before the first frame update
    void Start()
    {
        boxrigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            transform.Translate(1 * Time.deltaTime, 0, 0);
        }
        /*transform.Rotate(Vector3.forward * 2);
        if (isFallingWater&&transform.rotation.z!=0)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime*10);
        }*/
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Lian")&& isFallingWater)
        {
            boxrigidbody.freezeRotation = true;
            CanMove = true;
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            isFallingWater = true;
            boxrigidbody.sharedMaterial = NoFriction;
            gameObject.layer = 8;
            boxrigidbody.gravityScale = 3;
            boxrigidbody.mass = 1;
            boxCollider.offset = new Vector2(boxCollider.offset.x, -0.22f);
            boxCollider.size = new Vector2(boxCollider.size.x, 0.58f);
        }
    }
}
