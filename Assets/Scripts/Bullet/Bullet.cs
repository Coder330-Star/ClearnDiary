using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    public GameObject destroyPs;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    
    void Update()
    {
        transform.Translate(transform.up * Time.deltaTime * speed,Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        Instantiate(destroyPs, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
