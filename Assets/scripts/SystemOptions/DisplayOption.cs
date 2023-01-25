using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOption : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    [SerializeField] private List<string> DisplayOutputs = new List<string>();
    [SerializeField] private List<ObjectType> WantsDisplayed = new List<ObjectType>();



    private void FixedUpdate()
    {
        WhenPowered();
        Display();
    }

    private void WhenPowered()
    {
        DisplayOutputs.Clear();
        
        if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
        {
            DisplayOutput();
        }
        else
        {
            DisplayOutputs.Add("");
            DisplayOutputs.Add("");
        }
    }

    private void DisplayOutput()
    {
        DisplayOutputs.Add("Error Display\n"); // Default Text on Display 1 
        DisplayOutputs.Add("Component Display"); // Default Text on Display 2
        foreach(ObjectType objectType in WantsDisplayed)
        {
            if (baseSystem.itemRegister.HasObject(objectType,out List<ObjectGrabbable> ObjectList))
            {
                foreach (ObjectGrabbable Object in ObjectList)
                {
                    if (objectType == ObjectType.AirCanister || objectType == ObjectType.Co2Canister)
                    {
                        DisplayOutputs[1] += "\n" + objectType + ":\t" + Mathf.FloorToInt((float)Object.Pressure) + "%";
                    }
                    else
                    {
                        DisplayOutputs[1] += "\n" + objectType + ":\t" + Mathf.FloorToInt((float)Object.Durability) + "%";
                    }
                }
            }
            else DisplayOutputs[1] += "\n" + objectType + "Missing";
        }
        DisplayOutputs[0] += baseSystem.ErrorText;
    }

    private void Display()
    {
        string output;
        if (baseSystem.itemRegister.HasObject(ObjectType.Monitor, out List<ObjectGrabbable> ObjectList))
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
