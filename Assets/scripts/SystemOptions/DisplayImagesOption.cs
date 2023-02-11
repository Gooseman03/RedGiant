using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayImagesOption : MonoBehaviour
{
    [SerializeField] private BaseSystem _baseSystem;
    public BaseSystem baseSystem
    {
        get { return _baseSystem; }
    }
    [SerializeField] private List<Material> materials = new List<Material>();

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    private void WhenPowered()
    {
        if (baseSystem.itemRegister.HasObject(ObjectType.Monitor, out List<ObjectDirector> ListOfMonitors))
        {
            foreach (ObjectDirector monitor in ListOfMonitors)
            {
                monitor.MonitorPlaneEnable();
                monitor.SetMonitorPlaneMaterial(materials[0]);
            }
        }
    }
}
