using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarData : ICSVData<CarData>
{
    public CarData()
    {
        InitData("CarData");
    }

    public int getSpeed(int id)
    {
        return int.Parse(GetProperty("speed", id));
    }
}
