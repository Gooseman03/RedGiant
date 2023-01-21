using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchController : MonoBehaviour
{
    [SerializeField]private bool Activated;

    public bool GetState()
    {
        return Activated;
    }

    public void SetState(bool state)
    {
        Activated = state;
    }
}
