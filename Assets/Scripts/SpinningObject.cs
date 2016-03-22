using UnityEngine;
using System.Collections;

public class SpinningObject : MonoBehaviour {

    public float speed;
	// Use this for initialization
	void Start () {

        speed = 12f;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
