using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReactorOption : BaseSystem
{
    [SerializeField] private List<ObjectPlace> PowerSwitchs = new List<ObjectPlace>();
    [SerializeField] private List<Image> Fillbars = new List<Image>();
    [SerializeField] private List<ObjectPlace> PowerConnectors = new List<ObjectPlace>();
    [SerializeField] private int AvailablePowerLines = 0;
    [SerializeField] private List<BaseSystem> BaseSystems;
    [SerializeField] private List<bool> PowerDelivered;
    private Dictionary<BaseSystem, bool> PowerToSystems = new Dictionary<BaseSystem, bool>();
    void Update()
    {
        int i = 0;
        PowerToSystems.Clear();
        foreach (BaseSystem system in BaseSystems)
        {
            PowerToSystems.Add(system, PowerDelivered[i]);
            i++;
        }

        AvailablePowerLines = 0;
        if (itemRegister.HasObject<PowerConnectorController>(out List<PowerConnectorController> powerConnectors))
        {
            foreach(PowerConnectorController powerConnector in powerConnectors)
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
            if (PowerSwitch.ObjectGrabbable == null) { continue; }
            if (PowerSwitch.ObjectGrabbable is PowerSwitchController powerSwitch)
            {
                PowerDelivered[i] = powerSwitch.GetSwitchState();
                i++;
            }
        }

        if (PowerSwitchState == true)
        {
            i = 0;
            foreach (ItemRegister itemRegister in itemRegister.ship.itemRegisters)
            {
                if (itemRegister.baseSystem == this)
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
            foreach (ItemRegister itemRegister in itemRegister.ship.itemRegisters)
            {
                itemRegister.baseSystem.ReactorPower = false;
            }
        }

        int x = 0;
        foreach (ObjectPlace objectPlace in PowerConnectors)
        {
            Image FillImage = Fillbars[x];
            float Maxheight = 50;
            float height = 0;
            if (objectPlace.ObjectGrabbable == null)
            {
                height = 0;
            }
            else 
            {
                if (objectPlace.ObjectGrabbable is IDurable durableObject)
                {
                    height = durableObject.GetPercentDurability() * 100 / 2;
                }
                
            }

            FillImage.rectTransform.sizeDelta = new Vector2(10, height);
            FillImage.color = new Color((-height / Maxheight) + 1, height / Maxheight, 0);
            FillImage.rectTransform.localPosition = new Vector3( FillImage.rectTransform.localPosition.x, (height / 200) + .75f, FillImage.rectTransform.localPosition.z );
            x++;
        }
    }
}
