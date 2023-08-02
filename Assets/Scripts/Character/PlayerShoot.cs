using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //资源
    [Header("****************资源****************")]
    public GameObject[] bullets;//子弹 0.pistol/AK/MG  1.Snipe  2.ShotGun  3.CrossBow
    public GameObject[] turrets;//炮塔
    public AudioClip[] shootClips;//射击音频
    public AudioClip reloadClip;
    public GameObject minePrefabs;

    //引用
    [Header("****************引用****************")]
    public GameObject[] weapons;//武器    
    public GameObject lineLeft;
    public GameObject lineRight;
    public ParticleSystem flash;

    [Header("****************枪的属性****************")]
    public float maxInaccuracy;//最大不精确度   
    public float recoilForce;//后坐力
    public float destabilization;//不稳定性(在所有情况下都会影响精度)
    public float aimingDeSpeed;//瞄准精度减少速度
    public float minInaccuracy;//最小不精确度   
    public float attackCD;//攻击CD    
    public int magazine;//弹夹里的子弹数量
    public int totalBullets;//总子弹数量
    public float reload;//装弹时间
    public float weight;//枪重
    public float repulsion;//子弹击退力

    [Header("****************当前属性****************")]
    public float curInaccuracy;//当前不精确度
    public float curDestabilization;//当前不稳定性
    public float curAttackCD;//当前攻击CD
    public int curMagazine;//当前弹夹里的子弹数量
    public int curBullets;//当前全部字弹数量
    public float curReload;//装弹CD    
    public int curMine;//当前地雷数量
    public int mines;//当前可持有的最大地雷数量
    public bool hasTurret;//是否有炮台

    private WeaponProperties weaponProperties;
    private Player player;
    private AudioSource au;
    private int gunLevel;
    private int turretLevel;

    private void Start()
    {
        player = GameManager.Instance.player;
        gunLevel = (int)GameManager.Instance.gunLevel;
        hasTurret = true;
        curMine = mines = 3;        
        weaponProperties = GameManager.Instance.weaponPropertiesList[gunLevel-1];
        weapons[gunLevel - 1].SetActive(true);
        turretLevel = ((int)GameManager.Instance.gunLevel) / 2;
        if (turretLevel < 1)
        {
            turretLevel = 1;
        }

        maxInaccuracy = (float)weaponProperties.maxInaccuracy;
        recoilForce = (float)weaponProperties.recoilForce;
        destabilization = (float)weaponProperties.destabilization;
        aimingDeSpeed = (float)weaponProperties.aimingDeSpeed;
        minInaccuracy = (float)weaponProperties.minInaccuracy;
        attackCD = (float)weaponProperties.attackCD;
        magazine = weaponProperties.magazine;
        totalBullets = weaponProperties.totalBullects;
        reload = (float)weaponProperties.reload;
        weight = (float)weaponProperties.weight;
        repulsion = (float)weaponProperties.repulsion;

        au = GetComponent<AudioSource>();
        curBullets = totalBullets;
        curMagazine = magazine;
        curReload = curAttackCD = 0;
        player.playerShoot = this;
    }


    private void Update()
    {
        CalculateDestabilization();
        CalculateInaccuracy();
        CalculateCD();
        MonitorInput();
    }


    /// <summary>
    /// 计算不稳定性的值
    /// </summary>
    private void CalculateDestabilization()
    {
        if (player.isMoving)
        {
            //移动的时候不稳定性逐渐增加(扛枪走的)
            if (curDestabilization < destabilization)
            {
                curDestabilization += destabilization * Time.deltaTime * 1.5f;
            }
            else
            {
                curDestabilization = destabilization;
            }
        }
        else
        {
            curDestabilization = 0;
        }
    }


    /// <summary>
    /// 计算不精确度
    /// </summary>
    private void CalculateInaccuracy() 
    {
        //当前不精确度 >= 最小不精确度 + 当前不稳定性
        //表示当前偏差很大，
        if (curInaccuracy >= minInaccuracy + curDestabilization)
        {
            curInaccuracy -= Time.deltaTime * aimingDeSpeed;
        }
        else
        {
            curInaccuracy = minInaccuracy + curDestabilization;
        }
        lineLeft.transform.localRotation = Quaternion.AngleAxis(curInaccuracy, Vector3.forward);
        lineRight.transform.localRotation = Quaternion.AngleAxis(-curInaccuracy, Vector3.forward);
    }


    /// <summary>
    /// 监听玩家输入
    /// </summary>
    private void MonitorInput() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RelaodBullects();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ///炮塔
            if (hasTurret)
            {
                hasTurret = false;
                Instantiate(turrets[turretLevel-1], transform.position, Quaternion.identity);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            //地雷
            if (curMine <= 0)
            {
                return;
            }
            curMine -= 1;
            Instantiate(minePrefabs, transform.position, Quaternion.identity);
        }

    }

    /// <summary>
    /// 射击
    /// </summary>
    private void Shoot() 
    {
        //有子弹，弹夹里面有子弹，攻击cd<0
        if (curBullets > 0 && curMagazine > 0 && curAttackCD <= 0)
        {
            
            au.PlayOneShot(shootClips[gunLevel - 1],GameManager.Instance.volume);
            switch (GameManager.Instance.gunLevel)
            {
                case GunLevel.Pistol:
                case GunLevel.AK:
                case GunLevel.MG:
                    CreateBullect(0);
                    

                    break;
                case GunLevel.Snipe:
                    CreateBullect(1);

                    break;
                case GunLevel.Shotgun://散弹枪
                    CreateBullect(2);

                    break;
                case GunLevel.Crossbow:
                    CreateBullect(3);
                    break;
            }
            if (gunLevel != (int)GunLevel.Pistol && gunLevel != (int)GunLevel.Crossbow)
            {
                //手枪和弩不需要攻击特效
                flash.Play();
            }
            curMagazine -= 1;
            curBullets -= 1;
            curAttackCD = attackCD;
            curInaccuracy += recoilForce;
            if (curInaccuracy > maxInaccuracy)
            {
                curInaccuracy = maxInaccuracy;
            }
            if (curMagazine <= 0 && reload <= 0)
            {
                //自动装弹
                RelaodBullects();
            }
        }
    }


    /// <summary>
    /// 重新装子弹
    /// </summary>
    private void RelaodBullects() 
    {
        if (curMagazine < magazine)
        {
            au.PlayOneShot(reloadClip,GameManager.Instance.volume);
            curReload = reload;
            curMagazine = 0;
        }
    }


    /// <summary>
    /// 计算CD
    /// </summary>
    private void CalculateCD() 
    {
        if (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }

        if (curReload > 0)
        {
            //表示装弹中
            curReload -= Time.deltaTime;
        }
        else
        {
            if (curMagazine <= 0)
            {
                curMagazine = magazine;
            }
        }
    }

    /// <summary>
    /// 创建子弹
    /// </summary>
    private void CreateBullect(int bulletIndex)
    {
        if (bulletIndex == 2)
        {
            //霰弹枪特殊处理
            Instantiate(bullets[bulletIndex], transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z - curInaccuracy)));
            Instantiate(bullets[bulletIndex], transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z - curInaccuracy * 0.5f)));
            Instantiate(bullets[bulletIndex], transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z)));
            Instantiate(bullets[bulletIndex], transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z + curInaccuracy * 0.5f)));
            Instantiate(bullets[bulletIndex], transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z + curInaccuracy)));
        }
        else
        {
            Instantiate(bullets[bulletIndex], transform.position, Quaternion.Euler(
                        new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-curInaccuracy, curInaccuracy))));
        }
               
    }
}


/// <summary>
/// 武器属性结构体
/// </summary>
public struct WeaponProperties 
{    
    public float maxInaccuracy;//最大不精确度    
    public double recoilForce;//后坐力
    public double destabilization;//不稳定性(在所有情况下都会影响精度)
    public double aimingDeSpeed;//瞄准不精确度减少速度
    public double minInaccuracy;//最小不精确度    
    public double attackCD;//攻击CD        
    public int magazine;//弹夹的数量
    public int totalBullects;//总子弹数量
    public double reload;//装弹时间
    public double weight;//枪重
    public double repulsion;//子弹击退力
}
