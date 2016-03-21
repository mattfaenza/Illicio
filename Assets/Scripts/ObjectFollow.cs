using UnityEngine;
using System.Collections;

public class ObjectFollow : MonoBehaviour
{

    public GameObject markerPoints;
    public Animator holoAnim;
    public float followSpeed = 6;

    enum FollowStates {READY, RUNNING, IDLE};
    private FollowStates state = FollowStates.IDLE;
    private Vector3 destination, destVec;
    private DrawMarker dm;
    private int wayCur, wayNum, isRunning;
    private Renderer[] renderers;

    void Start() {
        dm = markerPoints.GetComponent<DrawMarker>();
        isRunning = Animator.StringToHash("Running");
        renderers = GetComponentsInChildren<Renderer>();
    }
    public void FollowPrime() {
        gameObject.SetActive(true);
        state = FollowStates.READY;
    }
    void FollowStart() {
        wayCur = 1;
        wayNum = dm.waypoints.Count;
        if (wayNum < 2) {
            FollowEnd();
            return;
        }
        transform.position = dm.waypoints[0];
        destination = dm.waypoints[1];
        transform.rotation = Quaternion.LookRotation(destination - transform.position);
        holoAnim.SetBool(isRunning, true);
        foreach (Renderer rend in renderers) rend.enabled = true;
        state = FollowStates.RUNNING;
    }
    void FollowNext() {
        if (++wayCur < wayNum) {
            destination = dm.waypoints[wayCur];
            FollowMain();
        } else FollowEnd();
    }
    void FollowMain() {
        destVec = destination - transform.position;
        if (destVec.magnitude > 0.4f) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destVec), Time.deltaTime * 360.0f);
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * followSpeed);
        } else FollowNext();
    }
    void FollowEnd() {
        state = FollowStates.IDLE;
        dm.DrawReset();
        holoAnim.SetBool(isRunning, false);
        //gameObject.SetActive(false);
        foreach (Renderer rend in renderers) rend.enabled = false;
    }
    void Update() {
        if (state == FollowStates.READY)   FollowStart();
        if (state == FollowStates.RUNNING) FollowMain();
    }
    void OnCollisionEnter(Collision col) {
        if (!col.gameObject.CompareTag("Player") && 
            !col.gameObject.CompareTag("Hazard")
            ) FollowEnd();
    }
}



