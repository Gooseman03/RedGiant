using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class SpaceRadar : MonoBehaviour
{
    [SerializeField] private GameObject Table;
    [SerializeField] private GameObject Space;
    [SerializeField] private Dictionary<GameObject,GameObject> Objects = new Dictionary<GameObject, GameObject>();
    private void Start()
    {
        foreach (SpaceShift child in Space.GetComponentsInChildren<SpaceShift>())
        {
            GameObject gameObject = Instantiate(child.gameObject);
            Objects.Add(gameObject,child.gameObject);
            Objects[gameObject].transform.parent = Table.transform;
            Objects[gameObject].transform.localPosition = child.transform.position / 5;
            Objects[gameObject].transform.localScale = child.transform.lossyScale;

        }
    }
    private void Update()
    {
        //foreach (GameObject Object in Objects.Keys)
        //{
        //    Objects[gameObject].transform.parent = Table.transform;
        //    Objects[gameObject].transform.position = Object.transform.position;
        //    Objects[gameObject].transform.localScale = Object.transform.localScale;
        //}
    }
}
