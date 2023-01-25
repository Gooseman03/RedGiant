using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBlownOption : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    [SerializeField] private List<ObjectType> LiveObjects;
    private void ObjectPulled(ObjectGrabbable item)
    {
        if (LiveObjects.Contains(item.objectType))
        {
            if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
            {
                BlowFuse();
            }
        }
    }
    private void BlowFuse()
    {
        bool ShockPlayer = false;
        if (baseSystem.itemRegister.HasObject(ObjectType.Fuse, out List<ObjectGrabbable> ObjectList))
        {
            foreach (ObjectGrabbable fuse in ObjectList)
            {
                if (fuse.Durability > 0)
                {
                    fuse.ChangeDurability(-(float)fuse.Durability);
                }
                else { ShockPlayer = true; }
            }
        }
        else
        {
            ShockPlayer = true;
        }
        if (ShockPlayer)
        {
            baseSystem.itemRegister.ShockPlayer();
        }
    }

}
