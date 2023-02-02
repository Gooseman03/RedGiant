using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public GameObject Space;
    private int Threshold = 100;
    [SerializeField] private bool DragEnabled;
    [SerializeField] private float MoveDrag;
    [SerializeField] private float RotationDrag;
    [SerializeField] private Vector3 moveVector = new();
    [SerializeField] private Vector3 Rotation = new();
    private Vector3 ShipNewSpeed = new();
    private Vector3 ShipNewRotation = new();
    private void ShipLook (Vector2 vectors)
    {
        vectors *= Time.deltaTime;
        Vector3 NewVectors = new();
        NewVectors.x = vectors.y * 2;
        NewVectors.z = vectors.x * 4;

        ShipNewRotation = NewVectors;
    }
    private void ShipMove(Vector2 vectors)
    {
        vectors *= Time.deltaTime;
        Vector3 NewVectors = new();
        NewVectors.x = vectors.x;
        NewVectors.z = vectors.y;
        ShipNewSpeed = NewVectors * 100;
    }
    void FixedUpdate()
    {
        moveVector = Vector3.Lerp( Vector3.zero , moveVector, MoveDrag);
        Rotation = Vector3.Lerp( Vector3.zero, Rotation, RotationDrag);
        moveVector = Clamp(moveVector);
        Rotation = Clamp(Rotation);
        moveVector += ShipNewSpeed;
        Rotation += ShipNewRotation;
        transform.position += moveVector * Time.deltaTime;
        Space.BroadcastMessage("RotateAroundSpace", Rotation, SendMessageOptions.DontRequireReceiver);
        if (transform.position.x > Threshold || transform.position.y > Threshold || transform.position.z > Threshold)
        {
            Space.BroadcastMessage("Shift", transform.position);
            transform.position = Vector3.zero;
        }
    }
    private Vector3 Clamp(Vector3 toClamp)
    {
        Vector3 ClampedValue = new Vector3 (toClamp.x, toClamp.y, toClamp.z);
        if (Mathf.Abs(toClamp.x) < .01f) { ClampedValue.x = 0; }
        if (Mathf.Abs(toClamp.y) < .01f) { ClampedValue.y = 0; }
        if (Mathf.Abs(toClamp.z) < .01f) { ClampedValue.z = 0; }
        return ClampedValue;
    }
}
