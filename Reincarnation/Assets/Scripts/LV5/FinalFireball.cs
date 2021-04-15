using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFireball : MonoBehaviour
{
    public float Speed;
    bool CanFire;
    public GameObject target;

    public GameObject Explose;
    // Start is called before the first frame update

    void Start()
    {
        Invoke("Fire",1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanFire)
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
        }

        if (RunnerKingController.WinNumber >= 20)
        {
            target = GameObject.Find("Mom");
            transform.LookAt(target.transform);
        }
    }

    void Fire()
    {
        CanFire = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mom")
        {
            Spawn();
        }
    }

    void Spawn()
    {
        Instantiate(Explose, transform.position, target.transform.rotation);
        gameObject.SetActive(false);
        CanFire = false;
    }
}
