﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EditorResourceData : CsvBase<EditorResourceData>
{
    private static EditorResourceData instance;
    public static EditorResourceData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EditorResourceData();
            }
            return instance;
        }
    }

    private bool isExist = false;
    private Dictionary<string, int> idDict = new Dictionary<string, int>();

    public override void InitDataFromFile(string filePath)
    {
        isExist = File.Exists(filePath);
        if (isExist) {
            base.InitDataFromFile(filePath);
            InitDict();
        }
    }

    private void InitDict()
    {
        idDict.Clear();
        int dataRow = GetDataRow();
        for(int i=1; i<=dataRow; ++i)
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

    public string GetMd5(string bundleName)
    {
        if (!isExist || !idDict.ContainsKey(bundleName))
        {
            return "";
        }
        return GetProperty("Md5", idDict[bundleName]);
    }
}