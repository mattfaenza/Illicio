using UnityEngine;
using System.Collections;

public class FireBlast : MonoBehaviour {

    private int a;
    private Vector3 temp;
    public Renderer rend;
    public BoxCollider col;
    private bool isActivated;
	// Use this for initialization
	void Start () {
        a = 1;
        rend = GetComponent<MeshRenderer>();
        col = GetComponent<BoxCollider>();
    }
	
    //listen for a deactivate call and destroy fire instance
    //play animation for showing safe
    //once time is up indicate unsafe

	// Update is called once per frame
	void Update () {
        //if fire has just been used
        if (isActivated)
        {
            addDelay(1.0f);
            rend.enabled = false;
            col.enabled = false;
            //appear safe, for a time, change color of material in nozzle? or add smoke?
            addDelay(3.0f);
        } else {
            rend.enabled = false;
            col.enabled = true;
            //appear dangerous but no flame until collision
            isActivated = false;
        }


    }

    //  only to be used within update
    public void addDelay(float delay)
    {
        while (delay >= 0)
        {
            delay -= Time.deltaTime;
        }
    }

    private void scaleIt(Transform t)
    {
        //scale up
        if (a > 0)
        {
            temp = transform.localScale;
            temp.x = temp.x * 2;
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
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Hologram" || coll.gameObject.tag == "Enemy")
        {
            rend.enabled = true;
            //scale up size
            //play fire animation, collider dies
            isActivated = true;

        }
    }
}
