using UnityEngine;
using System.Collections;

public class LightManager : MonoBehaviour {

	public GameObject lightOn;
	public GameObject lightOff;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col) {

		if (col.gameObject.tag == "Player") {
			
			lightOn.SetActive (true);
			lightOff.SetActive (false);

		}
	}
}
