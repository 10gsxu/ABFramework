using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LoadFinish(string bundle);

public class IABLoader {

    private string bundleName;
    private string commonBundlePath = "";
    private WWW commonLoader;

    private LoadFinish loadFinish;

    private IABResLoader abResLoader;

    public IABLoader()
    {
        commonBundlePath = "";
        bundleName = "";

        loadFinish = null;

        abResLoader = null;
    }

    public IABLoader(string bundleName, LoadFinish loadFinish) : this()
    {
        this.loadFinish = loadFinish;
        this.bundleName = bundleName;
        commonBundlePath = PathTools.WwwDataPath + "assetbundle/" + bundleName;
    }

    //携程加载
    public IEnumerator CommonLoad()
    {
        commonLoader = new WWW(commonBundlePath);
        while(!commonLoader.isDone)
        {
            yield return commonLoader.progress;
        }
        if (commonLoader.progress >= 1.0f)//表示加载完成
        {
            abResLoader = new IABResLoader(commonLoader.assetBundle);
            if (loadFinish != null)
            {
                loadFinish(bundleName);
            }
        }
        else
        {
            Debug.LogError("load bundle error == " + bundleName);
        }

        commonLoader = null;
    }

    #region  下层提供功能
    //获取单个资源
    public T LoadAsset<T>(string resName) where T : UnityEngine.Object
    {
        if (abResLoader == null)
            return default(T);
        return abResLoader.LoadAsset<T>(resName);
    }

    //卸载单个资源
    public void UnloadAsset(UnityEngine.Object resObj)
    {
        if (abResLoader == null)
            return;
        abResLoader.UnloadAsset(resObj);
    }

    //释放AssetBundle包
    public void Dispose()
    {
        if (abResLoader == null)
            return;
        abResLoader.Dispose();
        abResLoader = null;
    }

    //Debug
    public void DebugRes()
    {
        if (abResLoader == null)
            return;
        abResLoader.DebugRes();
    }
    #endregion
}
