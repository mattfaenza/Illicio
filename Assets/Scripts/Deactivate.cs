using UnityEngine;
using System.Collections;

public class Deactivate : MonoBehaviour {

    private float DeactivatedTime;
	// Use this for initialization
	void Start () {
    }
	
    //listen for a deactivate call and destroy fire instance
    //play animation for showing safe
    //once time is up indicate unsafe
    void Deactivated ()
    {
        DeactivatedTime = Time.time;
        gameObject.SetActive(false); //appear safe - no glow or is smoking
        while (Time.time < DeactivatedTime + 4) ;         //delay
        gameObject.SetActive(true);
        //appear dangerous
    }

	// Update is called once per frame
	void Update () {


	
	}
}
