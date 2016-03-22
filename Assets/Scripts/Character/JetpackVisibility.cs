using UnityEngine;
using System.Collections;

public class JetpackVisibility : MonoBehaviour
{

    public Material[] mats;
    public Material newMaterial;
    private bool activated;
    private Renderer rend;

    void Start()
    {
        activated = false;
        rend = gameObject.GetComponent<Renderer>();
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
            mats = rend.materials;
            mats[2] = newMaterial;
            rend.materials = mats;
        }
    }

}