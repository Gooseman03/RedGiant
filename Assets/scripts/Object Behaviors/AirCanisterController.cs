using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AirCanisterController : ObjectDirector , ICapacity, IGrabbable
{
    [SerializeField] private Image FillImage;

    private float _Maxheight;
    private float _Pressure = 100;
    private float _MaxPressure = 100;
    public float Maxheight
    {
        get { return _Maxheight; }
        set { _Maxheight = value; }
    }
    public float Pressure
    {
        get { return _Pressure; }
        set { _Pressure = value; }
    }
    public float MaxPressure
    {
        get { return _MaxPressure; }
        set { _MaxPressure = value; }
    }

    public void ChangePressure(float amount)
    {
        Pressure += amount;
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

    private void Update()
    {
        Maxheight = 100;
        float height = GetPercentPressure() * Maxheight;
        FillImage.rectTransform.sizeDelta = new Vector2(100, height);
        FillImage.color = new Color((-height / Maxheight) + 1, height / Maxheight, 0);
    }
}
