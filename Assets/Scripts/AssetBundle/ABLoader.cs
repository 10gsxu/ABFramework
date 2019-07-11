using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeoHui
{
    public class ABLoader
    {
        private string bundleName;
        private string bundlePath;

        private LoadFinish loadFinish;

        private AssetLoader assetLoader;

        public ABLoader(string bundleName, LoadFinish loadFinish)
        {
            this.loadFinish = loadFinish;
            this.bundleName = bundleName;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public void SyncLoadAssetBundle()
        {
            this.bundlePath = PathTools.DataPath + bundleName + "." + UpdateConfig.Instance.ExtName;
            AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);
            assetLoader = new AssetLoader(assetBundle);
            if (loadFinish != null)
                loadFinish(bundleName);
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public IEnumerator AsyncLoadAssetBundle()
        {
            this.bundlePath = PathTools.WwwDataPath + bundleName + "." + UpdateConfig.Instance.ExtName;
            using (WWW www = new WWW(bundlePath))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.Log(bundleName + "加载失败，地址为 : " + bundlePath);
                }
                //加载完成
                if (www.isDone)
                {
                    assetLoader = new AssetLoader(www.assetBundle);
                    if (loadFinish != null)
                        loadFinish(bundleName);
                }
            }
        }

        #region 下层提供功能
        /// <summary>
        /// 加载单个资源
        /// </summary>
        public T LoadAsset<T>(string resName) where T : UnityEngine.Object
        {
            return assetLoader.LoadAsset<T>(resName);
        }

        public void UnloadAsset(UnityEngine.Object asset)
        {
            assetLoader.UnloadAsset(asset);
        }

        public void UnloadAsset(string resName)
        {
            assetLoader.UnloadAsset(resName);
        }

        public void Dispose()
        {
            assetLoader.Dispose();
        }

        public void DisposeAll()
        {
            assetLoader.DisposeAll();
        }

        public void LogAllAssetNames()
        {
            assetLoader.LogAllAssetNames();
        }
        #endregion
    }
}