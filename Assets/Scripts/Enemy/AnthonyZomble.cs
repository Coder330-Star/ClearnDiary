using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 僵尸安东尼
/// </summary>
public class AnthonyZomble : EnemySoldier
{
    //资源
    public GameObject mineGo;
    public GameObject turretGo;

    //属性
    public float mineCD;//地雷cd
    public float turrentCD;//炮塔cd
    public float curMineCD;
    public float curTurretCD;

    protected override void Start()
    {
        base.Start(); 
        if (GameManager.Instance.anthonyIsDead)
        {
            Destroy(gameObject);
        }
        curTurretCD = 10;
    }

    protected override void Update()
    {
        base.Update();
        UseItem();
    }

    /// <summary>
    /// 安东尼使用地雷和炮塔
    /// </summary>
    void UseItem() 
    {
        if (hit2D.collider == null)
        {
            return;
        }

        if (!hit2D.collider.CompareTag("Player"))
        {
            return;
        }

        if (curMineCD <= 0)
        {
            Instantiate(mineGo, transform.position, Quaternion.identity);
            curMineCD = mineCD;
        }
        else
        {
            curMineCD -= Time.deltaTime;
        }


        if (curTurretCD <= 0)
        {
            Instantiate(turretGo, transform.position, Quaternion.identity);
            curTurretCD = turrentCD;
        }
        else
        {
            curTurretCD -= Time.deltaTime;
        }
    }

    protected override void LookAtPlayerAndAttack()
    {
        Vector3 moveDir = playerTrans.position - transform.position;
        if (moveDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            follow = true;
            playerLastPos = playerTrans.position;
        }
        if (curAttackCD <= 0 && curMagazine > 0)
        {
            au.PlayOneShot(shootClip, GameManager.Instance.volume * 0.5f);
            //霰弹枪特殊处理
            Instantiate(bulletGo, transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z - inaccuracy)));
            Instantiate(bulletGo, transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z - inaccuracy * 0.5f)));
            Instantiate(bulletGo, transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z)));
            Instantiate(bulletGo, transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z + inaccuracy * 0.5f)));
            Instantiate(bulletGo, transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z + inaccuracy)));
            curAttackCD = attackCD;
            curMagazine -= 1;
            if (curMagazine <= 0)
            {
                curReload = reload;
            }

        }
    }

}
