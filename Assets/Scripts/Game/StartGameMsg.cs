using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;

public class StartGameMsg : MsgBase
{
    public int carId;
    public int gameLevel;
    public int gameMode;
    public StartGameMsg(ushort msgId, int carId, int gameLevel, int gameMode)
    {
        this.msgId = msgId;
        this.carId = carId;
        this.gameLevel = gameLevel;
        this.gameMode = gameMode;
    }
}
