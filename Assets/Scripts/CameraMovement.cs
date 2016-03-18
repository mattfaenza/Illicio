using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	float speed = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.S)) {
			//Debug.Log ("it gets here");
			transform.Translate (Vector3.forward * Time.deltaTime * speed);
		}
	
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate (Vector3.left * Time.deltaTime * speed);
		}
		if (Input.GetKey (KeyCode.W)) {
			transform.Translate (Vector3.back * Time.deltaTime * speed);
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate (Vector3.right * Time.deltaTime * speed);
		}


	}
}
