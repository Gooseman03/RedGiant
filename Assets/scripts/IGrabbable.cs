using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IGrabbable
{
    public bool Grab(Transform objectGrabPointTransform)
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        //gameObject.GetComponent<Collider>().enabled = false;
        //gameObject.layer = 6;
        //foreach (Transform child in this.GetComponentsInChildren<Transform>())
        //{
        //    child.gameObject.layer = 6;
        //}
        //transform.position = objectGrabPointTransform.position;
        //transform.SetParent(objectGrabPointTransform);
        return false;
    }
    public void Place(Transform objectPlacePointTransform)
    {
        //transform.SetParent(objectPlacePointTransform);
        //GetComponent<Rigidbody>().isKinematic = true;
        //gameObject.layer = 0;
        //foreach (Transform child in this.GetComponentsInChildren<Transform>())
        //{
        //    child.gameObject.layer = 0;
        //}
        //transform.position = objectPlacePointTransform.position;
        //transform.rotation = objectPlacePointTransform.rotation;
        //gameObject.GetComponent<Collider>().enabled = true;
    }
    public void Drop(Transform NewParent)
    {
        //this.GetComponent<Rigidbody>().isKinematic = false;
        //this.gameObject.layer = 0;
        //foreach (Transform child in this.GetComponentsInChildren<Transform>())
        //{
        //    child.gameObject.layer = 0;
        //}
        //this.transform.SetParent(NewParent);
        //this.gameObject.GetComponent<Collider>().enabled = true;
    }
}