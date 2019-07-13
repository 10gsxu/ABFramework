using System.IO;
using System.Text;

/// <summary>
/// 文件或文件夹操作类
/// </summary>
public class IOTools
{
    /// <summary>
    /// 写文件，如果文件不存在，则直接创建一个新文件
    /// Encode in UTF-8 without BOM
    /// </summary>
    /// <param name="filePath">文件路径，注意是完整路径</param>
    /// <param name="fileText">文件内容</param>
    public static void WriteFile(string filePath, string fileText)
    {
        FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
        Encoding UTF8WithoutBom = new UTF8Encoding(false);
        StreamWriter sw = new StreamWriter(fs, UTF8WithoutBom);
        sw.Write(fileText);
        sw.Close();
        fs.Close();
    }

    /// <summary>
    /// 读文件
    /// </summary>
    /// <returns>The file.</returns>
    /// <param name="filePath">文件路径，注意是完整路径</param>
    public static string ReadFile(string filePath)
    {
        StreamReader sr = File.OpenText(filePath);
        string fileContent = sr.ReadToEnd();
        sr.Close();
        return fileContent;
    }

    /// <summary>
    /// Creates the folder.
    /// </summary>
    /// <param name="folderPath">Folder path.</param>
    public static void CreateFolder(string folderPath)
    {
        Directory.CreateDirectory(folderPath);
    }

    /// <summary>
    /// Ises the folder exists.
    /// </summary>
    /// <returns><c>true</c>, if folder exists was ised, <c>false</c> otherwise.</returns>
    /// <param name="folderPath">Folder path.</param>
    public static bool IsFolderExists(string folderPath)
    {
        return Directory.Exists(folderPath);
    }

    /// <summary>
    /// 复制文件夹
    /// </summary>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    public static void CopyFolder(string sourceFolderName, string destFolderName)
    {
        if (!Directory.Exists(destFolderName))
            Directory.CreateDirectory(destFolderName);

        ///* 子文件夹*/
        foreach (string dir in Directory.GetDirectories(sourceFolderName))
            CopyFolder(dir, destFolderName + Path.GetFileName(dir) + "/");

        ///* 文件*/
        foreach (string file in Directory.GetFiles(sourceFolderName))
            CopyFile(file, destFolderName + Path.GetFileName(file), true);
    }

    /// <summary>
    /// 复制文件
    /// </summary>
    /// <param name="sourceFileName">Source file name.</param>
    /// <param name="destFileName">Destination file name.</param>
    /// <param name="overwrite">If set to <c>true</c> overwrite.</param>
    public static void CopyFile(string sourceFileName, string destFileName, bool overwrite)
    {
        File.Copy(sourceFileName, destFileName, overwrite);
    }

    /// <summary>
    /// 移动文件，从一个路径到另一个路径，或者重命名
    /// </summary>
    /// <param name="sourceFileName">Source file name.</param>
    /// <param name="destFileName">Destination file name.</param>
    public static void MoveFile(string sourceFileName, string destFileName)
    {
        File.Move(sourceFileName, destFileName);
    }
}
