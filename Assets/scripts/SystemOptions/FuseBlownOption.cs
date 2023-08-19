using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBlownOption : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    [SerializeField] private List<ObjectType> LiveObjects;
    
    //TODO
    //private void ObjectPulled(ObjectDirector item)
    //{
    //    if (LiveObjects.Contains(item.objectType))
    //    {
    //        if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
    //        {
    //            BlowFuse();
    //        }
    //    }
    //}
    private void BlowFuse()
    {
        bool ShockPlayer = false;
        if (baseSystem.itemRegister.HasObject<FuseController>(out List<FuseController> ObjectList))
        {
            foreach (FuseController fuse in ObjectList)
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
