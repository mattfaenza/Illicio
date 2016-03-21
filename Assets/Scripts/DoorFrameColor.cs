using UnityEngine;
using System.Collections;

public class DoorFrameColor : MonoBehaviour {

    public Material[] mats;
    public Material newMaterial;
    private bool activated;
    private Color color;
    public MeshRenderer rend;

    void Start () {
        activated = false;
        rend = gameObject.GetComponent<MeshRenderer>();
    }

    void Activate()
    {
        activated = true;
    }


    // Update is called once per frame
    void Update () {
        if(activated)
        {
            mats = rend.materials;
            rend.materials[2] = newMaterial;
            rend.materials = mats;
        }
    }

}
