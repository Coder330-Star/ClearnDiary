using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player player;
    private GameObject mark;


    private void Start()
    {
        player = GameManager.Instance.player;
        player.enemys.Add(this);
    }
}
