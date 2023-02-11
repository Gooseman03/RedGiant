using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeErrorDisplay : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    private void FixedUpdate()
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
