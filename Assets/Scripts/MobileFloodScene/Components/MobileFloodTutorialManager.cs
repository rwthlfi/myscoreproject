using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;

public class MobileFloodTutorialManager : MonoBehaviour
{
    [Header("Video Container")]
    public BigVideoScreenReference videoContainer;
    //public List<VideoClip> videoList = new List<VideoClip>();


    [Header("Script object reference")]
    public GlassCube glassCube;
    public OilSprayCan oilSprayCan_0;
    public OilSprayCan oilSprayCan_1;
    public AutomatedScrewDriver automatedScrewDriver_0;
    public AutomatedScrewDriver automatedScrewDriver_1;
    public Component_Foundation componentFoundation_0;
    public Component_Foundation componentFoundation_1;
    public Component_Columns componentColumn_0;
    public Component_Columns componentColumn_1;
    public List<Component_Plug> component_plugsList;
    public List<Component_Screw> component_screwsList;
    public List<Component_Bar> component_barsList;
    public SimpleTriggerDetection stopLogDetector_1;
    public SimpleTriggerDetection stopLogDetector_2;
    public SimpleTriggerDetection barDetector0_1;
    public SimpleTriggerDetection barDetector0_2;
    public SimpleTriggerDetection barDetector1_1;
    public SimpleTriggerDetection barDetector1_2;
    public SimpleTriggerDetection barDetector2_1;
    public SimpleTriggerDetection barDetector2_2;
    public SimpleTriggerDetection barDetector3_1;
    public SimpleTriggerDetection barDetector3_2;
    public SimpleTriggerDetection LockingDetector0_1;
    public SimpleTriggerDetection LockingDetector0_2;
    public SimpleTriggerDetection LockingDetector1_1;
    public SimpleTriggerDetection LockingDetector1_2;

    public Component_LockingDevice lockingDevice_0;
    public Component_LockingDevice lockingDevice_1;


    public SimpleTriggerDetection barDetector;

    public enum tutorStep
    {
        //transport system
        nothing = 0,
        RemovePlugs = 1,
        AttachSupportColumn = 2,
        AttachScrews = 3,
        LubricateTheColumn = 4,
        AttachStopLogs = 5,
        AttachBars_0 = 6,
        AttachBars_1 = 7,
        AttachBars_2 = 8,
        AttachBars_3 = 9,
        AttachLockingDevice = 10,
        Finish = 11
    }


