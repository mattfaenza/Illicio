using UnityEngine;
using System.Collections;

public class TubesVisibility : MonoBehaviour
{

    //public Material[] mats;
    public Material newMaterial;
    private bool activated;
    private MeshRenderer rend;

    void Start()
    {
        activated = false;
        rend = gameObject.GetComponent<MeshRenderer>();
    }

    void Activate()
    {
        activated = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            rend.material = newMaterial;
        }
    }

}