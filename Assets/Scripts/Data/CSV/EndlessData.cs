using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessData : ICSVData<EndlessData>
{
    public EndlessData()
    {
        InitData("EndlessData");
    }

    public int getRange(int id)
    {
        return int.Parse(GetProperty("range", id));
    }

    public int getGroupLeft(int id)
    {
        int roadId = 0;
        string groupLeft = GetProperty("groupLeft", id);
        char[] separator = new char[] { ';' };
        string[] strArr = groupLeft.Split(separator);
        if (strArr.Length > 1)
        {
            for (int i = 0; i < strArr.Length; i++)
            {

            }
        }
        else
        {
            char[] separator2 = new char[] { '|' };
            string[] strArr2 = strArr[0].Split(separator2);
            roadId = int.Parse(strArr2[0]);
        }
        return roadId;
    }

    public string getGroupRigth(int id)
    {
        return GetProperty("groupRight", id);
    }

}
