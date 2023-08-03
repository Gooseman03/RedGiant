using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonScrubber : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    [SerializeField] private List<float?> CarbonCanisterPressure = new List<float?>();
    [SerializeField] List<OxygenCube> oxygenCubes = new List<OxygenCube>();

    private void FixedUpdate()
    {
        LoadCarbonCanister();
        WhenPowered();
    }

    private void WhenPowered()
    {
        if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
        {
            if (CheckAirline())
            {
                UpdateOxygenCubes();
                DirtyFilters();
                FillCarbonCanisters();
                SendMessage("pumpPlayAudio",true);
            }
        }
        else
        {
            CarbonCanisterPressure.Add(0f);
            SendMessage("pumpPlayAudio", false);
        }
    }
    private void DirtyFilters()
    {
        if (baseSystem.itemRegister.HasObject(ObjectType.AirFilter, out List<ObjectDirector> ObjectList))
        {
            foreach (ObjectDirector Filter in ObjectList)
            {
                Filter.ChangeDirt(Time.deltaTime);
            }
        }
    }
    private void FillCarbonCanisters()
    {
        if (baseSystem.itemRegister.HasObject(ObjectType.Co2Canister, out List<ObjectDirector> ObjectList))
        {
            int i = 0;
            foreach (ObjectDirector Co2Canister in ObjectList)
            {
                Co2Canister.ChangePressure(-.5f * Time.deltaTime);
                i++;
            }
        }
    }
    private bool CheckAirline()
    {
        if (!ErrorCodes.CheckWorking(baseSystem.itemRegister, ObjectType.Pump))
        {
            Debug.Log("Pump Not Working");
            return false;
        }
        if (!ErrorCodes.CheckWorking(baseSystem.itemRegister, ObjectType.AirFilter))
        {
            Debug.Log("AirFilter Not Working");
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
        if (baseSystem.itemRegister.HasObject(ObjectType.Co2Canister, out List<ObjectDirector> ObjectList))
        {
            int i = 0;
            foreach (ObjectDirector Co2Canister in ObjectList)
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
