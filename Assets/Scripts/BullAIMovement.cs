using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BullAIMovement : MonoBehaviour {
    Transform player; // Reference to the player's position.
    public GameObject[] movement; // Reference to general Enemy Movement waypoint
    NavMeshAgent nav; // Reference to the nav mesh agent.
    public bool followPlayer = false; // is the enemy following player?!
    public GameObject cautionTrigger; // caution trigger cylinder
    int movementIndex; // to check which waypoint enemy is on
    public bool chargeDisabled = false; //  enemy is set on charging Mode
    public bool moveInPath = true; // enemy moves in the set waypoint
    //Extra Elements
    public GameObject confusedStars; // when the enemy hits itself into a wall stars will show up
    public GameObject exclamation;  // When the enemy spots the player, this will show up
    public Text playerFollowText;
    private AudioSource hitSFX;
    private CapsuleCollider cap_col;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find Player
        movementIndex = 0; // start at waypoint 0
        nav = GetComponent<NavMeshAgent>(); // Navmesh agent 
        movementHandler(); // Start moving in the way point 
        hitSFX = GetComponent<AudioSource>();
        cap_col = cautionTrigger.GetComponent<CapsuleCollider>();
    }
    void Update() {
        //float temp;
        //	temp = Vector3.Distance (movement [movementIndex].transform.position, transform.position);
        //Debug.Log (temp);
        /* check if enemy is still moving in waypoint, if they get too close to the destination, pick the next point*/
        if (Vector3.Distance(movement[movementIndex].transform.position, transform.position) < 2f && moveInPath) {
            if (movementIndex++ == movement.Length - 1) movementIndex = 0;
            movementHandler(); // move the enemy
        }
        // If player detected & Charging the player is disabled
        if (followPlayer && !moveInPath) {
            exclamation.SetActive(true); // player detected
            if (chargeDisabled) {
                nav.speed = 5; // increase Navigation speed
                nav.SetDestination(player.position); // the navigation destination is the player
                                                     // if the player outruns the enemy, stop following
                if (Vector3.Distance(transform.position, player.transform.position) > 20f) {
                    movementHandler();
                    exclamation.SetActive(false);
                    moveInPath = true;
                    followPlayer = false;
                }
            } else {
                // If player detected & Charging the player is enabled
                attackPlayer(); // Charge the player
                followPlayer = false; // disable following player to not over power the enemy
            }
        }
    }
    Vector3 toOther; // Player's position
    public void attackPlayer() {
        toOther = player.transform.position - transform.position; // compare it with player's position
        StopAllCoroutines(); // Stop the running coroutines to not have overlapping behaviours
        StartCoroutine(ChargePlayer()); // charge towards the player
    }
    IEnumerator ChargePlayer() {
        // Charge the player when you see them, this will stop once the enemy hits into something
        do {
            transform.rotation = Quaternion.LookRotation(toOther);// Look at player
            transform.Translate(Vector3.forward * 20 * Time.deltaTime); // Move towards the player's position
            yield return null;
        } while (moveInPath);
        StartCoroutine(ChargePlayer());
    }
    void movementHandler() {
        nav.speed = 3.5f; // enemy movement speed
        exclamation.SetActive(false); // player hasn't been detected
        nav.SetDestination(movement[movementIndex].transform.position);
        // set all way point targets as black
        //        foreach (GameObject cylinder in movement)
        //        {
        //            cylinder.GetComponent<ChangeColor>().ObjectColor = Color.black;
        //        }
        //change the current way point target to blue
        // movement[movementIndex].GetComponent<ChangeColor>().ObjectColor = Color.blue;
    }
    void OnCollisionEnter(Collision col) {
        if (!followPlayer && !chargeDisabled && !moveInPath) {
            moveInPath = true;
            StopAllCoroutines();
            hitSFX.Play();
            if (col.gameObject.tag == "Player") {
                movementHandler();
            } else if (col.gameObject.tag != "Environment") {
                nav.Stop();
                if (cap_col) cap_col.enabled = false;
                confusedStars.SetActive(true);
                StartCoroutine("confused");
            }
        }
    }
    IEnumerator confused() {
        yield return new WaitForSeconds(4);
        confusedStars.SetActive(false);
        if (cap_col) cap_col.enabled = true;
        nav.Resume();
        movementHandler();
    }
}
