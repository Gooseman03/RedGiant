using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBay : BaseSystem
{
    [SerializeField] ObjectPlace RepairSpot;


    private void Update()
    {
        WhenPowered();
    }

    private void WhenPowered()
    {
        if (SystemPower && PowerSwitchState)
        {
            Repair();
        }
    }
    private void Repair()
    {
        if (RepairSpot.ObjectGrabbable is IDurable durableObject) 
        {
            durableObject.SetDurability(durableObject.MaxDurability);
        }
        
    }
}


