using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunScript : MonoBehaviour
{
    [SerializeField] private GameObject Ship;
    private void RotateAroundSpace(Vector3 vectors)
    {

        this.transform.RotateAround(Ship.transform.position, new Vector3(1, 0, 0), vectors.x);
        this.transform.RotateAround(Ship.transform.position, new Vector3(0, 1, 0), vectors.y);
        this.transform.RotateAround(Ship.transform.position, new Vector3(0, 0, 1), vectors.z);
    }
}
