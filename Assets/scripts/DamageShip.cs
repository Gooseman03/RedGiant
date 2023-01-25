using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageShip : MonoBehaviour
{
    [SerializeField] private bool ShouldDmg;
    private List<ItemRegister> itemRegisters = new List<ItemRegister>();
    private List<ObjectGrabbable> AllItemsInSystems = new List<ObjectGrabbable>();

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
        ReloadAllItems();
        ObjectGrabbable ObjectToDamage = AllItemsInSystems[Random.Range(0, AllItemsInSystems.Count)];
        bool reset = false;
        float i = 0;
        while (ObjectToDamage.Durability == null && !reset)
        { 
            ObjectToDamage = AllItemsInSystems[Random.Range(0, AllItemsInSystems.Count)];
            if (ObjectToDamage.objectType == ObjectType.Monitor)
            {
                if (Random.Range(0, 100) > 50)
                {
                    reset = true;
                }
            }
            if (i > 10) 
            {
                Debug.LogError("Damage Item Gave Up after 10 trys");
            } 
            i++;
        }
        

        ObjectToDamage.ChangeDurability(-Random.Range(0, 100));
        Debug.Log(ObjectToDamage + "Got Damaged");
    }
    private void ReloadAllItems()
    {
        foreach (ItemRegister item in itemRegisters)
        {
            item.ListAllObjects(out List<ObjectGrabbable> AllRegisterItems);
            foreach (ObjectGrabbable _object in AllRegisterItems)
            {
                AllItemsInSystems.Add(_object);
            }
        }
    }




}
