using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U3DEventFrame
{
    public class GameBase : MonoBase
    {

        public override void SendMsg(MsgBase msg)
        {
            GameManager.Instance.SendMsg(msg);
        }

        public override void ProcessEvent(MsgBase tmpMsg)
        {
            //throw new System.NotImplementedException ();
        }

        public void RegistSelf(MonoBase mono, params ushort[] msgs)
        {
            GameManager.Instance.RegistMsg(mono, msgs);
        }

        public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
        {
            GameManager.Instance.UnRegistMsg(mono, msgs);
        }

        public ushort[] msgIds;

        void OnDestroy()
        {
            if (msgIds != null)
            {
                UnRegistSelf(this, msgIds);
            }
        }
    }
}
