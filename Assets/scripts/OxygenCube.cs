using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenCube : MonoBehaviour
{

    [SerializeField] private float Oxygen = 100;

    public float GetOxygen()
    {
        return Oxygen;
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
}
