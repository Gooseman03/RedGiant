using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OxygenGenerator : MonoBehaviour
{
    ItemRegister itemRegister;
    [SerializeField] private bool SystemPower = true;
    [SerializeField] private List<string> DisplayOutputs = new List<string>();
    [SerializeField] private List<float?> AircanisterPressure = new List<float?>();


    private bool BadFuse;
    private bool BadPowerConnector;


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

        if (SystemPower)
        {
            DisplayOutputs.Add("Error Display"); // Default Text on Display 1 
            DisplayOutputs.Add("Component Display"); // Default Text on Display 2

            DisplayOutputs[1] = DisplayOutputs[1] + "\n Air Canister 0: " + AircanisterPressure[0].ToString();
            DisplayOutputs[1] = DisplayOutputs[1] + "\n Air Canister 1: " + AircanisterPressure[1].ToString();

            if (BadFuse == true)
            {
                DisplayOutputs[0] = DisplayOutputs[0] + "\n Bad Fuse";
            }
            if (BadPowerConnector == true)
            {
                DisplayOutputs[0] = DisplayOutputs[0] + "\n Bad Power Connector";
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
        BadFuse = true;
        if (itemRegister.HasObject(ObjectType.Fuse,out List<ObjectGrabbable> FuseList))
        {
            BadFuse = false;
            foreach (ObjectGrabbable Fuse in FuseList)
            {
                
                if (Fuse.GetDurability() <= 60)
                {
                    BadFuse = true;
                }
                if (Fuse.GetDurability() >= 20)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    private bool LoadPowerConnector()
    {
        if (itemRegister.HasObject(ObjectType.PowerConnector, out List<ObjectGrabbable> PowerConnectorList))
        {
            foreach (ObjectGrabbable PowerConnector in PowerConnectorList)
            {
                if (PowerConnector.GetDurability() >= 20)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }


private void OxygenCalculations()
    {
        AircanisterPressure.Clear();
        AircanisterPressure.Add(0f); // Default Air Pressure on Aircanister 1 0f Unless Debuging
        AircanisterPressure.Add(0f); // Default Air Pressure on Aircanister 2 0f Unless Debuging
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
