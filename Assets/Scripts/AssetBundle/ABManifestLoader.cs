using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeoHui
{
    public class ABManifestLoader
    {
        private AssetBundleManifest assetBundleManifest;
        private AssetBundle assetBundle;
        private string bundlePath;
        private bool isLoadFinish;

        private static ABManifestLoader instance = null;
        public static ABManifestLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ABManifestLoader();
                }
                return instance;
            }
        }

        public ABManifestLoader()
        {
            isLoadFinish = false;
        }

        public IEnumerator AsyncLoadManifest()
        {
            bundlePath = PathTools.WwwDataPath + PathTools.PlatformFolderName;
            using (WWW www = new WWW(bundlePath))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.Log("AssetBundleManifest加载失败，地址为 : " + bundlePath);
                }
                if (www.isDone)
                {
                    assetBundle = www.assetBundle;
                    assetBundleManifest = assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                    isLoadFinish = true;
                }
            }
        }

        public void SyncLoadManifest()
        {
            bundlePath = PathTools.DataPath + PathTools.PlatformFolderName;
            assetBundle = AssetBundle.LoadFromFile(bundlePath);
            assetBundleManifest = assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
        }

        public string[] GetDependence(string bundleName)
        {
            return assetBundleManifest.GetAllDependencies(bundleName);
        }

        public void UnloadManifest()
        {
            assetBundle.Unload(true);
        }

        public bool IsLoadFinish()
        {
            return isLoadFinish;
        }
    }
}