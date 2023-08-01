using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //资源
    [Header("*********资源**********")]
    public GameObject blood;//血迹
    public GameObject bloodPriticle;//减血特效
    public AudioClip bonus;//奖励音效

    //数据
    [Header("*********数据**********")]
    public bool isMoving = false;//玩家是否移动
    public float moveAngle;//玩家移动角度   
    public int speed;//移动速度
    public int maxHP;//血量上限
    public float curHp;//当前血量
    public float delayTimer;//可回复血量计时器
    public int regenHpSpeed;//HP回复速度
    public int delayRegen;//收到伤害可再次回复HP时的延迟时间
    public int targetkills;//通关需要的击杀数
    public int curKills;//当前击杀数
    public List<Enemy> enemys = new List<Enemy>();
    public PlayerShoot playerShoot;


    //[Header("*********组件**********")]
    private Rigidbody2D rig2D;
    private AudioSource au;
    

    private void Awake()
    {
        GameManager.Instance.player = this;
    }

    private void Start()
    {
        curHp = maxHP = 100;
        speed = 8;
        regenHpSpeed = delayRegen = 1;
        rig2D = GetComponent<Rigidbody2D>();
        au = GetComponent<AudioSource>();        
    }

    private void Update()
    {
        Move();
        RegonHP();
    }

    private void Move() 
    {
        isMoving = false;
        if (Input.GetKey(KeyCode.W))
        {
            //前
            rig2D.AddForce(Vector2.up * speed * 70 * Time.deltaTime);
            moveAngle = 0;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //后
            rig2D.AddForce(Vector2.down * speed * 70 * Time.deltaTime);
            moveAngle = 180;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //左
            rig2D.AddForce(Vector2.left * speed * 70 * Time.deltaTime);
            moveAngle = 90;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //右
            rig2D.AddForce(Vector2.right * speed * 70 * Time.deltaTime);
            moveAngle = -90;
            isMoving = true;
        }

        
    }


    /// <summary>
    /// 回复血量值
    /// </summary>
    private void RegonHP() 
    {
        if (curHp >= maxHP)
        {
            return;
        }

        if (delayTimer <= 0 && curHp < maxHP)
        {
            curHp += regenHpSpeed * Time.deltaTime;
            curHp = curHp > maxHP ? maxHP : curHp;
        }
        else if(delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string colTag = collision.tag;
        switch (colTag)
        {
            case "BulletItem":                
                if (playerShoot.curBullets < playerShoot.totalBullets)
                {
                    DestroyItem(collision.gameObject);
                    playerShoot.curBullets += (int)(playerShoot.totalBullets * 0.25f);
                    if (playerShoot.curBullets > playerShoot.totalBullets)
                    {
                        playerShoot.curBullets = playerShoot.totalBullets;
                    }
                }
                break;
            case "HealthItem":
                if (curHp < maxHP)
                {
                    DestroyItem(collision.gameObject);
                    curHp += 20;
                    if (curHp > maxHP)
                    {
                        curHp = maxHP;
                    }
                }
                break;
            case "MineItem":
                if (playerShoot.curMine < 3)
                {
                    DestroyItem(collision.gameObject);
                    playerShoot.curMine += 1;
                }
                break;

            case "TurretItem":
                if (!playerShoot.hasTurret)
                {
                    DestroyItem(collision.gameObject);
                    playerShoot.hasTurret = true;
                }

                break;

            case "MoneyItem":
                DestroyItem(collision.gameObject);
                GameManager.Instance.money += 10;
                break;
            case "Key":
                collision.GetComponent<Item_Key>().OpenDoor();
                DestroyItem(collision.gameObject);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            //子弹补给箱
            if (collision.gameObject.name.Contains("Bullet"))
            {
                if (playerShoot.curBullets < playerShoot.totalBullets)
                {
                    playerShoot.curBullets += (int)(playerShoot.totalBullets * 0.25f);
                    DestroyItem(collision.gameObject);
                    if (playerShoot.curBullets > playerShoot.totalBullets)
                    {
                        playerShoot.curBullets = playerShoot.totalBullets;
                    }
                }              
            }
            else if (collision.gameObject.name.Contains("Health")) 
            {
                //医疗补给箱
                if (curHp < maxHP)
                {
                    DestroyItem(collision.gameObject);
                    curHp += 20;
                    if (curHp > maxHP)
                    {
                        curHp = maxHP;
                    }
                }
            }
            else if (collision.gameObject.name.Contains("Turret"))
            {
                //炮塔补给箱
                if (playerShoot.curMine < 3)
                {
                    DestroyItem(collision.gameObject);
                    playerShoot.curMine += 1;
                }
                if (!playerShoot.hasTurret)
                {
                    DestroyItem(collision.gameObject);
                    playerShoot.hasTurret = true;
                }
            }

        }
    }

    private void DestroyItem(GameObject collision) 
    {
        Destroy(collision);
        au.PlayOneShot(bonus, GameManager.Instance.volume * 0.5f);
    }
}
