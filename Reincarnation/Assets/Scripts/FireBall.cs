using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float Speed;
    bool CanFire;
    public GameObject target;

    public GameObject Explose;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        transform.LookAt(target.transform);
        Invoke("Fire",3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanFire)
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
    }

    /*void OnTriggerEnter2D(Collider2D other)
    {
        Spawn();
    }*/
    void Fire()
    {
        CanFire = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Spawn();
    }

    void Spawn()
    {
        Instantiate(Explose, transform.position, target.transform.rotation);
        Destroy(gameObject);
    }
}
