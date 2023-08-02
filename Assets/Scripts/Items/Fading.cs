using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    private SpriteRenderer sp;
    private float speed=0.01f;
    private float delayTime = 2;

    private void Start()
    {
        sp = transform.GetChild(0).GetComponent<SpriteRenderer>();
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
