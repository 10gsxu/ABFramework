using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IABRelationManager
{
    List<string> dependenceBundle;
    List<string> referBundle;

    IABLoader assetLoader;
    string bundleName;
    LoadFinish loadFinish;

    public IABRelationManager(string bundleName, LoadFinish loadFinish)
    {
        dependenceBundle = new List<string>();
        referBundle = new List<string>();

        this.bundleName = bundleName;
        this.loadFinish = loadFinish;
        assetLoader = new IABLoader(bundleName, loadFinish);
    }

    public string GetBundleName()
    {
        return bundleName;
    }

    public LoadFinish GetLoadFinish()
    {
        return loadFinish;
    }

    public void AddRefference(string bundleName)
    {
        referBundle.Add(bundleName);
    }

    public List<string> GetRefference()
    {
        return referBundle;
    }

    public void SetDependence(string[] dependence)
    {
        if(dependence.Length > 0)
        {
            dependenceBundle.AddRange(dependence);
        }
    }

    public List<string> GetDependence()
    {
        return dependenceBundle;
    }

    public void RemoveDependence(string bundleName)
    {
        for (int i = 0; i < dependenceBundle.Count; ++i)
        {
            if (bundleName.Equals(dependenceBundle[i]))
            {
                dependenceBundle.RemoveAt(i);
                break;
            }
        }
    }

    /// <summary>
    /// 是否被释放
    /// </summary>
    /// <returns><c>true</c>, if refference was removed, <c>false</c> otherwise.</returns>
    /// <param name="bundleName">Bundle name.</param>
    public bool RemoveRefference(string bundleName)
    {
        for(int i=0; i<referBundle.Count; ++i)
        {
            if(bundleName.Equals(referBundle[i]))
            {
                referBundle.RemoveAt(i);
                break;
            }
        }
        if(referBundle.Count <=0)
        {
            Dispose();
            return true;
        }
        return false;
    }

    #region 由下层提供API
    //unity3d 5.3以上 协程 才可以多层传递
    public IEnumerator LoadAssetBundle()
    {
        yield return assetLoader.CommonLoad();
    }

    public T LoadAsset<T>(string resName) where T : UnityEngine.Object
    {
        return assetLoader.LoadAsset<T>(resName);
    }

    //释放过程
    public void Dispose()
    {
        if (assetLoader == null)
            return;
        assetLoader.Dispose();
    }

    public void DebugRes()
    {
        if (assetLoader == null)
            return;
        assetLoader.DebugRes();
    }
    #endregion
}
