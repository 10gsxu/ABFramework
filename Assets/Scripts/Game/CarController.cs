using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public static CarController Instance;

    private bool isFinish;
    private float maxSpeed;
    public float curMoveSpeed;
    public float curRightSpeed;
    private float fMaxRotationX = 20f;
    private float fMaxRotationZ = 10f;
    private Vector3 moveDir;
    private Vector3 rightDir;
    private Quaternion target;
    private bool isDriftRight = false;
    private bool isDriftLeft = false;
    private bool isUp = false;
    private float timer;
    private float rotationY;
    private bool isWin;

    private float driftSpeed;

    void Awake()
    {
        Instance = this;
    }

    public void starGame(int level, int carId)
    {
        moveDir = transform.right;
        rightDir = transform.right;
        // float roadSpeed = RoadData.Instance.getMaxSpeed(level);
        float carSpeed = 22f;
        // maxSpeed = carSpeed > roadSpeed ? roadSpeed : carSpeed;
        maxSpeed = 22;
        isDriftLeft = true;
        curRightSpeed = 0;
        isFinish = false;
    }

    private void Update()
    {
        if (!isFinish)
        {
            carMove(Time.deltaTime);
            if (Input.GetMouseButtonDown(0))
            {
                carDriftRight();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                carDriftLeft();
            }

        }
        else
        {
            if (isWin)
            {
                showEnd(Time.deltaTime);
            }
        }
    }

    public void carMove(float deltaTime)
    {
        if (isDriftRight)
        {
            if (curMoveSpeed > 0)
            {
                curMoveSpeed -= maxSpeed / 100 + timer * 0.5f;
                if (curMoveSpeed < 0)
                    curMoveSpeed = 0;
            }
            if (curRightSpeed < maxSpeed)
            {
                curRightSpeed += maxSpeed / 100 + timer * 0.3f;
                if (curRightSpeed > maxSpeed)
                    curRightSpeed = maxSpeed;
            }

            target = Quaternion.Euler(0, rotationY, 0);
            if (isUp && transform.position.y >= 0.6)
            {
                target = Quaternion.Euler(0, rotationY, 20);
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 8);
            rightDir = transform.right;
            timer += deltaTime;
        }
        if (isDriftLeft)
        {
            if (curRightSpeed > 0)
            {
                curRightSpeed -= maxSpeed / 100 + timer * 0.5f;
                if (curRightSpeed < 0)
                    curRightSpeed = 0;
            }
            if (curMoveSpeed < maxSpeed)
            {
                curMoveSpeed += maxSpeed / 100 + timer * 0.3f;
                if (curMoveSpeed > maxSpeed)
                    curMoveSpeed = maxSpeed;
            }
            target = Quaternion.Euler(0, rotationY, 0);
            if (isUp && transform.position.y >= 0.6)
            {
                target = Quaternion.Euler(0, rotationY, 20);
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 8);
            moveDir = transform.right;
            timer += deltaTime;
        }
        transform.Translate(moveDir * curMoveSpeed * deltaTime, Space.World);
        transform.Translate(rightDir * curRightSpeed * deltaTime, Space.World);
        GameController.Instance.updateProgress();
        checkDie();
    }

    public void showEnd(float deltaTime)
    {
        target = Quaternion.Euler(0, -90, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 20);
        if (transform.rotation == target)
        {
            curRightSpeed -= 0.7f;
            if (curRightSpeed < 0)
            {
                curRightSpeed = 0;
            }
            transform.Translate(-transform.right * curRightSpeed * Time.deltaTime, Space.World);
        }
    }

    private void carDriftRight()
    {
        rotationY = 90;
        timer = 0;
        this.isDriftLeft = false;
        this.isDriftRight = true;
        CameraController.Instance.turnRigth();
    }

    private void carDriftLeft()
    {
        rotationY = 0;
        timer = 0;
        this.isDriftRight = false;
        this.isDriftLeft = true;
        CameraController.Instance.turnLeft();
    }

    private void checkDie()
    {
        if (transform.position.y <= -10)
        {
            this.isFinish = true;
            GameController.Instance.GameEnd(false);
            isWin = false;
            Destroy(transform.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.name == "zhongdian")
        {
            Debug.Log("win");
            isFinish = true;
            curRightSpeed = maxSpeed;
            curMoveSpeed = 0;
            isWin = true;
            timer = 0;
            GameController.Instance.GameEnd(true);
        }
        GameController.Instance.completeResId++;
    }

    private void OnCollisionExit(Collision other)
    {
        isUp = transform.position.y < 1f;
    }

}
