using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorUtil {

    public const string MenuRoot = "自定义菜单/";

    const string MenuPath = EditorUtil.MenuRoot + "PlayerData/";

    [MenuItem(MenuPath + "Delete", false)]
    public static void DeletePlayerData()
    {
        FileTools.DelectFile("PlayerData");
    }

    [MenuItem(MenuPath + "Editor")]
    public static void EditorPlayerData()
    {

    }

    [MenuItem(MenuPath + "Delete", true)]
    static bool ValidatePlayerDataExists()
    {
        return FileTools.IsFileExists("PlayerData");
    }
}
