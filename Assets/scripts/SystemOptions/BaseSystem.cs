using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
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
            itemRegister.HasObject<PowerSwitchController>(out List<PowerSwitchController> PowerSwitchs);
            PowerSwitchState = true;
            foreach (PowerSwitchController objectDirector in PowerSwitchs)
            {
                objectDirector.ChangeSwitchState(true);
            }
            Start = false;
        }
    }

    private void WhenPowered()
    {
        SystemPower = CheckPowerLine();
        if (itemRegister.HasObject<PowerSwitchController>(out List<PowerSwitchController> ListOfPowerSwitchs))
        {
            foreach(PowerSwitchController objectDirector in ListOfPowerSwitchs)
            {
                if (PowerSwitch.ObjectGrabbable == objectDirector)
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
            if (type == ObjectType.Fuse && !ErrorCodes.CheckWorking<FuseController>(itemRegister))
            {
                return false;
            }
            if (type == ObjectType.PowerConnector && !ErrorCodes.CheckWorking<PowerConnectorController>(itemRegister))
            {
                return false;
            }
            if (type == ObjectType.Pump && !ErrorCodes.CheckWorking<PumpController>(itemRegister))
            {
                return false;
            }
        }
        return true;
    }
}
