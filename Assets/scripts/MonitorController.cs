using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class MonitorController : MonoBehaviour
{
    private GameObject MonitorTextObject;
    private TextMeshPro MonitorText;
    private Font font;
    private void Awake()
    {
        MonitorTextObject = new GameObject();
        MonitorTextObject.name = "MonitorText";
        MonitorTextObject.transform.parent = this.transform;
        MonitorText = MonitorTextObject.AddComponent<TextMeshPro>();
        MonitorText.rectTransform.localPosition = new Vector3(0,0,-.121f);
        MonitorText.rectTransform.localRotation = Quaternion.identity;
        MonitorText.rectTransform.localScale = Vector3.one / 50;
        MonitorText.rectTransform.sizeDelta = new Vector2(86, 58);
        MonitorText.faceColor = Color.green;
    }

    public GameObject GetMonitorTextGameobject()
    {
        return MonitorTextObject;
    }
    public string GetMonitorText()
    {
        return MonitorText.text;
    }

    public void ChangeMonitorText(string NewText)
    {
        MonitorText.text = NewText;
    }
}