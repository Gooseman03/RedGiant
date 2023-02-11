using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxScript : MonoBehaviour
{

    // set the main camera in the inspector
    public Camera MainCamera;

    // set the sky box camera in the inspector
    public Camera SkyCamera;

    // the additional rotation to add to the skybox
    // can be set during game play or in the
    // inspector
    public Quaternion SkyBoxRotation;
    public Quaternion TempSkyBoxRotation;

    private void Start()
    {
        SkyBoxRotation = Quaternion.identity;
    }
    public void RotateAroundSpace(Vector3 rotation)
    {
        TempSkyBoxRotation.eulerAngles = -rotation;
    }
    private void Update()
    {
        SkyCamera.transform.rotation = SkyBoxRotation * MainCamera.transform.rotation;
    }

    private void FixedUpdate()
    { 
        SkyBoxRotation = SkyBoxRotation * TempSkyBoxRotation;
        SkyCamera.transform.rotation = SkyBoxRotation * MainCamera.transform.rotation;
    }
}
