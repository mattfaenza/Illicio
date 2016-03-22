using UnityEngine;
using System.Collections;

namespace ScrapyardChar
{
    public class TPC : MonoBehaviour
    {
        private float h, v, b; // Axis
        private float markTime; //For Time checks
        private Rigidbody rb;
        private bool isMoving, isBoosting;
        private Animator anim;
        private Animator animDead;
        private Collider playerCollider;
        private bool dead;
        private bool allowBoost;
        private Vector3 playerPos;

        public GameObject deadChar;
        public float speed;
        public GameObject SpawnPoint;
        public LayerMask ground;
        public float deadTime = 2.0f;
        public GameObject target;

        private int isRunning, isBoost;

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>(); // Get Rigidbody
            anim = GetComponent<Animator>(); // Get the Animator
            isRunning = Animator.StringToHash("Running");
            isBoost = Animator.StringToHash("Boost");
            SpawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        }

        // Update is called once per frame
        void Update()
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            if (Input.GetButton("Draw")) h = v = 0.0f;
            //b = Input.GetKey("joystick 1 button 14");

            //if (Input.GetKey ("joystick 1 button 14")) {
            isBoosting = Input.GetButton("Boost");
            //GetComponent an axis for boost
            isMoving = h != 0 || v != 0;
        }

        void FixedUpdate()
        {
            MovementManagement(h, v); // Handles the direction its facing and moving

        }

        void boostAllowed()
        {
            allowBoost = true;
        }

        public void MovementManagement(float horizontal, float vertical)
        {
            rb.velocity = rb.angularVelocity = Vector3.zero;
            Vector3 newDirection = new Vector3(h, 0, v);

            if (isMoving)
            { // Checking if its moving

                Quaternion targetRotation = Quaternion.LookRotation(newDirection);
                Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, 8 * Time.deltaTime);

                GetComponent<Rigidbody>().MoveRotation(newRotation); // ROtate

                if (isBoosting && allowBoost)
                {
                    anim.SetBool(isBoost, true); // if moving enable run animation
                    rb.MovePosition(transform.position + transform.forward * 2 * speed * Time.deltaTime); //Translate the rigidbody
                                                                                                          //want to add a drag function - think logartihmic
                }
                else
                {
                    anim.SetBool(isRunning, true); // if moving enable run animation
                    anim.SetBool(isBoost, false);
                    rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime); //Translate the rigidbody
                }

            }
            else {
                anim.SetBool(isRunning, false); //Change Animation
            }
        }


        public void OnTriggerEnter(Collider col)
        {
            //if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Hazard"))
            if (col.gameObject.CompareTag("Hazard"))
            {
                target.SendMessage("isDead");
                playerPos = gameObject.transform.position;
                markTime = Time.time;
                GameObject clone;
                clone = (GameObject)Instantiate(deadChar, playerPos, gameObject.transform.rotation);
                animDead = clone.GetComponent<Animator>(); // Get the Animator
                animDead.Play("Dead");
                gameObject.transform.position = SpawnPoint.transform.position;
                target.SendMessage("isNotDead");
            } else if (col.gameObject.CompareTag("Spikes"))
            {
                if(!isBoosting)
                {
                    target.SendMessage("isDead");
                    playerPos = gameObject.transform.position;
                    markTime = Time.time;
                    GameObject clone;
                    clone = (GameObject)Instantiate(deadChar, playerPos, gameObject.transform.rotation);
                    animDead = clone.GetComponent<Animator>(); // Get the Animator
                    animDead.Play("Dead");
                    gameObject.transform.position = SpawnPoint.transform.position;
                    target.SendMessage("isNotDead");
                } else
                {
                    //get rekt
                }
            }
        }

    }
}

