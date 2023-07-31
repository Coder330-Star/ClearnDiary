using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppConfig
{    
    private static Dictionary<string, string> configs = new Dictionary<string, string>();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T GetConfig<T>(string key) 
    {
        string value = "";
        if (configs.ContainsKey(key))
        {
            value = configs[key];
        }

        if (typeof(T) == typeof(bool))
        {
            bool boolVal = value.Equals("1");
            return (T)Convert.ChangeType(boolVal, typeof(bool));
        }

        //ChangeType表示将value转换为bool类型的值
        return (T)Convert.ChangeType(value, typeof(bool));
        //return 
    }

    public static void ReadConfigByString(string path) 
    {
        byte[] data = null;
        data = File.ReadAllBytes(path);
        string a = System.Text.Encoding.Default.GetString(data);
        Debug.Log(a);
    }

}
