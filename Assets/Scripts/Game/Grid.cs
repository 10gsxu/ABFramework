using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid  {

    public int dir;
    public Vector3 pos;
    public float width;
    public float length;
    public int resId;


    public Grid(int resId, int dir,Vector3 pos,float width,float length){
        this.resId = resId;
        this.dir = dir;
        this.pos = pos;
        this.width = width;
        this.length = length;
    }
}
