using UnityEngine;
using System.Collections;

public class FireDetect : MonoBehaviour {

    public Animator anim;
    public GameObject targetToDeactivate;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collision col)
    {
        //play fire animation
        targetToDeactivate.SendMessage("Deactivated");


    }
}
