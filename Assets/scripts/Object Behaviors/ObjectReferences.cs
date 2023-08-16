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
    AirFilter,
    Keyboard,
    Alarm
}

public class ObjectReferences : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private  Mesh GenericMesh;
    [SerializeField] private Material TransparentMaterial;
    [SerializeField] private Material OpaqueMaterial;
    [SerializeField] private List<Mesh> AirCanisterMeshs;
    [SerializeField] private List<Mesh> FuseMeshs;
    [SerializeField] private List<Mesh> PowerConnectorMeshs;
    [SerializeField] private List<Mesh> MonitorMeshs;
    [SerializeField] private List<Mesh> PumpMeshs;
    [SerializeField] private List<Mesh> PowerSwitchMeshs;
    [SerializeField] private List<Mesh> AirFilterMeshs;

    [SerializeField] private Mesh DefaultPlane;

    [SerializeField] private List<Material> PowerSwitchMaterials;

    [Header("General Material Settings")]
    [Tooltip("The base color of the material when transparent")]
    [SerializeField] private Color GeneralColor;
    [SerializeField] private Material MontiorMaterial;
    [SerializeField] private Material FuseMaterial;
    [SerializeField] private Material AirFilterMaterial;

    [Header("Object Settings")]
    [SerializeField] private float FuseDurability;
    [SerializeField] private float PowerConnectorDurability;
    [SerializeField] private float MonitorDurability;
    [SerializeField] private float PowerSwitchDurability;
    [SerializeField] private float PumpDurability;

    [SerializeField] private float AirCanisterPressure;

    [SerializeField] private float AirFilterDirt;

    [SerializeField] private float FuseMaxCurrent;
    [SerializeField] private float PowerConnectorMaxCurrent;

    [Tooltip("First Is Starting Second Is Looping Third Is Stoping")]
    [SerializeField] private List<AudioClip> PumpAudioClips;
    [SerializeField] private List<AudioClip> PowerSwitchClips;
    [SerializeField] private List<AudioClip> AlarmClips;
    public void GetConstructorItemReferences(ObjectType Requested, bool Transparent, out List<Mesh> meshsOut, out Material materialOut)
    {
        meshsOut = new List<Mesh>();
        meshsOut.Add(GenericMesh);
        materialOut = OpaqueMaterial;
        if (Requested == ObjectType.Fuse)
        {
            meshsOut = FuseMeshs;
            materialOut = FuseMaterial;
        }
        if (Requested == ObjectType.AirCanister)
        {
            meshsOut = AirCanisterMeshs;
        }
        if (Requested == ObjectType.Co2Canister)
        {
            meshsOut = AirCanisterMeshs;
        }
        if (Requested == ObjectType.PowerConnector)
        {
            meshsOut = PowerConnectorMeshs;
        }
        if (Requested == ObjectType.Monitor)
        {
            meshsOut = MonitorMeshs;
            materialOut = MontiorMaterial;
        }
        if (Requested == ObjectType.PowerSwitch)
        {
            meshsOut = PowerSwitchMeshs;
        }
        if (Requested == ObjectType.AirFilter)
        {
            meshsOut = AirFilterMeshs;
            materialOut = AirFilterMaterial;
        }
        if (Requested == ObjectType.Pump)
        {
            meshsOut = PumpMeshs;
        }
        if (Transparent)
        {
            materialOut = TransparentMaterial;
            materialOut.color = GeneralColor;
        }
    }
    public void GetConstructorItemReferences(ObjectType Requested, out List<Mesh> meshsOut, out List<Material> materialsOut)
    {
        meshsOut = new List<Mesh>();
        meshsOut.Add(GenericMesh);
        materialsOut = new List<Material>();
        materialsOut.Add(OpaqueMaterial);
        if (Requested == ObjectType.PowerSwitch)
        {
            meshsOut = PowerSwitchMeshs;
            materialsOut = PowerSwitchMaterials;
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
        if (Requested == ObjectType.Alarm)
        {
            audioClipsOut = AlarmClips;
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
        if(Requested == ObjectType.AirFilter)
        {
            OutList["Dirt"] = AirFilterDirt;
        }
        if (OutList["Durability"] == null)
        {
            OutList.Remove("Durability");
        }
        if (OutList["Pressure"] == null)
        {
            OutList.Remove("Pressure");
        }
        if (OutList["Dirt"] == null)
        {
            OutList.Remove("Dirt");
        }
    }
    public Mesh GetConstructorPlane()
    {
        return DefaultPlane;
    }
}