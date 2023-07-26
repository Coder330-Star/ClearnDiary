using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private int money;//金币数量
    private int unlockLevel;//解锁关卡
    private int curSelectLevel;//当前选择的关卡
    private int gunLevel;

    private float volume;//音量大小
    private float joyStickSize;//虚拟摇杆的尺寸

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }



    /// <summary>
    /// 加载存储在本地的游戏数据
    /// </summary>
    private void LoadGameData() 
    {
        money = PlayerPrefs.HasKey("Money") ? PlayerPrefs.GetInt("Money") : 0;
        unlockLevel = PlayerPrefs.HasKey("UnlockLevel") ? PlayerPrefs.GetInt("UnlockLevel") : 1;
        volume = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : 1;
        joyStickSize = PlayerPrefs.HasKey("joyStickSize") ? PlayerPrefs.GetFloat("joyStickSize") : 0.3f;
        curSelectLevel = 1;
    }


}
