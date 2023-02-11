using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRegister : MonoBehaviour
{
    public BaseSystem baseSystem;
    [SerializeField] private DamageShip _ship;
    private ObjectDirector LastItemRemoved;

    public DamageShip ship
    {
        get { return _ship; }
        set { _ship = value; }
    }
    private void Start()
    {
        baseSystem = GetComponent<BaseSystem>();
    }
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
        this.SendMessage("ObjectPulled",item,SendMessageOptions.DontRequireReceiver);
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
