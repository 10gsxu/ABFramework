using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame
{
    public class GameManager : ManagerBase
    {

        public static GameManager Instance;

        void Awake()
        {
            Instance = this;
        }

        public override void SendMsg(MsgBase msg)
        {
            if ((ManagerID)msg.GetManager() == ManagerID.GameManager)
            {
                //ManagerBase 本模块自己处理
                ProcessEvent(msg);
            }
            else
            {
                MsgCenter.Instance.ProcessEvent(msg);
            }
        }

    }
}
