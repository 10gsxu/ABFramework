using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LeoHui
{
    /// <summary>
    /// 管理一个场景中的所有AssetBundle
    /// </summary>
    public class ABSceneManager
    {
        private Dictionary<string, ABManager> sceneDict;
        private Dictionary<string, string> fullNameDict;

        public ABSceneManager()
        {
            sceneDict = new Dictionary<string, ABManager>();
            fullNameDict = new Dictionary<string, string>();
        }

        private void CheckDict(string sceneName, string bundleName)
        {
            if (!sceneDict.ContainsKey(sceneName))
            {
                sceneDict.Add(sceneName, new ABManager(sceneName));
            }
            if (!fullNameDict.ContainsKey(bundleName))
            {
                fullNameDict.Add(bundleName, sceneName + "/" + bundleName);
            }
        }

        public void SyncLoadAssetBundle(string sceneName, string bundleName)
        {
            //检测字典
            CheckDict(sceneName, bundleName);
            sceneDict[sceneName].SyncLoadAssetBundle(fullNameDict[bundleName]);
        }

        public void AsyncLoadAssetBundle(string sceneName, string bundleName, LoadFinish loadFinish, LoadAssetBundleCallBack callBack)
        {
            //检测字典
            CheckDict(sceneName, bundleName);
            sceneDict[sceneName].AsyncLoadAssetBundle(fullNameDict[bundleName], loadFinish, callBack);
        }

        /// <summary>
        /// 协程加载AssetBundle
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleFullName"></param>
        /// <returns></returns>
        public IEnumerator AsyncLoadAssetBundle(string sceneName, string bundleFullName)
        {
            yield return sceneDict[sceneName].AsyncLoadAssetBundle(bundleFullName);
        }

        #region 下层提供
        public T LoadAsset<T>(string sceneName, string bundleName, string resName) where T : UnityEngine.Object
        {
            return sceneDict[sceneName].LoadAsset<T>(fullNameDict[bundleName], resName);
        }

        public void UnloadAsset(string sceneName, string bundleName, UnityEngine.Object asset)
        {
            sceneDict[sceneName].UnloadAsset(fullNameDict[bundleName], asset);
        }

        public void UnloadAsset(string sceneName, string bundleName, string resName)
        {
            sceneDict[sceneName].UnloadAsset(fullNameDict[bundleName], resName);
        }

        public void Release(string sceneName, string bundleName)
        {
            sceneDict[sceneName].Release(fullNameDict[bundleName]);
        }

        public void ReleaseAll(string sceneName, string bundleName)
        {
            sceneDict[sceneName].ReleaseAll(fullNameDict[bundleName]);
        }

        public void LogAllAssetNames(string sceneName, string bundleName)
        {
            sceneDict[sceneName].LogAllAssetNames(fullNameDict[bundleName]);
        }
        #endregion
    }
}