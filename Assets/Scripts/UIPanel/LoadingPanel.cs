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
    }

    private void FinishCallback(bool isUpdate)
    {
        AssetManager.Instance.SyncLoadAssetBundle("public", "car");
        iconImage.sprite = AssetManager.Instance.LoadAsset<Sprite>("public", "car", "1");
    }

    private void DecompressUpdate(int index, int total)
    {
        progressText.text = index + "/" + total;
    }
}
