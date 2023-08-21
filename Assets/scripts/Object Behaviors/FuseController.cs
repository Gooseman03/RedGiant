using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.Examples.TMP_ExampleScript_01;

public class FuseController : ObjectDirector , IDurable, IGrabbable
{
    private float _Durability;
    private float _MaxDurability;
    public float Durability
    {
        get { return _Durability; }
        set { _Durability = value; }
    }
    public float MaxDurability
    {
        get { return _MaxDurability; }
        set { _MaxDurability = value; }
    }
    public void ChangeDurability(float ammount)
    {
        Durability += ammount;
        if (Durability <= 0)
        {
            this.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            Durability = 0;
        }
    }
    public void SetDurability(float durability)
    {
        Durability = durability;
        if (Durability <= 0)
        {
            this.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            Durability = 0;
        }
    }
    public void SetMaxDurability(float durability)
    {
        MaxDurability = durability;
    }
    public float GetPercentDurability()
    {
        return Durability / MaxDurability;
    }

    public void ShockPlayer()
    {
        PlayerMessaging.ShockPlayer();
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
