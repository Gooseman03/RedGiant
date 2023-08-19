using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageShip : MonoBehaviour
{
    public List<ItemRegister> itemRegisters = new List<ItemRegister>();
    private List<ObjectDirector> AllItemsInSystems = new List<ObjectDirector>();
    public void Register(ItemRegister value)
    {
        itemRegisters.Add(value);
    }

    public void ElecticalDamage()
    {
        ReloadAllItems();
        ItemRegister ToDamage = itemRegisters[Random.Range(0, itemRegisters.Count)];
        if (ToDamage.baseSystem.PowerSwitchState && ToDamage.baseSystem.SystemPower)
        {
            if (ToDamage.HasObject<PowerConnectorController>(out List<PowerConnectorController> PowerConnectors))
            {
                foreach (PowerConnectorController PowerConnector in PowerConnectors)
                {
                    PowerConnector.SetDurability(0);
                }
            }
            if (ToDamage.HasObject<FuseController>(out List<FuseController> Fuses))
            {
                foreach (FuseController Fuse in Fuses)
                {
                    Fuse.SetDurability(0);
                }
            }
        }
        MenuRequester.AddMessageToConsole(ToDamage + " Got Shocked");
    }
    public void DamageRandomItem(int Ammount = 1, float DamageAmmout = 0)
    {
            
        bool IsDamageRandom = false;
        if (DamageAmmout == 0)
        {
            IsDamageRandom = true;
        }
        ReloadAllItems();

        for (int i = 0; i < Ammount; i++)
        {
            if (IsDamageRandom)
            {
                DamageAmmout = Random.Range(10, 100);
            }
            while (!TryDamageItem()) { }
        }
        bool TryDamageItem()
        {
            if (AllItemsInSystems[Random.Range(0, AllItemsInSystems.Count)] is IDurable ObjectToDamage)
            {
                ObjectToDamage.ChangeDurability(-DamageAmmout);
                MenuRequester.AddMessageToConsole(ObjectToDamage + " Got Damaged");
                return true;
            }
            return false;
        }
        
    }
    private void ReloadAllItems()
    {
        AllItemsInSystems.Clear();
        foreach (ItemRegister item in itemRegisters)
        {
            item.ListAllObjects(out List<ObjectDirector> AllRegisterItems);
            foreach (ObjectDirector _object in AllRegisterItems)
            {
                AllItemsInSystems.Add(_object);
            }
        }
    }




}
