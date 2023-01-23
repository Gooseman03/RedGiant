using System.Collections.Generic;
using UnityEngine;

public class OxygenGenerator : MonoBehaviour
{
    ItemRegister itemRegister;
    [SerializeField] private bool SystemPower = true;
    [SerializeField] private List<string> DisplayOutputs = new List<string>();
    [SerializeField] private List<float?> AircanisterPressure = new List<float?>();

    [SerializeField] bool PowerSwitchState;
    [SerializeField] List<OxygenCube> oxygenCubes = new List<OxygenCube>();

    [SerializeField] private int ErrorThresholdDurability = 60;
    [SerializeField] private int ErrorThresholdPressure = 20;

    private bool ErrorBadFuse;
    private bool ErrorBadPowerConnector;
    private bool ErrorBadMonitor;
    private bool ErrorBadPowerSwitch;
    private bool ErrorBadPump;
    private bool ErrorLowAirCanister;

    private void Awake()
    {
        itemRegister = GetComponent<ItemRegister>();
    }

    private void FixedUpdate()
    {
        OxygenCalculations();
        WhenPowered();
        CheckForErrors();
        Display();
    }

    private void CheckForErrors()
    {
        CheckForBadCompontent(ObjectType.Fuse , ref ErrorBadFuse);
        CheckForBadCompontent(ObjectType.Monitor, ref ErrorBadMonitor);
        CheckForBadCompontent(ObjectType.PowerSwitch, ref ErrorBadPowerSwitch);
        CheckForBadCompontent(ObjectType.PowerConnector, ref ErrorBadPowerConnector);
        CheckForBadCompontent(ObjectType.Pump, ref ErrorBadPump);
        CheckForBadCompontent(ObjectType.AirCanister, true, ref ErrorLowAirCanister);
    }

    private void CheckForBadCompontent(ObjectType objectType, ref bool Error)
    {
        Error = false;
        if (itemRegister.HasObject(objectType, out List<ObjectGrabbable> List))
        {
            foreach (ObjectGrabbable Object in List)
            {
                if (Object.Durability < ErrorThresholdDurability)
                {
                    Error = true;
                }
            }
        }
    }
    private void CheckForBadCompontent(ObjectType objectType, bool PressureCheck, ref bool Error)
    {
        if (!PressureCheck)
        { return; }
        Error = false;
        if (itemRegister.HasObject(objectType, out List<ObjectGrabbable> List))
        {
            foreach (ObjectGrabbable Object in List)
            {
                if (Object.Pressure < ErrorThresholdPressure)
                {
                    Error = true;
                }
            }
        }
    }

    private void WhenPowered()
    {
        DisplayOutputs.Clear();
        SystemPower = CheckPowerLine();
        if (itemRegister.HasObject(ObjectType.PowerSwitch, out List<ObjectGrabbable> ListOfPowerSwitchs))
        {
            bool? SwitchState = ListOfPowerSwitchs[0].GetSwitchState();
            if (SwitchState == true)
            {
                PowerSwitchState = true;
            }
            else if (SwitchState == false)
            {
                PowerSwitchState = false;
            }
        }
        if (SystemPower && PowerSwitchState)
        {
            DisplayOutput();
            if (CheckAirline())
            {
                UpdateOxygenCubes();
                DrainAircanisters();
            }
        }
        else
        {
            DisplayOutputs.Add("No Power 1");
            DisplayOutputs.Add("No Power 2");
            AircanisterPressure.Add(0f);
            AircanisterPressure.Add(0f);
        }
    }

    private void DisplayOutput()
    {
        DisplayOutputs.Add("Error Display\n"); // Default Text on Display 1 
        DisplayOutputs.Add("Component Display"); // Default Text on Display 2

        if (AircanisterPressure[0] == null)
        {
            DisplayOutputs[1] = DisplayOutputs[1] + "\n Air Canister 0: Missing";
        }
        else DisplayOutputs[1] = DisplayOutputs[1] + "\n Air Canister 0: " + Mathf.Ceil((float)AircanisterPressure[0]).ToString() + "%";

        if (AircanisterPressure[1] == null)
        {
            DisplayOutputs[1] = DisplayOutputs[1] + "\n Air Canister 1: Missing";
        }
        else
        {
            DisplayOutputs[1] = DisplayOutputs[1] + "\n Air Canister 1: " + Mathf.Ceil((float)AircanisterPressure[1]).ToString() + "%";
        }

        List<ErrorTypes> ErrorList = new List<ErrorTypes>();
        if (ErrorBadFuse){ ErrorList.Add(ErrorTypes.ErrorBadFuse); }
        if (ErrorBadMonitor) { ErrorList.Add(ErrorTypes.ErrorBadMonitor); }
        if (ErrorBadPowerConnector) { ErrorList.Add(ErrorTypes.ErrorBadPowerConnector); }
        if (ErrorBadPowerSwitch) { ErrorList.Add(ErrorTypes.ErrorBadPowerSwitch); }
        if (ErrorBadPump) { ErrorList.Add(ErrorTypes.ErrorBadPump); }
        if (ErrorLowAirCanister) { ErrorList.Add(ErrorTypes.ErrorLowAirCanister); }
        DisplayOutputs[0] += ErrorCodes.ErrorCheck(ErrorList, false);
    }

