using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossAIMovement : MonoBehaviour {
    public bool chargeDisabled = false; //  enemy is set on charging Mode
    public GameObject confusedStars;
    public GameObject exclamation;
    public float range = 10.0f;
    public bool isBoss;


    private NavMeshAgent nav; // Reference to the nav mesh agent.
    private AudioSource hitSFX;
    private Vector3 toOther; // Player's position
    private enum BullState { IDLE, WALK, CHARGE, STUNNED, JUMP, GROUNDPOUND, FOLLOW, DYING };
    private BullState state = BullState.IDLE;
    private float stunStart;
    private GameObject target;
    private Vector3 home;
    private Vector3 dest;
    private Rigidbody rb;
    //private bool isGrounded;
    private bool fightBegin, pound;
    private float jumpForce, attackTime, curTime;
    private Camera mainCam;
    public GameObject spikesModel;
    private int bossHealth, Walking, Charging;
    private float choice;
    private Animator anim;
    private BoxCollider[] FireAttack;

    void Start() {
        hitSFX = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>(); // Navmesh agent 
        home = transform.position;
        //UpdateWalk();
        jumpForce = 500;
        attackTime = 2.0f;
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        FireAttack = GetComponentsInChildren<BoxCollider>();
        Charging = Animator.StringToHash("BossCharge");
        Walking = Animator.StringToHash("BossWalk");
        //isGrounded = true;
        bossHealth = 1;
        fightBegin = true;
        state = BullState.IDLE;
    }
    void Update() {
        switch (state) {
        case BullState.IDLE:
            Idle();
            break;
        case BullState.WALK:
            Walk();
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
        case BullState.GROUNDPOUND:
            GroundPound();
            break;
        case BullState.FOLLOW:
            Follow();
            break;
        case BullState.DYING:
            Dying();
            break;
        }
    }

    void beginFight() {
        fightBegin = true;
    }

    void Walk()
    {
        // update nav if dest reached
        if (Vector3.Distance(dest, transform.position) < 2.0f)
        {
            CookNewDest();
            UpdateWalk();
        }
    }

    void Charge() {
        // move in saved direction until a collision
        anim.SetBool(Charging, true);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(toOther), 360.0f * Time.deltaTime);
        transform.Translate(Vector3.forward * 20 * Time.deltaTime);
    }
    void Stunned() {
        // wait, then resume idle status
        anim.SetBool(Charging, false);
        anim.SetBool(Walking, true);
        if (stunStart + 4.0f < Time.time) {
            confusedStars.SetActive(false);
            if (isBoss)
            {
                choice = Random.value;
                if (choice < 0.5) {
                    curTime = Time.time;
                    pound = true;
                    state = BullState.GROUNDPOUND;
                }
                else { state = BullState.JUMP; }
            } else {
                nav.Resume();
                UpdateWalk();
            }

        }
    }

    void Jump()
    {
            anim.Play("Jump");
            nav.enabled = false;
            //make immune to spikes
            spikesModel.SendMessage("ShootSpikes");
    }

    void GroundPound()
    {
        if (pound)
        {
            //play groundpound
            anim.Play("GroundPound");
            //after anim do camera shake
            mainCam.SendMessage("CameraShake");
            FireAttack[0].enabled = true;
            FireAttack[1].enabled = true;
            pound = false;
        } else if (Time.time > curTime + attackTime) {
            FireAttack[0].enabled = false;
            FireAttack[1].enabled = false;
            state = BullState.WALK;
        }
    }

    void Idle() {
        //this just plays the idle animation
        if(fightBegin)
        {
            state = BullState.WALK;
        }
    }
    void Dying() {
        if (isBoss)
        {
            if(bossHealth > 0)
            {
                anim.Play("Hit");
                hitSFX.Play();
                state = BullState.STUNNED;
                stunStart = Time.time;
                nav.Stop();
                confusedStars.SetActive(true);
                return;
            }
        }
        nav.Stop();
        anim.Play("Die");
        //transform.rotation = Quaternion.LookRotation(toOther);
        //transform.Translate(Vector3.forward * 40 * Time.deltaTime);
        //if (Vector3.Distance(home, transform.position) >= Vector3.Distance(home, dest))
        Destroy(gameObject);

    }
    void CookNewDest() {
        dest = home + range * new Vector3(Mathf.Sin(Time.realtimeSinceStartup), 0.0f, Mathf.Cos(Time.realtimeSinceStartup));
        anim.SetBool(Walking, true);
    }
    void Follow() {
        // use nav to chase until collision, resume idle if target moves out of range
        nav.speed = 5;
        nav.SetDestination(target.transform.position);
        if (Vector3.Distance(transform.position, target.transform.position) > 20f || !target.activeSelf) {
            UpdateWalk();
            exclamation.SetActive(false);
        }
    }
    void UpdateWalk() {
        state = BullState.WALK;
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
        if (col.gameObject.CompareTag("Floor")) //|| isGrounded == false)
        {
            if (Time.time > 5)
            {
                //isGrounded = true;
                nav.enabled = true;
                mainCam.SendMessage("CameraShake");
                nav.Resume();
                UpdateWalk();
            }
        }
        if (col.gameObject.tag == "Player") {
            hitSFX.Play();
            UpdateWalk();
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

