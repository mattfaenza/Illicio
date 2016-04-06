using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {
    private float markTime;
    public float deadTime;
    public bool dead;
	// Use this for initialization
	void Start () {
        markTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (dead && markTime + deadTime < Time.time)
        {
            Destroy(gameObject);
        }
    }

    void isDead()
    {
        dead = true;
    }
}
