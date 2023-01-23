using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerSwitchController : MonoBehaviour
{
    [SerializeField] private bool Activated = false;
    private ObjectReferences objectReferences;


    private List<Color> colorList;

    [SerializeField] private GameObject GreenButton;
    [SerializeField] private GameObject RedButton;


    public void StartUp(ObjectReferences newReferences) 
    {
        objectReferences = newReferences;
        objectReferences.GetConstructorItemReferences(ObjectType.PowerSwitch, out List<Mesh> ListOfMeshs, out Material material, out List<Color> ListOfColors);

        colorList = ListOfColors;
        GreenButton = new GameObject();
        RedButton = new GameObject();
        void Setup(GameObject Button)
        {
            Button.transform.parent = transform;
            Button.transform.localPosition = Vector3.zero;
            Button.transform.localRotation = Quaternion.identity;
            MeshFilter meshFilter = Button.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = Button.AddComponent<MeshRenderer>();
            if (Button == GreenButton)
            {
                meshFilter.sharedMesh = ListOfMeshs[2];
            }
            else
            {
                meshFilter.sharedMesh = ListOfMeshs[1];
            }
            meshRenderer.material = material;   

        }
        Setup(GreenButton);
        Setup(RedButton);
    }

    public bool GetState()
    {
        return Activated;
    }

    public void SetState(bool state)
    {
        
        float durability = (float)this.GetComponent<ObjectGrabbable>().Durability;
        if (durability < 60 && Random.Range(0, 100) > durability)
        {
            return;
        }
        Activated = state;
        if (Activated)
        {
            GreenButton.GetComponent<MeshRenderer>().material.color = colorList[0];
            RedButton.GetComponent<MeshRenderer>().material.color = colorList[3];
            GreenButton.transform.localPosition = new Vector3(0f, 0f, -.01f);
            RedButton.transform.localPosition = Vector3.zero;
        }
        else
        {
            GreenButton.GetComponent<MeshRenderer>().material.color = colorList[1];
            RedButton.GetComponent<MeshRenderer>().material.color = colorList[2];
            GreenButton.transform.localPosition = Vector3.zero; ;
            RedButton.transform.localPosition = new Vector3(0f, 0f, -.01f);
        }
    }
}
