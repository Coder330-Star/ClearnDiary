using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //资源
    public GameObject blood;//血迹
    public GameObject bloodPriticle;//减血特效
    public AudioClip bonus;//奖励音效

    //数据
    public bool isMoving = false;//玩家是否移动
    public float moveAngle;//玩家移动角度
    public int speed;//移动速度
    public int maxHP;
}
