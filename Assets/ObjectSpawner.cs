using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.Examples.TMP_ExampleScript_01;

public class ObjectSpawner : MonoBehaviour
{
    public List<ObjectScriptableObject> objects;
    public GameObject Spawn(Transform SpawnPosition, ObjectType objectType)
    {
        GameObject SpawnedObject = null;
        
        foreach (ObjectScriptableObject obj in objects)
        {
            if (obj != null)
            {
                if (objectType == obj.ObjectType)
                {
                    SpawnedObject = Instantiate(obj.Prefab, SpawnPosition);
                    if(SpawnedObject.GetComponent<ObjectDirector>() is IAudio audio) 
                    {
                        audio.audioClips = obj.AudioClipList;
                    }
                    AddStats();
                }
            }
            void AddStats()
            {
                if (SpawnedObject.TryGetComponent(out IDurable durable))
                {
                    durable.Durability = obj.Durability;
                    durable.MaxDurability = obj.MaxDurability;
                }
                if (SpawnedObject.TryGetComponent(out ICapacity capacity))
                {
                    capacity.Pressure = obj.Pressure;
                    capacity.MaxPressure = obj.MaxPressure;
                }
                if (SpawnedObject.TryGetComponent(out IDirt dirt))
                {
                    dirt.Dirt = obj.Dirt;
                    dirt.MaxDirt = obj.MaxDirt;
                }
            }
        }
        return SpawnedObject;
    }
    public List<Mesh> GetMeshes(ObjectType objectType)
    {
        foreach (ObjectScriptableObject obj in objects)
        {
            if (obj != null && objectType == obj.ObjectType)
            {
                return obj.GizmoMeshs;
            }
        }
        return new List<Mesh>();
    }
    public Mesh GetColliderMesh(ObjectType objectType)
    {
        foreach (ObjectScriptableObject obj in objects)
        {
            if (obj != null && objectType == obj.ObjectType)
            {
                return obj.ColisionMesh;
            }
        }
        return null;
    }
}
