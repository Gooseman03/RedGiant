using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectDirector : MonoBehaviour
{
    private ObjectBuilder objectBuilder;
    [SerializeField] private ObjectReferences objectReferences;
    [SerializeField] private ObjectType _objectType;
    [SerializeField] private float _StatScale = 1.0f;
    public float StatScale 
    { 
        get { return _StatScale; } 
        set { _StatScale = value; }
    }
    public ObjectType objectType 
    { 
        get { return _objectType; } 
        set { _objectType = value; }
    }
    private Rigidbody objectRigidbody;
    [SerializeField] private float DurabilityOverride;
    [SerializeField] private float PressureOverride;
    [SerializeField] private float DirtOverride;
    private float? _Durability;
    private float? _Pressure;
    private float? _Dirt;
    private float? _MaxDurability;
    private float? _MaxPressure;
    private float? _MaxDirt;
    public float? MaxDurability
    {
        get { return _MaxDurability; }
        private set { _MaxDurability = value; }
    }
    public float? MaxPressure
    {
        get { return _MaxPressure; }
        private set { _MaxPressure = value; }
    }
    public float? MaxDirt
    {
        get { return _MaxDirt; }
        private set { _MaxDirt = value; }
    }
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
    public float? Dirt
    {
        get { return _Dirt; }
        private set { _Dirt = value; }
    }

    private AudioHandler audioHandler;

    private void Start()
    {
        objectBuilder = this.gameObject.AddComponent<ObjectBuilder>();
        objectBuilder.objectReferences = objectReferences;
        objectBuilder.objectType = objectType;
        objectBuilder.enabled = true;
        objectBuilder.RequestBuildMeshs = true;
    }
    private void Update()
    {
        //if (DurabilityOverride != 0)
        //{
        //    Durability = DurabilityOverride;
        //}  
        //if (PressureOverride != 0)
        //{
        //    Pressure = PressureOverride;
        //}
        //if (DirtOverride != 0)
        //{
        //    Dirt = DirtOverride;
        //}
        if (Durability != null)
        {
            DurabilityOverride = (float)Durability;
        }
        if (Pressure != null)
        {
            PressureOverride = (float)Pressure;
        }
        if (Dirt != null)
        {
            DirtOverride = (float)Dirt;
        }
        
        

    }
    public void ShockPlayer()
    {
        SendMessageUpwards("ShockPlayer");
    }
    public void ChangeObjectType(ObjectType newObjectType)
    {
        objectType = newObjectType;
    }
    public void OnInteract(PlayerController playerController)
    {
        Debug.Log(this.name + " Has Been Interacted With");
        if (objectType == ObjectType.PowerSwitch)
        {
            SwapSwitchState();
        }
        if (objectType == ObjectType.AirFilter)
        {
            Dirt = 0;
        }
        if (objectType == ObjectType.Keyboard)
        {
            SendMessageUpwards("SystemInteract", playerController);
        }
        if (objectType == ObjectType.Alarm)
        {
            CancelAlarm();
        }
    }
    public void ChangePressure(float Ammount)
    {
        Pressure += Ammount;
    }
    public void ChangeDurability(float Ammount)
    {
        Durability += Ammount;
    }
    public void ChangeDirt(float Ammount)
    {
        Dirt += Ammount;
    }

    public void SetDurability(float? Amount)
    {
        Durability = Amount;
    }
    public void SetPressure(float? Amount)
    {
        Pressure = Amount;
    }
    public void SetDirt(float? Amount)
    {
        Dirt = Amount;
    }

    public void SetMaxDurability(float? Amount)
    {
        MaxDurability = Amount;
    }
    public void SetMaxPressure(float? Amount)
    {
        MaxPressure = Amount;
    }
    public void SetMaxDirt(float? Amount)
    {
        MaxDirt = Amount;
    }

    public float GetPersentDurability()
    {
        float Persentage = 0;
        if (Durability != null)
        {
            Persentage = (float)MaxDurability / (float)Durability;
        }
        return Persentage;
    }
    public float GetPersentPressure()
    {
        float Persentage = 0;
        if (Pressure != null)
        {
            Persentage = (float)Pressure / (float)MaxPressure;
        }
        return Persentage;
    }
    public float GetPersentDirt()
    {
        float Persentage = 0;
        if (Dirt != null)
        {
            Persentage =  (float)Dirt / (float)MaxDirt;
        }
        return Persentage;
    }

    ///Power Switch Stuff
    public bool? GetSwitchState()
    {
        if (this.GetComponent<PowerSwitchController>() == null)
        {
            return null;
        }
        return this.GetComponent<PowerSwitchController>().GetState();
    }
    public void ChangeSwitchState(bool newState)
    {
        this.GetComponent<PowerSwitchController>().SetState(newState);
    }
    public void SwapSwitchState()
    {
        this.GetComponent<PowerSwitchController>().SetState(!this.GetComponent<PowerSwitchController>().GetState());
    }
    //Monitor Stuff
    public string GetMonitorText()
    {
        return this.GetComponent<MonitorController>().GetMonitorText();
    }
    public void ChangeMonitorText(string NewText) 
    {
        if (this.GetComponent<MonitorController>() == null)
        {
            return;
        }
        this.GetComponent<MonitorController>().ChangeMonitorText(NewText);
    }
    public void ClearMonitorText()
    {
        if (GetComponent<MonitorController>() == null) { return; }
        this.GetComponent<MonitorController>().InstantChangeMonitorText("");
    }
    public void MonitorPlaneEnable()
    {
        if (GetComponent<MonitorController>() == null) { return; }
        this.GetComponent<MonitorController>().MonitorPlaneEnable();
    }
    public void MonitorPlaneDisable()
    {
        if (GetComponent<MonitorController>() == null) { return; }
        this.GetComponent<MonitorController>().MonitorPlaneDisable();
    }
    public void SetMonitorPlaneMaterial(Material material)
    {
        this.GetComponent<MonitorController>().SetMonitorPlaneMaterial(material);
    }
    public void Disconnect()
    {
        if (objectType == ObjectType.Monitor)
        {
            ClearMonitorText();
        }
        if (objectType == ObjectType.Pump)
        {
            stopAudio();
        }
    }
    //Audio
    public void AlarmUpdate()
    {
        if (GetComponent<AlarmController>() == null) { return; }
        this.GetComponent<AlarmController>().AlarmUpdate();
    }
    public void CancelAlarm()
    {
        if (GetComponent<AlarmController>() == null) { return; }
        this.GetComponent<AlarmController>().CancelAlarm();
    }
    public void UpdateAlarmErrors(List<ErrorTypes> ErrorsInput)
    {
        if (GetComponent<AlarmController>() == null) { return; }
        this.GetComponent<AlarmController>().UpdateErrorTypes(ErrorsInput);
    }
    public void playAudio()
    {
        if (audioHandler == null)
        {
            return;
        }
        audioHandler.ChangeAudioPlaying(true);
    }
    public void stopAudio()
    {
        if (audioHandler == null)
        {
            return;
        }
        audioHandler.ChangeAudioPlaying(false);
    }
    public bool? IsAudioPlaying()
    {
        if (audioHandler == null)
        {
            return null;
        }
        return audioHandler.IsAudioPlaying;
    }
    //Inventory System
    public void Grab(Transform objectGrabPointTransform)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.layer = 6;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 6;
        }
        transform.position = objectGrabPointTransform.position;
        transform.SetParent(objectGrabPointTransform);
    }
    public void Place(Transform objectPlacePointTransform)
    {
        transform.SetParent(objectPlacePointTransform);
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.layer = 0;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 0;
        }
        transform.position = objectPlacePointTransform.position;
        transform.rotation = objectPlacePointTransform.rotation;
        gameObject.GetComponent<Collider>().enabled = true;
    }
    public void Drop(Transform NewParent)
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.layer = 0;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 0;
        }
        this.transform.SetParent(NewParent);
        this.gameObject.GetComponent<Collider>().enabled = true;
    }

    //Scene View Only
    private void OnDrawGizmos()
    {
        objectReferences.GetConstructorItemReferences(objectType, false, out List<Mesh> newMesh, out Material newMaterial);
        gameObject.name = objectType.ToString();
        Gizmos.color = newMaterial.color;
        Gizmos.DrawMesh(newMesh[0], 0, transform.position, transform.rotation);
    }
}
