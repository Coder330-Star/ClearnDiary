using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Turret : MonoBehaviour
{
    [Header("****************资源****************")]
    public GameObject mark;
    public GameObject bulletGo;//子弹预设
    public Transform[] shootPoints;//射击点

    [Header("****************数据变量****************")]
    public AudioClip shootClip;    
    public LayerMask layer;
    public float attackCD;
    public int bullets;
    public float inaccuracy;
    public bool isEnemy;

    private float minDis;
    private float maxDis;
    private Enemy nearestEnemy;
    private RaycastHit2D hit2D;
    private AudioSource au;
    private Player player;
    private float curAttackCD;
    private int turretLevel;
    private float distance;


    


    void Start()
    {
        player = GameManager.Instance.player;
        minDis = maxDis = 7;
        au = GetComponent<AudioSource>();
        mark.SetActive(false);
        turretLevel = ((int)GameManager.Instance.gunLevel) / 2;
        if (turretLevel < 1)
        {
            turretLevel = 1;
        }
    }

    
    void Update()
    {
        if (curAttackCD > 0)
        {
            curAttackCD -= Time.deltaTime;
        }

        if (player == null)
        {
            return;
        }

        if (player.transform == null)
        {
            return;
        }

        if (isEnemy)
        {
            hit2D = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 3, layer);
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            if (hit2D.collider != null)
            {
                if (!hit2D.collider.gameObject.CompareTag("Wall"))
                {
                    ///终点减去起点，方向指向终点
                    Vector3 moveDir = player.transform.position - transform.position;
                    if (moveDir != Vector3.zero)
                    {
                        //正弦sin   对边比斜边
                        //余弦cos   邻边比斜边
                        //正切tan   对边比邻边
                        //Atan2返回的是弧度值，需要转化为角度值
                        float angle = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
                    }
                    if (curAttackCD <= 0 && bullets > 0)
                    {
                        Instantiate(bulletGo, shootPoints[0].position, Quaternion.Euler(0, 0, transform.eulerAngles.z +
                            Random.Range(-inaccuracy, inaccuracy)));
                        if (turretLevel >= 3)
                        {
                            Instantiate(bulletGo, shootPoints[1].position, Quaternion.Euler(0, 0, transform.eulerAngles.z +
                            Random.Range(-inaccuracy, inaccuracy)));
                        }
                        au.PlayOneShot(shootClip, GameManager.Instance.volume * 0.6f);
                        curAttackCD = attackCD;
                        bullets -= 1;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < player.enemys.Count; i++)
            {
                hit2D = Physics2D.Raycast(transform.position, player.enemys[i].transform.position - transform.position, 10, layer);
                Debug.DrawRay(transform.position, player.enemys[i].transform.position - transform.position, Color.red);
                if (hit2D.collider != null)
                {
                    if (!hit2D.collider.gameObject.CompareTag("Wall"))
                    {
                        distance = Vector3.Distance(transform.position, player.enemys[i].transform.position);
                        if (distance <= maxDis && distance < minDis)
                        {
                            minDis = distance;
                            //表示在玩家视野里面
                            nearestEnemy = player.enemys[i];
                        }
                    }
                }
            }

            if (nearestEnemy != null)
            {
                //mark.transform.SetParent(nearestEnemy.transform);
                //mark.transform.localPosition = Vector3.zero;
                //mark.transform.rotation = transform.rotation;
                mark.SetActive(true);

                ///终点减去起点，方向指向终点
                Vector3 moveDir = nearestEnemy.transform.position - transform.position;
                if (moveDir != Vector3.zero)
                {
                    //正弦sin   对边比斜边
                    //余弦cos   邻边比斜边
                    //正切tan   对边比邻边
                    //Atan2返回的是弧度值，需要转化为角度值
                    float angle = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
                }
                if (curAttackCD <= 0 && bullets > 0)
                {
                    Instantiate(bulletGo, shootPoints[0].position, Quaternion.Euler(0, 0, transform.eulerAngles.z +
                        Random.Range(-inaccuracy, inaccuracy)));
                    if (turretLevel >= 3)
                    {
                        Instantiate(bulletGo, shootPoints[1].position, Quaternion.Euler(0, 0, transform.eulerAngles.z +
                        Random.Range(-inaccuracy, inaccuracy)));
                    }
                    au.PlayOneShot(shootClip, GameManager.Instance.volume * 0.6f);
                    curAttackCD = attackCD;
                    bullets -= 1;
                }


            }
            else
            {
                mark.SetActive(false);
                ///旋转角度
                //transform.rotation = Quaternion.Euler(0, 0, player.moveAngle);
            }
        }        
    }

    private void LateUpdate()
    {
        if (!isEnemy)
        {
            //表示不是敌人的炮塔
            if (nearestEnemy != null)
            {
                mark.transform.position = nearestEnemy.transform.position;
                if (hit2D.collider.CompareTag("Wall"))
                {
                    nearestEnemy = null;
                    minDis = maxDis = 7;
                }
            }
            else
            {
                maxDis = minDis = 10;
                nearestEnemy = null;
            }
        }
        
    }
}
