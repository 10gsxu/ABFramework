using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
#if UNITY_EDITOR || UNITY_EDITOR_OSX
using UnityEditor;
using System.IO;
#endif

public class CallBackNode
{
    public string bundleName;
    public LuaFunction luaFunc;

    public CallBackNode(string bundleName, LuaFunction luaFunc)
    {
        this.bundleName = bundleName;
        this.luaFunc = luaFunc;
    }

    public void Dispose()
    {
        this.bundleName = null;
        this.luaFunc.Dispose();
    }
}

public class CallBackManager
{
    Dictionary<string, CallBackNode> manager = null;

    public CallBackManager()
    {
        manager = new Dictionary<string, CallBackNode>();
    }

    public void AddBundleCallBack(string bundle, CallBackNode tmpNode)
    {
        manager.Add(bundle, tmpNode);
    }

    public void Dispose(string bundle)
    {
        if(manager.ContainsKey(bundle))
        {
            manager.Remove(bundle);
        }
    }

    public void CallBackRes(string bundle)
    {
        if (manager.ContainsKey(bundle))
        {
            CallBackNode cbNpde = manager[bundle];
            cbNpde.luaFunc.Call(cbNpde.bundleName);
        }
        else
        {
            Debug.LogWarning("not contain bundle == " + bundle);
        }
    }

}

public class LuaLoadRes {
    private static LuaLoadRes instance;
    public static LuaLoadRes Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new LuaLoadRes();
            }
            return instance;
        }
    }

    private CallBackManager callbackManager;
    public CallBackManager CBManager
    {
        get
        {
            if (callbackManager == null)
            {
                callbackManager = new CallBackManager();
            }
            return callbackManager;
        }
    }

    void LoadFinishCB(string bundleName)
    {
        CBManager.CallBackRes(bundleName);
        CBManager.Dispose(bundleName);
    }

    #if UNITY_EDITOR || UNITY_EDITOR_OSX
    private T LoadAsset<T>(string bundleName, string resName) where T : UnityEngine.Object
    {
        string filePath = "";
        string ext = ".prefab";
        switch (bundleName)
        {
            case "Data":
                ext = ".csv";
                filePath = "Assets/Bundles/" + bundleName + "/Csv/" + resName + ext;
                if (!File.Exists(filePath))
                {
                    ext = ".txt";
                    filePath = "Assets/Bundles/" + bundleName + "/Json/" + resName + ext;
                }
                break;
            case "Audio":
            case "UIPanel":
                ext = ".prefab";
                filePath = "Assets/Bundles/" + bundleName + "/" + resName + ext;
                break;
            case "Sprite":
            case "Car":
                ext = ".png";
                filePath = "Assets/Bundles/" + bundleName + "/" + resName + ext;
                break;
        }
        return AssetDatabase.LoadAssetAtPath<T>(filePath);
    }
    #endif

    //加载AssetBundle
    public void LoadAssetBundle(string bundleName, LuaFunction luaFunc)
    {
        bundleName = bundleName.ToLower() + AppConst.ExtName;
        CallBackNode tmpNode = new CallBackNode(bundleName, luaFunc);
        CBManager.AddBundleCallBack(bundleName, tmpNode);
        ILoadManager.Instance.LoadAssetBundle(bundleName, LoadFinishCB);
    }

    public Sprite LoadSprite(string bundleName, string resName)
    {
        if (AppConst.DebugMode)
        {
            #if UNITY_EDITOR || UNITY_EDITOR_OSX
            return LoadAsset<Sprite>(bundleName, resName);
            #endif
            return null;
        }
        else
        {
            bundleName = bundleName.ToLower() + AppConst.ExtName;
            return ILoadManager.Instance.LoadAsset<Sprite>(bundleName, resName);
        }
    }

    public TextAsset LoadTextAsset(string bundleName, string resName)
    {
        if (AppConst.DebugMode)
        {
            #if UNITY_EDITOR || UNITY_EDITOR_OSX
            return LoadAsset<TextAsset>(bundleName, resName);
            #endif
            return null;
        }
        else
        {
            bundleName = bundleName.ToLower() + AppConst.ExtName;
            return ILoadManager.Instance.LoadAsset<TextAsset>(bundleName, resName);
        }
    }

    public UnityEngine.Object LoadObject(string bundleName, string resName)
    {
        if (AppConst.DebugMode)
        {
            #if UNITY_EDITOR || UNITY_EDITOR_OSX
            return LoadAsset<UnityEngine.Object>(bundleName, resName);
            #endif
            return null;
        }
        else
        {
            bundleName = bundleName.ToLower() + AppConst.ExtName;
            return ILoadManager.Instance.LoadAsset<UnityEngine.Object>(bundleName, resName);
        }
    }

    public void UnLoadResObj(string sceneName, string bundleName, string resName)
    {
        if (AppConst.DebugMode)
            return;
        ILoadManager.Instance.UnLoadResObj(sceneName, bundleName, resName);
    }
}
