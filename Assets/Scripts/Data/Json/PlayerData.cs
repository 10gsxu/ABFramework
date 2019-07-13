using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : IJsonData<PlayerData>
{
    public PlayerData()
    {
        InitData("PlayerData", true, false);
    }
}
