using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;

public class LuaEventProcess : MonoBase {

    private static LuaEventProcess instance;
    public static LuaEventProcess Instance
    {
        get
        {
            return instance;
        }
    }

    private MonoBase monoChild;

    void Awake()
    {
        instance = this;
    }

    public void setMonoChild(MonoBase tmpChild) {
        monoChild = tmpChild;
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        if(monoChild != null)
        {
            monoChild.ProcessEvent(tmpMsg);
        }
    }
}
