using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    [SerializeField] private ObjectType objectType;
    private Rigidbody objectRigidbody;

    [SerializeField] private ObjectReferences objectReferences;

    [SerializeField]private float? Durability;
    private float? Pressure;
    private float? MaxCurrent;

    private MonitorController MonitorText;
    private PowerSwitchController PowerSwitch;


    private void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        objectReferences.GetConstructorItemReferences(objectType, false, out Mesh newMesh, out Material newMaterial);
        objectReferences.GetStatsItemReferences(objectType, out Dictionary<string, float?> ObjectStats);

        Durability = ObjectStats["Durability"];
        Pressure = ObjectStats["Pressure"];
        MaxCurrent = ObjectStats["MaxCurrent"];

        meshFilter.mesh = newMesh;
        this.GetComponent<MeshCollider>().sharedMesh = newMesh;
        meshRenderer.material = newMaterial;


        if (objectType == ObjectType.Monitor)
        {
            MonitorText =  this.AddComponent<MonitorController>();
        }
        if (objectType == ObjectType.PowerSwitch)
        {
            PowerSwitch = this.AddComponent<PowerSwitchController>();
        }    
        
        this.name = objectType.ToString() + " Object";
    }

    public void OnInteract()
    {
        Debug.Log(this.name + " Has Been Interacted With");
        if (objectType == ObjectType.PowerSwitch)
        {
            SwapSwitchState();
        }

    }
    public ObjectType GetObjectType()
    {
        return objectType;
    }
    public void ChangeDurability(float Ammount)
    {
        Durability += Ammount;
    }
    public float? GetDurability() {
        return Durability;
    }
    public float? GetPressure() {
        return Pressure;
    }
    public float? GetMaxCurrent() {
        return MaxCurrent; 
    }
    public string GetMonitorText()
    {
        return MonitorText.GetMonitorText();
    }
    public bool? GetSwitchState()
    {
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

    public void ChangeMonitorText(string NewText) 
    {
        MonitorText.ChangeMonitorText(NewText);
    }
    public void ClearMonitorText()
    {
        MonitorText.ChangeMonitorText("");
    }


    public void Disconnect()
    {
        if (objectType == ObjectType.Monitor)
        {
            ClearMonitorText();
        }
    }

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.layer = 6;
        this.transform.position = objectGrabPointTransform.position;
        this.transform.SetParent(objectGrabPointTransform);
    }

    public void Place(Transform objectPlacePointTransform)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Collider>().enabled = true;
        this.gameObject.layer = 0;
        this.transform.SetParent(objectPlacePointTransform);
        transform.position = objectPlacePointTransform.position;
        transform.rotation = objectPlacePointTransform.rotation;
    }

    public void Drop()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.GetComponent<Collider>().enabled = true;
        this.gameObject.layer = 0;
        this.transform.SetParent(null);
    }

    private void OnDrawGizmos()
    {
        objectReferences.GetConstructorItemReferences(objectType, false, out Mesh newMesh, out Material newMaterial);
        Gizmos.color = newMaterial.color;
        Gizmos.DrawMesh(newMesh, 0, transform.position, transform.rotation);
    }

}
