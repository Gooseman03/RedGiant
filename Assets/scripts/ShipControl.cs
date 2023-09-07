using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputSystem;
public static class Ship
{
    public static GameObject ShipObject { get; private set; } 
    public static void Register(GameObject gameObject)
    {
        ShipObject = gameObject;
    }
}

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
    private void Start()
    {
        Ship.Register(this.gameObject);
    }
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

    void Update()
    {

        ShipMove(InputSystem.PlayerInputs.ShipMoveInput);
        ShipLook(InputSystem.PlayerInputs.ShipLookInput);



        GetThrustAmmounts();
        MoveSetup();

        Rotate();
        if ( DragEnabled && NewRotationVectors == Vector3.zero)
        {
            RotateSlowDown();
        }
        

        moveVector = Quaternion.Euler(Rotation) * moveVector;

        Move();
        if (DragEnabled && NewMoveVectors == Vector3.zero)
        {
            MoveSlowDown();
        }
        

        Space.BroadcastMessage("RotateAroundSpace", Rotation, SendMessageOptions.DontRequireReceiver);
    }
    private void GetThrustAmmounts()
    {
        AccelerationVectors = Vector3.zero;
        RotationAcceleration = Vector3.zero;
        foreach (Thruster thruster in thrusters)
        {
            AccelerationVectors += thruster.ThrustDirections * thruster.ThrustPersentAvailable;
            RotationAcceleration += thruster.ThrustRotationDirections * thruster.ThrustRotationPersentAvailable;
        }
    }
    private void MoveSetup()
    {
        ShipNewSpeed = new Vector3
        (
            NewMoveVectors.x * AccelerationVectors.x,
            NewMoveVectors.y * AccelerationVectors.y,
            NewMoveVectors.z * AccelerationVectors.z
        );
        moveVector += ShipNewSpeed * Time.deltaTime;
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
        ShipNewRotation = new Vector3
        (
            NewRotationVectors.x * RotationAcceleration.x,
            NewRotationVectors.y * RotationAcceleration.y,
            NewRotationVectors.z * RotationAcceleration.z
        );
        Rotation += ShipNewRotation * Time.deltaTime;
        if (Rotation.magnitude > RotationCap)
        {
            Rotation = Vector3.ClampMagnitude(Rotation, RotationCap);
        }
    }
    private void RotateSlowDown()
    {
        NewRotationVectors = -Rotation.normalized;
        //if (Mathf.Abs(Rotation.x) < 0.000001)
        //{
        //    NewRotationVectors.x = 0;
        //    Rotation.x = 0;
        //}
        //if (Mathf.Abs(Rotation.y) < 0.000001)
        //{
        //    NewRotationVectors.y = 0;
        //    Rotation.y = 0;
        //}
        //if (Mathf.Abs(Rotation.z) < 0.000001)
        //{
        //    NewRotationVectors.z = 0;
        //    Rotation.z = 0;
        //}
        ShipNewRotation = new Vector3
        (
            NewRotationVectors.x * RotationAcceleration.x,
            NewRotationVectors.y * RotationAcceleration.y,
            NewRotationVectors.z * RotationAcceleration.z
        );

        Rotation += ShipNewRotation * Time.deltaTime;
    }
    private void MoveSlowDown()
    {
        NewMoveVectors = -moveVector.normalized;
        //if (Mathf.Abs(moveVector.x) < 0.0001)
        //{
        //    NewMoveVectors.x = 0;
        //    moveVector.x = 0;
        //}
        //if (Mathf.Abs(moveVector.y) < 0.0001)
        //{
        //    NewMoveVectors.y = 0;
        //    moveVector.y = 0;
        //}
        //if (Mathf.Abs(moveVector.z) < 0.0001)
        //{
        //    NewMoveVectors.z = 0;
        //    moveVector.z = 0;
        //}
        ShipNewSpeed = new Vector3
        (
            NewMoveVectors.x * AccelerationVectors.x,
            NewMoveVectors.y * AccelerationVectors.y,
            NewMoveVectors.z * AccelerationVectors.z
        );
        moveVector += ShipNewSpeed * Time.deltaTime;
    }
    private Vector3 Clamp(Vector3 toClamp)
    {
        Vector3 ClampedValue = new Vector3 (toClamp.x, toClamp.y, toClamp.z);
        if (Mathf.Abs(toClamp.x) < .001f) { ClampedValue.x = 0; }
        if (Mathf.Abs(toClamp.y) < .001f) { ClampedValue.y = 0; }
        if (Mathf.Abs(toClamp.z) < .001f) { ClampedValue.z = 0; }
        return ClampedValue;
    }
}