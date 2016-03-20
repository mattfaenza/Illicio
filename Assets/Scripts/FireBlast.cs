using UnityEngine;
using System.Collections;

public class FireBlast : MonoBehaviour {

    private int a;
    private Vector3 temp;
    public Renderer rend;
    public BoxCollider col;
    private bool isDeactivated;
    private float delay;
	// Use this for initialization
	void Start () {
        delay = 3.0f;
        a = 1;
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
            rend.enabled = false;
            col.enabled = false;
            //appear safe, for a time (delay)
            if (delay >= 0)
            {
                delay -= Time.deltaTime;
            }
            else {
                rend.enabled = false;
                col.enabled = true;
                //appear dangerous but no flame until collision
                delay = 3.0f;
                isDeactivated = false;
            }
        }


    }

    private void scaleIt(Transform t)
    {
        //scale up
        if (a > 0)
        {
            temp = transform.localScale;
            temp.x = temp.x *2;
            temp.y = temp.y * 2;
            temp.z = temp.z * 2;
            transform.localScale = temp;
        }
        //scale down
        else
        {

        }
    }

    public void OnTriggerEnter(Collider coll)
    {
        //Only react to player, hologram, or enemies
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Hologram" || col.gameObject.tag == "Enemy")
        {
            rend.enabled = true;
            //scale up size
            //play fire animation, collider dies
            isDeactivated = true;

        }
    }
}
