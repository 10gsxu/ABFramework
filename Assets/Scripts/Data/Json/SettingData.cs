using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingData : IJsonData<SettingData>
{
    public SettingData()
    {
        InitData("SettingData", true, false);
    }
}
