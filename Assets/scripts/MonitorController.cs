using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonitorController : MonoBehaviour
{
    private GameObject MonitorTextObject;
    private TextMeshPro MonitorText;
    private string NewMonitorText;
    private Font font;
    private float Timer;
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

    private void Update()
    {
        if (Timer > 1f)
        {
            MonitorText.text = NewMonitorText;
            Timer = 0;
        }
        Timer += Time.deltaTime;
    }

    public GameObject GetMonitorTextGameobject()
    {
        return MonitorTextObject;
    }
    public string GetMonitorText()
    {
        return MonitorText.text;
    }
    public void InstantChangeMonitorText(string value)
    {
        MonitorText.text = value;
    }

    public void ChangeMonitorText(string NewText)
    {
        
        if (this.GetComponent<ObjectGrabbable>().Durability < 60f)
        {
            char[] CharArrayText = NewText.ToCharArray();
            int i = 0;
            
            foreach(char Character in CharArrayText)
            { 
                if (CharArrayText[i] != '\n' && Random.Range(0, 100) < 100 - (float)this.GetComponent<ObjectGrabbable>().Durability)
                { 
                    CharArrayText[i] = '\u25A1'; 
                }
                i++;
            }
            NewText = CharArrayText.ArrayToString();
        }
        NewMonitorText = NewText;
    }
}