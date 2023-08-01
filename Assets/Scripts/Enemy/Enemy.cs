using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //属性
    [Header("*********属性**********")]
    public float speed;//移动速度
    public float HP;//血量值    
    public int reward;//被击杀后的奖励
    [Tooltip("屏蔽Player层和IgnoreRaycast层")]
    public LayerMask layer;

    //资源
    [Header("*********资源**********")]
    public GameObject[] bloodGos;//血液预制体
    public GameObject bloodDeadGo;//死亡特效
    public GameObject bloodPS;//血液粒子特效
    public GameObject explosion;//踩到敌人后的爆炸特效

    //私有变量,引用
    private Transform playerTrans;
    private Rigidbody2D rigid2D;
    private Player player;
    private RaycastHit2D hit2D;
    private bool follow;//是否跟随
    private Vector3 playerLastPos;//玩家最后一次出现再怪物视野中的位置
    public float curHp;//当前血量值



    private void Start()
    {
        player = GameManager.Instance.player;
        player.enemys.Add(this);
        rigid2D = GetComponent<Rigidbody2D>();
        playerTrans = player.transform;
        curHp = HP;
    }


    private void Update()
    {
        hit2D = Physics2D.Raycast(transform.position, playerTrans.position - transform.position, 5, layer);
        //Debug.DrawRay(transform.position, playerTrans.position - transform.position,Color.red);
        SearchAndFollowPlayer();
        Move();
    }


    /// <summary>
    /// 搜索跟随玩家
    /// </summary>
    void SearchAndFollowPlayer() 
    {
        if (follow && Vector3.Distance(transform.position,playerTrans.position) <= 0.1f)
        {
            follow = false;
        }
        if (hit2D.collider != null)
        {            
            if (!hit2D.collider.gameObject.CompareTag("Wall"))
            {
                Vector3 moveDir = playerTrans.position - transform.position;
                if (moveDir != Vector3.zero)
                {
                    float angle = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
                    follow = true;
                    playerLastPos = playerTrans.position;
                }
            }
            else if (follow)
            {
                //表示搜索敌人
                Vector3 moveDir = playerLastPos - transform.position;
                if (moveDir != Vector3.zero)
                {
                    float angle = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
                }
            }
        }
        else if (follow)
        {
            //表示搜索敌人
            Vector3 moveDir = playerLastPos - transform.position;
            if (moveDir != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            }
        }
    }



    private void Move()
    {
        if (hit2D.collider != null)
        {
            if (hit2D.collider.gameObject.CompareTag("Player"))
            {
                rigid2D.AddRelativeForce(new Vector2(0, speed));
            }
        }
        if (follow)
        {
            rigid2D.AddRelativeForce(new Vector2(0,speed));
        }
    }
}
