using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR || UNITY_EDITOR_OSX
using UnityEditor;
#endif

namespace LeoHui
{
    public class AssetManager : MonoBehaviour
    {
        public static AssetManager Instance;
        private ABSceneManager sceneManager;
        private AssetType assetType = AssetType.None;

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
            if (Application.isMobilePlatform)
            {
                assetType = AssetType.AssetBundle;
            }
            else
            {
                assetType = AssetType.AssetDatabase;
            }
#if UNITY_EDITOR || UNITY_EDITOR_OSX
            if (UpdateConfig.Instance.assetType != AssetType.None)
            {
                assetType = UpdateConfig.Instance.assetType;
            }
#endif
        }

        private void OnDestroy()
        {
            System.GC.Collect();
        }

        public void LoadAssetBundleCallBack(string sceneName, string bundleFullName)
        {
            StartCoroutine(sceneManager.AsyncLoadAssetBundle(sceneName, bundleFullName));
        }

        //异步加载
        public void AsyncLoadAssetBundle(string sceneName, string bundleName, LoadFinish loadFinish)
        {
            sceneManager.AsyncLoadAssetBundle(sceneName, bundleName, loadFinish, LoadAssetBundleCallBack);
        }

        //同步加载
        public void SyncLoadAssetBundle(string sceneName, string bundleName)
        {
            sceneManager.SyncLoadAssetBundle(sceneName, bundleName);
        }

        #region 由下层API提供
        public T LoadAsset<T>(string sceneName, string bundleName, string resName) where T : UnityEngine.Object
        {
#if UNITY_EDITOR || UNITY_EDITOR_OSX
            if (assetType == AssetType.AssetDatabase)
            {
                string filePath = "Assets/" + PathTools.ABResFolderName + "/" + sceneName + "/" + bundleName + "/" + resName;
                switch (typeof(T).FullName)
                {
                    case "UnityEngine.GameObject":
                        filePath += ".prefab";
                        break;
                    case "UnityEngine.Sprite":
                        filePath += ".png";
                        break;
                    case "UnityEngine.TextAsset":
                        filePath += ".txt";
                        break;
                }
                return AssetDatabase.LoadAssetAtPath<T>(filePath);
            }
#endif
            if (assetType == AssetType.Resources)
            {
                string filePath = sceneName + "/" + bundleName + "/" + resName;
                return Resources.Load<T>(filePath);
            }
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
}
