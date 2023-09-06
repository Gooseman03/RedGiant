using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BaseSystem))]

public class ItemRegister : MonoBehaviour
{
    private BaseSystem _baseSystem;
    public BaseSystem baseSystem { private set { _baseSystem = value; } get { return _baseSystem; }}
    [SerializeField] private DamageShip _ship;
    public DamageShip ship { get { return _ship; } }
    private ObjectDirector LastItemRemoved;
    private void Awake()
    {
        baseSystem = GetComponent<BaseSystem>();
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
    public bool HasObject<T>(out List<T> OutputList)
    {
        OutputList = new List<T>();
        foreach (ObjectDirector item in Objects)
        {
            if (item is T listItem)
            {
                OutputList.Add(listItem);
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
