using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    private TextMesh txt;
    private Transform playerTrans;
    private float viewDis;    
    private float dis;


    private void Start()
    {
        txt = GetComponent<TextMesh>();
        viewDis = 1.5f;
        playerTrans = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        if (playerTrans == null)
        {
            return;
        }
        dis = Vector3.Distance(transform.position, playerTrans.position);
        if (dis < viewDis)
        {
            txt.characterSize = 0.05f * (1 - dis / viewDis);
        }
        else
        {
            txt.characterSize = 0;
        }
    }
}
