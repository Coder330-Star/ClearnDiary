using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    //资源
    public GameObject enemyGo;//敌人预制

    //属性    
    public float spawnTime;//固定值
    public int limit;//数量上限值
       
    public bool startSpawn;
    private float delayTimer;//计时器

    private void Start()
    {
        delayTimer = spawnTime;
    }

    private void Update()
    {
        if (startSpawn)
        {
            Debug.Log(delayTimer);
            if (delayTimer > 0)
            {
                delayTimer -= Time.deltaTime;
            }
            else
            {
                if (limit > 0)
                {
                    Instantiate(enemyGo, transform.position + new Vector3(Random.Range(-0.1f,0.1f), 
                        Random.Range(-0.1f, 0.1f),0), Quaternion.identity);
                    delayTimer = spawnTime;
                    limit -= 1;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!startSpawn)
            {
                startSpawn = true;
            }
        }
    }
}