    private bool pumpCheck()
    {
        if (itemRegister.HasObject(ObjectType.Pump, out List<ObjectGrabbable> PumpList))
        {
            bool HasGoodPump = false;
            foreach(ObjectGrabbable pump in PumpList)
            {
                if (pump.Durability > 20)
                {
                    HasGoodPump = true;
                }
            }
            if (!HasGoodPump)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    private bool CheckAirline()
    {
        if(!pumpCheck())
        {
            return false;
        }
        if (AircanisterPressure[0] + AircanisterPressure[1] < 0f)
        {
            return false;
        }
        return true;
    }

    private bool CheckPowerLine()
    {
        if (LoadPowerConnector() != true)
        {
            return false;
        }
        if (LoadFuses() == true)
        {
            return true;
        }
        return false;
    }

    private bool LoadFuses()
    {
        if (itemRegister.HasObject(ObjectType.Fuse,out List<ObjectGrabbable> FuseList))
        {
            bool HasGoodFuse = false;
            foreach (ObjectGrabbable Fuse in FuseList)
            {
                if (Fuse.Durability >= 20)
                {
                    HasGoodFuse = true;
                }
            }
            if (!HasGoodFuse)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    private bool LoadPowerConnector()
    {
        if (itemRegister.HasObject(ObjectType.PowerConnector, out List<ObjectGrabbable> PowerConnectorList))
        {
            bool HasGoodPowerConnector = false;
            foreach (ObjectGrabbable PowerConnector in PowerConnectorList)
            {
                if (PowerConnector.Durability >= 20)
                {
                    HasGoodPowerConnector = true;
                }
            }
            if (!HasGoodPowerConnector)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    private void OxygenCalculations()
    {
        ErrorLowAirCanister = false;
        AircanisterPressure.Clear();
        AircanisterPressure.Add(null); // Default Air Pressure on Aircanister 1 null Unless Debuging
        AircanisterPressure.Add(null); // Default Air Pressure on Aircanister 2 null Unless Debuging
        if (itemRegister.HasObject(ObjectType.AirCanister, out List<ObjectGrabbable> ObjectList))
        {
            int i = 0;
            foreach (ObjectGrabbable Aircanister in ObjectList)
            {
                if (AircanisterPressure[i] < 20)
                {
                    ErrorLowAirCanister = true;
                }
                AircanisterPressure[i] = Aircanister.Pressure;
                i++;
            }
        }
    }

    private void UpdateOxygenCubes()
    {
        foreach(OxygenCube cube in oxygenCubes)
        {
            if (cube.GetOxygen() < 100)
            {
                cube.ChangeOxygen(2*Time.deltaTime);
            }
        }
    }

    private void DrainAircanisters()
    {
        if (itemRegister.HasObject(ObjectType.AirCanister, out List<ObjectGrabbable> ObjectList))
        {
            float TotalLoss = -1 * Time.deltaTime;
            int i = 0;
            foreach (ObjectGrabbable Aircanister in ObjectList)
            {
                if (Aircanister.Pressure > 0f)
                {
                    if (i == 0 && ObjectList.Count >= 2) { TotalLoss /= 2;}
                    Aircanister.ChangePressure(TotalLoss);
                }
                i++;
            }
        }
    }

    private void Display()
    {
        string output;
        if (itemRegister.HasObject(ObjectType.Monitor , out List<ObjectGrabbable> ObjectList))
        {
            int i = 0;
            foreach (ObjectGrabbable Object in ObjectList)
            {
                output = DisplayOutputs[i];
                Object.ChangeMonitorText(output);
                i++;
            }
        }
    }
}
