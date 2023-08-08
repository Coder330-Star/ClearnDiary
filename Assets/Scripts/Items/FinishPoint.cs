using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 终点
/// </summary>
public class FinishPoint : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GameManager.Instance.player;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.curKills >= player.targetkills && player.bossIsDead)
            {
                PlayerPrefs.SetInt("Money", GameManager.Instance.money);
                PlayerPrefs.SetInt("UnlockLevel", GameManager.Instance.unlockLevel + 1);
                SceneManager.LoadScene(0);
            }
        }
    }
}
