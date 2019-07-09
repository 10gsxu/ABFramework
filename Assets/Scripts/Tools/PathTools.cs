using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PathTools
{
    //需要打包成AssetBundle的文件夹名称
    public static string ABResFolderName = "ABRes";
    public static string ABResPath
    {
        get
        {
            return Application.dataPath + "/" + ABResFolderName;
        }
    }

    //游戏资源的存放位置，如果路径为空，则从StreamingAssets复制过来
    public static string DataPath
    {
        get
        {
            string appName = AppConst.AppName.ToLower();
            if(Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + appName + "/";
            }
            else
            {
                return Application.dataPath.Replace("Assets", "") + appName + "/";
            }
        }
    }

    //游戏资源的默认位置，游戏包的初始资源
    public static string ResPath
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    //使用Www读取AssetBundle的Url
    public static string WwwDataPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return "file:///" + DataPath;
            }
            else
            {
                return "file://" + ResPath;
            }
        }
    }

    //根据平台，获取文件夹名称
    public static string PlatformFolderName
    {
        get
        {
            #if UNITY_EDITOR || UNITY_EDITOR_OSX
            return "Android";
            #endif
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "IOS";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                default:
                    return null;
            }
        }
    }
}
