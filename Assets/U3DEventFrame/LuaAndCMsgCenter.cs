using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;
using LuaInterface;

public class LuaAndCMsgCenter : MonoBase {

    private static LuaAndCMsgCenter instance;
    public static LuaAndCMsgCenter Instance
    {
        get
        {
            return instance;
        }
    }

    LuaFunction callBack = null;

    void Awake()
    {
        instance = this;
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        if(callBack != null)
        {
            callBack.Call(tmpMsg);
        }
    }

    public void SettingLuaCallBack(LuaFunction luaFunc)
    {
        callBack = luaFunc;
    }

}
