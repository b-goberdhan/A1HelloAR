using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public RobotHandler Parent;
    public float RotationSpeed = 10f;
    void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Parent.IsDone)
        {
            transform.LookAt(Parent.CurrentDestination.transform);
        }
        else
        {
            //transform.Rotate(Vector3.up, 160f* Time.deltaTime);
             
            var gyroRotation = Input.gyro.rotationRateUnbiased;
            transform.Rotate(0, -gyroRotation.x *RotationSpeed, 0);
        }
    }
}
