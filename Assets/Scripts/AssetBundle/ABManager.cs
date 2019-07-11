using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeoHui
{
    //对所有Bundle包管理
    public class ABManager
    {
        private string sceneName;
        private Dictionary<string, ABRelationManager> abDict = new Dictionary<string, ABRelationManager>();

        public ABManager(string sceneName)
        {
            this.sceneName = sceneName;
        }

        public IEnumerator AsyncLoadAssetBundleDependences(string bundleName, string referName, LoadFinish loadFinish)
        {
            if (abDict.ContainsKey(bundleName))
            {
                abDict[bundleName].AddReference(referName);
            }
            else
            {
                ABRelationManager abLoader = new ABRelationManager(bundleName, loadFinish);
                abDict.Add(bundleName, abLoader);
                abLoader.AddReference(referName);
                yield return AsyncLoadAssetBundle(bundleName);
            }
        }

        private void SyncLoadAssetBundleDependences(string bundleName, string referName)
        {
            if (abDict.ContainsKey(bundleName))
            {
                abDict[bundleName].AddReference(referName);
            }
            else
            {
                ABRelationManager abLoader = new ABRelationManager(bundleName, null);
                abDict.Add(bundleName, abLoader);
                abLoader.AddReference(referName);
                abLoader.SyncLoadAssetBundle();
            }
        }

        public void AsyncLoadAssetBundle(string bundleName, LoadFinish loadFinish, LoadAssetBundleCallBack callBack)
        {
            if (abDict.ContainsKey(bundleName))
            {
                if (loadFinish != null)
                    loadFinish(bundleName);
                return;
            }
            ABRelationManager abLoader = new ABRelationManager(bundleName, loadFinish);
            abDict.Add(bundleName, abLoader);
            if (callBack != null)
                callBack(sceneName, bundleName);
        }

        /// <summary>
        /// 加载AssetBundle,必须先加载Manifest
        /// </summary>
        public IEnumerator AsyncLoadAssetBundle(string bundleName)
        {
            while (!ABManifestLoader.Instance.IsLoadFinish())
                yield return null;
            ABRelationManager abLoader = abDict[bundleName];
            string[] dependence = ABManifestLoader.Instance.GetDependence(bundleName);
            abLoader.SetDependence(dependence);
            for (int i = 0; i < dependence.Length; ++i)
            {
                yield return AsyncLoadAssetBundleDependences(dependence[i], bundleName, abLoader.GetLoadFinish());
            }
            yield return abLoader.AsyncLoadAssetBundle();
        }

        public void SyncLoadAssetBundle(string bundleName)
        {
            if (abDict.ContainsKey(bundleName))
                return;
            ABRelationManager abLoader = new ABRelationManager(bundleName, null);
            abDict.Add(bundleName, abLoader);
            string[] dependence = ABManifestLoader.Instance.GetDependence(bundleName);
            abLoader.SetDependence(dependence);
            for (int i = 0; i < dependence.Length; ++i)
            {
                SyncLoadAssetBundleDependences(dependence[i], bundleName);
            }
            abLoader.SyncLoadAssetBundle();
        }

        public void Release(string bundleName)
        {
            if (!abDict.ContainsKey(bundleName))
                return;
            List<string> dependence = abDict[bundleName].GetDependence();
            for (int i = 0; i < dependence.Count; ++i)
            {
                ABRelationManager abLoader = abDict[dependence[i]];
                abLoader.RemoveReference(bundleName);
                //当没有引用时，释放
                if (abLoader.GetReference().Count <= 0)
                {
                    Dispose(dependence[i]);
                }
            }
            //当没有引用时，释放
            if (abDict[bundleName].GetReference().Count <= 0)
            {
                Dispose(bundleName);
            }
        }

        public void ReleaseAll(string bundleName)
        {
            if (!abDict.ContainsKey(bundleName))
                return;
            List<string> dependence = abDict[bundleName].GetDependence();
            for (int i = 0; i < dependence.Count; ++i)
            {
                ABRelationManager abLoader = abDict[dependence[i]];
                abLoader.RemoveReference(bundleName);
                //当没有引用时，释放
                if (abLoader.GetReference().Count <= 0)
                {
                    DisposeAll(dependence[i]);
                }
            }
            //当没有引用时，释放
            if (abDict[bundleName].GetReference().Count <= 0)
            {
                DisposeAll(bundleName);
            }
        }

        #region 由下层提供API
        public T LoadAsset<T>(string bundleName, string resName) where T : UnityEngine.Object
        {
            if (!abDict.ContainsKey(bundleName))
            {
                Debug.Log(bundleName + "不存在");
                return null;
            }
            return abDict[bundleName].LoadAsset<T>(resName);
        }

        public void UnloadAsset(string bundleName, string resName)
        {
            if (!abDict.ContainsKey(bundleName))
            {
                Debug.Log(bundleName + "不存在");
                return;
            }
            abDict[bundleName].UnloadAsset(resName);
        }

        public void UnloadAsset(string bundleName, UnityEngine.Object asset)
        {
            if (!abDict.ContainsKey(bundleName))
            {
                Debug.Log(bundleName + "不存在");
                return;
            }
            abDict[bundleName].UnloadAsset(asset);
        }

        private void Dispose(string bundleName)
        {
            if (!abDict.ContainsKey(bundleName))
            {
                Debug.Log(bundleName + "不存在");
                return;
            }
            abDict[bundleName].Dispose();
        }

        private void DisposeAll(string bundleName)
        {
            if (!abDict.ContainsKey(bundleName))
            {
                Debug.Log(bundleName + "不存在");
                return;
            }
            abDict[bundleName].DisposeAll();
        }

        public void LogAllAssetNames(string bundleName)
        {
            if (!abDict.ContainsKey(bundleName))
            {
                Debug.Log("abDict 不存在 " + bundleName);
                return;
            }
            abDict[bundleName].LogAllAssetNames();
        }
        #endregion
    }
}