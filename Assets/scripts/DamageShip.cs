using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageShip : MonoBehaviour
{
    [SerializeField] private bool ShouldDmg;
    [SerializeField] private bool ElectricDamage;
    [SerializeField] private bool TrueRandomDamage;
    public List<ItemRegister> itemRegisters = new List<ItemRegister>();
    private List<ObjectDirector> AllItemsInSystems = new List<ObjectDirector>();

    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 10)
        {
            timer = 0;
            DamageItem();
        }
    }

    public void Register(ItemRegister value)
    {
        itemRegisters.Add(value);
    }


    public void DamageItem()
    {
        if (ShouldDmg!= true) { return; }

        if (ElectricDamage)
        {
            ItemRegister ToDamage = itemRegisters[Random.Range(0, itemRegisters.Count)];
            if (ToDamage.baseSystem.PowerSwitchState && ToDamage.baseSystem.SystemPower)
            {
                if(ToDamage.HasObject(ObjectType.PowerConnector, out List<ObjectDirector> PowerConnectors))
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
            Debug.Log(ToDamage + " Got Shocked");
        }

        if (TrueRandomDamage)
        {
            ReloadAllItems();
            ObjectDirector ObjectToDamage = AllItemsInSystems[Random.Range(0, AllItemsInSystems.Count)];
            float i = 0;
            while (ObjectToDamage.Durability == null)
            {
                ObjectToDamage = AllItemsInSystems[Random.Range(0, AllItemsInSystems.Count)];
                if (i > 10)
                {
                    Debug.LogError("Damage Item Gave Up after 10 trys");
                }
                i++;
            }
            ObjectToDamage.ChangeDurability(-Random.Range(0, 100));
            Debug.Log(ObjectToDamage + " Got Damaged");
        }
    }
    private void ReloadAllItems()
    {
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
