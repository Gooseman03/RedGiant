using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SkyBoxScript : CameraSync
{

    // set the main camera in the inspector
    public Camera MainCameraObject;
    public CinemachineVirtualCamera MainCamera;

    // set the sky box camera in the inspector
    public CinemachineVirtualCamera SkyCamera;

    [SerializeField] private GameObject CameraFollowObj;

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
    protected override void OnCameraUpdate()
    {
        SkyCamera.m_Lens.FieldOfView = MainCameraObject.fieldOfView;
        SkyBoxRotation = SkyBoxRotation * TempSkyBoxRotation;
        CameraFollowObj.transform.rotation = SkyBoxRotation * MainCameraObject.transform.rotation;
    }
}
