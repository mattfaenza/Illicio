using UnityEngine;
using System.Collections;

public class Deactivate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    //listen for a deactivate call and destroy fire instance
    //play animation for showing safe
    //once time is up indicate unsafe
    void Deactivated ()
    {
        Destroy(gameObject);
        //appear safe - no glow or smoke
        //delay (3s?)
        //appear dangerous
    }

	// Update is called once per frame
	void Update () {


	
	}
}
