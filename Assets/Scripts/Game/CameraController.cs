using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController Instance;

    private bool isLeft;
    private bool isRight;
    private float disX;
    private float disZ;

    private bool isTurnLeft = false;
    private bool isTurnRight = false;


    private Transform target;

    void Awake()
    {
        Instance = this;
    }

    public void startGame()
    {
        target = CarController.Instance.transform;
        disX = transform.position.x - target.position.x;
        disZ = transform.position.z - target.position.z;
        isTurnLeft = true;
        isTurnRight = false;
    }

    private void Update()
    {
        if (target)
        {
            if (isTurnLeft || isTurnRight)
            {
                transform.position = new Vector3(target.position.x + disX, transform.position.y, target.position.z + disZ);
            }
        }
    }

    public void turnLeft()
    {
        disX = transform.position.x - target.position.x;
        isTurnLeft = true;
        isTurnRight = false;
    }

    public void turnRigth()
    {
        disZ = transform.position.z - target.position.z;
        isTurnLeft = false;
        isTurnRight = true;
    }

    public void carReborn(){
        target = CarController.Instance.transform;
    }
}
