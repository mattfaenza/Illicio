using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

    private Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>(); // Get the Animator
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider col)
    {
            anim.Play("SpikesUp");
    }

    public void OnTriggerExit(Collider col)
    {
        anim.Play("SpikesDown");

    }
     
}
