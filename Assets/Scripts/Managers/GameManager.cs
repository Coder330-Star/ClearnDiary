using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;

public enum GunLevel 
{
    Pistol = 1,
    AK,
    MG,
    Snipe,
    Shotgun,
    Crossbow,
}

public class GameManager : MonoSingleton<GameManager>
{
    [HideInInspector]
    public int money;//金币数量
    public int unlockLevel;//解锁关卡
    public int curSelectLevel;//当前选择的关卡    
    public float volume;//音量大小
    public bool anthonyIsDead;
    
    public GunLevel gunLevel;
    public List<WeaponProperties> weaponPropertiesList;
    public bool isBossDead;

    [HideInInspector]
    public Player player;

    private float joyStickSize;//虚拟摇杆的尺寸


    public override void Init()
    {
        DontDestroyOnLoad(gameObject);
        weaponPropertiesList = new List<WeaponProperties>();
        //SaveByJson();
        LoadWeaponPropertiesInfo();
        LoadMainScene();
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


    /// <summary>
    /// 存储为JSON格式的文件
    /// </summary>
    private void SaveByJson() 
    {
        //Json中无法识别float类型 可识别(bool,double,int,long,object,string)
        string jsonPath = Application.streamingAssetsPath + "/WeaponProperties.json";
        string saveJsonStr = JsonMapper.ToJson(weaponPropertiesList);
        StreamWriter sw = new StreamWriter(jsonPath);
        sw.Write(saveJsonStr);
        sw.Close();
    }

    private void LoadWeaponPropertiesInfo()
    {
        weaponPropertiesList = new List<WeaponProperties>();
        string filePath = Application.streamingAssetsPath + "/WeaponProperties.json";
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string info = sr.ReadToEnd();
            sr.Close();
            weaponPropertiesList = JsonMapper.ToObject<List<WeaponProperties>>(info);

            if (weaponPropertiesList.Count == 0)
            {
                Debug.Log("读取武器信息配置表失败！！");
                return;
            }
        }        
    }


    public void LoadMainScene() 
    {
        //表示主角死亡
        PlayerPrefs.SetInt("Money", money);
        Invoke("DelayLoadScene", 2);
    }

    private void DelayLoadScene() 
    {
        SceneManager.LoadScene(0);
    }
}
