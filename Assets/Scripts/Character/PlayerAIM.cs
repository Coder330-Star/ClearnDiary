using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 安德鲁的AI管理器,负责搜索敌人以及瞄准敌人
/// </summary>
public class PlayerAIM : MonoBehaviour
{    
    public Transform cameraTrans;
    public GameObject mark;//标记(用于突出敌人的标记)
    public float minDis;//最小距离
    public float maxDis;//视野范围
    public LayerMask layer;
    ///////private int layerValue;

    private Player player;
    private Enemy nearestEnemy;
    private RaycastHit2D hit2D;
    private float distance;//怪物和玩家之间的距离

    void Start()
    {
        player = GameManager.Instance.player;
        mark.SetActive(false);
        minDis = maxDis = 10;
        //////////layerValue = ~(1 << 9);
    }

    
    void Update()
    {        
        for (int i = 0; i < player.enemys.Count; i++)
        {
            hit2D = Physics2D.Raycast(transform.position, player.enemys[i].transform.position - transform.position,100,layer);
            Debug.DrawRay(transform.position, player.enemys[i].transform.position - transform.position, Color.red);
            if (hit2D.collider != null)
            {
                if (!hit2D.collider.gameObject.CompareTag("Wall"))
                {                    
                    distance = Vector3.Distance(transform.position, player.enemys[i].transform.position);
                    Debug.Log("distance:     " + distance);
                    if (distance <= maxDis && distance > minDis)
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
            mark.SetActive(true);
            mark.transform.position = hit2D.collider.transform.position;

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
        }
        
        
    }
}
