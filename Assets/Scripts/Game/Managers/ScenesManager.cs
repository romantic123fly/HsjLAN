using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : BaseManager<ScenesManager>
{
    private int barProgress;
    public delegate void DelAfterLoadScene();
    public DelAfterLoadScene theDel;
    public string willShowSceneName;

    //异步对象
    AsyncOperation asyn;
    public void LoadNextScene(string sceneName, DelAfterLoadScene del = null)
    {
        if (willShowSceneName == sceneName)
        {
            return;
        }
        willShowSceneName = sceneName;
        theDel = del;

        StartCoroutine(LoadScene(sceneName));

        GameTool.ClearMemory();
    }
    IEnumerator LoadScene(string sceneName)
    {
        asyn = SceneManager.LoadSceneAsync(sceneName);
        asyn.allowSceneActivation = false;
        yield return asyn;
        if (asyn.isDone)
        {
            if (theDel != null) theDel();

            asyn = null;
            GameTool.ClearMemory();
        }
    }

    protected override void Update()
    {
        if (asyn == null)
        {
            return;
        }
        int theProgress = 0;

        //asyn.progress的范围是0~1（最大检测范围到0.9，所以在0.9的时候就可以显示场景了）
        if (asyn.progress < 0.9f)
        {
            //正在加载场景中ing
            theProgress = (int)asyn.progress * 100;
        }
        else
        {
            theProgress = 100;
        }
        if (barProgress < theProgress)
        {
            barProgress++;
        }
        if (barProgress == 100)
        {
            asyn.allowSceneActivation = true;
        }
    }
}