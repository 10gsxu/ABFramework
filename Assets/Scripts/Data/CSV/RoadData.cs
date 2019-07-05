using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadData : ICSVData<RoadData>
{
    public RoadData()
    {
        InitData("RoadData");
    }

    public int[] GetBlockArr(int id)
    {
        int count = 0;
        for (int i = 0; i < 20; i++)
        {
            if (GetProperty("block" + (i + 1), id) == "" || GetProperty("block" + (i + 1), id) == null)
            {
                count = i;
                break;
            }
        }
        int[] block = new int[count];
        for (int i = 0; i < count; i++)
        {
            block[i] = int.Parse(GetProperty("block" + (i + 1), id));
        }
        return block;
    }

    public float getMaxSpeed(int id)
    {
        return float.Parse(GetProperty("speedmax", id));
    }
}
