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
    public int curHp;//当前血量
    public float delayTimer;//可回复血量计时器
    public int regenHpSpeed;//HP回复速度
    public int delayRegen;//收到伤害可再次回复HP时的延迟时间

    [Header("*********组件**********")]
    public Rigidbody2D rig2D;
    public AudioSource au;

    private void Awake()
    {
        GameManager.Instance.player = this;        
    }
}
