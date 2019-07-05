using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCreator : MonoBehaviour
{

    public Transform itemParent;
    private int gameType;
    public Grid firstGrid;
    private int[] block;
    private int resID = 0;
    private Grid lastItem;
    private int gridNum;
    private List<Grid> girdList = new List<Grid>();
    private List<int> rebornPointArr = new List<int>();
    private List<GameObject> roadItem = new List<GameObject>();
    private float roadLength;


    public static RoadCreator Instance;

    void Awake()
    {
        Instance = this;
    }

    public void creatRoad(int level, int gameType)
    {
        this.gameType = gameType;
        if (gameType == 1)
        {
            int gameId = 900;
            int[] blockArr = RoadData.Instance.GetBlockArr(gameId);
            gridNum = blockArr[blockArr.Length - 1];
            for (int i = 0; i < blockArr.Length; i++)
            {
                getGridGroup(blockArr[i]);
            }
            EndlessData.Instance.getGroupLeft(3);
        }
    }

    public void creatRandomRoad(){

    }

    public void getGridGroup(int id)
    {
        int[] girdGroup = GridGroupData.Instance.GetBlockArr(id);
        for (int i = 0; i < girdGroup.Length; i++)
        {
            int resId = GridData.Instance.GetGridResId(girdGroup[i]);
            string resName = GridResData.Instance.GetGridResName(resId);
            float width = GridData.Instance.GetGridWidth(girdGroup[i]);
            float length = GridData.Instance.GetGridLength(girdGroup[i]);
            roadLength += length;
            float scaleX = width / GridResData.Instance.GetGridResWidth(resId);
            float scaleZ = length / GridResData.Instance.GetGridResLength(resId);
            int dir = GridData.Instance.GetGridDir(girdGroup[i]);
            Vector3 pos = new Vector3(0, 0, 0);
            if (resID == 0)
            {
                pos = new Vector3(0, 0, 0);
            }
            else
            {
                if (dir == 0)
                {
                    if (lastItem.dir == 1)
                    {
                        pos = new Vector3(lastItem.pos.x + lastItem.width, lastItem.pos.y, lastItem.pos.z - (lastItem.length - width));
                    }
                    else
                    {
                        pos = new Vector3(lastItem.pos.x + lastItem.length, lastItem.pos.y, lastItem.pos.z);
                    }
                }
                else if (lastItem.dir == 0)
                {
                    pos = new Vector3(lastItem.pos.x + (lastItem.length - width), lastItem.pos.y, lastItem.pos.z - lastItem.width);
                }
                else
                {
                    pos = new Vector3(lastItem.pos.x, lastItem.pos.y, lastItem.pos.z - lastItem.length);
                }
            }
            Grid gridInfo = new Grid(resID, dir, pos, width, length);
            girdList.Add(gridInfo);
            lastItem = gridInfo;
            if (resID == 0)
            {
                firstGrid = gridInfo;
            }
            if (resName != "undefined")
            {
                GameObject itemPrefab = Resources.Load<GameObject>("Mesh/" + resName);
                GameObject item = Instantiate(itemPrefab) as GameObject;
                item.transform.parent = itemParent;
                if (dir == 0)
                {
                    item.transform.localScale = new Vector3(scaleZ, 1, scaleX);
                }
                else
                    item.transform.localScale = new Vector3(scaleX, 1, scaleZ);
                item.transform.position = pos;
                item.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
                roadItem.Add(item);
            }
            if (i == 0 && dir == 0 && length > 15)
            {
                rebornPointArr.Add(resID);
            }
            if (i == girdGroup.Length - 1 && id == gridNum && gameType == 1)
            {
                creatEndRoad();
            }
            resID++;
        }
    }

    private void creatEndRoad()
    {
        GameObject itemPrefab = Resources.Load<GameObject>("Mesh/zhongdian");
        GameObject item = Instantiate(itemPrefab) as GameObject;
        item.transform.parent = itemParent;
        Vector3 pos = new Vector3(0, 0, 0);
        if (lastItem.dir == 0)
        {
            pos = new Vector3(lastItem.pos.x + (lastItem.length - 12), lastItem.pos.y, lastItem.pos.z - lastItem.width);
        }
        else
        {
            pos = new Vector3(lastItem.pos.x + ((lastItem.width - 12) / 2), lastItem.pos.y, lastItem.pos.z - lastItem.length);
        }
        item.transform.position = pos;
        item.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
        roadItem.Add(item);
    }

    public Grid getRebornPoint(int resId)
    {
        int rebornId = rebornPointArr[rebornPointArr.Count - 1];
        for (int i = 0; i < rebornPointArr.Count; i++)
        {
            if (resId < rebornPointArr[i])
            {
                rebornId = rebornPointArr[i - 1];
                break;
            }
        }
        return girdList[rebornId];
    }

    public float getRoadLength()
    {
        return roadLength;
    }

    public void clear()
    {
        for (int i = 0; i < roadItem.Count; i++)
        {
            Destroy(roadItem[i]);
        }
        rebornPointArr.Clear();
        girdList.Clear();
        roadItem.Clear();
    }

}
