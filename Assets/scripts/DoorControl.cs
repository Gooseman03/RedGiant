using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    [SerializeField] private BoxCollider Trigger;
    [SerializeField] private GameObject Door;
    [SerializeField] private float TimeToOpen;
    [SerializeField] private float LeftToOpen;

    [SerializeField] private bool _Open;
    [SerializeField] private bool _Close;
    public bool Open
    {
        get { return _Open; }
        set { _Open = value; }
    }
    public bool Close
    {
        get { return _Close; }
        set { _Close = value; }
    }
    private void Update()
    {
        if (Open)
        {
            if (LeftToOpen >= TimeToOpen)
            {
                LeftToOpen = 0;
            }
            Open = false;
            StopCoroutine("DoorClose");
            StartCoroutine("DoorOpen");
        }
        if (Close)
        {
            if (LeftToOpen >= TimeToOpen)
            {
                LeftToOpen = 0;
            }
            Close = false;
            StopCoroutine("DoorOpen");
            StartCoroutine("DoorClose");
        }
    }
    private IEnumerator DoorOpen()
    {
        while (LeftToOpen < TimeToOpen)
        {
            Door.transform.localScale = new Vector3(Door.transform.localScale.x, Mathf.Lerp(3.1f, 0.1f,LeftToOpen/TimeToOpen ), Door.transform.localScale.z);
            Door.transform.localPosition = new Vector3(Door.transform.localPosition.x, Mathf.Lerp(1, 2.5f,  LeftToOpen / TimeToOpen), Door.transform.localPosition.z);
            LeftToOpen += Time.deltaTime;
            yield return null;
        }
        Door.transform.localScale = new Vector3(Door.transform.localScale.x, 0.1f, Door.transform.localScale.z);
        Door.transform.localPosition= new Vector3(Door.transform.localPosition.x, 2.5f , Door.transform.localPosition.z);
    }
    private IEnumerator DoorClose()
    {
        while (LeftToOpen < TimeToOpen)
        {
            Door.transform.localScale = new Vector3(Door.transform.localScale.x, Mathf.Lerp(0.1f, 3, LeftToOpen / TimeToOpen), Door.transform.localScale.z);
            Door.transform.localPosition = new Vector3(Door.transform.localPosition.x, Mathf.Lerp(2.5f, 1,  LeftToOpen / TimeToOpen), Door.transform.localPosition.z);
            LeftToOpen += Time.deltaTime;
            yield return null;
        }
        Door.transform.localScale = new Vector3(Door.transform.localScale.x, 3.1f, Door.transform.localScale.z);
        Door.transform.localPosition = new Vector3(Door.transform.localPosition.x, 1, Door.transform.localPosition.z);
    }
}
