using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBay : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    

    [SerializeField] ObjectPlace RepairSpot;


    private void FixedUpdate()
    {
        WhenPowered();
    }

    private void WhenPowered()
    {
        if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
        {
            Repair();
        }
    }
    private void Repair()
    {
        if (RepairSpot.objectGrabbable == null) { return; }
        if (RepairSpot.objectGrabbable.Durability == null) { return; }
        RepairSpot.objectGrabbable.ChangeDurability(100 - (float)RepairSpot.objectGrabbable.Durability);
    }
}


