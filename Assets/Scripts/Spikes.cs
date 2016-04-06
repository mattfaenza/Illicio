using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

    private int isActivated;
    private bool activated;
    private BoxCollider col;
    private float spikeTime;
    private float curTime;
    private GameObject spikes;
    private float moveAmount;
    private Vector3 moveToTarget;
    private Vector3 originalPos;
    // Use this for initialization
    void Start () {
        spikeTime = 1.5f;
        moveToTarget = gameObject.transform.position;
        moveToTarget.y = moveToTarget.y + 1.3f;
        originalPos = gameObject.transform.position;
        activated = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (activated)
        {
            if (Time.time < curTime + spikeTime*2)
            {
                moveAmount = Mathf.Abs(gameObject.transform.position.y - moveToTarget.y);
                transform.Translate(Vector3.forward * (Time.deltaTime * moveAmount * 3));
            } 
            else if (Time.time >= curTime + spikeTime * 2)
            {
                moveAmount = Mathf.Abs(gameObject.transform.position.y - originalPos.y);
                transform.Translate(-Vector3.forward * (Time.deltaTime * moveAmount * 3));
            }
            else if (Time.time >= curTime + (spikeTime * 3)) { activated = false; }
        }
	}

    void ShootSpikes()
    {
        activated = true;
        curTime = Time.time;

    }
}
