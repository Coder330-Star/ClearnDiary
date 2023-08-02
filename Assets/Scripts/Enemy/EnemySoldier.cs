using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier : Enemy
{
    //资源
    public GameObject bulletGo;
    public AudioClip shootClip;

    //引用
    public Transform shootPoint;
    protected AudioSource au;

    //属性
    public float attackCD;    
    public int magazine;//弹夹数量    
    public float reload;    
    public float inaccuracy;//不精确度

    protected float curAttackCD;
    protected int curMagazine;
    protected float curReload;


    protected override void Start()
    {
        base.Start();
        au = GetComponent<AudioSource>();
    }

    protected override void Update()
    {        
        CalculateCD();
        base.Update();
    }

    protected void CalculateCD() 
    {
        curAttackCD -= Time.deltaTime;
        curReload -= Time.deltaTime;
        if (curReload <= 0 && curMagazine <= 0)
        {
            //装弹
            curMagazine = magazine;
        }
    }


    /// <summary>
    /// 攻击方法
    /// </summary>
    protected override void LookAtPlayerAndAttack()
    {
        base.LookAtPlayerAndAttack();
        if (curAttackCD <=0  && curMagazine > 0)
        {
            au.PlayOneShot(shootClip, GameManager.Instance.volume * 0.5f);

            //transform.rotation.eulerAngles  //表示物体相对于世界坐标系的旋转角度。
            //transform.eulerAngles           //表示物体相对于父物体的本地旋转角度。
            //transform.localEulerAngles      //表示的就是相对于自身的旋转角度

            Instantiate(bulletGo, shootPoint.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z +
                Random.Range(-inaccuracy, inaccuracy)));
            curAttackCD = attackCD;
            curMagazine -= 1;
            if (curMagazine <= 0)
            {
                curReload = reload;
            }
            
        }
    }
}
