using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 安东尼
/// </summary>
public class Anthony : Enemy
{
    protected override void Update() 
    {

    }

    protected override void Die()
    {
        base.Die();
        if (curHp <= 0)
        {
            GameManager.Instance.anthonyIsDead = true;
        }
    }
}
