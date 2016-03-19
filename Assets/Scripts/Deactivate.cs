using UnityEngine;
using System.Collections;

public class Deactivate : MonoBehaviour {

    public Renderer rend;
    public MeshCollider col;
    private bool isDeactivated;
    private float delay;
	// Use this for initialization
	void Start () {
        delay = 3.0f;
        rend = GetComponent<MeshRenderer>();
        col = GetComponent<MeshCollider>();
    }
	
    //listen for a deactivate call and destroy fire instance
    //play animation for showing safe
    //once time is up indicate unsafe
    void Deactivated ()
    {
        isDeactivated = true;
        rend.enabled = false;
        col.enabled = false;
        //appear safe - no glow or is smoking
    }

	// Update is called once per frame
	void Update () {
        if(isDeactivated)
        {
            if (delay >= 0)
            {
                delay -= Time.deltaTime;
            }
            else {
                //delay here
                //turn on renderer
                //turn on collider
                rend.enabled = true;
                //col.enabled = true;
                //appear dangerous
                delay = 3.0f;
                isDeactivated = false;
            }
        }


    }

    public void OnTriggerEnter(Collider coll)
    {
        //play fire animation
        isDeactivated = true;
        rend.enabled = false;
        //col.enabled = false;
        //appear safe - no glow or is smoking

    }
}
