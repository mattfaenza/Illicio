using UnityEngine;
using System.Collections;

public class FireBlast : MonoBehaviour {

    public Renderer rend;
    public BoxCollider col;
    private bool isDeactivated;
    private float delay;
	// Use this for initialization
	void Start () {
        delay = 3.0f;
        rend = GetComponent<MeshRenderer>();
        col = GetComponent<BoxCollider>();
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
            //appear safe, for a time (delay)
            if (delay >= 0)
            {
                delay -= Time.deltaTime;
            }
            else {
                rend.enabled = false;
                //appear dangerous but no flame until collision
                delay = 3.0f;
                isDeactivated = false;
            }
        }


    }

    public void OnTriggerEnter(Collider coll)
    {
        //Only react to player, hologram, or enemies
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Hologram" || col.gameObject.tag == "Enemy")
        {
            isDeactivated = false;
            rend.enabled = true;
            //play fire animation, collider dies, begin to  recharge
            isDeactivated = true;
            rend.enabled = false;
        }
    }
}