    //to track the tutorial that the player has completed
    [System.NonSerialized] public int lastStep = -1;
    [System.NonSerialized] public int currentStep = 0; // reset to 0 during deployment


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("LogCurrentStep", 3, 3);
    }

    // Update is called once per frame
    void Update()
    {

        DetectStepChanges();


        //if the steps is still 0 which means that the user hasnt pressed the start tutorial
        //then do nothing
        if (currentStep == 0)
            return;

        
        //open the glass cube
        glassCube.OpenGlassCube();

        //the first foundation
        
        RemoveThePlugs(componentFoundation_0);
        AttachTheSupportColumn(componentFoundation_0);
        AttachTheScrews(componentFoundation_0);
        LubricateTheColumn(componentColumn_0);


        RemoveThePlugs(componentFoundation_1);
        AttachTheSupportColumn(componentFoundation_1);
        AttachTheScrews(componentFoundation_1);
        LubricateTheColumn(componentColumn_1);
        

        //the bar.
        AttachTheStopLog();
        AttachTheStopBars_0();
        AttachTheStopBars_1();
        AttachTheStopBars_2();
        AttachTheStopBars_3();
        

        //Locking device
        AttachTheLockingDevice_0();


        //finish tutorial.
        TutorialisFinish();

    }


    //try to detect the step changes
    private void DetectStepChanges()
    {
        if (currentStep != lastStep)
        {
            //disable all info
            componentFoundation_0.DisableAllInfo();
            componentFoundation_1.DisableAllInfo();
            
            componentColumn_0.DisableInfo(componentColumn_0.info_LubricateThecolumn);
            componentColumn_1.DisableInfo(componentColumn_1.info_LubricateThecolumn);

            //then make the last step equal to currenstep
            lastStep = currentStep;
        }
    }


    private void LogCurrentStep()
    {
        Debug.Log("current step" + currentStep);
    }


    //0. check if the Plugs are there, if yes then show "remove the plugs" sign
    private void RemoveThePlugs(Component_Foundation _componentFoundation)
    {
        //reset the current plug
        int totalPlug = 0;

        //if the Plugs still exist, remove them.
        //show the info to remove the plugs from the foundation
        if (_componentFoundation.collider_isExist(MobileFloodConfig.Plug))
        {
            _componentFoundation.ShowInfo(_componentFoundation.info_removePlugs);
            currentStep = (int)tutorStep.RemovePlugs;
        }

        //detect if there is a plug on the Foundation it self.
        foreach (Collider c in _componentFoundation.ColliderList)
        {
            if (_componentFoundation.ColliderList.Count == 0)
                continue;

            //print("a");
            if (c == null || !c.gameObject)
                continue;

            //print("b");
            //if there is a plug  
            if (ConverterFunction.ContainsAny(c.name, MobileFloodConfig.Plug))
            {
                totalPlug++;
                //AND if it is the plug is already out
                if (c.gameObject.transform.GetComponent<Component_Plug>().isAlreadyOut())
                {
                    //disable the message
                    c.gameObject.transform.GetComponent<Component_Plug>().DisablePlugThisInfo();
                    //unfreeze the object
                    UnfreezeTheComponent(c.gameObject, RigidbodyConstraints.None);
                    totalPlug--;
                }

                else
                {
                    FreezeTheComponent(c.gameObject, RigidbodyConstraints.FreezeAll);
                }

            }
        }

        //Debug.Log("Total Plugs " + totalPlug);

        if (totalPlug != 0)
        {
            _componentFoundation.ShowInfo(_componentFoundation.info_removePlugs);
            currentStep = (int)tutorStep.RemovePlugs;
            return;
        }

        //after the plugs is remove, then ask to attach the support column
        else
        {
            _componentFoundation.DisableInfo(_componentFoundation.info_removePlugs);
            currentStep = (int)tutorStep.AttachSupportColumn;
        }
    }



    //1. after the cover is not there, ask to attach the Support Column
    private void AttachTheSupportColumn(Component_Foundation _componentFoundation)
    {
        if (currentStep != (int)tutorStep.AttachSupportColumn)
            return;
        
        //if there is no support column , ask to attach the support column
        if (!_componentFoundation.collider_isExist(MobileFloodConfig.SupportColumn))
        {
            //Debug.Log("please attach the SupportColumn");
            _componentFoundation.ShowInfo(_componentFoundation.info_attachSupportColumn);
            currentStep = (int)tutorStep.AttachSupportColumn;

        }
        else // disable the info and Freeze the column
        {
            _componentFoundation.DisableInfo(_componentFoundation.info_attachSupportColumn);
            currentStep = (int)tutorStep.AttachScrews;

            FreezeTheComponent(_componentFoundation.gameObjectRequested(MobileFloodConfig.SupportColumn), RigidbodyConstraints.FreezeAll);

        }

    }


    
    //3. attach the screws
    private void AttachTheScrews(Component_Foundation _componentFoundation)
    {
        if (currentStep != (int)tutorStep.AttachScrews)
        {
            _componentFoundation.DisableInfo(_componentFoundation.info_attachScrews);
            return;
        }


        int totalScrew = 0;
        foreach (Collider c in _componentFoundation.ColliderList)
        {
            if (_componentFoundation.ColliderList.Count == 0)
                continue;

            //print("a");
            if (c == null || !c.gameObject)
                continue;

            //if there is a screw (detected by the gameobject name)
            if (ConverterFunction.ContainsAny(c.name, MobileFloodConfig.TopScrew)
                    && ConverterFunction.ContainsAny(c.transform.parent.name, MobileFloodConfig.screw))

                    //&& c.gameObject.transform.parent.name == MobileFloodConfig.screw)
            {

                //AND if it is NOT already screwed
                if (!c.gameObject.transform.parent.GetComponent<Component_Screw>().isAlreadyScrewed())
                {
                    c.gameObject.transform.parent.GetComponent<Component_Screw>().ShowScrewThisInfo();

                }
                //if it does screwed already
                else
                {
                    //disable the info screwing
                    c.gameObject.transform.parent.GetComponent<Component_Screw>().DisableScrewThisInfo();
                    //freeze the screw position
                    FreezeTheComponent(c.transform.parent.gameObject, RigidbodyConstraints.FreezeAll);

                    //freeze the foundation as well
                    var SupportColumnBoundary = _componentFoundation.gameObjectRequested(MobileFloodConfig.SupportColumn);
                    
                    /*
                    if(SupportColumnBoundary)
                        SupportColumnBoundary.transform.root.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    */
                    //add to the total Screw
                    totalScrew++;
                }
            }
        }

        //if there are no 4 screws, ask to attached AND screw them.
        //should be executed after all the screw are in.
        if (totalScrew < 4)
        {
            //Debug.Log("please attach the Screws");
            _componentFoundation.ShowInfo(_componentFoundation.info_attachScrews);
            currentStep = (int)tutorStep.AttachScrews;
            return;
        }

        else
        {
            //Otherwise -> Next Step
            _componentFoundation.DisableInfo(_componentFoundation.info_attachScrews);
            currentStep = (int)tutorStep.LubricateTheColumn;

        }
    }



    private void LubricateTheColumn(Component_Columns _componentColumn)
    {
        if (currentStep != (int)tutorStep.LubricateTheColumn)
            return;

        //if the component has NOT been oiled 
        else if (!_componentColumn.oiledValue)
        {  //do your logic here
            //Debug.Log("Lubricate theColumn");
            _componentColumn.ShowInfo(_componentColumn.info_LubricateThecolumn);
        }

        //if it does oiled, then proceed to ask to attach the bottom bar.
        if (_componentColumn.oiledValue)
        {
            _componentColumn.DisableInfo(_componentColumn.info_LubricateThecolumn);
            currentStep = (int)tutorStep.AttachStopLogs;
        }
    }


    private void AttachTheStopLog()
    {
        if (currentStep != (int)tutorStep.AttachStopLogs)
            return;

        CheckingDetector(stopLogDetector_1, stopLogDetector_2, new Vector3(0.213f, 0.126f, -1.144f), Vector3.zero, MobileFloodConfig.minStayDuration, tutorStep.AttachBars_0);
    }




    //- - - - Stop bar - - - -//
    //this is ugly... why do i need to repeat it 5 times for each stop bars.... :(
    private void AttachTheStopBars_0()
    {
        if (currentStep != (int)tutorStep.AttachBars_0)
            return;

        CheckingDetector(barDetector0_1, barDetector0_2, new Vector3(0.213f, 0.341f, -1.144f), Vector3.zero, MobileFloodConfig.minStayDuration, tutorStep.AttachBars_1);
        
    }

    private void AttachTheStopBars_1()
    {
        if (currentStep != (int)tutorStep.AttachBars_1)
            return;

        CheckingDetector(barDetector1_1, barDetector1_2, new Vector3(0.213f, 0.504f, -1.144f), Vector3.zero, MobileFloodConfig.minStayDuration, tutorStep.AttachBars_2);
        
    }

    private void AttachTheStopBars_2()
    {
        if (currentStep != (int)tutorStep.AttachBars_2)
            return;

        CheckingDetector(barDetector2_1, barDetector2_2, new Vector3(0.213f, 0.67f, -1.144f), Vector3.zero, MobileFloodConfig.minStayDuration, tutorStep.AttachBars_3);

    }


    private void AttachTheStopBars_3()
    {
        if (currentStep != (int)tutorStep.AttachBars_3)
            return;

        CheckingDetector(barDetector3_1, barDetector3_2, new Vector3(0.213f, 0.833f, -1.144f), Vector3.zero, MobileFloodConfig.minStayDuration, tutorStep.AttachLockingDevice);

    }



    //- - - - Locking Device - - - -//

    private void AttachTheLockingDevice_0()
    {
        if (currentStep != (int)tutorStep.AttachLockingDevice)
            return;

        
        //this is to check if the lock on the locking device is screwed properly
        
        //first locking device
        if (!lockingDevice_0.isAllLocksProperlyScrewed() || !lockingDevice_1.isAllLocksProperlyScrewed())
        {
            CheckingDetector(LockingDetector0_1, LockingDetector0_2, new Vector3(-0.74f, 0.872f, -1.1406f), Vector3.zero, 2f, tutorStep.AttachLockingDevice);
            CheckingDetector(LockingDetector1_1, LockingDetector1_2, new Vector3(1.1675f, 0.872f, -1.1406f), new Vector3(0,180,0), 2f, tutorStep.AttachLockingDevice);

        }
        else
        {
            CheckingDetector(LockingDetector0_1, LockingDetector0_2, new Vector3(-0.74f, 0.872f, -1.1406f), Vector3.zero, 2f, tutorStep.Finish);
            CheckingDetector(LockingDetector1_1, LockingDetector1_2, new Vector3(1.1675f, 0.872f, -1.1406f), new Vector3(0, 180, 0), 2f, tutorStep.Finish);
        }


        //CheckingDetector(LockingDetector1_1, LockingDetector1_2, new Vector3(-0.7453f, 0.853f, -1.1403f), Vector3.zero, tutorStep.Finish);
    }



    /// <summary>
    /// this it to check the 2 detectors, if the desired object already collider or not
    /// </summary>
    /// <param name="_trigger1"></param>
    /// <param name="_trigger2"></param>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    private void CheckingDetector(SimpleTriggerDetection _trigger1, SimpleTriggerDetection _trigger2, Vector3 _pos, Vector3 _rot, float _snapTime, tutorStep _step)
    {
        if (!_trigger1.isCollided && !_trigger2.isCollided)
        {
            //do your logic here.
            //Debug.Log("attached the stop log");
            _trigger1.ShowInfo();
            _trigger2.ShowInfo();
        }
        
        
        //but if it is collided, go to next tutorial
        else if (_trigger1.isCollided & _trigger2.isCollided
                 && _trigger1.stayDuration >= _snapTime)
        {
            //disable the info
            _trigger1.DisableInfo();
            _trigger2.DisableInfo();

            //freeze the position
            FreezeTheComponent(_trigger1.collidedObject.gameObject, RigidbodyConstraints.FreezeAll);
            FreezeTheComponent(_trigger2.collidedObject.gameObject, RigidbodyConstraints.FreezeAll);
            currentStep = (int)_step;

            //snap the position and rotation
            StartCoroutine(LerpingExtensions.MoveTo(_trigger1.collidedObject.transform, _pos, 0.25f));
            StartCoroutine(LerpingExtensions.RotateTo(_trigger1.collidedObject.transform, Quaternion.Euler(_rot), 0.25f));
        }
    }

    


    private void AttachTheStopBars()
    {
        if (currentStep != (int)tutorStep.AttachBars_0)
            return;

        //if bottom bar is not collided with the stop log, then show the info
        if (!barDetector.isCollided)
        {
            //do your logic here.
            //Debug.Log("attached the stop log");
            barDetector.ShowInfo();
        }

        if (barDetector.isCollided
            && barDetector.stayDuration >= MobileFloodConfig.minStayDuration)
        {
            //tell to disable the "attach log effect"
            barDetector.DisableInfo();
            currentStep = (int)tutorStep.AttachLockingDevice;
            FreezeTheComponent(barDetector.collidedObject.gameObject, RigidbodyConstraints.FreezeAll);
        }
    }


    public void Testing_Unfreeze()
    {
        UnfreezeTheComponent(componentColumn_0.gameObject, RigidbodyConstraints.None);

        UnfreezeTheComponent(componentColumn_1.gameObject, RigidbodyConstraints.None);
        
        //reset the screws position along with the unfreezing function.
        foreach (Component_Screw cs in component_screwsList)
            UnfreezeTheComponent(cs.gameObject, RigidbodyConstraints.None);
        
        UnfreezeTheComponent(barDetector.collidedObject.gameObject, RigidbodyConstraints.None);

    }




    private void TutorialisFinish()
    {
        if (currentStep != (int)tutorStep.Finish)
            return;

        //do your logic here.
        Debug.Log("TutorialFinish");

    }




    //this is to reset all the component position and transform back to its original place.
    public void ResetAllComponentTransform()
    {
        //return all the tools to its position
        oilSprayCan_0.ResetTransform();
        oilSprayCan_1.ResetTransform();
        automatedScrewDriver_0.ResetTransform();
        automatedScrewDriver_1.ResetTransform();


        //return the foundation back, (not necessarily though, since it never movetd ) 
        componentFoundation_0.ResetTransform();
        componentFoundation_1.ResetTransform();


        //return the component column and unfreeze the rigidbody
        componentColumn_0.ResetTransform();
        UnfreezeTheComponent(componentColumn_0.gameObject, RigidbodyConstraints.None);
        //componentColumn_0.resetOiled();


        componentColumn_1.ResetTransform();
        UnfreezeTheComponent(componentColumn_1.gameObject, RigidbodyConstraints.None);
        //componentColumn_1.resetOiled();



        foreach (Component_Bar cb in component_barsList)
            StartCoroutine(cb.ResetTransform());

        foreach (Component_Plug cp in component_plugsList)
        {
            cp.ResetTransform();
            cp.ShowPlugThisInfo();
            FreezeTheComponent(cp.gameObject, RigidbodyConstraints.FreezeAll);
        }

        //reset the screws position along with the unfreezing function.
        foreach (Component_Screw cs in component_screwsList)
        {
            cs.ResetTransform();
            cs.ShowScrewThisInfo();
            UnfreezeTheComponent(cs.gameObject, RigidbodyConstraints.None);
        }


        //reset the locking devices
        StartCoroutine(lockingDevice_0.ResetTransform());
        UnfreezeTheComponent(lockingDevice_0.gameObject, RigidbodyConstraints.None);

        StartCoroutine(lockingDevice_1.ResetTransform());
        UnfreezeTheComponent(lockingDevice_1.gameObject, RigidbodyConstraints.None);

    }




    //play the video Step according to the current steps.
    private void ShowVideoStep(VideoClip _videoStep)
    {
        videoContainer.GetComponent<VideoPlayer>().clip = _videoStep;
    }




    // - - - - - - - - - - - - - - - //


    //general functionality
    //this is to freeze all the functionality..., and therefore it cannot be moved or force teleport..
    //otherwise it wont get to real.
    private void FreezeTheComponent(GameObject _component, RigidbodyConstraints _constraints)
    {
        //if component is null then do nothing
        if (!_component || _component == null)
        {
            Debug.Log("requested component is not exist");
            return;
        }

        //freeze all the constraints
        _component.GetComponent<Rigidbody>().constraints = _constraints;
        //remove velocity
        _component.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _component.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //change the layer to default
        _component.layer = GlobalSettings.layerDefault;
        //disable the grabbable
        _component.GetComponent<ObjectAuthorities>().enabled = false;

        if (_component.GetComponent<XRGrabInteractable>())
            _component.GetComponent<XRGrabInteractable>().enabled = false;
    }



    //this is to reenable the ability to grab the component again.
    private void UnfreezeTheComponent(GameObject _component, RigidbodyConstraints _constraint)
    {
        //Unfreeze all the constraints
        _component.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _component.GetComponent<Rigidbody>().constraints = _constraint;

        //enable gravity and disable kinematic
        _component.GetComponent<Rigidbody>().useGravity = true;
        _component.GetComponent<Rigidbody>().isKinematic = false;

        //change the layer to Grabbable
        _component.layer = GlobalSettings.layerGrabbable;
        //_component.GetComponent<Grabbable>().enabled = true;

        //disable the grabbable
        _component.GetComponent<ObjectAuthorities>().enabled = true;

        if (_component.GetComponent<XRGrabInteractable>())
            _component.GetComponent<XRGrabInteractable>().enabled = true;
    }


}
