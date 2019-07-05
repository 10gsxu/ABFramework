using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleEditor : Editor
{
    const string MenuPath = EditorUtil.MenuRoot + "AssetBundle/";

    public static string GetFolderName(BuildTarget target)
    {
        switch(target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "IOS";
            case BuildTarget.StandaloneWindows:
                return "Windows";
            default:
                return null;
        }
    }

    [MenuItem(MenuPath + "BuildAssetBundle")]
    public static void BuildAssetBundle()
    {
        //streamAssetsPath/Windows
        string outPath = PathTools.ResPath + GetFolderName(EditorUserBuildSettings.activeBuildTarget) + "/";
        if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);
        outPath += "assetbundle";
        if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);
        BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);
        //拷贝Lua文件
        HandleLuaFile();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 处理Lua文件
    /// </summary>
    static void HandleLuaFile()
    {
        string resPath = PathTools.ResPath + GetFolderName(EditorUserBuildSettings.activeBuildTarget) + "/";
        string luaPath = resPath + "lua/";

        //----------复制Lua文件----------------
        if (!Directory.Exists(luaPath))
        {
            Directory.CreateDirectory(luaPath);
        }
        string[] luaPaths = { Application.dataPath + "/Lua/",
                              Application.dataPath + "/Tolua/Lua/" };

        for (int i = 0; i < luaPaths.Length; i++)
        {
            files.Clear();
            string luaDataPath = luaPaths[i].ToLower();
            Recursive(luaDataPath);
            int n = 0;
            foreach (string f in files)
            {
                if (f.EndsWith(".meta")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string newpath = luaPath + newfile;
                string path = Path.GetDirectoryName(newpath);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
                File.Copy(f, newpath, true);
            }
        }
    }

    static List<string> files = new List<string>();
    [MenuItem(MenuPath + "BuildFileTxt")]
    public static void BuildFileTxt()
    {
        string resPath = PathTools.ResPath + GetFolderName(EditorUserBuildSettings.activeBuildTarget) + "/";
        ///----------------------创建文件列表-----------------------
        string newFilePath = resPath + "files.txt";
        if (File.Exists(newFilePath)) File.Delete(newFilePath);
        //Debug.Log(newFilePath);
        files.Clear();
        Recursive(resPath);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            string ext = Path.GetExtension(file);
            if (ext.EndsWith(".meta") || file.Contains(".DS_Store")) continue;
            string md5 = UtilTools.md5file(file);
            int index = file.IndexOf("Assets");
            long fileSize = UtilTools.getFileSize(file);
            string value = file.Replace(resPath, string.Empty);
            sw.WriteLine(value + "|" + md5 + "|" + fileSize);
        }
        sw.Close();
        fs.Close();

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    private static void Recursive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.EndsWith(".meta") || filename.Contains(".DS_Store")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            Recursive(dir);
        }
    }

    [MenuItem(MenuPath + "MarkAssetBundle")]
    public static void MarkAssetBundle()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        string path = Application.dataPath + "/Bundles";
        /*
        DirectoryInfo dir = new DirectoryInfo(path);
        FileSystemInfo[] fileInfos = dir.GetFileSystemInfos();
        for (int i = 0; i < fileInfos.Length; ++i)
        {
            FileSystemInfo tmpFile = fileInfos[i];
            if (tmpFile is DirectoryInfo)
            {
                string tmpPath = Path.Combine(path, tmpFile.Name);
                SceneOverView(tmpPath);
            }
        }
        */
        SceneOverView(path);
        AssetDatabase.Refresh();
    }
    //遍历整个场景
    public static void SceneOverView(string scenePath)
    {
        /*
        string textFileName = "Record.txt";
        string tmpPath = scenePath + "/" + textFileName;
        Debug.Log("tmpPath == " + tmpPath);
        FileStream fs = new FileStream(tmpPath, FileMode.OpenOrCreate);
        StreamWriter bw = new StreamWriter(fs);
        */      
        ChangerHead(scenePath);
        /*
        bw.WriteLine(readDict.Count);
        foreach (string key in readDict.Keys)
        {
            bw.Write(key);
            bw.Write(" ");
            bw.Write(readDict[key]);
            bw.Write("\n");
        }

        bw.Close();
        fs.Close();
        */      
    }

    //截取相对路径    D:/ToLuaFish/Assets/Art/Scenes/  SceneOne
    // sceneone/load
    /// <summary>
    /// Changers the head.
    /// </summary>
    /// <param name="fullPath">Full path.</param>
    /// <param name="theWriter">The writer.文本记录</param>
    public static void ChangerHead(string fullPath)
    {
        //得到的是D:/ToLuaFish/总长度
        int tmpCount = fullPath.IndexOf("Assets");
        int tmpLength = fullPath.Length;
        //Assets/Art/Scenes/  SceneOne
        string replacePath = fullPath.Substring(tmpCount, tmpLength - tmpCount);
        DirectoryInfo dir = new DirectoryInfo(fullPath);
        Debug.Log(dir.Name);
        if (dir != null)
        {
            //Debug.Log("replacePath == " + replacePath);
            ListDirs(dir);
        }
        else
        {
            Debug.Log("the path is not exist");
        }
    }

    public static void ListDirs(DirectoryInfo dir)
    {
        DirectoryInfo[] dirs = dir.GetDirectories();
        for (int i = 0; i < dirs.Length; ++i)
        {
            DirectoryInfo child = dirs[i];
            ListFiles(child, child.Name);
        }
    }

    //遍历场景中的每一个功能文件夹
    public static void ListFiles(FileSystemInfo info, string dirName)
    {
        DirectoryInfo dir = info as DirectoryInfo;
        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; ++i)
        {
            FileInfo file = files[i] as FileInfo;
            //对于文件的操作
            if (file != null)
            {
                ChangerMark(file, dirName);
            }
            else//对于目录的操作
            {
                ListFiles(files[i], dirName);
            }
        }
    }

    public static string FixedWindowsPath(string path)
    {
        path = path.Replace("\\", "/");
        return path;
    }

    public static void ChangeAssetMark(FileInfo tmpFile, string markStr)
    {
        string fullPath = tmpFile.FullName;
        int assetCount = fullPath.IndexOf("Assets");
        string assetPath = fullPath.Substring(assetCount, fullPath.Length - assetCount);
        Debug.Log(assetPath);
        //Assets/Art/Scenes/SceneOne/TestOne.Prefab
        AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        //以下是改变标记
        importer.assetBundleName = markStr;
        if (tmpFile.Extension == ".unity")
        {
            importer.assetBundleVariant = "u3d";
        }
        else
        {
            importer.assetBundleVariant = "ld";
        }

        // Load  -- SceneOne/load
        string modelName = "";
        string[] subMark = markStr.Split("/".ToCharArray());
        if (subMark.Length > 1)
        {
            modelName = subMark[1];
        }
        else
        {
            // SceneOne  -- SceneOne
            modelName = markStr;
        }
        //sceneone/load.ld
        string modelPath = markStr.ToLower() + "." + importer.assetBundleVariant;
    }

    //改变物体的tag
    public static void ChangerMark(FileInfo tmpFile, string replacePath)
    {
        if (tmpFile.Extension == ".meta" || tmpFile.Name.Contains(".DS_Store"))
        {
            return;
        }
        string markStr = replacePath;
        Debug.Log("markStr == " + markStr);
        ChangeAssetMark(tmpFile, markStr);
    }
}
