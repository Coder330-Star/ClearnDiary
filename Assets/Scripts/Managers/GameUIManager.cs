using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public Slider hPSlider;
    public Slider bulletSlider;    
    public GameObject joyStick;
    public Sprite[] weaponSprs;
    public Image weaponImg;
    public Image imgAttackCD;
    public Image imgReloadCD;
    public Image mineImg;
    public Image turretImg;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GameManager.Instance.gameUIManager = this;
#if !UNITY_STANDALONE_WIN
        canvasGroup.interactable = false;
        //joyStick.SetActive(false);
        weaponImg.gameObject.SetActive(true);
        weaponImg.sprite = weaponSprs[((int)GameManager.Instance.gunLevel) - 1];
#elif UNITY_ANDROID
        canvasGroup.interactable = true;
        joyStick.SetActive(true);
        weaponImg.gameObject.SetActive(false);
#endif
    }


    /// <summary>
    /// 更新血条
    /// </summary>
    /// <param name="value"></param>
    public void UpdateHPSlider(float value) 
    {
        hPSlider.value = value;
    }


    /// <summary>
    /// 更新子弹数量条
    /// </summary>
    /// <param name="value"></param>
    public void UpdateBulletSlider(float value) 
    {
        bulletSlider.value = value;
    }


    /// <summary>
    /// 炮塔的
    /// </summary>
    /// <param name="hasTurret"></param>
    public void UpdateTurretUI(bool hasTurret) 
    {
        turretImg.fillAmount = hasTurret ? 1 : 0;
    }


    /// <summary>
    /// 剩余地雷显示
    /// </summary>
    /// <param name="value"></param>
    public void UpdateMineUI(float value) 
    {
        mineImg.fillAmount = value;
    }


    /// <summary>
    /// 更新攻击CD
    /// </summary>
    /// <param name="value"></param>
    public void UpdateAttackCDUI(float value) 
    {
        imgAttackCD.fillAmount = value;
    }

    /// <summary>
    /// 更新装弹CD
    /// </summary>
    /// <param name="value"></param>
    public void UpdateReloadAndMagzineUI(float value)
    {
        imgReloadCD.fillAmount = value;
    }
}
