using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseController : MonoBehaviour
{
    [SerializeField] ObjectGrabbable objectGrabbable;
    public void Setup(ObjectGrabbable _objectGrabbable)
    {
        objectGrabbable = _objectGrabbable;
    }
    private void Update()
    {
        if (objectGrabbable.Durability <= 0)
        {
            this.GetComponent<MeshRenderer>().material.color = Color.black;
        }
    }
}
