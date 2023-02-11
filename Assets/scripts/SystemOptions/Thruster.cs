using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    [SerializeField] private Vector3 _ThrustDirections;
    [SerializeField] private float _ThrustPersentAvailable;
    [SerializeField] private Vector3 _ThrustRotationDirections;
    [SerializeField] private float _ThrustRotationPersentAvailable;

    public Vector3 ThrustDirections
    {
        get { return _ThrustDirections; }
        private set { _ThrustDirections = value; }
    }
    public float ThrustPersentAvailable
    {
        get { return _ThrustPersentAvailable; }
        set { _ThrustPersentAvailable = value; }
    }
    public Vector3 ThrustRotationDirections
    {
        get { return _ThrustRotationDirections; }
        private set { _ThrustRotationDirections = value; }
    }
    public float ThrustRotationPersentAvailable
    {
        get { return _ThrustRotationPersentAvailable; }
        set { _ThrustRotationPersentAvailable = value; }
    }

    private void FixedUpdate()
    {
        WhenPowered();
    }

    private void WhenPowered()
    {
        if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
        {
            ThrustPersentAvailable = 1;
            ThrustRotationPersentAvailable = 1;
        }
        else
        {
            ThrustPersentAvailable = 0;
            ThrustRotationPersentAvailable = 0;
        }
    }



}
