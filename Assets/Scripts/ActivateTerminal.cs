using UnityEngine;
using System.Collections;

public class ActivateTerminal : MonoBehaviour {

    public Material newMaterial;
    public Material[] mats;
    public Renderer rend;
    public GameObject target1ToActivate;
    public GameObject target2ToActivate;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Hologram")
        {
            mats = rend.materials;
            mats[1] = newMaterial;
            rend.materials = mats;
            target1ToActivate.SendMessage("Activate");
            target2ToActivate.SendMessage("Activate");
            if (col.gameObject.tag == "Hologram")
            {
                //col.gameObject.SetActive(false);
            }
        }
    }
}
