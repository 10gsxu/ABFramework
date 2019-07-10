using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string serverUrl = "https://";
    public AssetType assetType = AssetType.None;
}
