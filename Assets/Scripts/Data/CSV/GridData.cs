using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData : ICSVData<GridData>
{
    public GridData()
    {
        InitData("GridData");
    }

    public float GetGridLength(int id)
    {
        return float.Parse(GetProperty("length", id));
    }

    public float GetGridWidth(int id)
    {
        return float.Parse(GetProperty("width", id));
    }

    public int GetGridDir(int id)
    {
        return int.Parse(GetProperty("dir", id));
    }

    public int GetGridResId(int id)
    {
        return int.Parse(GetProperty("resId", id));
    }

    public int GetGridHeight(int id)
    {
        return int.Parse(GetProperty("liftHeight", id));
    }

//    public int GetGridResId(int id)
//    {
//        return int.Parse(GetProperty("resId", id));
//    }
}
