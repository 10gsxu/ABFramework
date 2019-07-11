using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace LeoHui
{
    public class UtilTools
    {
        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        public static string md5(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++)
            {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string md5file(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        public static long getFileSize(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            long fileSize = fs.Length;
            fs.Close();
            return fileSize;
        }

        public static string getFileSizeFormat(long fileSize)
        {
            return string.Format("{0}M", (fileSize / 1024d / 1024).ToString("0.00"));
        }
    }
}