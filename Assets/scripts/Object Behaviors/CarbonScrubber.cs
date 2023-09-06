using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonScrubber : BaseSystem
{
    [SerializeField] private List<float?> CarbonCanisterPressure = new List<float?>();
    [SerializeField] List<OxygenCube> oxygenCubes = new List<OxygenCube>();

    private void Update()
    {
        LoadCarbonCanister();
        WhenPowered();
    }

    private void WhenPowered()
    {
        if (SystemPower && PowerSwitchState)
        {
            if (CheckAirline())
            {
                UpdateOxygenCubes();
                DirtyFilters();
                FillCarbonCanisters();
                if (itemRegister.HasObject(out List<PumpController> pumps))
                {
                    pumps[0].ChangeAudioPlaying(true);
                }
            }
        }
        else
        {
            CarbonCanisterPressure.Add(0f);
            if (itemRegister.HasObject(out List<PumpController> pumps))
            {
                pumps[0].ChangeAudioPlaying(false);
            }
        }
    }
    private void DirtyFilters()
    {
        if (itemRegister.HasObject(out List<AirFilterController> ObjectList))
        {
            foreach (AirFilterController Filter in ObjectList)
            {
                Filter.ChangeDirt(Time.deltaTime);
            }
        }
    }
    private void FillCarbonCanisters()
    {
        if (itemRegister.HasObject(out List<Co2CanisterController> ObjectList))
        {
            int i = 0;
            foreach (Co2CanisterController Co2Canister in ObjectList)
            {
                Co2Canister.ChangePressure(-.5f * Time.deltaTime);
                i++;
            }
        }
    }
    private bool CheckAirline()
    {
        if (!ErrorCodes.CheckWorking<PumpController>(itemRegister))
        {
            return false;
        }
        if (!ErrorCodes.CheckWorking<AirFilterController>(itemRegister))
        {
            return false;
        }
        if (CarbonCanisterPressure[0] <= 0)
        {
            return false;
        }
        return true;
    }
    private void LoadCarbonCanister()
    {
        CarbonCanisterPressure.Clear();
        CarbonCanisterPressure.Add(null); // Default Air Pressure on Aircanister 1 null Unless Debuging
        if (itemRegister.HasObject(out List<Co2CanisterController> ObjectList))
        {
            int i = 0;
            foreach (Co2CanisterController Co2Canister in ObjectList)
            {
                CarbonCanisterPressure[i] = Co2Canister.Pressure;
                i++;
            }
        }
    }
    private void UpdateOxygenCubes()
    {
        foreach (OxygenCube cube in oxygenCubes)
        {
            if (cube.Carbon < 100)
            {
                cube.ChangeCarbon(-2 * Time.deltaTime);
            }
        }
    }
}
