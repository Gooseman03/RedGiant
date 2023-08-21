using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class ObjectPlace : MonoBehaviour
{
    public int Number;
    [SerializeField] private ObjectType _objectType;
    [SerializeField] private bool TempStorage;
    public ObjectType objectType
    {
        get { return _objectType; }
        private set { _objectType = value; }
    }
    private ObjectDirector _objectGrabbable;
    public ObjectDirector ObjectGrabbable 
    {
        get { return _objectGrabbable; } 
        private set { _objectGrabbable = value; } 
    }
    [SerializeField] protected ObjectReferences objectReferences;
    [SerializeField] private ObjectType Preplace;

    private GameObject Appearance;
    private bool isRegistered = false;
    private ObjectDirector LastObjectGrabbable;
    bool SetupComplete = false;

    [SerializeField] private bool WillPreplace;
    [SerializeField] private GameObject PrefabObjectGrabbable;
    private void Start()
    {
        Appearance = new GameObject();
        Appearance.transform.parent = this.transform;
        Appearance.transform.localPosition = Vector3.zero;
        Appearance.transform.localRotation = Quaternion.identity;
        Appearance.name = "Look";
        MeshFilter meshFilter = Appearance.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = Appearance.AddComponent<MeshRenderer>();
        objectReferences.GetConstructorItemReferences(objectType, true, out List<Mesh> newMeshs, out Material newMaterial);
        meshFilter.mesh = newMeshs[0];
        this.GetComponent<MeshCollider>().sharedMesh = newMeshs[0];
        meshRenderer.material = newMaterial;


        if (Number == 0)
        {
            this.name = objectType.ToString() + " Place";
        }
        else
        {
            this.name = Number.ToString() + " " + objectType.ToString() + " Place";
        }
        
        if (Preplace == objectType && WillPreplace)
        {
            PrefabObjectGrabbable.SetActive(false);
            GameObject gameObject = Instantiate(PrefabObjectGrabbable);
            SetupComplete = true;
            if (objectType == ObjectType.Monitor)
            {
                MonitorController newObject = gameObject.AddComponent<MonitorController>();
                newObject.objectReferences = objectReferences;
                newObject.objectType = ObjectType.Monitor;
            }
            if (objectType == ObjectType.Fuse)
            {
                FuseController newObject = gameObject.AddComponent<FuseController>();
                newObject.objectReferences = objectReferences;
                newObject.objectType = ObjectType.Fuse;
            }
            if (objectType == ObjectType.PowerConnector)
            {
                PowerConnectorController newObject = gameObject.AddComponent<PowerConnectorController>();
                newObject.objectReferences = objectReferences;
                newObject.objectType = ObjectType.PowerConnector;
            }
            if (objectType == ObjectType.PowerSwitch)
            {
                PowerSwitchController newObject = gameObject.AddComponent<PowerSwitchController>();
                newObject.objectReferences = objectReferences;
                newObject.objectType = ObjectType.PowerSwitch;
            }
            if (objectType == ObjectType.AirCanister)
            {
                AirCanisterController newObject = gameObject.AddComponent<AirCanisterController>();
                newObject.objectReferences = objectReferences;
                newObject.objectType = ObjectType.AirCanister;
            }
            if (objectType == ObjectType.Co2Canister)
            {
                Co2CanisterController newObject = gameObject.AddComponent<Co2CanisterController>();
                newObject.objectReferences = objectReferences;
                newObject.objectType = ObjectType.Co2Canister;
            }
            if (objectType == ObjectType.Pump)
            {
                PumpController newObject = gameObject.AddComponent<PumpController>();
                newObject.objectReferences = objectReferences;
                newObject.objectType = ObjectType.Pump;
            }
            if (objectType == ObjectType.AirFilter)
            {
                AirFilterController newObject = gameObject.AddComponent<AirFilterController>();
                newObject.objectReferences = objectReferences;
                newObject.objectType = ObjectType.AirFilter;
            }
            if (objectType == ObjectType.Alarm)
            {
                AlarmController newObject = gameObject.AddComponent<AlarmController>();
                newObject.objectReferences = objectReferences;
                newObject.objectType = ObjectType.Alarm;
            }
            if (objectType == ObjectType.Keyboard)
            {
                KeyboardController newObject = gameObject.AddComponent<KeyboardController>();
                newObject.objectReferences = objectReferences;
                newObject.objectType = ObjectType.Keyboard;
            }
            if (objectType == ObjectType.Generic)
            {
                MenuRequester.AddMessageToConsole("Cannot Spawn Generic item", MessageType.Error);
            }
            gameObject.SetActive(true);
            gameObject.GetComponent<IGrabbable>().Place(this.transform);
        }
        SetupComplete = true;
    }

    private void OnTransformChildrenChanged()
    {
        if (!SetupComplete) { return; }
        
        LastObjectGrabbable = ObjectGrabbable;
        ObjectGrabbable = gameObject.transform.GetComponentInChildren<ObjectDirector>();
        

        if (ObjectGrabbable != null)
        {
            this.GetComponent<Collider>().enabled = false;
            Appearance.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            this.GetComponent<Collider>().enabled = true;
            Appearance.GetComponent<MeshRenderer>().enabled = true;
        }

        ItemRegister parentObjectRegister;
        if (this.transform.parent.TryGetComponent<ItemRegister>(out parentObjectRegister) != true)
        {
            return;
        }
        if (TempStorage) { return; }
        if (isRegistered && ObjectGrabbable == null)
        {
            parentObjectRegister.UnregisterObject(LastObjectGrabbable);
            if (LastObjectGrabbable is IDisconnect disconnectObject) { disconnectObject.Disconnect(); }
            isRegistered = false;
            MenuRequester.AddMessageToConsole(
                "ObjectPlace.OnTransformChildrenChanged() Unregistered object: "
                    + LastObjectGrabbable.name
            );
            return;
        }
        if (!isRegistered && parentObjectRegister.CheckForObject(ObjectGrabbable) == false)
        {
            parentObjectRegister.RegisterObject(ObjectGrabbable);
            isRegistered = true;
            MenuRequester.AddMessageToConsole(
                "ObjectPlace.OnTransformChildrenChanged() Registered object: "
                    + ObjectGrabbable.name
            );
            return;
        }

        MenuRequester.AddMessageToConsole(
            "Error: ObjectPlace.OnTransformChildrenChanged() isRegistered is neither true nor false", MessageType.Error
        );
    }

    private void OnDrawGizmos()
    {
        objectReferences.GetConstructorItemReferences(objectType, true, out List<Mesh> newMeshs, out Material newMaterial);
        if (Number == 0)
        {
            this.name = objectType.ToString() + " Place";
        }
        else
        {
            this.name = Number.ToString() + " " + objectType.ToString() + " Place";
        }
        Gizmos.color = newMaterial.color;
        Gizmos.DrawMesh(newMeshs[0], 0, transform.position,transform.rotation);
    }
}
