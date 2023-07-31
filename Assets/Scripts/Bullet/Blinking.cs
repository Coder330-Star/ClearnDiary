using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    public GameObject bink;
    private float timer;
    private float timeVal = 0.5f;

    private void Update()
    {
        if (timer <= 0)
        {
            bink.SetActive(!bink.activeSelf);
            timer = timeVal;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
