using UnityEngine;
using System.Collections;

public class FireBlast : MonoBehaviour {

    public GameObject target;
    private enum FireState { READY, ON, OFF };
    private FireState state = FireState.READY;
    private float markTime;

    public float onTime = 1.0f, offTime = 3.0f, scaleFactor = 10.0f;
    
    public Renderer rend;
    public BoxCollider col;
    public Transform scaleTarget;
    // Use this for initialization
    void Start () {
        rend = GetComponent<MeshRenderer>();
        col  = GetComponent<BoxCollider>();
    }
	
    void Update () {
        if (state == FireState.ON)  FireOn();
        if (state == FireState.OFF) FireOff();
    }

    void Scale(float factor) {
        scaleTarget.localScale = Vector3.MoveTowards(
            scaleTarget.localScale, 
            Vector3.one * factor, 
            scaleFactor * Time.deltaTime);
    }
    void FireOn () {
        Scale(1.0f);
        if (markTime + onTime < Time.time) {
            state = FireState.OFF;
            markTime = Time.time;
            rend.enabled = true;
            col.enabled = false;
            target.SendMessage("Deactivate");
        }
    }
    void FireOff() {
        Scale(0.0f);
        if (markTime + offTime < Time.time) {
            state = FireState.READY;
            rend.enabled = false;
            col.enabled = true;
            scaleTarget.localScale = Vector3.one;
            target.SendMessage("Activate");
        }

    }
    
    public void OnTriggerEnter(Collider coll) {
        if (state == FireState.READY) {
            if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Hologram" || coll.gameObject.tag == "Enemy") {
                state = FireState.ON;
                markTime = Time.time;
                rend.enabled = true;
                col.enabled = true;
                scaleTarget.localScale = Vector3.zero;
            }
        }
    }
}
