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
        //transform.LookAt(target.transform);
        Invoke("Fire",1f);
    }
    void Start()
    {
        Physics2D.IgnoreLayerCollision(5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanFire)
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
        }

        if (RunnerKingController.WinNumber >= 21)
        {
            Destroy(gameObject);
        }
    }

    /*void OnTriggerEnter2D(Collider2D other)
    {
        Spawn();
    }*/
    void Fire()
    {
        transform.LookAt(target.transform);
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
