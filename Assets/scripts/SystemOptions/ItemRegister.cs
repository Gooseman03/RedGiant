using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRegister : MonoBehaviour
{
    [SerializeField]private DamageShip ship;
    private ObjectGrabbable LastItemRemoved;
    private void Awake()
    {
        ship.Register(this);
    }
    public List<ObjectGrabbable> Objects = new List<ObjectGrabbable>();

    public void RegisterObject(ObjectGrabbable item)
    {
        Objects.Add(item);
        LastItemRemoved = null;
    }

    public void ShockPlayer()
    {
        LastItemRemoved.SendMessage("ShockPlayer");
    }

    public void UnregisterObject(ObjectGrabbable item)
    {
        LastItemRemoved = item;
        Objects.Remove(item);
        this.SendMessage("ObjectPulled",item);
    }

    public bool CheckForObject(ObjectGrabbable item)
    {
        if (Objects.Contains(item))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasObject(ObjectType type, out List<ObjectGrabbable> OutputList)
    {
        OutputList = new List<ObjectGrabbable>();
        foreach (ObjectGrabbable item in Objects)
        {
            if (item.objectType == type)
            {
                OutputList.Add(item);
            }
        }

        if (OutputList.Count > 0)
        {
            return true;
        }
        return false;
    }

    public void ListAllObjects(out List<ObjectGrabbable> OutputList)
    {
        OutputList = Objects;
    }
}
