using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABManifestLoader {
    public AssetBundleManifest assetBundleManifest;
    public string manifestPath;

    private bool isLoadFinish;

    public AssetBundle manifestLoader;

    public IABManifestLoader()
    {
        assetBundleManifest = null;
        manifestLoader = null;
        isLoadFinish = false;

        manifestPath = PathTools.WwwDataPath + "assetbundle/assetbundle";
    }

    public IEnumerator LoadManifest()
    {
        WWW manifest = new WWW(manifestPath);
        yield return manifest;
        if(!string.IsNullOrEmpty(manifest.error))
        {
            Debug.Log(manifest.error);
        } else
        {
            if(manifest.progress >= 1.0f)
            {
                manifestLoader = manifest.assetBundle;
                assetBundleManifest = manifestLoader.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                isLoadFinish = true;
            }
        }
    }

    public string[] GetDependences(string bundleName)
    {
        return assetBundleManifest.GetAllDependencies(bundleName);
    }

    public void UnloadManifest()
    {
        manifestLoader.Unload(true);
    }

    private static IABManifestLoader instance = null;
    public static IABManifestLoader Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new IABManifestLoader();
            }
            return instance;
        }
    }

    public bool IsLoadFinish()
    {
        return isLoadFinish;
    }
}
