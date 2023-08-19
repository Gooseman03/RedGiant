using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class AirFilterController : ObjectDirector , IDirt, IInteract
{
    private float _Dirt;
    private float _MaxDirt;
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
}
