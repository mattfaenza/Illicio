using UnityEngine;
using System.Collections;

public class animateHologram : MonoBehaviour {
    public Animator holoAnim;
    public int isRunning;
    // Use this for initialization
    void Start () {
        holoAnim = GetComponent<Animator>(); // Get the Animator
    }
	
	// Update is called once per frame
	void Update () {
        //holoAnim.SetBool(isRunning, true); // if moving enable run animation
    }
}
