using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWriterEffect : MonoBehaviour
{
    public LoadGame loadGame;
    private float charPerSecond;//打字时间间隔
    private string words;//日记内容
    private bool startWrite;//是否开始打字特效
    private float timer;//计时器    
    private TextMeshProUGUI txt_Writer;
    private int curPos;
    private AudioSource au;
     

    private void Start()
    {        
        timer = 0;
        startWrite = false;
        charPerSecond = 0.2f;
        txt_Writer = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        au = GetComponent<AudioSource>();
        int storyIndex = GameManager.Instance.curSelectLevel - 1;
        if (storyIndex == 5)
        {
            //表示是第6关
            if (GameManager.Instance.anthonyIsDead)
            {
                storyIndex = 5;
            }
            else
            {
                storyIndex = 7;
            }
            if (GameManager.Instance.showEnd)
            {
                storyIndex++;
            }
        }

        words = GameManager.Instance.stories[GameManager.Instance.curSelectLevel-1];        
        StartEffect();        
    }

    private void Update()
    {
        StartWritting();
    }

    void StartEffect() 
    {
        startWrite = true;
    }


    void StartWritting() 
    {
        if (startWrite)
        {
            if (!au.isPlaying)
            {
                au.Play();
            }
            timer += Time.deltaTime;
            if (timer >= charPerSecond)
            {
                timer = 0;
                curPos++;
                txt_Writer.text = words.Substring(0, curPos);
                if (curPos >= words.Length)
                {
                    txt_Writer.text = words;
                    startWrite = false;
                    timer = 0;
                    curPos = 0;
                    au.Stop();
                    Invoke("LoadNextSceneDelay", 2);
                }
            }
        }
    }

    private void LoadNextSceneDelay() 
    {
        loadGame.LoadNextScene();
    }

}
