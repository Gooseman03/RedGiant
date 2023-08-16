using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public GameObject Space;
    [SerializeField] private CommandController CommandStation;
    [SerializeField] private bool DragEnabled;
    [SerializeField] private Vector3 moveVector = new();
    [SerializeField] private Vector3 Rotation = new();
    private Vector3 ShipNewSpeed = new();
    private Vector3 ShipNewRotation = new();
    [SerializeField] private Vector3 AccelerationVectors = new();
    [SerializeField] private Vector3 RotationAcceleration = new();
    private Vector3 NewMoveVectors = new();
    private Vector3 NewRotationVectors = new();
    [SerializeField] private float SpeedCap;
    [SerializeField] private float RotationCap;
    [SerializeField] private DamageShip DamageShip;
    [SerializeField] private List<Thruster> thrusters = new List<Thruster>();
    private void ShipLook (Vector2 vectors)
    {
        vectors *= Time.deltaTime;
        NewRotationVectors.x = vectors.y;
        NewRotationVectors.z = vectors.x;
        NewRotationVectors.Normalize();
    }
    private void ShipMove(Vector3 vectors)
    {
        vectors *= Time.deltaTime;
        
        NewMoveVectors.x = vectors.x;
        NewMoveVectors.y = vectors.y;
        NewMoveVectors.z = vectors.z;

        NewMoveVectors.Normalize();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<SpaceShift>(out SpaceShift spaceShift))
        {
            moveVector = -moveVector * 2;
            ShipCollision(moveVector);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<SpaceShift>(out SpaceShift spaceShift))
        {
            moveVector = -moveVector * 2;
        }
    }

    private void ShipCollision(Vector3 vector3)
    {
        int ItemsToDamage = ((int)vector3.magnitude) / 5;
        DamageShip.DamageRandomItem(ItemsToDamage);
    }
    void FixedUpdate()
    {
        if (NewMoveVectors != Vector3.zero)
        {
            MoveSetup();
        }
        if ( DragEnabled && NewMoveVectors == Vector3.zero)
        { 
            MoveSlow();
        }
        if (NewRotationVectors != Vector3.zero)
        {
            Rotate();
        }
        if (DragEnabled && NewRotationVectors == Vector3.zero)
        {
            RotateSlow();
        }
        if (Rotation != Vector3.zero)
        {
            moveVector = Quaternion.Euler(Rotation) * moveVector;
        }
        Move();

        Space.BroadcastMessage("RotateAroundSpace", Rotation, SendMessageOptions.DontRequireReceiver);
    }
    private void MoveSlow()
    {
        AccelerationVectors = Vector3.zero;
        foreach (Thruster thruster in thrusters)
        {
            AccelerationVectors += thruster.ThrustDirections * thruster.ThrustPersentAvailable;
        }
        NewMoveVectors = -moveVector.normalized;
        if (AccelerationVectors.x > Mathf.Abs(moveVector.x))
        {
            NewMoveVectors.x = 0;
            moveVector.x = 0;
        }
        if (AccelerationVectors.y > Mathf.Abs(moveVector.y))
        {
            NewMoveVectors.y = 0;
            moveVector.y = 0;
        }
        if (AccelerationVectors.z > Mathf.Abs(moveVector.z))
        {
            NewMoveVectors.z = 0;
            moveVector.z = 0;
        }
        ShipNewSpeed = new Vector3(NewMoveVectors.x * AccelerationVectors.x, NewMoveVectors.y * AccelerationVectors.y, NewMoveVectors.z * AccelerationVectors.z);
        moveVector += ShipNewSpeed;
    }
    private void MoveSetup()
    {
        AccelerationVectors = Vector3.zero;
        foreach (Thruster thruster in thrusters)
        {
            AccelerationVectors += thruster.ThrustDirections * thruster.ThrustPersentAvailable;
        }

        ShipNewSpeed = new Vector3(NewMoveVectors.x * AccelerationVectors.x, NewMoveVectors.y * AccelerationVectors.y, NewMoveVectors.z * AccelerationVectors.z);

        moveVector += ShipNewSpeed;

        if (moveVector.magnitude > SpeedCap)
        {
            moveVector = Vector3.ClampMagnitude(moveVector, SpeedCap);
        }
    }
    private void Move()
    {
        Space.BroadcastMessage("Shift", moveVector * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
    }
    private void Rotate()
    {
        RotationAcceleration = Vector3.zero;
        foreach (Thruster thruster in thrusters)
        {
            RotationAcceleration += thruster.ThrustRotationDirections * thruster.ThrustRotationPersentAvailable;
        }
        ShipNewRotation = new Vector3( NewRotationVectors.x * RotationAcceleration.x, NewRotationVectors.y * RotationAcceleration.y, NewRotationVectors.z * RotationAcceleration.z);

        Rotation += ShipNewRotation;

        if (Rotation.magnitude > RotationCap)
        {
            Rotation = Vector3.ClampMagnitude(Rotation, RotationCap);
        }
    }
    private void RotateSlow()
    {
        RotationAcceleration = Vector3.zero;
        foreach (Thruster thruster in thrusters)
        {
            RotationAcceleration += thruster.ThrustRotationDirections * thruster.ThrustRotationPersentAvailable;
        }
        NewRotationVectors = -Rotation.normalized;
        if (RotationAcceleration.x > Mathf.Abs(Rotation.x))
        {
            NewRotationVectors.x = 0;
            Rotation.x = 0;
        }
        if (RotationAcceleration.y > Mathf.Abs(Rotation.y))
        {
            NewRotationVectors.y = 0;
            Rotation.y = 0;
        }
        if (RotationAcceleration.z > Mathf.Abs(Rotation.z))
        {
            NewRotationVectors.z = 0;
            Rotation.z = 0;
        }
        ShipNewRotation = new Vector3(NewRotationVectors.x * RotationAcceleration.x, NewRotationVectors.y * RotationAcceleration.y, NewRotationVectors.z * RotationAcceleration.z);

        Rotation += ShipNewRotation;
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