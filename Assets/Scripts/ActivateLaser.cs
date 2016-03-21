using UnityEngine;
using System.Collections;

public class ActivateLaser : MonoBehaviour {
    private Renderer rend;
    private BoxCollider col;
    private bool activated;
	// Use this for initialization
	void Start () {
        rend = gameObject.GetComponent<Renderer>();
        col = gameObject.GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update () {
	    if(activated)
        {
            rend.enabled = true;
            col.enabled = true;
        }
	}

    void Activate()
    {
        activated = true;
    }
}
