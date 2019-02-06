using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public RobotHandler Parent;
    void Start()
    {
        
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
            transform.Rotate(Vector3.up, 160f* Time.deltaTime);
        }
    }
}
