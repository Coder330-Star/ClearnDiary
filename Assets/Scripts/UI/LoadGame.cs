using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public TextMeshProUGUI tipsTxt;    
    public GameObject textWriterEffect;
    public AsyncOperation ao;

    private void Start()
    {
        if (GameManager.Instance.showEnd)
        {
            ao = SceneManager.LoadSceneAsync(0);
        }
        else
        {
            ao = SceneManager.LoadSceneAsync(GameManager.Instance.curSelectLevel);
        }
        
        ao.allowSceneActivation = false;
    }

    private void Update()
    {
        if (ao.progress >= 0.9f)
        {
            tipsTxt.text = "按下任意键继续......";
            if (Input.anyKeyDown)
            {

                if (GameManager.Instance.firstEnterLevels[GameManager.Instance.curSelectLevel-1])
                {
                    GameManager.Instance.firstEnterLevels[GameManager.Instance.curSelectLevel - 1] = false;
                    GameManager.Instance.SetBoolArray("firstEnter", GameManager.Instance.firstEnterLevels);
                    textWriterEffect.SetActive(true);
                    gameObject.SetActive(false);
                }
                else
                {
                    LoadNextScene();
                }                               
            }
        }
    }

    public void LoadNextScene() 
    {
        ao.allowSceneActivation = true;
    }
}
