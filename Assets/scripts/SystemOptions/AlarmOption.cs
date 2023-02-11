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
            if (baseSystem.itemRegister.HasObject(ObjectType.Alarm, out List<ObjectDirector> Alarms))
            {
                foreach (ObjectDirector alarm in Alarms)
                {
                    alarm.UpdateAlarmErrors(baseSystem.Errors);
                    alarm.AlarmUpdate();
                }
            }
        }
        else
        {
            if (baseSystem.itemRegister.HasObject(ObjectType.Alarm, out List<ObjectDirector> Alarms))
            {
                foreach (ObjectDirector alarm in Alarms)
                {
                    alarm.UpdateAlarmErrors(new List<ErrorTypes>());
                    alarm.AlarmUpdate();
                }
            }
        }
        
    }
}
