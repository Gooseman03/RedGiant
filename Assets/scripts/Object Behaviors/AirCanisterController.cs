using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class AirCanisterController : MonoBehaviour
{
    private GameObject FillObject;
    private ObjectDirector ObjectGrabbable;
    private Canvas canvas;
    private Image FillImage;
    private float _height = 1;
    public float height
    {
        get { return _height; }
        set { _height = value; }
    }

    private void Awake()
    {
        ObjectGrabbable = GetComponent<ObjectDirector>();
        FillObject = new GameObject();
        FillObject.transform.parent = this.transform;
        canvas = FillObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        FillObject.name = "FillBar";
        FillImage = FillObject.AddComponent<Image>();
        FillImage.rectTransform.localPosition = new Vector3(-0.251f, -.25f, 0);
        FillImage.rectTransform.localScale = Vector3.one/100;
        FillImage.rectTransform.localEulerAngles = new Vector3(0, 90, 0);
    }

    private void Update()
    {
        height = (float)ObjectGrabbable.Pressure/2;
        if (ObjectGrabbable.objectType == ObjectType.AirCanister)
        {
            FillImage.rectTransform.sizeDelta = new Vector2(30, height);
            FillImage.color = new Color((-height / 50) + 1, height / 50, 0);
            FillImage.rectTransform.localPosition = new Vector3(-0.251f, (height / 200) - .25f, 0);
        }
        else if(ObjectGrabbable.objectType == ObjectType.Co2Canister)
        {
            FillImage.rectTransform.sizeDelta = new Vector2(30, -height+50);
            FillImage.color = new Color((-height / 50) + 1, height / 50, 0);
            FillImage.rectTransform.localPosition = new Vector3(-0.251f, (-height / 200), 0);
        }
    }
}
