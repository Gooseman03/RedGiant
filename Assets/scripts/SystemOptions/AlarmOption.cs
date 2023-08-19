using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmOption : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    void Update()
    {
        if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
        {
            if (baseSystem.itemRegister.HasObject<AlarmController>(out List<AlarmController> Alarms))
            {
                foreach (AlarmController alarm in Alarms)
                {
                    alarm.UpdateErrorTypes(baseSystem.Errors);
                    alarm.AlarmUpdate();
                }
            }
        }
        else
        {
            if (baseSystem.itemRegister.HasObject<AlarmController>(out List<AlarmController> Alarms))
            {
                foreach (AlarmController alarm in Alarms)
                {
                    alarm.UpdateErrorTypes(new List<ErrorTypes>());
                    alarm.AlarmUpdate();
                }
            }
        }
        
    }
}
