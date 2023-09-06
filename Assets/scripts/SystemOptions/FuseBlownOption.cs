using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BaseSystem))]

public class FuseBlownOption : MonoBehaviour
{
    private BaseSystem baseSystem;
    [SerializeField] private List<ObjectType> LiveObjects;
    private void Awake()
    {
        baseSystem = GetComponent<BaseSystem>();
    }
    private void ObjectPulled(ObjectDirector item)
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
        if (baseSystem.itemRegister.HasObject<FuseController>(out List<FuseController> ObjectList))
        {
            foreach (FuseController fuse in ObjectList)
            {
                if (fuse.Durability > 0)
                {
                    fuse.SetDurability(0);
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
