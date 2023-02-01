using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour
{
    private List<Mesh> MeshList;
    private Material material;
    public ObjectReferences objectReferences;
    public ObjectType objectType;
    public ObjectDirector objectDirector;

    public bool RequestBuildMeshs = false;

    private List<GameObject> ConstructedGameObjects;
    private Dictionary<string, float?> StatsDictionary;


    private void Start()
    {
        objectDirector = this.GetComponent<ObjectDirector>();
        objectReferences.GetConstructorItemReferences(
            objectType,
            false,
            out MeshList,
            out material
            );
        objectReferences.GetStatsItemReferences(objectType, out StatsDictionary);
        GetComponent<MeshCollider>().sharedMesh = MeshList[0];
        if (RequestBuildMeshs)
        {
            ConstructMeshs();
        }
        AddBehaviors();
        AddStats();
        Destroy(this);
    }

    public void ConstructMeshs()
    {
        ConstructedGameObjects = new List<GameObject>();
        foreach (Mesh mesh in MeshList)
        {
            GameObject gameObject = new GameObject(mesh.name);
            gameObject.transform.parent = this.transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshFilter.sharedMesh = mesh;
            meshRenderer.sharedMaterial = material;
            ConstructedGameObjects.Add(gameObject);
        }
    }

    public void AddBehaviors()
    {
        
        if (objectType == ObjectType.Monitor)
        {
            MonitorController monitor = this.AddComponent<MonitorController>();
            monitor.StartUp(objectReferences.GetConstructorPlane());
        }
        if (objectType == ObjectType.Fuse)
        {
            FuseController fuse = this.AddComponent<FuseController>();
            fuse.Setup(objectDirector);
        }
        if (objectType == ObjectType.PowerConnector)
        {
            // Add Script Here
        }
        if (objectType == ObjectType.PowerSwitch)
        {
            PowerSwitchController powerSwitch = this.AddComponent<PowerSwitchController>();
            powerSwitch.StartUp(objectReferences, ConstructedGameObjects[1], ConstructedGameObjects[2]);
        }
        if (objectType == ObjectType.AirCanister)
        {
            AirCanisterController airCanister = this.AddComponent<AirCanisterController>();
            // No StartUp function
        }
        if (objectType == ObjectType.Pump)
        {
            PumpController pump = this.AddComponent<PumpController>();
            pump.StartUp(objectReferences);
        }
        if (objectType == ObjectType.Co2Canister)
        {
            AirCanisterController airCanister = this.AddComponent<AirCanisterController>();
            // No StartUp function
        }
        if (objectType == ObjectType.AirFilter)
        {
            AirFilterController airFilter = this.AddComponent<AirFilterController>();
            airFilter.StartUp(objectReferences);
        }
    }

    public void AddStats()
    {
        if (StatsDictionary.ContainsKey("Durability"))
        {
            objectDirector.SetDurability(StatsDictionary["Durability"]);
        }
        if (StatsDictionary.ContainsKey("Pressure"))
        {
            objectDirector.SetPressure(StatsDictionary["Pressure"]);
        }
        if (StatsDictionary.ContainsKey("Dirt"))
        {
            objectDirector.SetDirt(StatsDictionary["Dirt"]);
        }
    }
}
