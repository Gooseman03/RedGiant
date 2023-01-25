using System.Collections.Generic;
using UnityEngine;
public enum ObjectType
{
    Generic,
    Fuse,
    Monitor,
    PowerConnector,
    PowerSwitch,
    AirCanister,
    Pump,
    Co2Canister,
    AirFilter
}

public class ObjectReferences : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private  Mesh GenericMesh;
    [SerializeField] private Material TransparentMaterial;
    [SerializeField] private Material OpaqueMaterial;
    [SerializeField] private Mesh AirCanisterMesh;
    [SerializeField] private Mesh FuseMesh;
    [SerializeField] private Mesh PowerConnectorMesh;
    [SerializeField] private Mesh MonitorMesh;
    [SerializeField] private Mesh PumpMesh;
    [SerializeField] private List<Mesh> PowerSwitchMeshs;

    [SerializeField] private List<Color> PowerSwitchColors;


    [Header("General Material Settings")]
    [Tooltip("The base color of the material when transparent")]
    [SerializeField] private Color GeneralColor;
    [SerializeField] private Material MontiorMaterial;
    [SerializeField] private Material FuseMaterial;


    [Header("Object Settings")]
    [SerializeField] private float FuseDurability;
    [SerializeField] private float PowerConnectorDurability;
    [SerializeField] private float MonitorDurability;
    [SerializeField] private float PowerSwitchDurability;
    [SerializeField] private float PumpDurability;


    [SerializeField] private float AirCanisterPressure;

    [SerializeField] private float FuseMaxCurrent;
    [SerializeField] private float PowerConnectorMaxCurrent;

    [Tooltip("First Is Starting Second Is Looping Third Is Stoping")]
    [SerializeField] private List<AudioClip> PumpAudioClips;
    [SerializeField] private List<AudioClip> PowerSwitchClips;


    public void GetConstructorItemReferences(ObjectType Requested, bool Transparent, out Mesh meshOut, out Material materialOut)
    {
        meshOut = GenericMesh;
        materialOut = OpaqueMaterial;
        if (Requested == ObjectType.Fuse)
        {
            meshOut = FuseMesh;
            materialOut = FuseMaterial;
        }
        if (Requested == ObjectType.AirCanister)
        {
            meshOut = AirCanisterMesh;
        }
        if (Requested == ObjectType.Co2Canister)
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
            materialOut = MontiorMaterial;
        }
        if (Requested == ObjectType.PowerSwitch)
        {
            meshOut = PowerSwitchMeshs[0];
        }
        if (Transparent)
        {
            materialOut = TransparentMaterial;
            materialOut.color = GeneralColor;
        }
    }

    public void GetConstructorItemReferences(ObjectType Requested, out List<Mesh> meshsOut, out Material materialOut,out List<Color> ColorsOut)
    {
        meshsOut = new List<Mesh>();
        meshsOut.Add(GenericMesh);
        ColorsOut = new List<Color>();
        ColorsOut.Add(new Color (0f,0f,0f,1f));
        materialOut = OpaqueMaterial;
        if (Requested == ObjectType.PowerSwitch)
        {
            meshsOut = PowerSwitchMeshs;
            ColorsOut = PowerSwitchColors;
        }
        
    }
    public void GetConstructorAudioReferences(ObjectType Requested,out List<AudioClip> audioClipsOut)
    {
        audioClipsOut = null;
        if (Requested == ObjectType.Pump)
        {
            audioClipsOut = PumpAudioClips;
        }
        if (Requested == ObjectType.PowerSwitch)
        {
            audioClipsOut = PowerSwitchClips;
        }
    }
    public void GetStatsItemReferences(ObjectType Requested, out Dictionary<string, float?> OutList)
    {
        OutList = new Dictionary<string, float?>();
        OutList.Add("Durability", null);
        OutList.Add("Pressure", null);
        OutList.Add("Dirt", null);

        if (Requested == ObjectType.Fuse)
        {
            OutList["Durability"] = FuseDurability;
        }
        if (Requested == ObjectType.AirCanister)
        {
            OutList["Pressure"] = AirCanisterPressure;
        }
        if (Requested == ObjectType.PowerConnector)
        {
            OutList["Durability"] = PowerConnectorDurability;
        }
        if (Requested == ObjectType.Monitor)
        {
            OutList["Durability"] = MonitorDurability;
        }
        if (Requested == ObjectType.PowerSwitch)
        {
            OutList["Durability"] = PowerSwitchDurability;
        }
        if (Requested == ObjectType.Pump)
        {
            OutList["Durability"] = PumpDurability;
        }
        if (Requested == ObjectType.Co2Canister)
        {
            OutList["Pressure"] = AirCanisterPressure;
        }
    }
}