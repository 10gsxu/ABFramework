using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LeoHui
{
    public class AssetLoader : IDisposable
    {

        private AssetBundle assetBundle;
        private Dictionary<string, UnityEngine.Object> assetDict;

        public AssetLoader(AssetBundle bundle)
        {
            assetBundle = bundle;
            assetDict = new Dictionary<string, UnityEngine.Object>();
        }

        /// <summary>
        /// 加载单个资源
        /// </summary>
        public T LoadAsset<T>(string resName) where T : UnityEngine.Object
        {
            if (assetDict.ContainsKey(resName))
            {
                return assetDict[resName] as T;
            }
            if (!this.assetBundle.Contains(resName))
            {
                Debug.Log("AssetBundle not contain : " + resName);
                return null;
            }
            T asset = assetBundle.LoadAsset<T>(resName);
            assetDict.Add(resName, asset);
            return asset;
        }

        /// <summary>
        /// 卸载单个资源
        /// </summary>
        public void UnloadAsset(UnityEngine.Object asset)
        {
            Resources.UnloadAsset(asset);
        }

        /// <summary>
        /// 卸载单个资源
        /// </summary>
        public void UnloadAsset(string resName)
        {
            if (assetDict.ContainsKey(resName))
            {
                UnloadAsset(assetDict[resName]);
                assetDict.Remove(resName);
            }
        }

        /// <summary>
        /// 释放AssetBundle包
        /// </summary>
        public void Dispose()
        {
            assetBundle.Unload(false);
        }

        public void DisposeAll()
        {
            assetBundle.Unload(true);
            assetDict.Clear();
        }

        /// <summary>
        /// 打印所有资源的名称
        /// </summary>
        public void LogAllAssetNames()
        {
            string[] assetNames = assetBundle.GetAllAssetNames();
            for (int i = 0; i < assetNames.Length; ++i)
            {
                Debug.Log("AssetBundle Contain Asset Name : " + assetNames[i]);
            }
        }
    }

}