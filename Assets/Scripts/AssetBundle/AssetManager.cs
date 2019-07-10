using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour {
    public static AssetManager Instance;
    private ABSceneManager sceneManager;

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        //第一步 加载 ABManifest
        //StartCoroutine(ABManifestLoader.Instance.AsyncLoadManifest());
        ABManifestLoader.Instance.SyncLoadManifest();
        sceneManager = new ABSceneManager();
    }

    private void OnDestroy()
    {
        System.GC.Collect();
    }

    public void LoadCallBack(string sceneName, string bundleName)
    {
        StartCoroutine(sceneManager.AsyncLoadAssetBundle(sceneName, bundleName));
    }

    //异步加载
    public void AsyncLoadAssetBundle(string sceneName, string bundleName, LoadFinish loadFinish)
    {
        sceneManager.AsyncLoadAssetBundle(sceneName, bundleName, loadFinish, LoadCallBack);
    }

    //同步加载
    public void SyncLoadAssetBundle(string sceneName, string bundleName)
    {
        sceneManager.SyncLoadAssetBundle(sceneName, bundleName);
    }

    #region 由下层API提供
    public T LoadAsset<T>(string sceneName, string bundleName, string resName) where T : UnityEngine.Object
    {
        return sceneManager.LoadAsset<T>(sceneName, bundleName, resName);
    }

    public void UnloadAsset(string sceneName, string bundleName, string resName)
    {
        sceneManager.UnloadAsset(sceneName, bundleName, resName);
    }

    public void UnloadAsset(string sceneName, string bundleName, UnityEngine.Object asset)
    {
        sceneManager.UnloadAsset(sceneName, bundleName, asset);
    }

    public void Release(string sceneName, string bundleName)
    {
        sceneManager.Release(sceneName, bundleName);
    }

    public void ReleaseAll(string sceneName, string bundleName)
    {
        sceneManager.ReleaseAll(sceneName, bundleName);
    }

    public void LogAllAssetNames(string sceneName, string bundleName)
    {
        sceneManager.LogAllAssetNames(sceneName, bundleName);
    }
    #endregion
}
