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

    private bool ErrorBadFuse;
    private bool ErrorBadPowerConnector;
    private bool ErrorBadMonitor;

    private void Awake()
    {
        itemRegister = GetComponent<ItemRegister>();
    }

    private void FixedUpdate()
    {
        OxygenCalculations();
        CheckPower();
        Display();
    }

    private void CheckPower()
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
            LoadMonitor();
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
        DisplayOutputs.Add("Error Display"); // Default Text on Display 1 
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
        if (ErrorBadFuse == true)
        {
            DisplayOutputs[0] = DisplayOutputs[0] + "\n Bad Fuse";
        }
        if (ErrorBadPowerConnector == true)
        {
            DisplayOutputs[0] = DisplayOutputs[0] + "\n Bad Power Connector";
        }
        if (ErrorBadMonitor == true)
        {
            DisplayOutputs[0] = DisplayOutputs[0] + "\n Bad Monitor";
        }
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
        ErrorBadFuse = false;
        if (itemRegister.HasObject(ObjectType.Fuse,out List<ObjectGrabbable> FuseList))
        {
            bool HasGoodFuse = false;
            foreach (ObjectGrabbable Fuse in FuseList)
            {
                if (Fuse.Durability >= 20)
                {
                    HasGoodFuse = true;
                }
                if (Fuse.Durability < 40)
                {
                    ErrorBadFuse = true;
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
        ErrorBadPowerConnector = false;
        if (itemRegister.HasObject(ObjectType.PowerConnector, out List<ObjectGrabbable> PowerConnectorList))
        {
            bool HasGoodPowerConnector = false;
            foreach (ObjectGrabbable PowerConnector in PowerConnectorList)
            {
                if (PowerConnector.Durability >= 20)
                {
                    HasGoodPowerConnector = true;
                }
                if (PowerConnector.Durability < 40)
                {
                    ErrorBadPowerConnector = true;
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
    private bool LoadMonitor()
    {
        ErrorBadMonitor = false;
        if (itemRegister.HasObject(ObjectType.Monitor, out List<ObjectGrabbable> MonitorList))
        {
            bool HasGoodMonitor = false;
            foreach (ObjectGrabbable Monitor in MonitorList)
            {
                if (Monitor.Durability >= 20)
                {
                    HasGoodMonitor = true;
                }
                if (Monitor.Durability < 60)
                {
                    ErrorBadMonitor = true;
                }
            }
            if (!HasGoodMonitor)
            {
                return false;
            }
            return true;
        }
        return false;
    }






    private void OxygenCalculations()
    {
        AircanisterPressure.Clear();
        AircanisterPressure.Add(null); // Default Air Pressure on Aircanister 1 0f Unless Debuging
        AircanisterPressure.Add(null); // Default Air Pressure on Aircanister 2 0f Unless Debuging
        if (itemRegister.HasObject(ObjectType.AirCanister, out List<ObjectGrabbable> ObjectList))
        {
            int i = 0;
            foreach (ObjectGrabbable Aircanister in ObjectList)
            {
                AircanisterPressure[i] = Aircanister.GetPressure();
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
            foreach (ObjectGrabbable Aircanister in ObjectList)
            {
                if (ObjectList.Count > 1)
                {
                    Aircanister.ChangePressure(TotalLoss / 2);
                }
                else Aircanister.ChangePressure(TotalLoss);
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
