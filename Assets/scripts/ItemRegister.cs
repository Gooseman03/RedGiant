using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRegister : MonoBehaviour
{
    public List<ObjectGrabbable> Objects = new List<ObjectGrabbable>();

    public void RegisterObject(ObjectGrabbable item)
    {
        Objects.Add(item);
    }

    public void UnregisterObject(ObjectGrabbable item)
    {
        Objects.Remove(item);
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
            if (item.GetObjectType() == type)
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
}
