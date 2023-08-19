using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICapacity
{
    public float Pressure { get; set; }
    public float MaxPressure { get; set; }
    public void ChangePressure(float ammount)
    {
        Pressure += ammount;
        if (Pressure < 0)
        {
            Pressure = 0;
        }
    }
    public void SetPressure(float pressure)
    {
        Pressure = pressure;
    }
    public void SetMaxPressure(float pressure)
    {
        MaxPressure = pressure;
    }
    public float GetPercentPressure()
    {
        return Pressure / MaxPressure;
    }
}
