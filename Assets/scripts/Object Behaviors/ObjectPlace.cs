using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class ObjectPlace : MonoBehaviour
{
    [SerializeField] private ObjectType _objectType;
    [SerializeField] private bool TempStorage;
    public ObjectType objectType
    {
        get { return _objectType; }
        private set { _objectType = value; }
    }
    private ObjectDirector _objectGrabbable;
    public ObjectDirector objectGrabbable 
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
        this.name = objectType.ToString() + " Place";
        if (Preplace == objectType && WillPreplace)
        {
            PrefabObjectGrabbable.SetActive(false);
            GameObject gameObject = Instantiate(PrefabObjectGrabbable);
            SetupComplete = true;
            PrefabObjectGrabbable.SetActive(true);
            gameObject.GetComponent<ObjectDirector>().ChangeObjectType(Preplace);
            gameObject.SetActive(true);
            gameObject.GetComponent<ObjectDirector>().Place(this.transform);
        }
        SetupComplete = true;
    }

    private void OnTransformChildrenChanged()
    {
        if (!SetupComplete) { return; }
        
        LastObjectGrabbable = objectGrabbable;
        objectGrabbable = gameObject.transform.GetComponentInChildren<ObjectDirector>();
        

        if (objectGrabbable != null)
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
        try
        {
            if (this.transform.parent.TryGetComponent<ItemRegister>(out parentObjectRegister) != true)
            {
                return;
            }
        }
        catch { return; }


        if (TempStorage) { return; }
        if (isRegistered && objectGrabbable == null)
        {
            parentObjectRegister.UnregisterObject(LastObjectGrabbable);
            LastObjectGrabbable.Disconnect();
            isRegistered = false;
            Debug.Log(
                "ObjectPlace.OnTransformChildrenChanged() Unregistered object: "
                    + LastObjectGrabbable.name
            );
            return;
        }
        if (!isRegistered && parentObjectRegister.CheckForObject(objectGrabbable) == false)
        {
            parentObjectRegister.RegisterObject(objectGrabbable);
            isRegistered = true;
            Debug.Log(
                "ObjectPlace.OnTransformChildrenChanged() Registered object: "
                    + objectGrabbable.name
            );
            return;
        }

        Debug.LogError(
            "Error: ObjectPlace.OnTransformChildrenChanged() isRegistered is neither true nor false"
        );
    }

    private void OnDrawGizmos()
    {
        objectReferences.GetConstructorItemReferences(objectType, true, out List<Mesh> newMeshs, out Material newMaterial);
        gameObject.name = objectType.ToString();
        Gizmos.color = newMaterial.color;
        Gizmos.DrawMesh(newMeshs[0], 0, transform.position,transform.rotation);
    }
}
