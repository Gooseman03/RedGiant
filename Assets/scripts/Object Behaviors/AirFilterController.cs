using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class AirFilterController : MonoBehaviour
{
    [SerializeField] GameObject Filter;
    [SerializeField] private ObjectReferences objectReferences;
    public void StartUp(ObjectReferences newReferences)
    {
        objectReferences = newReferences;
        objectReferences.GetConstructorItemReferences(ObjectType.AirFilter, out List<Mesh> ListOfMeshs, out Material material, out List<Color> ListOfColors);
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