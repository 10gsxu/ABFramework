using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridResData : ICSVData<GridResData>
{
    public GridResData()
    {
        InitData("GridResData");
    }

    public float GetGridResLength(int id)
    {
        return float.Parse(GetProperty("length", id));
    }

    public float GetGridResWidth(int id)
    {
        return float.Parse(GetProperty("width", id));
    }

    public string GetGridResName(int id)
    {
        return GetProperty("name", id);
    }
}
