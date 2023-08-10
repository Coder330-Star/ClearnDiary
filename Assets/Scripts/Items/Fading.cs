using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public AudioClip enemyDieClip;
    public AudioClip playerDieClip;
    public bool isEnemyDie;

    private SpriteRenderer sp;
    private AudioSource au;
    private float speed=0.01f;
    private float delayTime = 2;

    private void Start()
    {
        sp = transform.GetChild(0).GetComponent<SpriteRenderer>();
        au = GetComponent<AudioSource>();
        if (isEnemyDie)
        {
            au.PlayOneShot(enemyDieClip, GameManager.Instance.volume);
        }
        else
        {
            au.PlayOneShot(playerDieClip, GameManager.Instance.volume);
        }
        StartCoroutine(StartFading());
    }

    IEnumerator StartFading()
    {
        yield return new WaitForSeconds(delayTime);
        while (true)
        {
            if (sp.color.a <= 0)
            {
                Destroy(gameObject);
                yield return null;
            }

            Color temp = sp.color;
            temp.a -= 0.1f;
            temp.a = Mathf.Clamp(temp.a, 0, 1);
            sp.color = temp;
            yield return new WaitForSeconds(speed);
        }
    }

}
