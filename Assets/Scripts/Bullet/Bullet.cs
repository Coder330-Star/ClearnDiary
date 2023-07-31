using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    public ParticleSystem destroyPs;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    
    void Update()
    {
        transform.Translate(transform.up * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject, lifeTime);
            Instantiate(destroyPs, transform.position, Quaternion.identity);
        }   

    }
}
