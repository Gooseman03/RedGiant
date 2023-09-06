using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BaseSystem))]
public class DisplayOption : MonoBehaviour
{
    private BaseSystem baseSystem;
    private List<string> DisplayOutputs = new List<string>();
    private void Awake()
    {
        baseSystem = GetComponent<BaseSystem>();
    }
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

        }
    }
    private void DisplayOutput()
    {
        DisplayOutputs.Add("Error Display\n"); // Default Text on Display 1 
        DisplayOutputs[0] += baseSystem.ErrorText;
    }
    private void Display()
    {
        string output= "";
        if (baseSystem.itemRegister.HasObject<MonitorController>(out List<MonitorController> ObjectList))
        {
            int i = 0;
            foreach (MonitorController Object in ObjectList)
            {
                output = DisplayOutputs[i];
                Object.ChangeMonitorText(output);
                i++;
            }
        }
    }
}
