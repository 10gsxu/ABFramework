using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U3DEventFrame;

public class GameController : GameBase
{
    public static GameController Instance;
    public int completeResId;
    public Vector3 carStartPoint;
    private GameObject car;
    private float finishProgress;
    private int rebornCount;
    private int carId;
    private int gameLevel;
    private int gameMode;

    private void Awake()
    {
        Instance = this;
        msgIds = new ushort[]
        {
            (ushort)GameEvent.StartGame
        };
        RegistSelf(this, msgIds);
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)GameEvent.StartGame:
                startGame(tmpMsg);
                break;
        }
    }

    void startGame(MsgBase tmpMsg)
    {
        StartGameMsg msg = (StartGameMsg)tmpMsg;
        print(msg.carId + " : " + msg.gameLevel + " : " + msg.gameMode);
        carId = msg.carId;
        gameLevel = msg.gameLevel;
        gameMode = msg.gameMode;
        completeResId = 0;
        finishProgress = 0;
        rebornCount = 0;
        RoadCreator.Instance.creatRoad(msg.gameLevel, msg.gameMode);
        initCar();
        carMove();
        CameraController.Instance.startGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            reborn();
        }
    }

    private void initCar()
    {
        int carId = 1;
        GameObject itemPrefab = Resources.Load<GameObject>("Mesh/car" + carId);
        GameObject item = Instantiate(itemPrefab) as GameObject;
        car = item;
        item.transform.parent = RoadCreator.Instance.itemParent;
        Grid firstGrid = RoadCreator.Instance.firstGrid;
        Debug.Log(firstGrid);
        carStartPoint = new Vector3(firstGrid.length / 2, 0.5f, -firstGrid.width / 2);
        item.transform.position = carStartPoint;
        item.AddComponent<CarController>();
    }

    private void carMove(){
        CarController.Instance.starGame(gameLevel, gameMode);
    }

    private void reborn()
    {
        int carId = 1;
        GameObject itemPrefab = Resources.Load<GameObject>("Mesh/car" + carId);
        GameObject item = Instantiate(itemPrefab) as GameObject;
        car = item;
        item.transform.parent = RoadCreator.Instance.itemParent;
        Grid firstGrid = RoadCreator.Instance.firstGrid;
        Grid rebornPoint = RoadCreator.Instance.getRebornPoint(completeResId);
        item.transform.position = new Vector3(rebornPoint.pos.x + rebornPoint.length / 8, 0.5f, rebornPoint.pos.z - rebornPoint.width / 2);
        item.AddComponent<CarController>();
        CameraController.Instance.carReborn();
        completeResId = rebornPoint.resId;
        carMove();
        rebornCount++;
    }

    public void updateProgress()
    {
        finishProgress = (car.transform.position.x - carStartPoint.x + (carStartPoint.z - car.transform.position.z)) / RoadCreator.Instance.getRoadLength();
        ObjectMsg<float> msg = new ObjectMsg<float>((ushort)UIEvent.GameProgress, finishProgress);
        SendMsg(msg);
    }

    public void GameEnd(bool isWin)
    {
        if (isWin)
        {
            MsgBase msg = new MsgBase((ushort)UIEvent.GameWin);
            SendMsg(msg);
        }
        else
        {
            if (finishProgress >= 0.5 && rebornCount == 0)
            {
                MsgBase msg = new MsgBase((ushort)UIEvent.GameReborn);
                SendMsg(msg);
            }
            else
            {
                MsgBase msg = new MsgBase((ushort)UIEvent.GameLose);
                SendMsg(msg);
            }
        }
    }

    private void clear()
    {
        if (car)
        {
            Destroy(car);
        }
        RoadCreator.Instance.clear();
    }

}