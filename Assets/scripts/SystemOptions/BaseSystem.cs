using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSystem : MonoBehaviour
{
    [SerializeField] private List<ObjectType> RequiredForPower = new List<ObjectType>();
    private ItemRegister _itemRegister;
    private bool _SystemPower = true;
    private bool _PowerSwitchState;
    private List<ErrorTypes> _Errors;
    public string ErrorText;
    [SerializeField] private ObjectPlace PowerSwitch;

    private Dictionary<int, List<ObjectPlace>> PowerLines = new Dictionary<int, List<ObjectPlace>>();

    [SerializeField] private bool Start;

    [SerializeField] private bool _ReactorPower = false;

    public bool ReactorPower
    {
        get { return _ReactorPower; }
        set { _ReactorPower = value; }
    }

    public List<ErrorTypes> Errors
    {
        get { return _Errors; }
        private set { _Errors = value; }
    }

    public bool SystemPower
    {
        get { return _SystemPower; }
        private set { _SystemPower = value; }
    }
    public ItemRegister itemRegister
    {
        get { return _itemRegister; }
        private set { _itemRegister = value; }
    }

    public bool PowerSwitchState
    {
        get { return _PowerSwitchState; }
        private set { _PowerSwitchState = value; }
    }

    private void Awake()
    {
        
        itemRegister = GetComponent<ItemRegister>();
    }
    private void FixedUpdate()
    {

        CheckForErrors();
        WhenPowered();
    }

    private void LateUpdate()
    {
        if (Start)
        {
            itemRegister.HasObject(ObjectType.PowerSwitch, out List<ObjectDirector> PowerSwitchs);
            PowerSwitchState = true;
            foreach (ObjectDirector objectDirector in PowerSwitchs)
            {
                objectDirector.ChangeSwitchState(true);
            }
            Start = false;
        }
    }

    private void WhenPowered()
    {
        SystemPower = CheckPowerLine();
        if (itemRegister.HasObject(ObjectType.PowerSwitch, out List<ObjectDirector> ListOfPowerSwitchs))
        {
            foreach(ObjectDirector objectDirector in ListOfPowerSwitchs)
            {
                if (PowerSwitch.objectGrabbable == objectDirector)
                {
                    bool? SwitchState = objectDirector.GetSwitchState();

                    if (SwitchState == true)
                    {
                        PowerSwitchState = true;
                    }
                    else if (SwitchState == false)
                    {
                        PowerSwitchState = false;
                    }
                    break;
                }
            }
        }
    }

    private void CheckForErrors()
    {
        ErrorCodes.ErrorCheck(itemRegister, out ErrorText, out _Errors);
    } 

    private bool CheckPowerLine()
    {
        if (!ReactorPower) { return false; }
        foreach (ObjectType type in RequiredForPower)
        {
            if(!ErrorCodes.CheckWorking(itemRegister, type))
            {
                return false;
            }
        }
        return true;
    }
}
