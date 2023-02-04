using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public GameObject Space;
    private int Threshold = 100;
    [SerializeField] private CommandController CommandStation;
    [SerializeField] private bool DragEnabled;
    [SerializeField] private float MoveDrag;
    [SerializeField] private float RotationDrag;
    [SerializeField] private Vector3 moveVector = new();
    [SerializeField] private Vector3 Rotation = new();
    private Vector3 ShipNewSpeed = new();
    private Vector3 ShipNewRotation = new();
    [SerializeField] private float Acceleration;
    [SerializeField] private float RotationAcceleration;

    [SerializeField] private float SpeedCap;
    [SerializeField] private float RotationCap;
    private void ShipLook (Vector2 vectors)
    {
        vectors *= Time.deltaTime;

        Vector3 NewVectors = new();

        vectors.Normalize();

        NewVectors.x = vectors.y * RotationAcceleration;
        NewVectors.z = vectors.x * RotationAcceleration;

        ShipNewRotation = NewVectors;
    }
    private void ShipMove(Vector3 vectors)
    {
        vectors *= Time.deltaTime;
        Vector3 NewVectors = new();
        NewVectors.x = vectors.x;
        NewVectors.y = vectors.y;
        NewVectors.z = vectors.z;

        ShipNewSpeed = NewVectors;
        ShipNewSpeed.Normalize();
        ShipNewSpeed *= Acceleration;
    }
    void FixedUpdate()
    {
        if (CommandStation != null && CommandStation.baseSystem.SystemPower && CommandStation.baseSystem.PowerSwitchState)
        {
            moveVector = Vector3.Lerp(Vector3.zero, moveVector, MoveDrag);
            Rotation = Vector3.Lerp(Vector3.zero, Rotation, RotationDrag);
        }
        moveVector = Clamp(moveVector);
        Rotation = Clamp(Rotation);
        moveVector += ShipNewSpeed;
        Rotation += ShipNewRotation;

        if (Rotation != Vector3.zero)
        {
            moveVector = Quaternion.Euler(Rotation) * moveVector;
        }
        if (Rotation.magnitude > RotationCap)
        {
            Rotation = Vector2.ClampMagnitude(Rotation, RotationCap);
        }
        if (moveVector.magnitude > SpeedCap)
        {
            moveVector = Vector3.ClampMagnitude(moveVector, SpeedCap);
        }
        
        transform.position += moveVector * Time.deltaTime;
        Space.BroadcastMessage("RotateAroundSpace", Rotation, SendMessageOptions.DontRequireReceiver);
        if (Mathf.Abs(transform.position.x) > Threshold || Mathf.Abs(transform.position.y) > Threshold || Mathf.Abs(transform.position.z) > Threshold)
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
