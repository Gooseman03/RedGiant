using System;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.Examples.TMP_ExampleScript_01;

[RequireComponent(typeof(Rigidbody))]
public class ObjectDirector : MonoBehaviour
{
    public ObjectReferences objectReferences;
    [SerializeField] private float _StatScale = 1.0f;
    public float StatScale 
    { 
        get { return _StatScale; } 
        set { _StatScale = value; }
    }

    private Rigidbody objectRigidbody;

    protected List<Mesh> MeshList;
    protected Material material;
    protected List<GameObject> ConstructedGameObjects;
    protected Dictionary<string, float?> StatsDictionary;
    public ObjectType objectType;

    
    private void Awake()
    {
        objectReferences.GetConstructorItemReferences(
            objectType,
            false,
            out MeshList,
            out material
            );
        objectReferences.GetStatsItemReferences(objectType, out StatsDictionary);
        GetComponent<MeshCollider>().sharedMesh = MeshList[0];
        ConstructMeshs();
        AddStats();
        this.name = objectType.ToString();
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
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
            meshFilter.sharedMesh = mesh;
            meshRenderer.sharedMaterial = material;
            ConstructedGameObjects.Add(gameObject);
        }
    }
    public void AddStats()
    {
        if (StatsDictionary.ContainsKey("Durability") && this is IDurable durableObject)
        {
            durableObject.SetMaxDurability((float)StatsDictionary["Durability"] * StatScale);
            durableObject.SetDurability((float)StatsDictionary["Durability"] * StatScale);
        }
        if (StatsDictionary.ContainsKey("Pressure") && this is ICapacity capacityObject)
        {
            capacityObject.SetMaxPressure((float)StatsDictionary["Pressure"] * StatScale);
            capacityObject.SetPressure((float)StatsDictionary["Pressure"] * StatScale);
        }
        if (StatsDictionary.ContainsKey("Dirt") && this is IDirt dirtObject)
        {
            dirtObject.SetMaxDirt(100 * StatScale);
            dirtObject.SetDirt(0);
        }
    }
    public void ShockPlayer()
    {
        PlayerMessaging.ShockPlayer();
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
    private void OnDrawGizmos()
    {
        objectReferences.GetConstructorItemReferences(objectType, false, out List<Mesh> newMesh, out Material newMaterial);
        gameObject.name = objectType.ToString();
        Gizmos.color = newMaterial.color;
        Gizmos.DrawMesh(newMesh[0], 0, transform.position, transform.rotation);
    }
}
