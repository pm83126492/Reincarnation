using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerLV4 : MonoBehaviour
{
    private Player player;
    public GameObject RebirthPoint, RebirthPoint2;
    public GameObject RebirthCollider, RebirthCollider2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (SceneSingleton.Instance.m_RebirthNumber == 1)
        {
            player.transform.position = RebirthPoint.transform.position;
            Destroy(RebirthCollider);
        }
        else if (SceneSingleton.Instance.m_RebirthNumber >= 2)
        {
            SceneSingleton.Instance.m_RebirthNumber = 3;
            player.transform.position = RebirthPoint2.transform.position;
            Destroy(RebirthCollider);
            Destroy(RebirthCollider2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rebirth();
    }

    void Rebirth()
    {
        if (player.isObstacle && player.hit2.collider.gameObject.tag == "Rebirth")
        {
            SceneSingleton.Instance.m_RebirthNumber++;
            Destroy(player.hit2.collider.gameObject);
        }

    }
}
