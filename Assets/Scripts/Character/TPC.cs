using UnityEngine;
using System.Collections;

namespace ScrapyardChar {
    public class TPC : MonoBehaviour {
        
        public GameObject deadChar;
        public float speed;
        public GameObject SpawnPoint;
        public float deadTime = 2.0f;
        
        private int isRunning, isBoost;
        private float h, v;
        private Rigidbody rb;
        private bool isMoving, isBoosting, allowBoost;
        private Animator anim;

        public bool timedJetpack = false;
        public float fuelMax = 4.0f;
        private float fuelLeft;
        public float delayOnJetpackRecharge = 1.0f;
        private float jetpackRecharge;

        void Start() {
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            isRunning = Animator.StringToHash("Running");
            isBoost = Animator.StringToHash("Boost");
            SpawnPoint = GameObject.FindGameObjectWithTag("Respawn");

            fuelLeft = fuelMax;
            jetpackRecharge = delayOnJetpackRecharge;
        }
        void Update() {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            if (Input.GetButton("Draw")) h = v = 0.0f;
            
            isBoosting = Input.GetButton("Boost") && allowBoost;
            isMoving = h != 0 || v != 0;

            if (timedJetpack) TimedJetpack();
        }
        void TimedJetpack() {
            jetpackRecharge -= Time.deltaTime;
            if (jetpackRecharge <= 0.0f) jetpackRecharge = 0.0f;
            if (Input.GetButton("Boost")) jetpackRecharge = delayOnJetpackRecharge;
            if (isBoosting) fuelLeft -= Time.deltaTime;
            if (jetpackRecharge == 0.0f) fuelLeft += Time.deltaTime;
            if (fuelLeft <= 0.0f) fuelLeft = 0.0f;
            if (fuelMax <= fuelLeft) fuelLeft = fuelMax;
            if (fuelLeft == 0.0f) isBoosting = false;
        }
        void FixedUpdate() {
            rb.velocity = rb.angularVelocity = Vector3.zero;
            Vector3 newDirection = new Vector3(h, 0, v);

            anim.SetBool(isRunning, isMoving && !isBoosting);
            anim.SetBool(isBoost, isBoosting && isMoving);
            if (isMoving) {
                Quaternion targetRotation = Quaternion.LookRotation(newDirection);
                Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, 8 * Time.deltaTime);
                GetComponent<Rigidbody>().MoveRotation(newRotation); // Rotate
                rb.MovePosition(transform.position +
                    transform.forward * speed * Time.deltaTime * (isBoosting ? 2 : 1) );
            }
        }

        void boostAllowed() {
            allowBoost = true;
        }
        void SwapCorpseIn() {
            GameObject clone = (GameObject)Instantiate(deadChar, transform.position, transform.rotation);
            clone.GetComponent<Animator>().Play("Dead");
            transform.position = SpawnPoint.transform.position;
        }
        void OnCollisionEnter(Collision col) {
            if (col.gameObject.CompareTag("Enemy")) {
                SwapCorpseIn();
            }
        }
        void OnTriggerEnter(Collider col) {
            if (col.gameObject.CompareTag("Hazard") ||
                (col.gameObject.CompareTag("Spikes") && !isBoosting)) {
                SwapCorpseIn();
            }
        }
    }
}

