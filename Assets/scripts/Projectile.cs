using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 Travel;
    private void Shift(Vector3 ShiftDistance)
    {
        this.transform.position -= ShiftDistance;
    }
    private void RotateAroundSpace(Vector3 vectors)
    {
        this.transform.RotateAround(Ship.ShipObject.transform.position, new Vector3(1, 0, 0), vectors.x);
        this.transform.RotateAround(Ship.ShipObject.transform.position, new Vector3(0, 1, 0), vectors.y);
        this.transform.RotateAround(Ship.ShipObject.transform.position, new Vector3(0, 0, 1), vectors.z);
    }
}

public class Laser : Projectile
{
    private Ray ray;
    private void Start()
    {
        ray = new Ray(transform.position,Travel);
        CheckForColliders();
        Destroy(this.gameObject);
    }
    void CheckForColliders()
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            foreach(Collider objectHit in Physics.OverlapSphere(hit.point, 10))
            {
                if (objectHit.gameObject.TryGetComponent<ObjectDirector>(out ObjectDirector objectDirector) && objectDirector is IDurable durableObject)
                {
                    durableObject.ChangeDurability(-100);
                }
            }
        }
    }
}