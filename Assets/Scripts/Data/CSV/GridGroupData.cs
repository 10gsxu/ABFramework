using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGroupData : ICSVData<GridGroupData>
{
    public GridGroupData()
    {
        InitData("GridGroupData");
    }

    public int[] GetBlockArr(int id)
    {
        int count = 0;
        for (int i = 0; i < 15; i++)
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
}
