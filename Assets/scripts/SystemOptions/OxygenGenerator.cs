using System.Collections.Generic;
using UnityEngine;

public class OxygenGenerator : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    [SerializeField] private List<float?> AircanisterPressure = new List<float?>();
    [SerializeField] List<OxygenCube> oxygenCubes = new List<OxygenCube>();

    private void FixedUpdate()
    {
        OxygenCalculations();
        WhenPowered();
    }
    private void WhenPowered()
    {
        if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
        {
            if (CheckAirline())
            {
                UpdateOxygenCubes();
                DrainAircanisters();
                if (baseSystem.itemRegister.HasObject<PumpController>(out List<PumpController> pumps))
                {
                    pumps[0].playAudio();
                }
            }
        }
        else
        {
            AircanisterPressure.Add(0f);
            AircanisterPressure.Add(0f);
            if (baseSystem.itemRegister.HasObject<PumpController>(out List<PumpController> pumps))
            {
                pumps[0].stopAudio();
            }
        }
    }
    private bool CheckAirline()
    {
        if (!ErrorCodes.CheckWorking<PumpController>(baseSystem.itemRegister))
        {
            return false;
        }
        if (AircanisterPressure[0] + AircanisterPressure[1] < 0f)
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
        if (baseSystem.itemRegister.HasObject<AirCanisterController>(out List<AirCanisterController> ObjectList))
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
        if (baseSystem.itemRegister.HasObject<AirCanisterController>(out List<AirCanisterController> ObjectList))
        {
            foreach (AirCanisterController Aircanister in ObjectList)
            {
                if (Aircanister.Pressure > 0f)
                {
                    Aircanister.ChangePressure(- .5f * Time.deltaTime);
                }
            }
        }
    }
}
