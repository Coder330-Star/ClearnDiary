using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //资源
    [Header("*********资源**********")]
    public GameObject blood;//血迹
    public GameObject bloodPriticle;//减血特效
    public AudioClip bonus;//奖励音效
    public GameObject explosion;//爆炸特效
    public GameObject deadBooldGo;

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
    public bool bossIsDead;
    private bool decreasHP;//被安东尼感染后持续掉血

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
        speed = 15;
        regenHpSpeed = delayRegen = 1;
        rig2D = GetComponent<Rigidbody2D>();
        au = GetComponent<AudioSource>();
        if (GameManager.Instance.curSelectLevel == 6)
        {
            //表示是boss
            bossIsDead = false;
        }
        if (GameManager.Instance.curSelectLevel == 6 && !GameManager.Instance.anthonyIsDead)
        {
            decreasHP = true;
        }
    }

    private void Update()
    {
        Move();
        if (decreasHP)
        {
            curHp -= 1 * Time.deltaTime;
            LimitHP();
            Die();
        }
        RegonHP();
    }

    private void Move() 
    {
        isMoving = false;
        if (Input.GetKey(KeyCode.W))
        {
            //前
            rig2D.AddForce(Vector2.up * speed / (1 + playerShoot.weight * 0.1f) * 70 * Time.deltaTime);
            moveAngle = 0;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //后
            rig2D.AddForce(Vector2.down * speed / (1 + playerShoot.weight * 0.1f) * 70 * Time.deltaTime);
            moveAngle = 180;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //左
            rig2D.AddForce(Vector2.left * speed / (1 + playerShoot.weight * 0.1f) * 70 * Time.deltaTime);
            moveAngle = 90;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //右
            rig2D.AddForce(Vector2.right * speed / (1 + playerShoot.weight * 0.1f) * 70 * Time.deltaTime);
            moveAngle = -90;
            isMoving = true;
        }        
    }


    /// <summary>
    /// 回复血量值
    /// </summary>
    private void RegonHP() 
    {
        //if (curHp >= maxHP)
        //{
        //    return;
        //}

        if (delayTimer <= 0 && curHp < maxHP)
        {
            curHp += regenHpSpeed * Time.deltaTime;
            LimitHP();

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
            case "EnemyBullet":
                Instantiate(bloodPriticle, transform.position, Quaternion.Euler(0, 0, collision.transform.rotation.eulerAngles.z + 180));
                Instantiate(blood, transform.position, Quaternion.Euler(0, 0, collision.transform.rotation.eulerAngles.z + 180));
                DestroyItem(collision.gameObject);
                curHp -= 10;
                LimitHP();
                Die();
                break;
            case "BulletItem":                
                if (playerShoot.curBullets < playerShoot.totalBullets)
                {                    
                    playerShoot.curBullets += (int)(playerShoot.totalBullets * 0.25f);
                    playerShoot.LimitBullet();
                    DestroyItem(collision.gameObject);
                }
                break;
            case "HealthItem":
                if (curHp < maxHP)
                {
                    DestroyItem(collision.gameObject);
                    curHp += 20;
                    LimitHP();
                }
                break;
            case "MineItem":
                if (playerShoot.curMine < 3)
                {
                    DestroyItem(collision.gameObject);
                    playerShoot.curMine += 1;
                    GameManager.Instance.gameUIManager.UpdateMineUI((float)playerShoot.curMine / playerShoot.mines);
                }
                break;
            case "TurretItem":
                if (!playerShoot.hasTurret)
                {
                    DestroyItem(collision.gameObject);
                    playerShoot.hasTurret = true;
                    GameManager.Instance.gameUIManager.UpdateTurretUI(playerShoot.hasTurret);
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
            case "EnemyMine":
                //地雷
                Instantiate(explosion, transform.position, Quaternion.identity);
                curHp -= 60;
                LimitHP();
                Destroy(collision.gameObject);
                Die();
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
                    playerShoot.LimitBullet();
                    DestroyItem(collision.gameObject);                    
                }              
            }
            else if (collision.gameObject.name.Contains("Health")) 
            {
                //医疗补给箱
                if (curHp < maxHP)
                {
                    DestroyItem(collision.gameObject);
                    curHp += 20;
                    LimitHP();
                }
            }
            else if (collision.gameObject.name.Contains("Turret"))
            {
                //炮塔补给箱
                if (playerShoot.curMine < 3)
                {
                    DestroyItem(collision.gameObject);
                    playerShoot.curMine += 1;
                    GameManager.Instance.gameUIManager.UpdateMineUI((float)playerShoot.curMine / playerShoot.mines);
                }
                if (!playerShoot.hasTurret)
                {
                    DestroyItem(collision.gameObject);
                    playerShoot.hasTurret = true;
                    GameManager.Instance.gameUIManager.UpdateTurretUI(playerShoot.hasTurret);
                }
            }

        }
    }

    private void DestroyItem(GameObject collision) 
    {
        Destroy(collision);
        au.PlayOneShot(bonus, GameManager.Instance.volume * 0.5f);
    }


    /// <summary>
    /// 角色死亡
    /// </summary>
    public void Die() 
    {
        if (curHp <= 0)
        {
            GameManager.Instance.LoadMainScene();
            Instantiate(deadBooldGo, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }


    /// <summary>
    ///限制血量值和更新血条
    /// </summary>
    public void LimitHP() 
    {
        if (curHp >= maxHP)
        {
            curHp = maxHP;
        }
        else if (curHp < 0)
        {
            curHp = 0;
        }
        GameManager.Instance.gameUIManager.UpdateHPSlider(curHp / maxHP);
    }
}
