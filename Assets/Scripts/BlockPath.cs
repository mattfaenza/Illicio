using UnityEngine;
using System.Collections;

public class BlockPath : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            target.SendMessage("Activate");
        }
    }
}
