using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        AssetManager.Instance.SyncLoadAssetBundle("public", "car");
        iconImage.sprite = AssetManager.Instance.LoadAsset<Sprite>("public", "car", "1");
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
