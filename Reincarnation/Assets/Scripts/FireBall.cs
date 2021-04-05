using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour,IPoolObject
{
    public float Speed;
    bool CanFire;
    public GameObject target;

    public GameObject Explose;
    // Start is called before the first frame update

    public void OnObjectSpawn()
    {
        if (RunnerKingController.WinNumber <= 7)
        {
            Speed = 15;
        }
        else if (RunnerKingController.WinNumber > 7 && RunnerKingController.WinNumber <= 14)
        {
            Speed = 17;
        }
        else if (RunnerKingController.WinNumber > 14)
        {
            Speed = 20;
        }
        target = GameObject.Find("Player");
        transform.LookAt(target.transform);
        Invoke("Fire",1f);
    }
    /*oid Start()
    {
        target = GameObject.Find("Player");
        transform.LookAt(target.transform);
        Invoke("Fire",1f);
    }*/

    // Update is called once per frame
    void Update()
    {
        if (CanFire)
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
        }

        if (RunnerKingController.WinNumber >= 20)
        {
            target= GameObject.Find("Mom");
            transform.LookAt(target.transform);
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
        if (collision.gameObject.tag != "RunnerKingAttack"&& RunnerKingController.WinNumber < 20)
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
