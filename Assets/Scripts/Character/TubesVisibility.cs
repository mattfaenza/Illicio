using UnityEngine;
using System.Collections;

public class TubesVisibility : MonoBehaviour {

    public Material[] mats;
    public Material newMaterial;
    private bool activated;
    public Renderer rend;

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
            mats[0] = newMaterial;
            rend.materials = mats;
        }
    }

}