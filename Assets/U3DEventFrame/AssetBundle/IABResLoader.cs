using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IABResLoader : IDisposable {

    private AssetBundle ABRes;

    public IABResLoader(AssetBundle bundle)
    {
        ABRes = bundle;
    }

    /// <summary>
    /// 加载单个资源
    /// </summary>
    /// <param name="resName">Res name.</param>
    public T LoadAsset<T>(string resName) where T : UnityEngine.Object
    {
        if (this.ABRes == null || !this.ABRes.Contains(resName))
        {
            Debug.Log(resName + " not contain");
            return default(T);
        }
        return ABRes.LoadAsset<T>(resName);
    }
	
    /// <summary>
    /// 卸载单个资源
    /// </summary>
    /// <param name="resObj">Res object.</param>
    public void UnloadAsset(UnityEngine.Object resObj)
    {
        Resources.UnloadAsset(resObj);
    }

    /// <summary>
    /// 释放AssetBundle包
    /// </summary>
    /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:IABResLoader"/>. The
    /// <see cref="Dispose"/> method leaves the <see cref="T:IABResLoader"/> in an unusable state. After calling
    /// <see cref="Dispose"/>, you must release all references to the <see cref="T:IABResLoader"/> so the garbage
    /// collector can reclaim the memory that the <see cref="T:IABResLoader"/> was occupying.</remarks>
    public void Dispose()
    {
        if (this.ABRes == null)
            return;
        ABRes.Unload(false);
    }

    //调试
    public void DebugRes()
    {
        string[] tmpAssetName = ABRes.GetAllAssetNames();
        for(int i=0; i<tmpAssetName.Length; ++i)
        {
            Debug.Log("ABRes Contain asset name == " + tmpAssetName[i]);
        }
    }
}
