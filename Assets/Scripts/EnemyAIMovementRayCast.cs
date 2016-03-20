using UnityEngine;
using UnityEngine.UI;

using System.Collections;


public class EnemyAIMovementRayCast : MonoBehaviour {

	Transform player; // Reference to the player's position.
	public GameObject[] movement; // Reference to general Enemy Movement waypoint
	NavMeshAgent nav; // Reference to the nav mesh agent.
	public bool followPlayer = false; // is the enemy following player?!
	int movementIndex; // to check which waypoint enemy is on
	public bool moveInPath = true; // enemy moves in the set waypoint

	//Extra Elements
	public GameObject confusedStars; // when the enemy hits itself into a wall stars will show up
	public GameObject exclamation;  // When the enemy spots the player, this will show up

	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").transform; // Find Player
		movementIndex = 0; // start at waypoint 0
		nav = GetComponent <NavMeshAgent> (); // Navmesh agent 
		movementHandler (); // Start moving in the way point 

	}


	void Update ()
	{

		/* check if enemy is still moving in waypoint, if they get too close to the destination, pick the next point*/
		if(Vector3.Distance(movement[movementIndex].transform.position, transform.position) < 1f && moveInPath){

			if (movementIndex == movement.Length-1) {
				movementIndex = 0; // if the waypoint index exceeds given points, reset!
			} else {
				movementIndex++; // to the next Way point
			}
			movementHandler(); // move the enemy

		}


		// If player detected & Charging the player is enabled

		else if (followPlayer  && !moveInPath){
		
			exclamation.SetActive (true); // player detected
			attackPlayer (); // Charge the player
			followPlayer = false; // disable following player to not over power the enemy
		
		}

	}


	Vector3 toOther; // Player's position

	void attackPlayer(){

		StopAllCoroutines(); // Stop the running coroutines to not have overlapping behaviours
		StartCoroutine (ChargePlayer ()); // charge towards the player

	}


	IEnumerator ChargePlayer ()
	{
		// Charge the player when you see them, this will stop once the enemy hits into something
		do { 
			transform.Translate (Vector3.forward * 13 * Time.deltaTime); // Move towards the player's position
			yield return null;
		} while (!moveInPath == false);
		StartCoroutine (ChargePlayer ());
	}
		

	void movementHandler() {
		nav.speed = 3.5f; // enemy movement speed
		exclamation.SetActive (false); // player hasn't been detected
		nav.SetDestination (movement[movementIndex].transform.position); //set the way point dest
		// set all way point targets as black
		foreach (GameObject cylinder in movement) {
			cylinder.GetComponent<ChangeColor> ().ObjectColor = Color.black;
		}
		//change the current way point target to blue
		movement [movementIndex].GetComponent<ChangeColor> ().ObjectColor = Color.blue;
	}

	void OnCollisionEnter (Collision col) {

		if (col.gameObject.tag == "Player" && !followPlayer  && !moveInPath ) {
		
			moveInPath = true;
			StopAllCoroutines ();
			Destroy (col.gameObject);
			StartCoroutine ("RestartLevel");
			movementHandler ();


		}
		else if (!followPlayer && !moveInPath && col.gameObject.tag !="Environment"){
		
			nav.Stop();
			moveInPath = true;
			confusedStars.SetActive (true);
			StopAllCoroutines ();
			StartCoroutine ("confused");

		}

	}

	IEnumerator RestartLevel () {

		yield return new WaitForSeconds(2);
		Application.LoadLevel(Application.loadedLevel);

	}

	IEnumerator confused () {


		yield return new WaitForSeconds(4);
		confusedStars.SetActive (false);
		nav.Resume ();
		movementHandler ();


	}

}
