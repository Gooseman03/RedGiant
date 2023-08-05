using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReactorOption : MonoBehaviour
{
    [SerializeField] private BaseSystem _baseSystem;
    public BaseSystem baseSystem
    {
        get { return _baseSystem; }
    }

    [SerializeField] private List<ObjectPlace> PowerSwitchs = new List<ObjectPlace>();
    [SerializeField] private int AvailablePowerLines = 0;
    [SerializeField] private List<BaseSystem> BaseSystems;
    [SerializeField] private List<bool> PowerDelivered;
    private Dictionary<BaseSystem, bool> PowerToSystems = new Dictionary<BaseSystem, bool>();

    private void Start()
    {
        int i = 0;
        foreach(BaseSystem system in BaseSystems)
        {
            PowerDelivered.Add(false);
            PowerToSystems.Add(system, PowerDelivered[i]);
            i++;
        }
    }
    void FixedUpdate()
    {
        int i = 0;
        PowerToSystems.Clear();
        foreach (BaseSystem system in BaseSystems)
        {
            PowerToSystems.Add(system, PowerDelivered[i]);
            i++;
        }

        AvailablePowerLines = 0;
        if (baseSystem.itemRegister.HasObject(ObjectType.PowerConnector,out List<ObjectDirector> powerConnectors))
        {
            foreach(ObjectDirector powerConnector in powerConnectors)
            {
                if (ErrorCodes.CheckWorking(powerConnector))
                {
                    AvailablePowerLines++;
                }
            }
        }

        i = 0;
        foreach(ObjectPlace PowerSwitch in PowerSwitchs)
        {
            if (i >= PowerDelivered.Count) { break; }
            if (PowerSwitch.objectGrabbable == null || PowerSwitch.objectGrabbable.GetSwitchState() == null) { continue; }
            PowerDelivered[i] = (bool)PowerSwitch.objectGrabbable.GetSwitchState();
            i++;
        }

        if (baseSystem.PowerSwitchState == true)
        {
            i = 0;
            foreach (ItemRegister itemRegister in baseSystem.itemRegister.ship.itemRegisters)
            {
                if (itemRegister.baseSystem == baseSystem)
                {
                    itemRegister.baseSystem.ReactorPower = true;
                    continue;
                }
                if ((i < AvailablePowerLines && PowerToSystems.ContainsKey(itemRegister.baseSystem) && PowerToSystems[itemRegister.baseSystem]))
                {
                    itemRegister.baseSystem.ReactorPower = true;
                    i++;
                }
                else
                {
                    itemRegister.baseSystem.ReactorPower = false;
                }
            }
        }
        else
        {
            foreach (ItemRegister itemRegister in baseSystem.itemRegister.ship.itemRegisters)
            {
                itemRegister.baseSystem.ReactorPower = false;
            }
        }
    }
}
