using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LeoHui;

public class PoolManager : SingletonBase<PoolManager>
{
    private Dictionary<string, Queue<Transform>> itemDict = new Dictionary<string, Queue<Transform>>();
    public override void Init()
    {
    }

    private string GetKeyName(string sceneName, string bundleName, string resName)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(sceneName);
        sb.Append("_");
        sb.Append(bundleName);
        sb.Append("_");
        sb.Append(resName);
        return sb.ToString();
    }

    public Transform Spawn(string sceneName, string bundleName, string resName)
    {
        string keyName = GetKeyName(sceneName, bundleName, resName);
        if (!itemDict.ContainsKey(keyName))
        {
            itemDict.Add(keyName, new Queue<Transform>());
        }
        Queue<Transform> itemQueue = itemDict[keyName];
        if(itemQueue.Count <= 0)
        {
            GameObject asset = AssetManager.Instance.LoadAsset<GameObject>(sceneName, bundleName, resName);
            itemQueue.Enqueue(Object.Instantiate(asset).transform);
        }
        Transform itemTran = itemQueue.Dequeue();
        itemTran.gameObject.SetActive(true);
        return itemTran;
    }

    public void Despawn(string sceneName, string bundleName, string resName, Transform itemTran)
    {
        string keyName = GetKeyName(sceneName, bundleName, resName);
        itemDict[keyName].Enqueue(itemTran);
        itemTran.gameObject.SetActive(false);
    }
}
