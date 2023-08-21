using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorController : ObjectDirector , IDurable, IDisconnect, IGrabbable
{
    private float _Durability;
    private float _MaxDurability;
    public float Durability
    {
        get { return _Durability; }
        set { _Durability = value; }
    }
    public float MaxDurability
    {
        get { return _MaxDurability; }
        set { _MaxDurability = value; }
    }
    public void ChangeDurability(float ammount)
    {
        Durability += ammount;
        if (Durability < 0)
        {
            Durability = 0;
        }
    }
    public void SetDurability(float durability)
    {
        Durability = durability;
    }
    public void SetMaxDurability(float durability)
    {
        MaxDurability = durability;
    }
    public float GetPercentDurability()
    {
        return Durability / MaxDurability;
    }

    private GameObject MonitorTextObject;
    private TextMeshPro MonitorText;
    private string NewMonitorText;
    private GameObject MonitorPlane;
    private MeshFilter MonitorPlaneMeshFilter;
    private MeshRenderer MonitorPlaneMeshRenderer;
    private Font font;
    private float Timer;
    public void Start()
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
    public void Disconnect()
    {
        InstantChangeMonitorText("");
    }
    public GameObject GetMonitorTextGameobject()
    {
        return MonitorTextObject;
    }
    public string GetMonitorText()
    {
        MonitorText.enabled = true;
        return MonitorText.text;
    }
    public void InstantChangeMonitorText(string value)
    {
        MonitorText.enabled = true;
        MonitorText.text = value;
        NewMonitorText = value;
    }
    public void ChangeMonitorText(string NewText)
    {
        if (MonitorText == null) { return; }
        MonitorText.enabled = true;
        if (Durability < 60f)
        {
            char[] CharArrayText = NewText.ToCharArray();
            int i = 0;
            
            foreach(char Character in CharArrayText)
            { 
                if (CharArrayText[i] != '\n' && Random.Range(0, 100) < 100 - Durability)
                { 
                    CharArrayText[i] = '\u25A1'; 
                }
                i++;
            }
            NewText = CharArrayText.ArrayToString();
        }
        NewMonitorText = NewText;
    }
    public void ClearMonitorText()
    {
        InstantChangeMonitorText("");
    }
    public void Grab(Transform objectGrabPointTransform)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.layer = 6;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 6;
        }
        transform.position = objectGrabPointTransform.position;
        transform.SetParent(objectGrabPointTransform);
    }
    public void Place(Transform objectPlacePointTransform)
    {
        transform.SetParent(objectPlacePointTransform);
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.layer = 0;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 0;
        }
        transform.position = objectPlacePointTransform.position;
        transform.rotation = objectPlacePointTransform.rotation;
        gameObject.GetComponent<Collider>().enabled = true;
    }
    public void Drop(Transform NewParent)
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.layer = 0;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 0;
        }
        this.transform.SetParent(NewParent);
        this.gameObject.GetComponent<Collider>().enabled = true;
    }
}