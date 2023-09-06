using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ObjectPlace : MonoBehaviour
{
    

    [SerializeField] private ObjectSpawner objectSpawner;
    [SerializeField] private Material material;
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

    private bool isRegistered = false;
    private ObjectDirector LastObjectGrabbable;

    [SerializeField] private bool WillPreplace;
    private void Awake()
    {
        if (Number == 0)
        {
            this.name = objectType.ToString() + " Place";
        }
        else
        {
            this.name = Number.ToString() + " " + objectType.ToString() + " Place";
        }
        
        if (WillPreplace)
        {
            GameObject SpawnedObject = objectSpawner.Spawn(this.transform , objectType);
            SpawnedObject.GetComponent<IGrabbable>().Place(this.transform);
            OnTransformChildrenChanged();
        }
    }
    private void OnTransformChildrenChanged()
    {
        LastObjectGrabbable = ObjectGrabbable;
        ObjectGrabbable = gameObject.transform.GetComponentInChildren<ObjectDirector>();
        

        if (ObjectGrabbable != null)
        {
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            this.GetComponent<Collider>().enabled = true;
            this.GetComponent<Renderer>().enabled = true;
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
    public void Setup() 
    {
        if (objectSpawner == null)
        {
            Debug.LogWarning("Define ObjectSpawner");
            return;
        }
        Mesh mesh1 = objectSpawner.GetColliderMesh(objectType);
        MeshCollider meshCollider = this.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh1;
        meshCollider.convex = true;
        this.GetComponent<MeshFilter>().sharedMesh = mesh1;
        this.GetComponent<MeshRenderer>().sharedMaterial = material;
        this.gameObject.name = objectType.ToString() + " Place";

    }
    void OnDrawGizmos()
    {
        if (objectSpawner == null) { return; }
        Gizmos.color = new Color(0,0,0.75f,0.1f);
        foreach (Mesh mesh in objectSpawner.GetMeshes(objectType))
        {
            Gizmos.DrawMesh(mesh,transform.position,transform.rotation);
        }
    }
}
