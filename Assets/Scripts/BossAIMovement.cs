using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossAIMovement : MonoBehaviour
{
    public bool chargeDisabled = false; //  enemy is set on charging Mode
    public GameObject confusedStars;
    public GameObject exclamation;
    public float range = 10.0f;
    public bool isBoss;


    private UnityEngine.AI.NavMeshAgent nav; // Reference to the nav mesh agent.
    private AudioSource hitSFX;
    private Vector3 toOther; // Player's position
    private enum BullState { IDLE, WALK, CHARGE, STUNNED, JUMP, GROUNDPOUND, FOLLOW, DYING };
    private BullState state = BullState.IDLE;
    private float stunStart;
    private GameObject target;
    private Vector3 home;
    private Vector3 dest;
    private bool fightBegin, pound, jumping, spiked;
    private float jumpForce, windUpTime, fireTime, curTime, jumpTime;
    private Camera mainCam;
    public GameObject spikesModel;
    private int bossHealth, Walking, Charging, Idling;
    private float choice;
    private Animator anim;
    private BoxCollider[] FireAttack;
    private GameObject IceSpikes;

    void Start()
    {
        hitSFX = GetComponent<AudioSource>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>(); // Navmesh agent 
        home = transform.position;
        jumpTime = 4.375f;
        windUpTime = 2.458f;
        fireTime = 2.0f;
        mainCam = Camera.main;
        anim = GetComponent<Animator>();
        FireAttack = GetComponentsInChildren<BoxCollider>();
        IceSpikes = GameObject.Find("IceAttack");
        Charging = Animator.StringToHash("BossCharge");
        Walking = Animator.StringToHash("BossWalk");
        Walking = Animator.StringToHash("BossIdle");
        bossHealth = 4;
        fightBegin = true;
        spiked = false;
    }
    void Update()
    {
        switch (state)
        {
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

    void beginFight()
    {
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


    void Idle()
    {
        //this just plays the idle animation - rather than wandering like a regular enemy, waits for signal to start fight
        if (fightBegin)
        {
            CookNewDest();
            UpdateWalk();
            state = BullState.WALK;
        }
    }

    void Charge()
    {
        // move in saved direction until a collision
        anim.SetBool(Charging, true);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(toOther), 360.0f * Time.deltaTime);
        transform.Translate(Vector3.forward * 30 * Time.deltaTime);
    }

    void Stunned()
    {
        // wait, then resume idle status
        nav.Stop();
        anim.SetBool(Charging, false);
        anim.SetBool(Walking, true);
        if (stunStart + 4.0f < Time.time)
        {
            confusedStars.SetActive(false);
            if (isBoss)
            {
                choice = 0.3f;
               // choice = Random.value;
                if (choice < 0.5)
                {
                    curTime = Time.time;
                    pound = true;
                    state = BullState.GROUNDPOUND;
                }
                else {
                    jumping = true;
                    state = BullState.JUMP;
                }
            }
            else {
                nav.Resume();
                UpdateWalk();
            }

        }
    }

    void Jump()
    {
        if(jumping)
        {
            curTime = Time.time;
            anim.Play("Jump");
            nav.Stop();
            //make immune to spikes
            spikesModel.SendMessage("ShootSpikes");
            jumping = false;
        } else if (Time.time >= curTime + jumpTime)
        {
            nav.Resume();
            state = BullState.WALK;
        }


        //when grounded, reenable the navmesh
    }

    void GroundPound()
    {
        if (pound)
        {
            //nav.Stop();
            //play groundpound
            anim.Play("GroundPound");
            //after anim do camera shake
            pound = false;
        }
        else if (Time.time > curTime + windUpTime + fireTime && state == BullState.GROUNDPOUND)
        {
            FireAttack[0].enabled = false;
            FireAttack[1].enabled = false;
            IceSpikes.BroadcastMessage("Deactivate");
            spiked = false;
            pound = true;
            nav.Resume();
            state = BullState.WALK;
        }
        else if (Time.time > curTime + windUpTime)
        {
            //show fire
            //mainCam.SendMessage("CameraShake");
            FireAttack[0].enabled = true;
            FireAttack[1].enabled = true;
            if (!spiked) {
                IceSpikes.BroadcastMessage("Activate");
                spiked = true;
            }

        }
    }

    void Dying()
    {
        if (isBoss)
        {
            if (bossHealth > 0)
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
        Destroy(gameObject);

    }

    void CookNewDest()
    {
        dest = home + range * new Vector3(Mathf.Sin(Time.realtimeSinceStartup), 0.0f, Mathf.Cos(Time.realtimeSinceStartup));
        anim.SetBool(Walking, true);
    }

    void Follow()
    {
        // use nav to chase until collision, resume idle if target moves out of range
        nav.speed = 5;
        nav.SetDestination(target.transform.position);
        if (Vector3.Distance(transform.position, target.transform.position) > 20f || !target.activeSelf)
        {
            UpdateWalk();
            exclamation.SetActive(false);
        }
    }

    void UpdateWalk()
    {
        state = BullState.WALK;
        nav.speed = 3.5f;
        exclamation.SetActive(false);
        nav.SetDestination(dest);
    }

    void OnTriggerStay(Collider col)
    {
        if (state == BullState.DYING || state == BullState.JUMP) return;
        if (state == BullState.IDLE || state == BullState.WALK)
        {
            if (col.tag == "Player" || col.tag == "Hologram")
            {
                exclamation.SetActive(true);
                target = col.gameObject;
                home = target.transform.position;
                toOther = target.transform.position - transform.position;
                state = chargeDisabled ? BullState.FOLLOW : BullState.CHARGE;
            }
        }
        if (col.tag == "Wall" || col.tag == "Pillar")
        {
            CookNewDest();
            nav.SetDestination(dest);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (state == BullState.DYING) return;
        if (col.gameObject.CompareTag("Floor"))
        {
            if (Time.time > 5)
            {
                mainCam.SendMessage("CameraShake");
                nav.Resume();
                UpdateWalk();
            }
        }
        if (col.gameObject.tag == "Player")
        {
            anim.SetBool(Charging, false);
            hitSFX.Play();
            state = BullState.WALK;
            UpdateWalk();
        }
        else if (col.gameObject.tag == "Hologram"
          || col.gameObject.tag == "Marker"
          || col.gameObject.tag == "Debris")
        {
        }
        else if (col.gameObject.tag == "Pillar")
        {
            state = BullState.DYING;
            bossHealth--;
            if (isBoss && bossHealth > 0)
            {
                //don't think we need anything here, just takes damage?
            }
            else {
                dest = col.transform.position;
                home = transform.position;
                toOther = dest - home;
            }
        }
        else {
            if (state == BullState.CHARGE)
            {
                hitSFX.Play();
                state = BullState.STUNNED;
                stunStart = Time.time;
                nav.Stop();
                confusedStars.SetActive(true);
                anim.SetBool(Charging, false);
            }
        }
    }

    void OnCollisionStay(Collision col)
    {

    }
}
