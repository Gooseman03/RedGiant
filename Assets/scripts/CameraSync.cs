using UnityEngine;

public abstract class CameraSync : MonoBehaviour
{
    void LateUpdate()
    {
        OnCameraUpdate();
    }
    protected abstract void OnCameraUpdate();
}
