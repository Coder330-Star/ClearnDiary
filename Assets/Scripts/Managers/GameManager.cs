using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;
using System;
using UnityEngine.Networking;
using System.Collections;

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
    //[HideInInspector]
    public int money;//金币数量
    public int unlockLevel;//解锁关卡
    public int curSelectLevel;//当前选择的关卡    
    public float volume;//音量大小
    public bool anthonyIsDead;
    public float joyStickSize;//虚拟摇杆的尺寸

    public GunLevel gunLevel;
    public List<WeaponProperties> weaponPropertiesList;
    public bool isBossDead;
    public bool showEnd;
    public bool[] firstEnterLevels;

    [HideInInspector]
    public Player player;
    [HideInInspector]
    public GameUIManager gameUIManager;

    public string[] stories;
    public Vector2 inputValue;
    public float inputAngle;

    public override void Init()
    {
        DontDestroyOnLoad(gameObject);
        weaponPropertiesList = new List<WeaponProperties>();
#if UNITY_STANDALONE_WIN
        LoadWeaponPropertiesInfo();
        LoadStories();

#elif UNITY_ANDROID
        StartCoroutine(LoadWeaponPropertiesInfo());
        StartCoroutine(LoadStories());
#endif
        LoadGameData();

        //SaveByJson();
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
        if (PlayerPrefs.HasKey("firstEnter"))
        {
            firstEnterLevels = GetBoolArray("firstEnter");
        }
        else 
        {
            firstEnterLevels = new bool[6] { true, true, true, true, true, true };
        }
        if (PlayerPrefs.HasKey("AnthonyIsDead"))
        {
            anthonyIsDead = Convert.ToBoolean(PlayerPrefs.GetInt("AnthonyIsDead"));
        }
        else
        {
            anthonyIsDead = false;
        }
        curSelectLevel = 1;
        gunLevel = GunLevel.Pistol;
        SetBoolArray("firstEnter",firstEnterLevels);
    }


    /// <summary>
    /// 存储为JSON格式的文件
    /// </summary>
    private void SaveByJson() 
    {
        //Json中无法识别float类型 可识别(bool,double,int,long,object,string)
        string jsonPath = Application.streamingAssetsPath + "/stories.json";
        string saveJsonStr = JsonMapper.ToJson(stories);
        StreamWriter sw = new StreamWriter(jsonPath);
        sw.Write(saveJsonStr);
        sw.Close();
    }

#if UNITY_STANDALONE_WIN
    /// <summary>
    /// 加载武器属性
    /// </summary>
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

    /// <summary>
    /// 读取故事情节
    /// </summary>
    private void LoadStories()
    {
        string filePath = Application.streamingAssetsPath + "/stories.json";
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string info = sr.ReadToEnd();
            sr.Close();
            stories = JsonMapper.ToObject<string[]>(info);

            if (stories.Length == 0)
            {
                Debug.Log("读取故事信息配置表失败！！");               
            }
        }
    }

#elif UNITY_ANDROID
    public IEnumerator LoadWeaponPropertiesInfo() 
    {
        weaponPropertiesList = new List<WeaponProperties>();
        string filePath = Application.streamingAssetsPath + "/WeaponProperties.json";
        //UnityWebRequest
        UnityWebRequest uWRequest = UnityWebRequest.Get(filePath);
        yield return uWRequest.SendWebRequest();
        string json = uWRequest.downloadHandler.text;        

        ///www弃用了
        //WWW www = new WWW(filePath);
        //while (!www.isDone)
        //{
        //    yield return null;
        //}
        //string json = www.text;
        weaponPropertiesList = JsonMapper.ToObject<List<WeaponProperties>>(json);
        if (weaponPropertiesList.Count == 0)
        {
            Debug.Log("读取武器信息配置表失败！！");
        }
    }

    private IEnumerator LoadStories() 
    {
        string filePath = Application.streamingAssetsPath + "/WeaponProperties.json";

        //UnityWebRequest
        UnityWebRequest uWRequest = UnityWebRequest.Get(filePath);
        yield return uWRequest.SendWebRequest();
        string json = uWRequest.downloadHandler.text;

        //www弃用了
        //WWW www = new WWW(filePath);
        //while (!www.isDone)
        //{
        //    yield return null;
        //}
        //string json = www.text;
        stories = JsonMapper.ToObject<string[]>(json);
        if (weaponPropertiesList.Count == 0)
        {
            Debug.Log("读取故事信息配置表失败！！");
        }
    }

#endif

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


    public void SetBoolArray(string key,bool[] array) 
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < array.Length-1; i++)
        {
            sb.Append(array[i]).Append("|");
        }
        sb.Append(array[array.Length - 1]);
        PlayerPrefs.SetString(key, sb.ToString());
    }

    public bool[] GetBoolArray(string key) 
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(key)))
        {
            return new bool[10];
        }
        string[] strArry = PlayerPrefs.GetString(key).Split("|");
        bool[] boolArray = new bool[strArry.Length];
        for (int i = 0; i < strArry.Length; i++)
        {
            boolArray[i] = Convert.ToBoolean(strArry[i]);
        }

        return boolArray;

    }

}
