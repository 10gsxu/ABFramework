using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeoHui
{
    public class ResourceData : CsvBase<ResourceData>
    {
        private Dictionary<string, int> idDict = new Dictionary<string, int>();

        public override void InitData(string text)
        {
            base.InitData(text);
            InitDict();
        }

        public override void InitDataFromFile(string filePath)
        {
            base.InitDataFromFile(filePath);
            InitDict();
        }

        private void InitDict()
        {
            idDict.Clear();
            int dataRow = GetDataRow();
            for (int i = 1; i <= dataRow; ++i)
            {
                idDict.Add(GetBundleName(i), i);
            }
        }

        public string GetBundleName(int id)
        {
            return GetProperty("BundleName", id);
        }

        public string GetBundleFullName(int id)
        {
            return GetProperty("BundleFullName", id);
        }

        public string GetMd5(int id)
        {
            return GetProperty("Md5", id);
        }

        public string GetMd5ByBundleName(string bundleName)
        {
            if (!idDict.ContainsKey(bundleName))
                return "";
            return GetProperty("Md5", idDict[bundleName]);
        }

        public string GetBundleFullNameByBundleName(string bundleName)
        {
            if (!idDict.ContainsKey(bundleName))
                return "";
            return GetProperty("BundleFullName", idDict[bundleName]);
        }

        public long GetSizeByBundleName(string bundleName)
        {
            if (!idDict.ContainsKey(bundleName))
                return 0;
            return long.Parse(GetProperty("Size", idDict[bundleName]));
        }
    }
}