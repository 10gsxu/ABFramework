using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ILoadManager : MonoBehaviour {
    public static ILoadManager Instance;
    private IABManager abManager;

    private void Awake()
    {
        Instance = this;
        //第一步 加载 IABManifest
        StartCoroutine(IABManifestLoader.Instance.LoadManifest());
        abManager = new IABManager();
    }

    private void OnDestroy()
    {
        System.GC.Collect();
    }

    public void LoadCallBack(string bundleName)
    {
        StartCoroutine(abManager.LoadAssetBundle(bundleName));
    }

    //提供加载功能
    public void LoadAssetBundle(string bundleName, LoadFinish loadFinish)
    {
        abManager.LoadAssetBundle(bundleName, loadFinish, LoadCallBack);
    }

    #region 由下层API提供
    public T LoadAsset<T>(string bundleName, string resName) where T : UnityEngine.Object
    {
        return abManager.LoadAsset<T>(bundleName, resName);
    }

    public void UnLoadResObj(string sceneName, string bundleName, string resName)
    {
        abManager.UnloadAsset(bundleName, resName);
    }

    public void UnLoadAssetBundle(string sceneName, string bundleName)
    {
        abManager.DisposeBundle(bundleName);
    }

    public void UnLoadAllAssetBundle(string sceneName)
    {
        abManager.DisposeAllBundle();
        System.GC.Collect();
    }

    public void UnLoadAllAssetBundleAndResObjs(string sceneName)
    {
        abManager.DisposeAllBundleAndRes();
        System.GC.Collect();
    }

    public void DebugRes(string bundleName)
    {
        abManager.DebugRes(bundleName);
    }
    #endregion
}
