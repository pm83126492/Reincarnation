using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WaterGhostController : MonoBehaviour
{
    public Transform target;
    public PlayerLV4 playerLV4;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        playerLV4 = target.GetComponentInParent<PlayerLV4>();
        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (playerLV4.isEnemyAttack)
        {
            if (path == null)
                return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            //normalized把需要處理的數據normalized後限制在0~1内
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rb.velocity=(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            Vector2 direction2 = (target.position - transform.position);
            float angle = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            if (angle < -90 || angle > 90)
            {
                transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
            }
            else if (angle < 90 || angle > -90)
            {
                transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
            }
        }
    }
}
