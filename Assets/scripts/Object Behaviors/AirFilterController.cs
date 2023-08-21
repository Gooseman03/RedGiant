using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class AirFilterController : ObjectDirector , IDirt, IInteract, IGrabbable
{
    private float _Dirt = 0;
    private float _MaxDirt = 100;
    public float Dirt
    {
        get { return _Dirt; }
        set { _Dirt = value; }
    }
    public float MaxDirt
    {
        get { return _MaxDirt; }
        set { _MaxDirt = value; }
    }
    public void ChangeDirt(float ammount)
    {
        Dirt += ammount;
        if (Dirt < 0)
        {
            Dirt = 0;
        }
    }
    public void SetDirt(float dirt)
    {
        Dirt = dirt;
    }
    public void SetMaxDirt(float dirt)
    {
        MaxDirt = dirt;
    }
    public float GetPercentDirt()
    {
        return Dirt / MaxDirt;
    }

    [SerializeField] GameObject Filter;

    public void Start()
    {
        objectReferences.GetConstructorItemReferences(ObjectType.AirFilter, false, out List<Mesh> ListOfMeshs, out Material material);
        Filter = new GameObject();

        Filter.transform.parent = transform;
        Filter.transform.localPosition = Vector3.zero;
        Filter.transform.localRotation = Quaternion.identity;
        MeshFilter meshFilter = Filter.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = Filter.AddComponent<MeshRenderer>();
        meshFilter.sharedMesh = ListOfMeshs[1];
        meshRenderer.material = material;
    }

    public void OnInteract(PlayerController playerController)
    {
        MenuRequester.AddMessageToConsole(this.name + " Has Been Interacted With");
        Dirt = 0;
    }
    public bool Grab(Transform objectGrabPointTransform)
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
        return true;
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
}
