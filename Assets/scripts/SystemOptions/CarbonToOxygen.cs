using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonToOxygen : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    private void FixedUpdate()
    {
        WhenPowered();
    }
    private void WhenPowered()
    {
        if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
        {
            Exchange();
        }
    }
    private void Exchange()
    {
        bool HasEmptyOxygen = false;
        bool HasFullCarbon = false;
        if (!baseSystem.itemRegister.HasObject(ObjectType.AirCanister, out List<ObjectDirector> Aircanisters)) { return; }
        if (!baseSystem.itemRegister.HasObject(ObjectType.Co2Canister, out List<ObjectDirector> Co2Canisters)) { return; }


        foreach (ObjectDirector aircanister in Aircanisters)
        {
            if ((float)aircanister.Pressure > 0 && (float)aircanister.Pressure <= 100)
            {
                HasEmptyOxygen = true;
            }
        }
        foreach (ObjectDirector co2canister in Co2Canisters)
        {
            if ((float)co2canister.Pressure > 0 && (float)co2canister.Pressure <= 100)
            {
                HasFullCarbon = true;
            }
        }
        if (HasEmptyOxygen && HasFullCarbon)
        {
            foreach (ObjectDirector aircanister in Aircanisters)
            {
                aircanister.ChangePressure(Time.deltaTime);
            }
            foreach (ObjectDirector co2canister in Co2Canisters)
            {
                co2canister.ChangePressure(Time.deltaTime);
            }
        }
    }
}
