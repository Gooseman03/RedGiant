using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeErrorDisplay : MonoBehaviour
{
    private BaseSystem baseSystem;
    private void Start()
    {
        baseSystem = GetComponent<BaseSystem>();
    }
    private void Update()
    {
        WhenPowered();
    }
    private void WhenPowered()
    {
        if(baseSystem.SystemPower && baseSystem.PowerSwitchState)
        {
            ErrorCodes.Printed = true;
        }
        else
        {
            ErrorCodes.Printed = false;
        }
    }
}
