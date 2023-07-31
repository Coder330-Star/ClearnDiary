using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScripts : MonoBehaviour
{


    private void Start()
    {
        string path = Application.streamingAssetsPath + "/WeaponProperties.json";
        AppConfig.ReadConfigByString(path);
    }
}
