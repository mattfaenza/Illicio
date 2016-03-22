using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {
    private float markTime;
    public float deadTime;
	// Use this for initialization
	void Start () {
        markTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (markTime + deadTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
