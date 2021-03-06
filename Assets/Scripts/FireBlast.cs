﻿using UnityEngine;
using System.Collections;

public class FireBlast : MonoBehaviour {

    public GameObject target;
    private enum FireState { READY, ON, OFF };
    private FireState state = FireState.READY;
    private float markTime;

    public float onTime = 1.0f, offTime = 3.0f, scaleFactor = 10.0f;
    
    private Renderer rend;
    private Collider col_detect;
    private Transform scaleTarget;
    // Use this for initialization
    void Start () {
        rend = GetComponent<MeshRenderer>();
        col_detect = GetComponent<BoxCollider>();
        scaleTarget = transform.parent;
        rend.enabled = false;
        col_detect.enabled = true;
        scaleTarget.localScale = Vector3.one;
        target.SendMessage("Activate");
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
            col_detect.enabled = false;
            target.SendMessage("Deactivate");
        }
    }
    void FireOff() {
        Scale(0.0f);
        if (markTime + offTime < Time.time) {
            state = FireState.READY;
            rend.enabled = false;
            col_detect.enabled = true;
            scaleTarget.localScale = Vector3.one;
            target.SendMessage("Activate");
        }

    }
    
    public void OnTriggerEnter(Collider coll) {
        if (state == FireState.READY) {
            if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Hologram" || coll.gameObject.tag == "Enemy") {
                state = FireState.ON;
                markTime = Time.time;
                scaleTarget.localScale = Vector3.zero;
                rend.enabled = true;
                col_detect.enabled = true;
            }
        }
    }
    
}
