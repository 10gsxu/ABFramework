using UnityEngine;
using System.Text;
using System.IO;
using System;

/// <summary>
/// 文件操作类
/// </summary>
public class FileTools
{

    public static string RootPath
    {
        get
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
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
            else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                ///*如果是电脑的编辑模式，先放在项目外面*/
                return Application.dataPath.Replace("Assets", "");
            }
            else
            {
                return Application.dataPath + "/";
            }
        }
    }

    /// <summary>
    /// 写文件操作
    /// 指定路径文件不存在会被创建
    /// </summary>
    /// <param name="path">文件路径（包含Application.persistentDataPath）.</param>
    /// <param name="name">文件名.</param>
    /// <param name="info">写入内容.</param>
    public static void CreateOrWriteFile(string fileName, string info)
    {
        FileStream fs = new FileStream(RootPath + fileName, FileMode.Create, FileAccess.Write);
        fs.Close();
        StreamWriter sw = new StreamWriter(RootPath + fileName);
        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }

    /// <summary>
    /// 写文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileText"></param>
    public static void WriteFile(string filePath, string fileText)
    {
        FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
        Encoding utf8WithoutBom = new UTF8Encoding(false);
        StreamWriter sw = new StreamWriter(fs, utf8WithoutBom);
        sw.Write(fileText);
        sw.Close();
        fs.Close();
    }

    /// <summary>
    /// 读取文件内容  仅读取第一行
    /// </summary>
    /// <returns>The file.</returns>
    /// <param name="path">Path.</param>
    /// <param name="name">Name.</param>
    public static string ReadFile(string fileName, bool onlyreadline = false)
    {
        string fileContent = "";
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(RootPath + fileName);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return null;
        }
        if (onlyreadline)
        {
            while ((fileContent = sr.ReadLine()) != null)
            {
                break;
            }
        }
        else
        {
            fileContent = sr.ReadToEnd();
        }
        sr.Close();
        sr.Dispose();
        return fileContent;
    }

    public static bool IsFileExists(string fileName)
    {
        return File.Exists(RootPath + fileName);
    }

    public static void DelectFile(string fileName)
    {
        File.Delete(RootPath + fileName);
    }

    public static bool IsFolderExists(string folderName)
    {
        return Directory.Exists(RootPath + folderName);
    }

    public static void CreateFolder(string folderName)
    {
        Directory.CreateDirectory(RootPath + folderName);
    }

    /// <summary>
    /// 复制文件夹
    /// </summary>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    public static void CopyFolder(string from, string to)
    {
        if (!Directory.Exists(to))
            Directory.CreateDirectory(to);

        ///* 子文件夹*/
        foreach (string sub in Directory.GetDirectories(from))
            CopyFolder(sub, to + Path.GetFileName(sub) + "/");

        ///* 文件*/
        foreach (string file in Directory.GetFiles(from))
            File.Copy(file, to + Path.GetFileName(file), true);
    }

    public static void CopyFile(string from, string to, bool overWrite)
    {
        File.Copy(from, to, overWrite);
    }

}