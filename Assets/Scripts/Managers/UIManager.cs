using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private int panelChoice;//当前选择界面的索引
    public GameObject[] panels;//设置界面，武器界面，关卡界面
    public Image[] imgSelects;//
    public TextMeshProUGUI txtMoney;

    //设置界面
    public Image imgJoystickSize;
    public Image imgVolume;
    public Sprite imgVolSpr;
    public Sprite imgMuteSpr;

    //武器界面
    public Image[] imgSelectedWeapon;
    private Button[] btnSelectedWeapons;
    private TextMeshProUGUI[] txtWeaponPrices;
    private int[] weaponPrice;

    //关卡界面
    public Image[] imgLevels;
    private Button[] btnLevels;
    private TextMeshProUGUI[] txtLevels;

    //音频
    public AudioClip[] clipSounds;
    private AudioSource au;   

    private void Start()
    {
        au = GetComponent<AudioSource>();
        weaponPrice = new int[] { 0, 100, 200, 350, 400, 500 };
        btnLevels = new Button[imgLevels.Length];
        txtLevels = new TextMeshProUGUI[imgLevels.Length];
        for (int i = 0; i < imgLevels.Length; i++)
        {
            btnLevels[i] = imgLevels[i].GetComponent<Button>();
        }
        for (int i = 0; i < imgLevels.Length; i++)
        {
            txtLevels[i] = imgLevels[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        btnSelectedWeapons = new Button[imgSelectedWeapon.Length];
        txtWeaponPrices = new TextMeshProUGUI[imgSelectedWeapon.Length];
        for (int i = 0; i < imgSelectedWeapon.Length; i++)
        {
            btnSelectedWeapons[i] = imgSelectedWeapon[i].GetComponent<Button>();
        }
        for (int i = 0; i < imgSelectedWeapon.Length; i++)
        {
            txtWeaponPrices[i] = imgSelectedWeapon[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        panelChoice = 3;
        InitUI();
        UpdateMoneyText();
    }

    #region 初始化所有UI
    /// <summary>
    /// 初始化所有UI
    /// </summary>
    void InitUI() 
    {
        InitCommpn();
        InitSetting();
        InitWeapon();
        InitLevels();
    }


    /// <summary>
    /// 公共界面
    /// </summary>
    private void InitCommpn() 
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        for (int i = 0; i < imgSelects.Length; i++)
        {
            imgSelects[i].color = new Color(0, 1, 0, 0.3f);
        }
        imgSelects[panelChoice - 1].color = new Color(0, 1, 0, 1);
        panels[panelChoice - 1].SetActive(true);    
    }

    /// <summary>
    /// 设置界面
    /// </summary>
    private void InitSetting() 
    {
        imgJoystickSize.rectTransform.localScale = new Vector3(GameManager.Instance.joyStickSize,
            GameManager.Instance.joyStickSize, 1);
        if (GameManager.Instance.volume == 0)
        {
            imgVolume.sprite = imgMuteSpr;
            imgVolume.color = new Color(1, 0, 0, 1);
        }
        else
        {
            imgVolume.sprite = imgVolSpr;
            imgVolume.color = new Color(0, 1, 0, 1);
        }
    }

    private void InitWeapon() 
    {
        for (int i = 0; i < imgSelectedWeapon.Length; i++)
        {
            if ( GameManager.Instance.money >= weaponPrice[i])
            {
                //表示解锁
                imgSelectedWeapon[i].color = new Color(0, 1, 0, 0.3f);
                txtWeaponPrices[i].color = new Color(0, 1, 0, 0.3f);
                btnSelectedWeapons[i].interactable = true;                
            }
            else
            {
                imgSelectedWeapon[i].color = new Color(1, 0, 0, 0.3f);
                txtWeaponPrices[i].color = new Color(1, 0, 0, 0.3f);
                btnSelectedWeapons[i].interactable = false;
            }
        }

        imgSelectedWeapon[((int)GameManager.Instance.gunLevel)-1].color = new Color(0, 1, 0, 1);
        txtWeaponPrices[((int)GameManager.Instance.gunLevel) - 1].color = new Color(0, 1, 0, 1);
    }

    private void InitLevels() 
    {
        for (int i = 0; i < imgLevels.Length; i++)
        {
            if (GameManager.Instance.unlockLevel >= i + 1)
            {
                //表示解锁
                imgLevels[i].color = new Color(0, 1, 0, 0.3f);
                txtLevels[i].color = new Color(0, 1, 0, 0.3f);
                btnLevels[i].interactable = true;
            }
            else
            {
                imgLevels[i].color = new Color(1, 0, 0, 0.5f);
                txtLevels[i].color = new Color(1, 0, 0, 0.5f);
                btnLevels[i].interactable = false;
            }
        }
        imgLevels[GameManager.Instance.curSelectLevel-1].color = new Color(0, 1, 0, 1);
        txtLevels[GameManager.Instance.curSelectLevel - 1].color = new Color(0, 1, 0, 1);
    }

    #endregion

    #region 公用面板的方法

    private void UpdateMoneyText()
    {
        txtMoney.text = "" + GameManager.Instance.money;
    }

    public  void PlayButtonSound() 
    {
        au.PlayOneShot(clipSounds[0], GameManager.Instance.volume * 0.5f);
    }

    public void PlayPanelSound() 
    {
        au.PlayOneShot(clipSounds[1], GameManager.Instance.volume * 0.5f);
    }

    /// <summary>
    /// 显示相关面板
    /// </summary>
    public void ShowPanel(int choiceIndex) 
    {
        panelChoice = choiceIndex;
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
            imgSelects[i].color = new Color(0, 1, 0, 0.5f);
        }
        panels[panelChoice - 1].SetActive(true);
        imgSelects[panelChoice - 1].color = new Color(0, 1, 0, 1);

    }

    public void StartGame() 
    {
        SceneManager.LoadScene(7);
    }

    #endregion

    #region 设置面板

    public void SetJoyStickSize() 
    {
        GameManager.Instance.joyStickSize += 0.05f;
        if (GameManager.Instance.joyStickSize > 0.5f)
        {
            GameManager.Instance.joyStickSize = 0.3f;
        }
        imgJoystickSize.rectTransform.localScale = new Vector3(GameManager.Instance.joyStickSize,
            GameManager.Instance.joyStickSize, 1);
        PlayerPrefs.SetFloat("joyStickSize", GameManager.Instance.joyStickSize);
    }

    public void SetVolumeValue() 
    {
        GameManager.Instance.volume += 0.2f;
        if (GameManager.Instance.volume >1)
        {
            GameManager.Instance.volume = 0;
            imgVolume.sprite = imgMuteSpr;
            imgVolume.color = new Color(1, 0, 0, 0.3f);
        }
        else
        {
            imgVolume.sprite = imgVolSpr;
            imgVolume.color = new Color(0, 1, 0, GameManager.Instance.volume);
        }
        PlayerPrefs.SetFloat("Volume", GameManager.Instance.volume);
    }


    /// <summary>
    /// 清空存档
    /// </summary>
    public void ClearData() 
    {
        PlayerPrefs.DeleteAll();
        GameManager.Instance.unlockLevel = 1;
        GameManager.Instance.volume = 1;
        GameManager.Instance.money = 0;
        GameManager.Instance.joyStickSize = 0.3f;
        GameManager.Instance.firstEnterLevels = new bool[6] { true, true, true, true, true, true };
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 
    /// 退出游戏
    /// </summary>
    public void ExitGame() 
    {
        PlayerPrefs.SetInt("Money", GameManager.Instance.money);
        PlayerPrefs.SetInt("UnlockLevel", GameManager.Instance.unlockLevel);
        PlayerPrefs.SetFloat("volume", GameManager.Instance.volume);
        PlayerPrefs.SetFloat("joyStickSize", GameManager.Instance.joyStickSize);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion

    #region 关卡面板

    public void SelectLevel(int levelIndex) 
    {
        GameManager.Instance.curSelectLevel = levelIndex;
        //SceneManager.LoadScene(levelIndex);
        InitLevels();
    }

    #endregion

    #region 武器界面

    public void SelectGun(int gunIndex) 
    {
        GameManager.Instance.gunLevel = (GunLevel)gunIndex;
        InitWeapon();
    }

    #endregion
}
