using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class AirCanisterController : MonoBehaviour
{
    private GameObject FillObject;
    private ObjectDirector objectDirector;
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
        get { return _height; }
        set { _height = value; }
    }

    private void Awake()
    {
        objectDirector = GetComponent<ObjectDirector>();
        FillObject = new GameObject();
        FillObject.transform.parent = this.transform;
        canvas = FillObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        FillObject.name = "FillBar";
        FillImage = FillObject.AddComponent<Image>();
        FillImage.rectTransform.localPosition = new Vector3(-0.251f, 0, 0);
        FillImage.rectTransform.localScale = Vector3.one/100;
        FillImage.rectTransform.localEulerAngles = new Vector3(0, 90, 0);
    }

    private void Update()
    {
        Maxheight = 50;
        height = objectDirector.GetPersentPressure() * Maxheight;
        if (objectDirector.objectType == ObjectType.AirCanister)
        {
            FillImage.rectTransform.sizeDelta = new Vector2(30, height);
            FillImage.color = new Color((-height / Maxheight) + 1, height / Maxheight, 0);
            FillImage.rectTransform.localPosition = new Vector3(-0.251f, (height / 200) - .25f, 0);
        }
        else if(objectDirector.objectType == ObjectType.Co2Canister)
        {
            FillImage.rectTransform.sizeDelta = new Vector2(30, -height+50);
            FillImage.color = new Color((-height / 50) + 1, height / 50, 0);
            FillImage.rectTransform.localPosition = new Vector3(-0.251f, (-height / 200), 0);
        }
    }
}
