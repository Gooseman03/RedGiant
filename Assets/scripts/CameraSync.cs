using UnityEngine;

public abstract class CameraSync : MonoBehaviour
{
    void Update()
    {
        OnCameraUpdate();
    }
    protected abstract void OnCameraUpdate();
}
