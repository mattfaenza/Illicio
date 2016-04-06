using UnityEngine;
using System.Collections;

public class IceBlast : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Activate()
    {
        Debug.Log("blasted");
        //BroadcastMessage("Activate");
    }

    void Deactivate()
    {
        //BroadcastMessage("Deactivate");
    }
}
