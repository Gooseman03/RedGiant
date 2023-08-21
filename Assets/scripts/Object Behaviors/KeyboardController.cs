using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : ObjectDirector , IInteract,IGrabbable
{
    public void OnInteract(PlayerController playerController)
    {
        SendMessageUpwards("SystemInteract", playerController);
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
