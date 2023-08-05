using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class AirFilterController : MonoBehaviour
{
    [SerializeField] GameObject Filter;
    [SerializeField] private ObjectReferences objectReferences;
    public void StartUp(ObjectReferences newReferences)
    {
        objectReferences = newReferences;
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
}
