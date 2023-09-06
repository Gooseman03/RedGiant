using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "ScriptableObjects/ObjectScriptableObject", order = 1)]
public class ObjectScriptableObject : ScriptableObject
{
    public ObjectType ObjectType;
    public GameObject Prefab;

    public List<AudioClip> AudioClipList;

    [Header("Stats")]
    public float Durability;
    public float Dirt;
    public float Pressure;
    [Header("MaxStats")]
    public float MaxDurability;
    public float MaxDirt;
    public float MaxPressure;
    [Header("Meshs")]
    public Mesh ColisionMesh;
    public List<Mesh> GizmoMeshs;
}
