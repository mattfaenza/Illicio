using UnityEngine;
using System.Collections;

public class ActivateTerminal : MonoBehaviour {

    public Material newMaterial;
    public Material[] mats;
    public Renderer rend;
    public GameObject targetToActivate;

    void Start()
    {
        rend = GetComponent<Renderer>();
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
            targetToActivate.SendMessage("Activate");
            if (col.gameObject.tag == "Hologram")
            {
                //col.gameObject.SetActive(false);
            }
        }
    }
}
