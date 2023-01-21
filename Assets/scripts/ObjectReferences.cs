using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum ObjectType
{
    Generic,
    Fuse,
    AirCanister,
    PowerConnector,
    Monitor,
    PowerSwitch
}
public class ObjectReferences: MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private Mesh GenericMesh;
    [SerializeField] private Material TransparentMaterial;
    [SerializeField] private Material OpaqueMaterial;
    [SerializeField] private Mesh AirCanisterMesh;
    [SerializeField] private Mesh FuseMesh;
    [SerializeField] private Mesh PowerConnectorMesh;
    [SerializeField] private Mesh MonitorMesh;


    [Header("General Material Settings")]
    [Tooltip("The base color of the material when transparent")]
    [SerializeField] private Color BaseColor;


    [Header("Object Settings")]
    [SerializeField] private float FuseDurability;
    [SerializeField] private float AirCanisterDurability;
    [SerializeField] private float PowerConnectorDurability;
    [SerializeField] private float MonitorDurability;
    [SerializeField] private float PowerSwitchDurability;


    [SerializeField] private float AirCanisterPressure;

    [SerializeField] private float FuseMaxCurrent;
    [SerializeField] private float PowerConnectorMaxCurrent;

    public void GetConstructorItemReferences( ObjectType Requested , bool Transparent ,out Mesh meshOut, out Material materialOut)
    {
        meshOut = GenericMesh;
        materialOut = OpaqueMaterial;
        if (Transparent)
        {
            materialOut = TransparentMaterial;
            materialOut.color = BaseColor;
        }
        if (Requested == ObjectType.Fuse)
        {
            meshOut = FuseMesh;
        }
        if (Requested == ObjectType.AirCanister)
        {
            meshOut = AirCanisterMesh;
        }
        if (Requested == ObjectType.PowerConnector)
        {
            meshOut = PowerConnectorMesh;
        }
        if (Requested == ObjectType.Monitor)
        {
            meshOut = MonitorMesh;
        }
    }

    public void GetStatsItemReferences( ObjectType Requested , out Dictionary<string,float?> OutList)
    {
        OutList = new Dictionary<string, float?>();
        if (Requested == ObjectType.Fuse)
        {
            OutList.Add("Durability", FuseDurability);
            OutList.Add("Pressure", null);
            OutList.Add("MaxCurrent", FuseMaxCurrent);
        }
        if (Requested == ObjectType.AirCanister)
        {
            OutList.Add("Durability", AirCanisterDurability);
            OutList.Add("Pressure", AirCanisterPressure);
            OutList.Add("MaxCurrent", null);
        }
        if (Requested == ObjectType.PowerConnector)
        {
            OutList.Add("Durability", PowerConnectorDurability);
            OutList.Add("Pressure", null);
            OutList.Add("MaxCurrent", PowerConnectorMaxCurrent);
        }
        if (Requested == ObjectType.Monitor)
        {
            OutList.Add("Durability", MonitorDurability);
            OutList.Add("Pressure", null);
            OutList.Add("MaxCurrent", null);
        }
        if (Requested == ObjectType.PowerSwitch)
        {
            OutList.Add("Durability", PowerSwitchDurability);
            OutList.Add("Pressure", null);
            OutList.Add("MaxCurrent", null);
        }
    }
}
