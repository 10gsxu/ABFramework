using UnityEngine;
using System.Text;
using System.IO;
using System;

/// <summary>
/// 文件操作类，注意本类只针对RootPath路径下的文件操作
/// </summary>
public class FileTools
{

    public static string RootPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                string tempPath = Application.persistentDataPath, dataPath;
                if (!string.IsNullOrEmpty(tempPath))
                {

                    dataPath = PlayerPrefs.GetString("DataPath", "");
                    if (string.IsNullOrEmpty(dataPath))
                    {
                        PlayerPrefs.SetString("DataPath", tempPath);
                    }

                    return tempPath + "/";
                }
                else
                {
                    Debug.Log("Application.persistentDataPath Is Null.");

                    dataPath = PlayerPrefs.GetString("DataPath", "");

                    return dataPath + "/";
                }
            }
            else
            {
                return Application.dataPath.Replace("Assets", "");
            }
        }
    }

    /// <summary>
    /// 写文件，如果文件不存在，则直接创建一个新文件
    /// Encode in UTF-8 without BOM
    /// </summary>
    /// <param name="fileName">文件名称，注意不带路径</param>
    /// <param name="fileText">文件内容</param>
    public static void WriteFile(string fileName, string fileText)
    {
        FileStream fs = new FileStream(RootPath + fileName, FileMode.OpenOrCreate, FileAccess.Write);
        Encoding UTF8WithoutBom = new UTF8Encoding(false);
        StreamWriter sw = new StreamWriter(fs, UTF8WithoutBom);
        sw.Write(fileText);
        sw.Close();
        fs.Close();
    }

    /// <summary>
    /// 读文件
    /// </summary>
    /// <param name="fileName">文件名称，注意不带路径</param>
    public static string ReadFile(string fileName)
    {
        StreamReader sr = File.OpenText(RootPath + fileName);
        string fileContent = sr.ReadToEnd();
        sr.Close();
        return fileContent;
    }

    /// <summary>
    /// 判断文件是否存在
    /// </summary>
    /// <param name="fileName">文件名称，注意不带路径</param>
    public static bool IsFileExists(string fileName)
    {
        return File.Exists(RootPath + fileName);
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="fileName">文件名称，注意不带路径</param>
    public static void DelectFile(string fileName)
    {
        File.Delete(RootPath + fileName);
    }

    /// <summary>
    /// 判断文件夹是否存在
    /// </summary>
    /// <param name="fileName">文件夹名称，注意不带路径</param>
    public static bool IsFolderExists(string folderName)
    {
        return Directory.Exists(RootPath + folderName);
    }

    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="fileName">文件夹名称，注意不带路径</param>
    public static void CreateFolder(string folderName)
    {
        Directory.CreateDirectory(RootPath + folderName);
    }

}