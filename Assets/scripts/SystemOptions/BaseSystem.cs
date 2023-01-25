using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class BaseSystem : MonoBehaviour
{
    [SerializeField] private List<ObjectType> RequiredForPower = new List<ObjectType>();
    private ItemRegister _itemRegister;
    private bool _SystemPower = true;
    private bool _PowerSwitchState;
    private List<ErrorTypes> Errors;
    public string ErrorText;

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

    private void WhenPowered()
    {
        if (itemRegister.HasObject(ObjectType.PowerSwitch, out List<ObjectGrabbable> ListOfPowerSwitchs))
        {
            bool? SwitchState = ListOfPowerSwitchs[0].GetSwitchState();
            if (SwitchState == true)
            {
                PowerSwitchState = true;
            }
            else if (SwitchState == false)
            {
                PowerSwitchState = false;
            }
        }
    }

    private void CheckForErrors()
    {
        ErrorCodes.ErrorCheck(itemRegister, true, out ErrorText, out Errors);
    }

    private bool CheckPowerLine()
    {
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
