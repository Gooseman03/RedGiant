using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour

{
    [SerializeField] private ObjectReferences objectReferences;
    [SerializeField] private ObjectType _objectType;
    public ObjectType objectType 
    { 
        get { return _objectType; } 
        set { _objectType = value; }
    }
    private Rigidbody objectRigidbody;
    [SerializeField] private float DurabilityOverride;
    [SerializeField, Range(0f, 100f)] private float? _Durability;
    [SerializeField, Range(0f, 100f)] private float? _Pressure;
    [SerializeField] private float? _MaxCurrent;
    public float? Durability 
    { 
        get { return _Durability; } 
        private set { _Durability = value; } 
    }
    public float? Pressure   
    {
        get { return _Pressure; } 
        private set { _Pressure = value; }
    }
    public float? MaxCurrent 
    { 
        get { return _MaxCurrent; }
        private set { _MaxCurrent = value; }
    }

    public int method { get; internal set; }

    private MonitorController MonitorText;
    private PowerSwitchController PowerSwitch;

    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        objectReferences.GetConstructorItemReferences(objectType, false, out Mesh newMesh, out Material newMaterial);
        objectReferences.GetStatsItemReferences(objectType, out Dictionary<string, float?> ObjectStats);

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        meshFilter.mesh = newMesh;
        meshRenderer.material = newMaterial;

        this.GetComponent<MeshCollider>().sharedMesh = newMesh;

        if (objectType == ObjectType.Monitor)
        {
            MonitorText =  this.AddComponent<MonitorController>();
        }
        if (objectType == ObjectType.PowerSwitch)
        {
            PowerSwitch = this.AddComponent<PowerSwitchController>();
            PowerSwitch.StartUp(objectReferences);
        }    

        this.name = objectType.ToString() + " Object";

        Durability = ObjectStats["Durability"];
        Pressure = ObjectStats["Pressure"];
        MaxCurrent = ObjectStats["MaxCurrent"];
    }
    private void Update()
    {
        if (DurabilityOverride != 0)
        {
            Durability = DurabilityOverride;
        }  
    }
    public void ChangeObjectType(ObjectType newObjectType)
    {
        objectType = newObjectType;
    }
    public void OnInteract()
    {
        Debug.Log(this.name + " Has Been Interacted With");
        if (objectType == ObjectType.PowerSwitch)
        {
            SwapSwitchState();
        }

    }
    public void ChangePressure(float Ammount)
    {
        Pressure += Ammount;
    }

    
    // Power Switch Stuff
    public bool? GetSwitchState()
    {
        if (PowerSwitch == null)
        {
            return null;
        }
        return PowerSwitch.GetState();
    }
    public void ChangeSwitchState(bool newState)
    { 
        PowerSwitch.SetState(newState);
    }
    public void SwapSwitchState()
    {
        PowerSwitch.SetState(!PowerSwitch.GetState());
    }
    //End Power Switch Stuff

    //Monitor Stuff
    public string GetMonitorText()
    {
        return MonitorText.GetMonitorText();
    }
    public void ChangeMonitorText(string NewText) 
    {
        if (MonitorText == null)
        {
            return;
        }
        MonitorText.ChangeMonitorText(NewText);
    }
    public void ClearMonitorText()
    {
        MonitorText.InstantChangeMonitorText("");
    }
    public void Disconnect()
    {
        if (objectType == ObjectType.Monitor)
        {
            ClearMonitorText();
        }
    }
    //End Monitor Stuff

    //Inventory System
    public void Grab(Transform objectGrabPointTransform)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.layer = 6;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 6;
        }
        this.transform.position = objectGrabPointTransform.position;
        this.transform.SetParent(objectGrabPointTransform);
    }
    public void Place(Transform objectPlacePointTransform)
    {
        this.transform.SetParent(objectPlacePointTransform);
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.layer = 0;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 0;
        }
        transform.position = objectPlacePointTransform.position;
        transform.rotation = objectPlacePointTransform.rotation;
        this.gameObject.GetComponent<Collider>().enabled = true;
    }
    public void Drop()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.layer = 0;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 0;
        }
        this.transform.SetParent(null);
        this.gameObject.GetComponent<Collider>().enabled = true;
    }
    //End Inventory System

    //Scene View Only
    private void OnDrawGizmos()
    {
        objectReferences.GetConstructorItemReferences(objectType, false, out Mesh newMesh, out Material newMaterial);
        Gizmos.color = newMaterial.color;
        Gizmos.DrawMesh(newMesh, 0, transform.position, transform.rotation);
    }
}
