using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BullAIMovement : MonoBehaviour {
    public GameObject[] movement;
    public bool chargeDisabled = false; //  enemy is set on charging Mode
    public GameObject confusedStars;
    public GameObject exclamation;
    public float range = 10.0f;

    private NavMeshAgent nav; // Reference to the nav mesh agent.
    private int movementIndex = 0; // to check which waypoint enemy is on
    private AudioSource hitSFX;
    private Vector3 toOther; // Player's position
    private enum BullState { IDLE, CHARGE, STUNNED, FOLLOW };
    private BullState state = BullState.IDLE;
    private float stunStart;
    private GameObject target;

    private Vector3 home;
    private Vector3 dest;

    void Start() {
        hitSFX = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>(); // Navmesh agent 
        home = transform.position;
        UpdateIdle();
    }
    void Update() {
        switch (state) {
        case BullState.IDLE:
            Idle();
            break;
        case BullState.CHARGE:
            Charge();
            break;
        case BullState.STUNNED:
            Stunned();
            break;
        case BullState.FOLLOW:
            Follow();
            break;
        }
    }
    void Charge() {
        // move in saved direction until a collision
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(toOther), 360.0f * Time.deltaTime);
        transform.Translate(Vector3.forward * 20 * Time.deltaTime);
    }
    void Stunned() {
        // wait, then resume idle status
        if (stunStart + 4.0f < Time.time) {
            confusedStars.SetActive(false);
            nav.Resume();
            UpdateIdle();
        }
    }
    void Idle() {
        // update nav if dest reached
        if (Vector3.Distance(dest, transform.position) < 2.0f) {
            if (movementIndex++ == movement.Length - 1) movementIndex = 0;
            CookNewDest();
            UpdateIdle();
        }
    }
    void CookNewDest() {
        //dest = movement[movementIndex].transform.position;
        dest = home + range * new Vector3(Mathf.Sin(Time.realtimeSinceStartup), 0.0f, Mathf.Cos(Time.realtimeSinceStartup));
    }
    void Follow() {
        // use nav to chase until collision, resume idle if target moves out of range
        nav.speed = 5;
        nav.SetDestination(target.transform.position);
        if (Vector3.Distance(transform.position, target.transform.position) > 20f || !target.activeSelf) {
            UpdateIdle();
            exclamation.SetActive(false);
        }
    }
    void UpdateIdle() {
        state = BullState.IDLE;
        nav.speed = 3.5f;
        exclamation.SetActive(false);
        nav.SetDestination(dest);
    }
    void OnTriggerEnter(Collider col) {
        if (state == BullState.IDLE) {
            if (col.tag == "Player" || col.tag == "Hologram") {
                exclamation.SetActive(true);
                target = col.gameObject;
                home = target.transform.position;
                toOther = target.transform.position - transform.position;
                state = chargeDisabled ? BullState.FOLLOW : BullState.CHARGE;
            }
        }
    }
    void OnTriggerStay(Collider col) {
        if (col.tag == "Wall") {
            CookNewDest();
            nav.SetDestination(dest);
        }
    }
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Player") {
            hitSFX.Play();
            UpdateIdle();
        } else if (col.gameObject.tag == "Hologram" || col.gameObject.tag == "Marker") {
        } else {
            if (state == BullState.CHARGE) {
                hitSFX.Play();
                state = BullState.STUNNED;
                stunStart = Time.time;
                nav.Stop();
                confusedStars.SetActive(true);
            }
        }
    }
}

