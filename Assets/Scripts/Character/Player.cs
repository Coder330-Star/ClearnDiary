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
    public List<Enemy> enemys = new List<Enemy>();


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
        speed = 2;
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
        if (Input.GetKey(KeyCode.W))
        {
            //前
            rig2D.AddForce(Vector2.up * speed * 70 * Time.deltaTime);
            moveAngle = 0;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //后
            rig2D.AddForce(Vector2.down * speed * 70 * Time.deltaTime);
            moveAngle = 180;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //左
            rig2D.AddForce(Vector2.left * speed * 70 * Time.deltaTime);
            moveAngle = 90;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //右
            rig2D.AddForce(Vector2.right * speed * 70 * Time.deltaTime);
            moveAngle = -90;
        }

        ///旋转角度
        transform.rotation = Quaternion.Euler(0, 0, moveAngle);
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
}
