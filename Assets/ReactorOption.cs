using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReactorOption : MonoBehaviour
{
    [SerializeField] private BaseSystem _baseSystem;
    public BaseSystem baseSystem
    {
        get { return _baseSystem; }
    }

    [SerializeField] private List<ObjectPlace> PowerSwitchs = new List<ObjectPlace>();
    [SerializeField] private List<Image> Fillbars = new List<Image>();
    [SerializeField] private List<ObjectPlace> PowerConnectors = new List<ObjectPlace>();
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
        if (baseSystem.itemRegister.HasObject<PowerConnectorController>(out List<PowerConnectorController> powerConnectors))
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

        int x = 0;
        foreach (ObjectPlace objectPlace in PowerConnectors)
        {
            Image FillImage = Fillbars[x];
            int Maxheight = 50;
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
            FillImage.rectTransform.localPosition = new Vector3(
                FillImage.rectTransform.localPosition.x,
                (float)((height / 200) + .75),
                FillImage.rectTransform.localPosition.z
                );
            x++;
        }
    }
}
