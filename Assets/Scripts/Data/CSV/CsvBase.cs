using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LeoHui
{
    /// <summary>
    /// 数据读取的基类
    /// </summary>
    public class CsvBase<T> where T : new()
    {
        //存数数据
        protected Hashtable DataTable = new Hashtable();
        protected List<List<string>> levelArray = new List<List<string>>();

        protected int DataRow;

        public virtual void InitDataFromFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            InitData(sr.ReadToEnd());
            sr.Close();
            fs.Close();
        }

        //设置读取数据表的名称
        public virtual void InitData(string text)
        {
            DataTable.Clear();
            levelArray.Clear();

            //读取每一行的内容
            string[] lineArray = text.Split('\r');
            if (lineArray.Length <= 1)
                lineArray = text.Split('\n');
            //创建二维数组
            for (int i = 0; i < lineArray.Length; ++i)
            {
                levelArray.Add(new List<string>());
            }
            //把csv中的数据储存在二位数组中
            for (int i = 0; i < lineArray.Length; ++i)
            {
                levelArray[i].AddRange(lineArray[i].Split(','));
            }

            //将数据存储到哈希表中，存储方法：Key为name+id，Value为值
            int nRow = levelArray.Count;
            int nCol = levelArray[0].Count;

            DataRow = nRow - 1;

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < levelArray.Count; ++i)
            {
                if (levelArray[i][0] == "\n" || levelArray[i][0] == "")
                {
                    nRow--;
                    DataRow = nRow - 1;
                    continue;
                }

                string id = levelArray[i][0].Trim();

                for (int j = 1; j < nCol; ++j)
                {
                    sb.Append(levelArray[0][j]);
                    sb.Append("_");
                    sb.Append(id);

                    DataTable.Add(sb.ToString(), levelArray[i][j]);

                    sb.Length = 0;
                }
            }
        }

        /// <summary>
        /// Gets the data row.
        /// 返回表格的行数
        /// </summary>
        /// <returns>
        /// The data row.
        /// </returns>
        public virtual int GetDataRow()
        {
            return DataRow;
        }

        //根据name和id获取相关属性，返回string类型
        protected virtual string GetProperty(string name, int id)
        {
            return GetProperty(name, id.ToString());
        }

        StringBuilder keysb = new StringBuilder();

        protected virtual string GetProperty(string name, string id)
        {
            keysb.Length = 0;
            keysb.Append(name);
            keysb.Append("_");
            keysb.Append(id);
            if (DataTable.ContainsKey(keysb.ToString()))
                return DataTable[keysb.ToString()].ToString();
            else
                return "";
        }

        public void JustInit()
        {
        }


        public int GetPropertyToInt(string name, int id)
        {
            return GetPropertyToInt(name, id.ToString());
        }

        public int GetPropertyToInt(string name, string id)
        {
            string str = GetProperty(name, id).ToString();

            if (string.IsNullOrEmpty(str))
                return 0;

            int iData = int.Parse(str);
            return iData;
        }

        public float GetPropertyToFloat(string name, int id)
        {
            string str = GetProperty(name, id).ToString();

            if (string.IsNullOrEmpty(str))
                return 0f;

            float fData = float.Parse(str);
            return fData;
        }

        public string GetPropertyToString(string name, int id)
        {
            string str = GetProperty(name, id).ToString();

            return str;
        }

    }
}