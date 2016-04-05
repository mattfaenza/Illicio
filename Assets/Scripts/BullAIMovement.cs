using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BullAIMovement : MonoBehaviour {
    public bool chargeDisabled = false; //  enemy is set on charging Mode
    public GameObject confusedStars;
    public GameObject exclamation;
    public float range = 10.0f;
    public bool isBoss;

    private NavMeshAgent nav; // Reference to the nav mesh agent.
    private AudioSource hitSFX;
    private Vector3 toOther; // Player's position
    private enum BullState { IDLE, CHARGE, STUNNED, JUMP, FOLLOW, DYING };
    private BullState state = BullState.IDLE;
    private float stunStart;
    private GameObject target;
    private Vector3 home;
    private Vector3 dest;
    private Rigidbody rb;
    private bool isGrounded;
    private float jumpForce;
    private Camera mainCam;
    public GameObject spikesModel;
    private int bossHealth;

    void Start() {
        hitSFX = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>(); // Navmesh agent 
        home = transform.position;
        UpdateIdle();
        jumpForce = 500;
        mainCam = Camera.main;
        //spikes = GameObject.FindGameObjectWithTag("SpikeFloor");
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
        bossHealth = 1;
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
        case BullState.JUMP:
            Jump();
            break;
        case BullState.FOLLOW:
            Follow();
            break;
        case BullState.DYING:
            Dying();
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
            if (isBoss) { state = BullState.JUMP; }
            else {
                nav.Resume();
                UpdateIdle();
            }

        }
    }

    void Jump()
    {
        if (isGrounded) {
            //play jump animation here
            nav.enabled = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
            isGrounded = false;
        }
    }

    void Idle() {
        // update nav if dest reached
        if (Vector3.Distance(dest, transform.position) < 2.0f) {
            CookNewDest();
            UpdateIdle();
        }
    }
    void Dying() {
        if (isBoss)
        {
            if(bossHealth > 0)
            {
                hitSFX.Play();
                state = BullState.STUNNED;
                stunStart = Time.time;
                nav.Stop();
                confusedStars.SetActive(true);
                return;
            }
        }
        transform.rotation = Quaternion.LookRotation(toOther);
        transform.Translate(Vector3.forward * 40 * Time.deltaTime);
        if (Vector3.Distance(home, transform.position) >= Vector3.Distance(home, dest))
            Destroy(gameObject);

    }
    void CookNewDest() {
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
    void OnTriggerStay(Collider col) {
        if (state == BullState.DYING || state == BullState.JUMP) return;
        if (state == BullState.IDLE) {
            if (col.tag == "Player" || col.tag == "Hologram") {
                exclamation.SetActive(true);
                target = col.gameObject;
                home = target.transform.position;
                toOther = target.transform.position - transform.position;
                state = chargeDisabled ? BullState.FOLLOW : BullState.CHARGE;
            }
        }
        if (col.tag == "Wall" || col.tag == "Pillar") {
            CookNewDest();
            nav.SetDestination(dest);
        }
    }
    void OnCollisionEnter(Collision col) {
        if (state == BullState.DYING) return;
        if (col.gameObject.CompareTag("Floor") || isGrounded == false)
        {
            isGrounded = true;
            nav.enabled = true;
            spikesModel.SendMessage("ShootSpikes");
            mainCam.SendMessage("CameraShake");
            nav.Resume();
            UpdateIdle();
        }
        if (col.gameObject.tag == "Player") {
            hitSFX.Play();
            UpdateIdle();
        } else if (col.gameObject.tag == "Hologram" 
            || col.gameObject.tag == "Marker" 
            || col.gameObject.tag == "Debris") {
        } else if (col.gameObject.tag == "Pillar") {
            state = BullState.DYING;
            bossHealth--;
            if (isBoss && bossHealth > 0)
            {
                //don't think we need anything here, just takes damage?
            } else {
                dest = col.transform.position;
                home = transform.position;
                toOther = dest - home;
            }
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

    void OnCollisionStay(Collision col) {

    }
}

