using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRegister : MonoBehaviour
{
    [SerializeField]private DamageShip ship;
    private ObjectDirector LastItemRemoved;
    private void Awake()
    {
        ship.Register(this);
    }
    public List<ObjectDirector> Objects = new List<ObjectDirector>();

    public void RegisterObject(ObjectDirector item)
    {
        Objects.Add(item);
        LastItemRemoved = null;
    }

    public void ShockPlayer()
    {
        LastItemRemoved.SendMessage("ShockPlayer");
    }

    public void UnregisterObject(ObjectDirector item)
    {
        LastItemRemoved = item;
        Objects.Remove(item);
        this.SendMessage("ObjectPulled",item);
    }

    public bool CheckForObject(ObjectDirector item)
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

    public bool HasObject(ObjectType type, out List<ObjectDirector> OutputList)
    {
        OutputList = new List<ObjectDirector>();
        foreach (ObjectDirector item in Objects)
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

    public void ListAllObjects(out List<ObjectDirector> OutputList)
    {
        OutputList = Objects;
    }
}
