using UnityEngine;
using System.Collections;

public class ChangeMaterial : MonoBehaviour {

    public Material[] mats;
    public Material OnMaterial;
    public Material OffMaterial;
    private bool activated;
    public Renderer rend;

    void Start()
    {
        activated = true;
        rend = gameObject.GetComponent<Renderer>();
    }

    void Activate()
    {
        activated = true;
    }

    void Deactivate()
    {
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            mats = rend.materials;
            mats[0] = OnMaterial;
            rend.materials = mats;
        } else
        {
            mats = rend.materials;
            mats[0] = OffMaterial;
            rend.materials = mats;
        }
    }
}
