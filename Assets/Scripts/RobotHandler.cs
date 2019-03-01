using EasyAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum TargetingState
{
    None,
    First,
    Second,
    Three
}

public class RobotHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject FirstLocation, SecondLocation, ThirdLocation, RobotObject;
    public ImageTrackerBehaviour ImageTracker;
    public float MoveSpeed = 1.0f;
    private bool HasAllTargets { get { return totalNumberOfTargets == 3; } }
    private int totalNumberOfTargets;
    private TargetingState state;
    public bool IsDone { get { return state == TargetingState.Three; } }
    public GameObject CurrentDestination { get; private set; }
    private bool _hasNotStarted = true;
    private Text textui;
    void Start()
    {
        state = TargetingState.None;
        textui = GameObject.Find("Text").GetComponent<Text>();
        transform.GetChild(0).gameObject.SetActive(false);
        ImageTracker.TargetLoad += ImageTracker_TargetLoad;
        ImageTracker.TargetUnload += ImageTracker_TargetUnload;
    }

    #region TargetLoading
    private void ImageTracker_TargetLoad(ImageTrackerBaseBehaviour trackerBase, ImageTargetBaseBehaviour targetBase, Target target, bool arg4)
    {
        HookupTarget(targetBase);        
    }

    private void ImageTracker_TargetUnload(ImageTrackerBaseBehaviour trackerBase, ImageTargetBaseBehaviour targetBase, Target target, bool arg4)
    {
        UnHookTarget(targetBase);
    }
    #endregion

    private void HookupTarget(ImageTargetBaseBehaviour target)
    {
        target.TargetFound += Target_TargetFound;
        target.TargetLost += Target_TargetLost;
    }
    private void UnHookTarget(ImageTargetBaseBehaviour target)
    {
        target.TargetFound -= Target_TargetFound;
        target.TargetLost -= Target_TargetLost;
    }


    #region TargetDiscovery
    private void Target_TargetFound(TargetAbstractBehaviour obj)
    {
        totalNumberOfTargets++;
    }

    private void Target_TargetLost(TargetAbstractBehaviour obj)
    {
        totalNumberOfTargets--;
    }
    #endregion
    private void AdjustLocationAndRotation(GameObject location, float step)
    {
        RobotObject.transform.position = Vector3.MoveTowards(RobotObject.transform.position,
            location.transform.position, step);
        RobotObject.transform.rotation = location.transform.rotation;
    }
    private void SetState()
    {
        if(RobotObject.transform.position == FirstLocation.transform.position)
        {
            state = TargetingState.First;
        }
        else if (RobotObject.transform.position == SecondLocation.transform.position)
        {
            state = TargetingState.Second;
        }
        else if (RobotObject.transform.position == ThirdLocation.transform.position)
        {
            state = TargetingState.Three;
        }
    }

    private bool isFacingNorth()
    {

        Input.compass.enabled = true;
        int location = (int)Input.compass.trueHeading;
        textui.text = "Get your device to face \nsouth in order to get started \ncurrent deggrees off: " + (location - 180);
        return location == 180;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_hasNotStarted)
        {
            if (isFacingNorth())
            {
                _hasNotStarted = false;
                textui.gameObject.SetActive(false);
            }
        }
        else
        {
            if (totalNumberOfTargets != 3)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                return;
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            float step = MoveSpeed * Time.deltaTime;
            switch (state)
            {
                case TargetingState.None:
                    AdjustLocationAndRotation(FirstLocation, step);
                    CurrentDestination = FirstLocation;
                    break;
                case TargetingState.First:
                    AdjustLocationAndRotation(SecondLocation, step);
                    CurrentDestination = SecondLocation;
                    break;
                case TargetingState.Second:
                case TargetingState.Three:
                    AdjustLocationAndRotation(ThirdLocation, step);
                    CurrentDestination = ThirdLocation;
                    break;
            }
            SetState();
        }

        

    }
}
