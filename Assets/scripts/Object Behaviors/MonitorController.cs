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
    private GameObject MonitorPlane;
    private MeshFilter MonitorPlaneMeshFilter;
    private MeshRenderer MonitorPlaneMeshRenderer;
    private Font font;
    private float Timer;
    public void StartUp(Mesh Plane)
    {
        MonitorTextObject = new GameObject();
        MonitorTextObject.name = "MonitorText";
        MonitorTextObject.transform.parent = this.transform;
        MonitorText = MonitorTextObject.AddComponent<TextMeshPro>();
        MonitorText.rectTransform.localPosition = new Vector3(0, 0, -.121f);
        MonitorText.rectTransform.localRotation = Quaternion.identity;
        MonitorText.rectTransform.localScale = Vector3.one / 50;
        MonitorText.rectTransform.sizeDelta = new Vector2(86, 58);
        MonitorText.faceColor = Color.green;
        MonitorPlane = new GameObject("RenderPlane");
        MonitorPlane.transform.parent = transform;
        MonitorPlane.transform.localPosition = new Vector3(0, 0, -.121f);
        MonitorPlane.transform.localEulerAngles = new Vector3(90, 180, 0);
        MonitorPlane.transform.localScale = new Vector3(.172f, 1, .116f);
        MonitorPlaneMeshFilter = MonitorPlane.AddComponent<MeshFilter>();
        MonitorPlaneMeshRenderer = MonitorPlane.AddComponent<MeshRenderer>();
        MonitorPlaneMeshFilter.mesh = Plane;
        MonitorPlaneMeshRenderer.enabled = false;
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
        MonitorText.enabled = true;
        MonitorPlaneMeshRenderer.enabled = false;
        return MonitorText.text;
    }
    public void InstantChangeMonitorText(string value)
    {
        MonitorText.enabled = true;
        MonitorPlaneMeshRenderer.enabled = false;
        MonitorText.text = value;
        NewMonitorText = value;
    }
    public void ChangeMonitorText(string NewText)
    {
        MonitorText.enabled = true;
        MonitorPlaneMeshRenderer.enabled = false;
        if (this.GetComponent<ObjectDirector>().Durability < 60f)
        {
            char[] CharArrayText = NewText.ToCharArray();
            int i = 0;
            
            foreach(char Character in CharArrayText)
            { 
                if (CharArrayText[i] != '\n' && Random.Range(0, 100) < 100 - (float)this.GetComponent<ObjectDirector>().Durability)
                { 
                    CharArrayText[i] = '\u25A1'; 
                }
                i++;
            }
            NewText = CharArrayText.ArrayToString();
        }
        NewMonitorText = NewText;
    }
    public void MonitorPlaneEnable()
    {
        MonitorText.enabled = false;
        MonitorPlaneMeshRenderer.enabled = true;
    }
    public void SetMonitorPlaneMaterial(Material material)
    {
        MonitorPlaneMeshRenderer.material = material;
    }
}