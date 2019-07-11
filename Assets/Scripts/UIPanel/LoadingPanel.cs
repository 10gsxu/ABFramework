using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LeoHui;

public class LoadingPanel : MonoBehaviour
{
    public Image iconImage;
    public Text progressText;

    void Start()
    {
        UpdateManager.Instance.finishCallback = FinishCallback;
        UpdateManager.Instance.decompressUpdate = DecompressUpdate;
        UpdateManager.Instance.downloadUpdate = DownloadUpdate;
        UpdateManager.Instance.sizeUpdate = SizeUpdate;
    }

    private void FinishCallback(bool isUpdate)
    {
        AssetManager.Instance.Init();
        //同步加载
        //AssetManager.Instance.SyncLoadAssetBundle("Public", "Car");
        //iconImage.sprite = AssetManager.Instance.LoadAsset<Sprite>("Public", "Car", "1");
        //异步加载
        AssetManager.Instance.AsyncLoadAssetBundle("Public", "Car", LoadFinish);

        //同步加载
        //AssetManager.Instance.SyncLoadAssetBundle("Public", "UIPanel");
        //Transform itemTran = PoolManager.Instance.Spawn("Public", "UIPanel", "CarItem");
        //异步加载
        AssetManager.Instance.AsyncLoadAssetBundle("Public", "UIPanel", LoadFinish);
    }

    private void LoadFinish(string bundleName)
    {
        Debug.Log(bundleName);
        if(bundleName == "Public/Car")
        {
            iconImage.sprite = AssetManager.Instance.LoadAsset<Sprite>("Public", "Car", "1");
        }
        if(bundleName == "Public/UIPanel")
        {
            Transform itemTran = PoolManager.Instance.Spawn("Public", "UIPanel", "CarItem");
        }
    }

    private void DecompressUpdate(int index, int total)
    {
        progressText.text = index + "/" + total;
    }

    private void DownloadUpdate(int index, int total)
    {
        progressText.text = index + "/" + total;
    }

    private void SizeUpdate(long curSize, long totalSize)
    {
        progressText.text = UtilTools.getFileSizeFormat(curSize) + "/" + UtilTools.getFileSizeFormat(totalSize);
    }
}
