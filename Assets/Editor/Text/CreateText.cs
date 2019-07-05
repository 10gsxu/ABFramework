using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;

public class CreateText {
	[MenuItem("Assets/Create/Text Sctipt",false,80)]
	public static void CreatNewLua() {
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(),
			GetSelectedPathOrFallback() + "/New Text.txt", null, "Assets/Editor/Text/Template/text.txt");
	}

	public static string GetSelectedPathOrFallback() {
		string path = "Assets";
		foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
			path = AssetDatabase.GetAssetPath(obj);
			if (!string.IsNullOrEmpty(path)&&File.Exists(path)) {
				path = Path.GetDirectoryName(path);
				break;
			}
		}
		return path;
	}
}