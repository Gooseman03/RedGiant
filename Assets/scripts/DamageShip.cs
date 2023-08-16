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
            if (ToDamage.HasObject(ObjectType.PowerConnector, out List<ObjectDirector> PowerConnectors))
            {
                foreach (ObjectDirector PowerConnector in PowerConnectors)
                {
                    PowerConnector.SetDurability(0);
                }
            }
            if (ToDamage.HasObject(ObjectType.Fuse, out List<ObjectDirector> Fuses))
            {
                foreach (ObjectDirector Fuse in Fuses)
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
            TryDamageItem();
        }
        void TryDamageItem()
        {
            ObjectDirector ObjectToDamage = AllItemsInSystems[Random.Range(0, AllItemsInSystems.Count)];
            float i = 0;
            while (ObjectToDamage.Durability == null || ObjectToDamage.Durability < 0)
            {
                ObjectToDamage = AllItemsInSystems[Random.Range(0, AllItemsInSystems.Count)];
                if (i > 10)
                {
                    MenuRequester.AddMessageToConsole("Damage Item Gave Up after 10 trys", MessageType.Warn);
                    return;
                }
                i++;
            }
            ObjectToDamage.ChangeDurability(-DamageAmmout);
            MenuRequester.AddMessageToConsole(ObjectToDamage + " Got Damaged");
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
