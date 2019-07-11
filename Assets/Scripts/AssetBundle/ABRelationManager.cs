using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LeoHui
{
    public class ABRelationManager
    {
        private List<string> dependenceList;
        private List<string> referenceList;
        private ABLoader abLoader;
        private string bundleName;
        private LoadFinish loadFinish;

        public ABRelationManager(string bundleName, LoadFinish loadFinish)
        {
            dependenceList = new List<string>();
            referenceList = new List<string>();

            this.bundleName = bundleName;
            this.loadFinish = loadFinish;
            abLoader = new ABLoader(bundleName, loadFinish);
        }

        public LoadFinish GetLoadFinish()
        {
            return this.loadFinish;
        }

        public void AddReference(string bundleName)
        {
            referenceList.Add(bundleName);
        }

        public List<string> GetReference()
        {
            return referenceList;
        }

        public void SetDependence(string[] dependence)
        {
            dependenceList.AddRange(dependence);
        }

        public List<string> GetDependence()
        {
            return dependenceList;
        }

        public void RemoveDependence(string bundleName)
        {
            dependenceList.Remove(bundleName);
        }

        public void RemoveReference(string bundleName)
        {
            referenceList.Remove(bundleName);
        }

        #region 由下层提供API
        //unity3d 5.3以上 协程 才可以多层传递
        public IEnumerator AsyncLoadAssetBundle()
        {
            yield return abLoader.AsyncLoadAssetBundle();
        }

        public void SyncLoadAssetBundle()
        {
            abLoader.SyncLoadAssetBundle();
        }

        public T LoadAsset<T>(string resName) where T : UnityEngine.Object
        {
            return abLoader.LoadAsset<T>(resName);
        }

        public void UnloadAsset(UnityEngine.Object asset)
        {
            abLoader.UnloadAsset(asset);
        }

        public void UnloadAsset(string resName)
        {
            abLoader.UnloadAsset(resName);
        }

        public void Dispose()
        {
            abLoader.Dispose();
        }

        public void DisposeAll()
        {
            abLoader.DisposeAll();
        }

        public void LogAllAssetNames()
        {
            abLoader.LogAllAssetNames();
        }
        #endregion
    }
}