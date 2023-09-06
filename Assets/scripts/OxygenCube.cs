using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenCube : MonoBehaviour
{
    [SerializeField] private float _Oxygen = 100;
    [SerializeField] private float _Carbon = 0;
    
    public float Oxygen
    {
        get { return _Oxygen; }
        set { _Oxygen = value; }
    }

    public float Carbon
    {
        get { return _Carbon; }
        set { _Carbon = value; }
    }


    public void ChangeOxygen(float Amount)
    {
        Oxygen += Amount;
        if (Oxygen < 0)
        {
            Oxygen = 0;
        }
        if (Oxygen > 100)
        {
            Oxygen = 100;
        }
    }

    public void ChangeCarbon(float Amount)
    {
        Carbon += Amount;
        if (Carbon < 0)
        {
            Carbon = 0;
        }
        if (Carbon > 100)
        {
            Carbon = 100;
        }
    }
}
