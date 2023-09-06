using System.Collections.Generic;
using UnityEngine;

public class OxygenGenerator : BaseSystem
{
    [SerializeField] private List<float?> AircanisterPressure = new List<float?>();
    [SerializeField] List<OxygenCube> oxygenCubes = new List<OxygenCube>();

    private void Update()
    {
        OxygenCalculations();
        WhenPowered();
    }
    private void WhenPowered()
    {
        if (SystemPower && PowerSwitchState)
        {
            if (CheckAirline())
            {
                UpdateOxygenCubes();
                DrainAircanisters();
                if (itemRegister.HasObject(out List<PumpController> pumps))
                {
                    pumps[0].ChangeAudioPlaying(true);
                }
            }
        }
        else
        {
            AircanisterPressure.Add(0f);
            AircanisterPressure.Add(0f);
            if (itemRegister.HasObject(out List<PumpController> pumps))
            {
                pumps[0].ChangeAudioPlaying(false);
            }
        }
    }
    private bool CheckAirline()
    {
        if (!ErrorCodes.CheckWorking<PumpController>(itemRegister))
        {
            return false;
        }
        if (AircanisterPressure[0] + AircanisterPressure[1] <= 0f)
        {
            return false;
        }
        return true;
    }
    private void OxygenCalculations()
    {
        AircanisterPressure.Clear();
        AircanisterPressure.Add(null); // Default Air Pressure on Aircanister 1 null Unless Debuging
        AircanisterPressure.Add(null); // Default Air Pressure on Aircanister 2 null Unless Debuging
        if (itemRegister.HasObject(out List<AirCanisterController> ObjectList))
        {
            int i = 0;
            foreach (AirCanisterController Aircanister in ObjectList)
            {
                AircanisterPressure[i] = Aircanister.Pressure;
                i++;
            }
        }
    }
    private void UpdateOxygenCubes()
    {
        foreach(OxygenCube cube in oxygenCubes)
        {
            if (cube.Oxygen < 100)
            {
                cube.ChangeOxygen(2 * Time.deltaTime);
            }
        }
    }
    private void DrainAircanisters()
    {
        if (itemRegister.HasObject(out List<AirCanisterController> ObjectList))
        {
            foreach (AirCanisterController Aircanister in ObjectList)
            {
                if (Aircanister.Pressure > 0f)
                {
                    Aircanister.ChangePressure(-.5f * Time.deltaTime);
                }
            }
        }
    }
}
