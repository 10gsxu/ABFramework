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
    /// 复制文件夹
    /// </summary>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    public static void CopyFolder(string from, string to)
    {
        if (!Directory.Exists(to))
            Directory.CreateDirectory(to);

        ///* 子文件夹*/
        foreach (string dir in Directory.GetDirectories(from))
            CopyFolder(dir, to + Path.GetFileName(dir) + "/");

        ///* 文件*/
        foreach (string file in Directory.GetFiles(from))
            CopyFile(file, to + Path.GetFileName(file), true);
    }

    public static void CopyFile(string from, string to, bool overWrite)
    {
        File.Copy(from, to, overWrite);
    }
}
