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
        if (!baseSystem.itemRegister.HasObject<AirCanisterController>(out List<AirCanisterController> Aircanisters)) { return; }
        if (!baseSystem.itemRegister.HasObject<Co2CanisterController>(out List<Co2CanisterController> Co2Canisters)) { return; }


        foreach (AirCanisterController aircanister in Aircanisters)
        {
            if (aircanister.Pressure > 0 && aircanister.Pressure <= 100)
            {
                HasEmptyOxygen = true;
            }
        }
        foreach (Co2CanisterController co2canister in Co2Canisters)
        {
            if (co2canister.Pressure > 0 && co2canister.Pressure <= 100)
            {
                HasFullCarbon = true;
            }
        }
        if (HasEmptyOxygen && HasFullCarbon)
        {
            foreach (AirCanisterController aircanister in Aircanisters)
            {
                aircanister.ChangePressure(Time.deltaTime);
            }
            foreach (Co2CanisterController co2canister in Co2Canisters)
            {
                co2canister.ChangePressure(Time.deltaTime);
            }
        }
    }
}
