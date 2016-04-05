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
    private GameObject spikes;
    private bool doneJump;
    private bool jumping;

    void Start() {
        hitSFX = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>(); // Navmesh agent 
        home = transform.position;
        UpdateIdle();
        jumpForce = 9999;
        mainCam = Camera.main;
        spikes = GameObject.FindGameObjectWithTag("SpikeFloor");
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
        jumping = false;
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
                //Jump();
            jumping = true;
            break;
        case BullState.FOLLOW:
            Follow();
            break;
        case BullState.DYING:
            Dying();
            break;
        }
    }

    void FixedUpdate()
    {
        if(jumping)
        {
            Jump();
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
        //play jump animation here
        if (doneJump) {
            doneJump = false;
            UpdateIdle();
        }
        else if (isGrounded) {
            nav.enabled = false;
            rb.AddForce(Vector3.forward * jumpForce, ForceMode.Acceleration);
            isGrounded = false;
            jumping = false;
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
        if (state == BullState.DYING) return;
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
            if (col.gameObject.tag == "Player") {
            hitSFX.Play();
            UpdateIdle();
        } else if (col.gameObject.tag == "Hologram" 
            || col.gameObject.tag == "Marker" 
            || col.gameObject.tag == "Debris") {
        } else if (col.gameObject.tag == "Pillar") {
            state = BullState.DYING;
            dest = col.transform.position;
            home = transform.position;
            toOther = dest - home;
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
        if (col.gameObject.CompareTag("Floor"))
        {
            mainCam.SendMessage("CameraShake");
            spikes.SendMessage("ShootSpikes");
            isGrounded = true;
            doneJump = true;
            nav.enabled = true;
        }
    }
}

