using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LoadAssetBundleCallBack(string bundleName);

//对所有Bundle包管理
public class IABManager {
    //把每一个包都存起来
    Dictionary<string, IABRelationManager> loadHelper = new Dictionary<string, IABRelationManager>();

    Dictionary<string, Dictionary<string, UnityEngine.Object>> loadObj = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();

    public IABManager()
    {
    }

    public T LoadAsset<T>(string bundleName, string resName) where T : UnityEngine.Object
    {
        if(loadObj.ContainsKey(bundleName))
        {
            if (loadObj[bundleName].ContainsKey(resName))
            {
                return loadObj[bundleName][resName] as T;
            }
        }

        //表示已经加载过Bundle包
        if(loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            T resObj = loader.LoadAsset<T>(resName);
            if (!loadObj.ContainsKey(bundleName))
                loadObj.Add(bundleName, new Dictionary<string, UnityEngine.Object>());
            loadObj[bundleName].Add(resName, resObj);
            return resObj;
        }

        return default(T);
    }

    #region 释放缓存物体

    public void UnloadAsset(string bundleName, string resName)
    {
        if(loadObj.ContainsKey(bundleName))
        {
            if(loadObj[bundleName].ContainsKey(resName))
            {
                UnityEngine.Object resObj = loadObj[bundleName][resName];
                Resources.UnloadAsset(resObj);
                loadObj[bundleName].Remove(resName);
            }
        }
    }

    public void DisposeResObj(string bundleName)
    {
        if (loadObj.ContainsKey(bundleName))
        {
            List<string> keys = new List<string>();
            keys.AddRange(loadObj[bundleName].Keys);
            for (int i = 0; i < keys.Count; ++i)
            {
                UnloadAsset(bundleName, keys[i]);
            }
            loadObj[bundleName].Clear();
        }
        Resources.UnloadUnusedAssets();
    }

    public void DisposeAllObj()
    {
        List<string> keys = new List<string>();
        keys.AddRange(loadObj.Keys);
        for(int i=0; i<keys.Count; ++i)
        {
            DisposeResObj(keys[i]);
        }
        loadObj.Clear();
    }

    public void DisposeBundle(string bundleName)
    {
        if(loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            List<string> dependences = loader.GetDependence();
            for(int i=0; i<dependences.Count; ++i)
            {
                if(loadHelper.ContainsKey(dependences[i]))
                {
                    IABRelationManager dependence = loadHelper[dependences[i]];
                    if(dependence.RemoveRefference(bundleName))
                    {
                        DisposeBundle(dependence.GetBundleName());
                    }
                }
            }
            if(loader.GetRefference().Count <=0)
            {
                loader.Dispose();
                loadHelper.Remove(bundleName);
            }
        }
    }

    public void DisposeAllBundle()
    {
        List<string> keys = new List<string>();
        keys.AddRange(loadHelper.Keys);
        for (int i = 0; i < keys.Count; ++i)
        {
            IABRelationManager loader = loadHelper[keys[i]];
            loader.Dispose();
        }
        loadHelper.Clear();
    }

    public void DisposeAllBundleAndRes()
    {
        DisposeAllBundle();
        DisposeAllObj();
    }
    #endregion

    public string[] GetDependence(string bundleName)
    {
        return IABManifestLoader.Instance.GetDependences(bundleName);
    }

    //对外的接口
    public void LoadAssetBundle(string bundleName, LoadFinish loadFinish, LoadAssetBundleCallBack callBack)
    {
        if(!loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager(bundleName, loadFinish);
            loadHelper.Add(bundleName, loader);
            callBack(bundleName);
        } else
        {
            Debug.Log("IABManager not contain bundle");
        }
    }

    public IEnumerator LoadAssetBundleDependences(string bundleName, string refName, LoadFinish loadFinish)
    {
        if(!loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager(bundleName, loadFinish);

            if(refName != null)
            {
                loader.AddRefference(refName);
            }

            loadHelper.Add(bundleName, loader);

            yield return LoadAssetBundle(bundleName);
        } else
        {
            if (refName != null)
            {
                IABRelationManager loader = loadHelper[bundleName];
                loader.AddRefference(refName);
            }
        }
    }

    /// <summary>
    /// 加载AssetBundle,必须先加载Manifest
    /// </summary>
    /// <returns>The asset bundle.</returns>
    /// <param name="bundleName">Bundle name.</param>
    public IEnumerator LoadAssetBundle(string bundleName)
    {
        while(!IABManifestLoader.Instance.IsLoadFinish())
        {
            yield return null;
        }
        IABRelationManager loader = loadHelper[bundleName];
        string[] dependence = GetDependence(bundleName);
        loader.SetDependence(dependence);
        for(int i=0; i<dependence.Length; ++i) {
            yield return LoadAssetBundleDependences(dependence[i], bundleName, loader.GetLoadFinish());
        }
        yield return loader.LoadAssetBundle();
    }

    #region 由下层提供API
    public void DebugRes(string bundleName)
    {
        if(loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            loader.DebugRes();
        } else
        {
            Debug.Log("IABRelationManager not contain");
        }
    }
    #endregion
}
