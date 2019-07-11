using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeoHui;

[CreateAssetMenu(menuName = "LeoHui/UpdateConfig")]
public class UpdateConfig : ScriptableObject
{
    private static UpdateConfig instance;
    public static UpdateConfig Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<UpdateConfig>("Config/UpdateConfig");
            return instance;
        }
    }

    [Header("资源地址")]
    public string serverUrl = "https://xiaochu.gyatechnology.com:3993/cardrift/";
    [Header("资源类型")]
    public AssetType assetType = AssetType.None;
    [Header("启动更新模式")]
    public bool UpdateMode = true;
    [Header("游戏帧频")]
    public int FrameRate = 30;
    [Header("游戏名称")]
    public string AppName = "CarDrift";
    [Header("AssetBundle后缀")]
    public string ExtName = "ab";
}
