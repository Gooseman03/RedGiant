using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxScript : MonoBehaviour
{

    // set the main camera in the inspector
    public Camera MainCamera;

    // set the sky box camera in the inspector
    public Camera SkyCamera;

    public Transform Lookat;

    // the additional rotation to add to the skybox
    // can be set during game play or in the inspector
    public Vector3 SkyBoxRotation;
    public Vector3 TempSkyBoxRotation;

    public void RotateAroundSpace(Vector3 rotation)
    {
        TempSkyBoxRotation = rotation;
    }

    void FixedUpdate()
    {
        SkyBoxRotation += TempSkyBoxRotation;

        Vector3 vector3 = SkyBoxRotation.x * SkyCamera.transform.right;
        SkyCamera.transform.rotation = Quaternion.Euler(new Vector3(-vector3.x, -vector3.y, -SkyBoxRotation.z));

    }
}
