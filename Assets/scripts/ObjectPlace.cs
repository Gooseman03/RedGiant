using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class ObjectPlace : MonoBehaviour
{
    [SerializeField]
    private ObjectType objectType;
    private ObjectGrabbable objectGrabbable;
    [SerializeField] private ObjectReferences objectReferences;
    [SerializeField] private ObjectType Preplace;
    [SerializeField] private bool WillPreplace;
    [SerializeField] private GameObject PrefabObjectGrabbable;

    private GameObject Appearance;

    [SerializeField]
    private bool isRegistered = false;

    [SerializeField]
    private ObjectGrabbable LastObjectGrabbable;

    private void Start()
    {
        Appearance = new GameObject();
        Appearance.transform.parent = this.transform;
        Appearance.transform.localPosition = Vector3.zero;
        Appearance.transform.localRotation = Quaternion.identity;
        Appearance.name = "Look";
        MeshFilter meshFilter = Appearance.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = Appearance.AddComponent<MeshRenderer>();
        objectReferences.GetConstructorItemReferences(objectType, true, out Mesh newMesh, out Material newMaterial);
        meshFilter.mesh = newMesh;
        this.GetComponent<MeshCollider>().sharedMesh = newMesh;
        meshRenderer.material = newMaterial;
        this.name = objectType.ToString() + " Place";
        if (Preplace == objectType && WillPreplace)
        {
            PrefabObjectGrabbable.SetActive(false);
            GameObject gameObject = Instantiate(PrefabObjectGrabbable);
            PrefabObjectGrabbable.SetActive(true);
            gameObject.GetComponent<ObjectGrabbable>().ChangeObjectType(Preplace);
            gameObject.SetActive(true);
            gameObject.GetComponent<ObjectGrabbable>().Place(this.transform);
        }
    }

    private void OnTransformChildrenChanged()
    {
        LastObjectGrabbable = objectGrabbable;
        objectGrabbable = gameObject.transform.GetComponentInChildren<ObjectGrabbable>();
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

    public ObjectType GetObjectType()
    {
        return objectType;
    }

    public ObjectGrabbable GetObjectGrabbable()
    {
        return objectGrabbable;
    }

    private void OnDrawGizmos()
    {
        objectReferences.GetConstructorItemReferences(objectType, true, out Mesh newMesh, out Material newMaterial);
        Gizmos.color = newMaterial.color;
        Gizmos.DrawMesh(newMesh, 0, transform.position,transform.rotation);
    }
}
