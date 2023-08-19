using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Co2CanisterController : ObjectDirector , ICapacity
{
    private float _Pressure;
    private float _MaxPressure;
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

    private GameObject FillObject;
    private Canvas canvas;
    private Image FillImage;
    private float _height;

    private float _Maxheight;
    public float height
    {
        get { return _height; }
        set { _height = value; }
    }
    public float Maxheight
    {
        get { return _Maxheight; }
        set { _Maxheight = value; }
    }


    //private void Start()
    //{
    //    //FillObject = new GameObject();
    //    //FillObject.transform.parent = this.transform;
    //    //canvas = FillObject.AddComponent<Canvas>();
    //    //canvas.renderMode = RenderMode.WorldSpace;
    //    //FillObject.name = "FillBar";
    //    //FillImage = FillObject.AddComponent<Image>();
    //    //FillImage.rectTransform.localPosition = new Vector3(-0.251f, 0, 0);
    //    //FillImage.rectTransform.localScale = Vector3.one / 100;
    //    //FillImage.rectTransform.localEulerAngles = new Vector3(0, 90, 0);
    //}

    //private void Update()
    //{
    //    //TODO: FIX ME
    //    //Maxheight = 50;
    //    //height = GetPercentPressure() * Maxheight;
    //    //if (objectDirector.objectType == ObjectType.AirCanister)
    //    //{
    //    //    FillImage.rectTransform.sizeDelta = new Vector2(30, height);
    //    //    FillImage.color = new Color((-height / Maxheight) + 1, height / Maxheight, 0);
    //    //    FillImage.rectTransform.localPosition = new Vector3(-0.251f, (height / 200) - .25f, 0);
    //    //}
    //    //else if(objectDirector.objectType == ObjectType.Co2Canister)
    //    //{
    //    //    FillImage.rectTransform.sizeDelta = new Vector2(30, -height+50);
    //    //    FillImage.color = new Color((-height / 50) + 1, height / 50, 0);
    //    //    FillImage.rectTransform.localPosition = new Vector3(-0.251f, (-height / 200), 0);
    //    //}
    //}
}
