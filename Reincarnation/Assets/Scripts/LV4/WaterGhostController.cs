using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WaterGhostController : MonoBehaviour
{
    public Transform target;//追擊目標物
    public Transform BeAttackedPoint, BeAttackedPoint2;//Player被抓位置 Bait被抓位置

    public float speed;//追擊速度

    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public bool isPlayerBeAttacked;//Player被抓到
    bool isBaitHeadBeAttacked;//Bait被抓到
    public bool isPlayerPlayAttackAnim;
    private bool isCurrentGhost;//當前Ghost
    public bool isCurrentAttackGhost;//當前攻擊Ghost
    public bool WoodTrackGhost;
    bool TrackingWood;
    string currentState;//動畫名稱String

    public float CatchDistance;//抓到距離
    public float MaxPosition;//限制X軸最大Position.x
    public float MinPosition;//限制X軸最小Position.x

    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;
    BaitController baitController;
    PlayerLV4 playerLV4;
    WoodGround woodGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLV4 = target.GetComponentInParent<PlayerLV4>();
        baitController = FindObjectOfType<BaitController>();
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.1f);
        woodGround=FindObjectOfType<WoodGround>();
        SceneSingleton._Instance.SetState(0);
    }

    void Update()
    {
        // anim.SetFloat("Speed", 10) ;
        DistanceEvent();//判斷追擊Player Or Bait

        BeAttackedEvent();//判斷抓到Player Or Bait

        AfterAttackEvent();//抓到Player Or Bait

        CatchPositionXEvent();//判斷是否追擊Player

        if(WoodTrackGhost&& woodGround.isFallingWater&& playerLV4.isWoodGround&&!playerLV4.isEnemyAttack&&woodGround.transform.localPosition.x<127)
        {
            target = woodGround.gameObject.transform.GetChild(0);
            anim.speed = 0.5f;
            speed = 120;
            TrackingWood = true;
        }
        else if(WoodTrackGhost&&(playerLV4.isEnemyAttack||woodGround.transform.localPosition.x > 127))
        {
            TrackingWood = false;
            anim.speed = 1f;
            speed = 300;
        }

        if (isPlayerPlayAttackAnim)
        {
            playerLV4.gameObject.transform.parent = BeAttackedPoint.transform;
            playerLV4.gameObject.transform.localPosition = Vector3.zero;
            playerLV4.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            playerLV4.gameObject.transform.localScale = new Vector3(-1, -1, transform.localScale.z);
            playerLV4.anim.enabled = true;
            playerLV4.anim.SetBool("SwimingBeginIdle", false);
            playerLV4.anim.SetBool("SwimingIdle", false);
            playerLV4.anim.SetBool("Swiming", false);
            playerLV4.anim.SetBool("WaterGhostAttack", true);
        }
    }

    //判斷追擊Player Or Bait
    void DistanceEvent()
    {
        if (baitController.isAllure)
        {
            target = baitController.gameObject.transform;
        }
        else if(!TrackingWood)
        {
            target = playerLV4.gameObject.transform;
        }
        CatchDistance = Vector2.Distance(transform.position, target.position);
    }

    //判斷抓到Player Or Bait
    void BeAttackedEvent()
    {
        if (CatchDistance < 2 && playerLV4.isEnemyAttack && !baitController.isAllure)
        {
            AudioManager.Instance.PlaySource("WaterGhostAttack", 0.5f, "4");
            isPlayerBeAttacked = true;
            isCurrentAttackGhost = true;
        }
        else if (CatchDistance < 2 && baitController.isAllure)
        {
            isBaitHeadBeAttacked = true;
        }
    }

    //抓到Player Or Bait
    void AfterAttackEvent()
    {
        if (isPlayerBeAttacked && !isBaitHeadBeAttacked)
        {
            playerLV4.rigidbody2D.velocity = Vector3.zero;
            playerLV4.isInWater = playerLV4.isCanMove = playerLV4.isEnemyAttack = false;
            playerLV4.isBeEnemyAttacked = true;
            ChangeAnimationState("Grab");
            rb.velocity = Vector2.zero;
            playerLV4.sortingGroup.sortingOrder = -1;
        }
        else if (isBaitHeadBeAttacked)
        {
            Rigidbody2D BaitRigidbody2D = baitController.gameObject.GetComponent<Rigidbody2D>();
            BaitRigidbody2D.velocity = Vector3.zero;
            BaitRigidbody2D.isKinematic = true;
            rb.velocity = Vector2.zero;
            baitController.gameObject.transform.rotation = new Quaternion(0, 0, baitController.gameObject.transform.rotation.z, 0);
        }
    }

    //判斷是否追擊Player
    void CatchPositionXEvent()
    {
        if (playerLV4.transform.position.x <= MaxPosition && playerLV4.transform.position.x >= MinPosition)
            //if (playerLV4.isInWater && playerLV4.transform.position.x <= MaxPosition && playerLV4.transform.position.x >= MinPosition)
        {
            isCurrentGhost = true;
            
            if (isCurrentGhost&& playerLV4.isInWater)
            {
                playerLV4.isEnemyAttack = true;
            }
        }
        else if (playerLV4.transform.position.x >= MaxPosition || playerLV4.transform.position.x <= MinPosition)
        {
            if (isCurrentGhost)
            {
                playerLV4.isEnemyAttack = false;
                isCurrentGhost = false;
            }
        }
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

    void FixedUpdate()
    {
        if ((playerLV4.isEnemyAttack || baitController.isAllure)&& !isBaitHeadBeAttacked&&isCurrentGhost|| TrackingWood)
        {
            ChangeAnimationState("Run");
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
            
            if (angle < -90 || angle > 90)
            {
                transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
                rb.rotation = angle + 30;
            }
            else if (angle < 90 || angle > -90)
            {
                transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
                rb.rotation = angle - 30;
            }
        }
        else if(!playerLV4.isEnemyAttack&&!isBaitHeadBeAttacked)
        {
            rb.velocity = Vector2.zero;
            if (!playerLV4.isBeEnemyAttacked||!isCurrentAttackGhost)
            {
                ChangeAnimationState("Idle");
            }
            
        }
        else if (isBaitHeadBeAttacked)
        {
            rb.velocity = Vector2.zero;
            ChangeAnimationState("Grab");
        }
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        anim.Play(newState);

        currentState = newState;
    }

    public void CatchPlayerPlayBeAttackedAnim()
    {
        if (isPlayerBeAttacked)
        {
            isPlayerPlayAttackAnim = true;
            Invoke("ReloadScenes", 2f);
        }
        else if (isBaitHeadBeAttacked)
        {
            baitController.gameObject.transform.parent = BeAttackedPoint2.transform;
            baitController.gameObject.transform.localPosition = Vector3.zero;
            Invoke("DestoryBaitObject", 3f);
        }
    }

    void ReloadScenes()
    {
        AudioManager.Instance.CanPausePlaySource(true, true, "UnderWater", "4", 1);
        SceneSingleton._Instance.SetState(2);
    }

    void DestoryBaitObject()
    {
        baitController.isAllure=isBaitHeadBeAttacked = false;
        Destroy(baitController.gameObject);
    }
}
