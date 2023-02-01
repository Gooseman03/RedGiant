using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseController : MonoBehaviour
{
    [SerializeField] ObjectDirector objectGrabbable;
    public void Setup(ObjectDirector _objectGrabbable)
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
