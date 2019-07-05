using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.IO;
using System.Text;

public class AssetBundleEditor : EditorWindow
{
    [MenuItem(EditorUtil.MenuRoot + "AssetBundle")]
    public static void OpenAssetBundleWindow()
    {
        AssetBundleEditor window = EditorWindow.GetWindow<AssetBundleEditor>();
        window.Show();
    }

    private string curVersionName = "1.0.0";
    private int curVersionCode = 1;

    private AnimBool step1flag;
    private AnimBool step2flag;
    private AnimBool step3flag;

    private BuildTarget curBuildTarget = BuildTarget.Android;
    private List<string> fileList = new List<string>();
    private List<AssetBundleItem> abItemList = new List<AssetBundleItem>();
    private bool isChange = false;

    void OnEnable()
    {
        step1flag = new AnimBool(true);
        step2flag = new AnimBool(true);
        step3flag = new AnimBool(true);

        step1flag.valueChanged.AddListener(Repaint);
        step2flag.valueChanged.AddListener(Repaint);
        step3flag.valueChanged.AddListener(Repaint);
    }

    void OnGUI()
    {
        GUILayout.Label("当前版本信息", EditorStyles.whiteLargeLabel);
        curVersionName = EditorGUILayout.TextField("版本名", curVersionName);
        curVersionCode = EditorGUILayout.IntField("版本号", curVersionCode);
        EditorGUILayout.Space();
        step1flag.target = EditorGUILayout.ToggleLeft("步骤1 - 设置AssetBundleName", step1flag.target);
        if (EditorGUILayout.BeginFadeGroup(step1flag.faded))
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("Set AssetBundleName"))
            {
                SetAssetBundleName();
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("清理缓存"))
            {
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndFadeGroup();
        step2flag.target = EditorGUILayout.ToggleLeft("步骤2 - 打包AssetBundle", step2flag.target);
        if (EditorGUILayout.BeginFadeGroup(step2flag.faded))
        {
            EditorGUILayout.Space();
            curBuildTarget = (BuildTarget)EditorGUILayout.EnumPopup("选择AssetBundle平台", curBuildTarget);
            if (GUILayout.Button("Build AssetBundle (资源打包)"))
            {
                BuildAssetBundle();
            }
            if (GUILayout.Button("复制AssetBundle 到 StreamingAsset"))
            {
                CopyAssetBundleToStreamingAssets();
            }
            if (GUILayout.Button("打开AssetBundle目录"))
            {
                EditorUtility.RevealInFinder(GetAssetBundlePath(curBuildTarget));
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndFadeGroup();
        step3flag.target = EditorGUILayout.ToggleLeft("步骤3 - 设置AssetBundle Name", step3flag.target);
        if (EditorGUILayout.BeginFadeGroup(step3flag.faded))
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("设置AssetBundle Name"))
            {
                SetAssetBundleName();
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("清理缓存"))
            {
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndFadeGroup();
    }

    private void CopyAssetBundleToStreamingAssets()
    {
        string sourcePath = GetAssetBundlePath(curBuildTarget);
        string desPath = Application.streamingAssetsPath;
        FileUtil.DeleteFileOrDirectory(desPath);
        FileUtil.CopyFileOrDirectory(sourcePath, desPath);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("AssetBundle", "Copy AssetBundle To StreamingAssets", "Finish");
    }
    #region 设置AssetBundleName
    private void SetAssetBundleName()
    {
        /* 方法局部变量 */
        //需要给AB做标记的根目录
        string strNeedSetLabelRoot = string.Empty;
        //目录信息(场景目录信息数组，表示所有的根目录下场景目录)
        DirectoryInfo[] dirScenesDIRArray = null;


        //清空无用AB包标记
        AssetDatabase.RemoveUnusedAssetBundleNames();
        //需要打包资源的文件夹根目录。
        //strNeedSetLabelRoot = Application.dataPath + "/" + "AB_Res";
        strNeedSetLabelRoot = PathTools.ABResPath;
        DirectoryInfo dirTempInfo = new DirectoryInfo(strNeedSetLabelRoot);
        dirScenesDIRArray = dirTempInfo.GetDirectories();
        //2： 遍历每个“场景”文件夹（目录）
        foreach (DirectoryInfo currentDIR in dirScenesDIRArray)
        {
            //2.1 遍历本场景目录下所有的目录或者文件。
            //如果是目录，则继续“递归”访问里面的文件，直到定位到文件
            string tmpScenesDIR = strNeedSetLabelRoot + "/" + currentDIR.Name;          //全路径
                                                                                        //DirectoryInfo tmpScenesDIRInfo = new DirectoryInfo(tmpScenesDIR);
            int tmpIndex = tmpScenesDIR.LastIndexOf("/");
            string tmpScenesName = tmpScenesDIR.Substring(tmpIndex + 1);                  //场景名称
                                                                                          // 2.2  递归调用方法， 找到文件，则使用AssetImporter类，标记“包名”与“后缀名”
            JudgeDIRorFileByRecursive(currentDIR, tmpScenesName);
        }


        //刷新
        AssetDatabase.Refresh();
        //提示信息，标记包名完成。
        Debug.Log("AssetBundle 本次操作设置标记完成！");
    }

    /// <summary>
    /// 递归判断是否为目录与文件，修改AssetBundle 的标记(lable)
    /// </summary>
    /// <param name="currentDIR">当前文件信息（文件信息与目录信息可以相互转换）</param>
    /// <param name="scenesName">当前场景名称</param>
    private void JudgeDIRorFileByRecursive(FileSystemInfo fileSysInfo, string scenesName)
    {
        //参数检查
        if (!fileSysInfo.Exists)
        {
            Debug.LogError("文件或者目录名称： " + fileSysInfo + " 不存在，请检查");
            return;
        }

        //得到当前目录下一级的文件信息集合
        DirectoryInfo dirInfoObj = fileSysInfo as DirectoryInfo;                         //文件信息转换为目录信息
        FileSystemInfo[] fileSysArray = dirInfoObj.GetFileSystemInfos();
        foreach (FileSystemInfo fileInfo in fileSysArray)
        {
            FileInfo fileinfoObj = fileInfo as FileInfo;
            //文件类型
            if (fileinfoObj != null)
            {
                //修改此文件的AssetBundle标签
                SetFileABLabel(fileinfoObj, scenesName);
            }
            //目录类型
            else
            {
                //如果是目录则递归调用
                JudgeDIRorFileByRecursive(fileInfo, scenesName);
            }
        }
    }


    /// <summary>
    /// 对指定的文件设置“AB包名称”
    /// </summary>
    /// <param name="fileinfoObj">文件信息（包含文件绝对路径）</param>
    /// <param name="scenesName">场景名称</param>
    private void SetFileABLabel(FileInfo fileinfoObj, string scenesName)
    {
        //Debug.Log(fileinfoObj.FullName);//调试
        //AssetBundle 包名称
        string strABName = string.Empty;
        //文件路径（相对路径）
        string strAssetFilePath = string.Empty;


        //参数检查（*.meta 文件不做处理）
        if (fileinfoObj.Extension == ".meta") return;
        //得到AB包名称
        strABName = GetABName(fileinfoObj, scenesName);
        //获取资源文件的相对路径
        int tmpIndex = fileinfoObj.FullName.IndexOf("Assets");
        strAssetFilePath = fileinfoObj.FullName.Substring(tmpIndex);                    //得到文件相对路径
                                                                                        //给资源文件设置AB名称以及后缀
        AssetImporter tmpImporterObj = AssetImporter.GetAtPath(strAssetFilePath);
        tmpImporterObj.assetBundleName = strABName;//这里的字符串需要替换
        if (fileinfoObj.Extension == ".unity")
        {
            //定义AB包的扩展名
            tmpImporterObj.assetBundleVariant = "u3d";
        }
        else
        {
            tmpImporterObj.assetBundleVariant = "ab";
        }
    }

    /// <summary>
    /// 获取AB包的名称
    /// </summary>
    /// <param name="fileinfoObj">文件信息</param>
    /// <param name="scenesName">场景名称</param>
    /// AB 包形成规则：
    ///     文件AB包名称=“所在二级目录名称”（场景名称）+“三级目录名称”（下一级的“类型名称”）
    /// 
    /// <returns>
    /// 返回一个合法的“AB包名称”
    /// </returns>
    private string GetABName(FileInfo fileinfoObj, string scenesName)
    {
        //Debug.Log(fileinfoObj.FullName);//调试
        //返回AB包名称
        string strABName = string.Empty;


        //Win路径
        string tmpWinPath = fileinfoObj.FullName;                                       //文件信息的全路径（Win格式）
                                                                                        //Unity路径
        string tmpUnityPath = tmpWinPath.Replace("\\", "/");                             //替换为Unity字符串分割符
                                                                                         //定位“场景名称”后面字符位置
        int tmpSceneNamePostion = tmpUnityPath.IndexOf(scenesName) + scenesName.Length;
        //AB包中“类型名称”所在区域
        string strABFileNameArea = tmpUnityPath.Substring(tmpSceneNamePostion + 1);
        //测试
        //Debug.Log("@@@strABFileNameArea:  "+ strABFileNameArea);
        if (strABFileNameArea.Contains("/"))
        {
            string[] tempStrArray = strABFileNameArea.Split('/');
            //AB包名称正式形成
            //Debug.Log("###tempStrArray[0]:  " + tempStrArray[0]);
            strABName = scenesName + "/" + tempStrArray[0];
        }
        else
        {
            //定义*.Unity 文件形成的特殊AB包名称
            strABName = scenesName + "/" + scenesName;
        }

        return strABName;
    }
    #endregion

    #region 打包AssetBundle
    private void BuildAssetBundle()
    {
        //streamAssetsPath/Windows
        string outPath = GetAssetBundlePath(curBuildTarget);
        if (!Directory.Exists(outPath))
            Directory.CreateDirectory(outPath);
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None, curBuildTarget);
        //拷贝Lua文件
        //HandleLuaFile();

        //生成对应的数据文件
        GenerateDataFile();
        //刷新文件
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取打包的AssetBundle路径
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private string GetAssetBundlePath(BuildTarget target)
    {
        return Application.dataPath.Replace("Assets", "AssetBundle") + "/" + GetFolderName(curBuildTarget) + "/";
    }

    private string GetFolderName(BuildTarget target)
    {
        switch (target)
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

    private void GenerateDataFile()
    {
        string resourceFilePath = GetAssetBundlePath(curBuildTarget) + "/resource.csv";
        EditorResourceData.Instance.Init(resourceFilePath);
        abItemList.Clear();
        isChange = false;
        string dirPath = GetAssetBundlePath(curBuildTarget) + "/";
        AddManifestAssetBundle(dirPath);
        string[] dirArr = Directory.GetDirectories(dirPath);
        for(int i=0;i<dirArr.Length; ++i)
        {
            AddAssetBundleItem(dirArr[i], dirPath);
        }

        GenerateResourceFile();
        GenerateVersionFile();
    }

    struct AssetBundleItem
    {
        public string bundleName;
        public string bundleFullName;
        public long size;
        public string md5;
    }

    private void AddManifestAssetBundle(string dirPath)
    {
        string fileName = GetFolderName(curBuildTarget);
        string filePath = dirPath + fileName;
        AssetBundleItem item = new AssetBundleItem();
        item.bundleName = fileName;
        item.bundleFullName = fileName;
        item.size = UtilTools.getFileSize(filePath);
        item.md5 = UtilTools.md5file(filePath);
        string oldMd5 = EditorResourceData.Instance.GetMd5(item.bundleName);
        isChange = oldMd5 != item.md5;
        abItemList.Add(item);
    }

    /// <summary>
    /// 根据文件夹，添加文件
    /// </summary>
    /// <param name="dirPath"></param>
    private void AddAssetBundleItem(string dirPath, string rootPath)
    {
        string[] fileArr = Directory.GetFiles(dirPath);
        for (int i = 0; i < fileArr.Length; ++i)
        {
            string fileName = fileArr[i];
            if (fileName.EndsWith(".meta") || fileName.EndsWith(".DS_Store") || fileName.EndsWith(".manifest")) continue;
            AssetBundleItem item = new AssetBundleItem();
            item.bundleName = fileName.Substring(fileName.LastIndexOf("/") + 1);
            item.bundleFullName = fileName.Replace(rootPath, "");
            item.size = UtilTools.getFileSize(fileName);
            item.md5 = UtilTools.md5file(fileName);
            string oldMd5 = EditorResourceData.Instance.GetMd5(item.bundleName);
            isChange = oldMd5 != item.md5;
            abItemList.Add(item);
        }
    }

    private void GenerateResourceFile()
    {
        StringBuilder csvFileStr = new StringBuilder();
        csvFileStr.Append("Id,BundleName,BundleFullName,Size,Md5\r");
        for (int i = 0; i < abItemList.Count; ++i)
        {
            AssetBundleItem item = abItemList[i];
            csvFileStr.Append((i + 1).ToString());
            csvFileStr.Append(",");
            csvFileStr.Append(item.bundleName);
            csvFileStr.Append(",");
            csvFileStr.Append(item.bundleFullName);
            csvFileStr.Append(",");
            csvFileStr.Append(item.size.ToString());
            csvFileStr.Append(",");
            csvFileStr.Append(item.md5);
            if (i < abItemList.Count - 1)
                csvFileStr.Append('\r');
        }
        
        string resourceFilePath = GetAssetBundlePath(curBuildTarget) + "/resource.csv";
        FileStream fs = File.Open(resourceFilePath, FileMode.Create);
        Encoding utf8WithoutBom = new UTF8Encoding(false);
        StreamWriter sw = new StreamWriter(fs, utf8WithoutBom);
        sw.Write(csvFileStr);
        sw.Close();
        fs.Close();
    }

    private void GenerateVersionFile()
    {
        int resVerCode = 0;
        string resVerName = "1.0.0";

        ReadResVer(ref resVerCode, ref resVerName);

        if (isChange)
        {
            ++resVerCode;

            string[] verNames = resVerName.Split('.');
            int[] verValues = new int[3];
            for(int i=0; i<3; ++i)
            {
                verValues[i] = int.Parse(verNames[i]);
            }
            int verNamesValue = verValues[0] * 100 + verValues[1] * 10 + verValues[2];
            ++verNamesValue;
            verValues[0] = verNamesValue / 100;
            verNamesValue -= verValues[0] * 100;
            verValues[1] = verNamesValue / 10;
            verNamesValue -= verValues[1] * 10;
            verValues[2] = verNamesValue;
            resVerName = verValues[0] + "." + verValues[1] + "." + verValues[2];
        }

        WriteResVer(resVerCode, resVerName);
    }

    public void ReadResVer(ref int resVerCode, ref string resVerName)
    {
        string versionFilePath = GetAssetBundlePath(curBuildTarget) + "/version.txt";
        if (File.Exists(versionFilePath))
        {
            FileStream fs = new FileStream(versionFilePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            resVerCode = int.Parse(sr.ReadLine());
            resVerName = sr.ReadLine();
            sr.Close();
            fs.Close();
        }
    }

    public void WriteResVer(int resVerCode, string resVerName)
    {
        string versionFilePath = GetAssetBundlePath(curBuildTarget) + "/version.txt";

        FileStream assetFile = File.Open(versionFilePath, FileMode.Create);
        Encoding utf8WithoutBom = new UTF8Encoding(false);
        StreamWriter sw = new StreamWriter(assetFile, utf8WithoutBom);
        sw.WriteLine(resVerCode.ToString());
        sw.WriteLine(resVerName);
        sw.Close();
        assetFile.Close();
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    private void Recursive(string path)
    {
        string[] fileArr = Directory.GetFiles(path);
        string[] dirArr = Directory.GetDirectories(path);
        foreach (string file in fileArr)
        {
            string ext = Path.GetExtension(file);
            if (ext.EndsWith(".meta") || ext.EndsWith(".DS_Store")) continue;
            fileList.Add(file.Replace('\\', '/'));
        }
        foreach (string dir in dirArr)
        {
            Recursive(dir);
        }
    }

    #endregion

    /// <summary>
    /// 处理Lua文件
    /// </summary>
    void HandleLuaFile()
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
            fileList.Clear();
            string luaDataPath = luaPaths[i].ToLower();
            Recursive(luaDataPath);
            int n = 0;
            foreach (string f in fileList)
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
}