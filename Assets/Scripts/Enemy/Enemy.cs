using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    //属性
    [Header("*********属性**********")]
    public float speed;//移动速度
    public float HP;//血量值    
    public int reward;//被击杀后的奖励
    [Tooltip("屏蔽Player层和IgnoreRaycast层")]
    public LayerMask layer;
    public bool isBoss;

    //资源
    [Header("*********资源**********")]
    public GameObject[] bloodGos;//血液预制体
    public GameObject bloodDeadGo;//死亡特效
    public GameObject bloodPS;//血液粒子特效
    public GameObject explosion;//踩到敌人后的爆炸特效

    //私有变量,引用
    protected Transform playerTrans;
    protected Rigidbody2D rigid2D;
    protected Player player;
    protected RaycastHit2D hit2D;
    protected bool follow;//是否跟随
    protected Vector3 playerLastPos;//玩家最后一次出现再怪物视野中的位置
    protected float curHp;//当前血量值



    protected virtual void Start()
    {
        player = GameManager.Instance.player;
        player.enemys.Add(this);
        rigid2D = GetComponent<Rigidbody2D>();
        playerTrans = player.transform;
        curHp = HP;
    }


    protected virtual void Update()
    {        
        if (playerTrans == null)
        {
            return;
        }
        hit2D = Physics2D.Raycast(transform.position, playerTrans.position - transform.position, 4, layer);        
        SearchAndFollowPlayer();
        Move();
    }



    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Instantiate(bloodPS, transform.position, Quaternion.Euler(0, 0,
                collision.transform.rotation.eulerAngles.z + 180));
            int gunlevel = (int)GameManager.Instance.gunLevel;
            if (collision.gameObject.name.Contains("0"))
            {
                curHp -= 10 * gunlevel * 0.5f;
                Instantiate(bloodGos[0], transform.position, Quaternion.Euler(0, 0,
                    collision.transform.rotation.eulerAngles.z + Random.Range(-15, 15)));
            }
            else if (collision.gameObject.name.Contains("1"))
            {
                curHp -= 50;
                Instantiate(bloodGos[1], transform.position, Quaternion.Euler(0, 0,
                    collision.transform.rotation.eulerAngles.z));
            }
            else if (collision.gameObject.name.Contains("2"))
            {
                curHp -= 20;
                Instantiate(bloodGos[0], transform.position, Quaternion.Euler(0, 0,
                    collision.transform.rotation.eulerAngles.z + Random.Range(-20, 02)));
            }
            else if (collision.gameObject.name.Contains("3"))
            {
                curHp -= 40;
                Instantiate(bloodGos[2], transform.position, Quaternion.Euler(0, 0,
                    collision.transform.rotation.eulerAngles.z));
            }
            //给敌人一个击退力
            rigid2D.AddRelativeForce(new Vector2(0, player.playerShoot.repulsion));
         }

        if (collision.CompareTag("Mine"))
        {
            //地雷
            Instantiate(explosion, transform.position, Quaternion.identity);
            curHp -= 60;
            Destroy(collision.gameObject);
        }
        Die();
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.curHp -= 0.1f;
            player.LimitHP();
            player.delayTimer = player.delayRegen;
            player.Die();
        }
    }

    /// <summary>
    /// 搜索跟随玩家
    /// </summary>
    protected void SearchAndFollowPlayer() 
    {
        if (follow && Vector3.Distance(transform.position,playerTrans.position) <= 0.1f)
        {
            follow = false;
        }
        if (hit2D.collider != null)
        {            
            if (!hit2D.collider.gameObject.CompareTag("Wall"))
            {
                LookAtPlayerAndAttack();
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


    protected void Move()
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
            rigid2D.AddRelativeForce(new Vector2(0, speed));
        }
    }


    /// <summary>
    /// 敌人死亡
    /// </summary>
    protected virtual void Die() 
    {
        if (curHp <= 0)
        {
            if (isBoss)
            {
                player.bossIsDead = true;
            }
            Instantiate(bloodDeadGo, transform.position, transform.rotation);
            GameManager.Instance.player.curKills += 1;
            GameManager.Instance.money += reward;
            player.enemys.Remove(this);
            Destroy(gameObject);
        }
    }

    protected virtual void LookAtPlayerAndAttack() 
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

}
