using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

    private Animator anim;
    private int isActivated;
    private bool activated;
    private BoxCollider col;
    private float spikeTime;
    private float curTime;
    private GameObject spikes;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>(); // Get the Animator
        col = GetComponent<BoxCollider>();
        isActivated = Animator.StringToHash("Activate");
        spikeTime = 2.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (activated)
        {
            //add a delay here?
            anim.SetBool(isActivated, true);
            col.enabled = true;
            //transform.Translate(Vector3.forward * 1.3f);
            if (Time.time >= curTime + spikeTime)
            {
                anim.SetBool(isActivated, false);
                activated = false;
                col.enabled = false;
            }
        }
	}

    void ShootSpikes()
    {
        activated = true;
        curTime = Time.time;

    }

    /*
    public void OnTriggerEnter(Collider col)
    {
        anim.SetBool(isActivated, true);
        // anim.Play("SpikesUp");
    }

    public void OnTriggerExit(Collider col)
    {
        //anim.Play("SpikesDown");

    }
     */
}
