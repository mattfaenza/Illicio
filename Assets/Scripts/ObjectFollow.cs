using UnityEngine;
using System.Collections;

public class ObjectFollow : MonoBehaviour {

	public GameObject markerPoints; //Get the Marker Gameobject (Drag & Drop it in editor)
	public bool startFollow; //Tell the object to start Following
	private Vector3 newPosition; //Get the new position from the marker path
	private int directionTracker; //Tracking index in the marker array
	private float dist; //Distance from the assigned point in the marker array

    public Animator holoAnim;
    public float followSpeed = 6;
    public int isRunning;

    void Start () {
		markerPoints.GetComponent<DrawMarker> (); //Get the marker script from the marker gameobject
        //holoAnim = GetComponent<Animator>(); // Get the Animator
        isRunning = Animator.StringToHash("Running");
    }
	
	void Update () {
	
		//If the object is allowed to follow
		if (startFollow) {
            transform.position = markerPoints.GetComponent<DrawMarker>().waypoints[0]; // set the marker waypoint index to 0, first vector in the path
            StartCoroutine (TargetDirection()); //Start following the path
			startFollow = false; //Stop following, this will stop the coroutine from running multiple times
        } else
        {
            holoAnim.SetBool(isRunning, false);
        }
	}


	IEnumerator TargetDirection ()
	{

		newPosition = markerPoints.GetComponent<DrawMarker>().waypoints[directionTracker]; // Assign new positions

		do { 

			Vector3 lookPos = newPosition - transform.position; //Get the difference between new assigned position vs. Current position, This will be used to get rotation later on
			dist = Vector3.Distance(newPosition, transform.position); //Check the distance between 2 points

			Quaternion rot = Quaternion.LookRotation (lookPos); // Look towards the new rotation
			transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 4); //smoothly change rotation of current look position to newer
			transform.position = Vector3.MoveTowards(transform.position, newPosition, followSpeed * Time.deltaTime);//Move the object towards new assigned position
            holoAnim.SetBool(isRunning, true); // if moving enable run animation
            yield return null;  

		} while (dist > 0.4f); //While the distance is more than X units, keep moving the object

		directionTracker++; // Once it reaches the destination, assign it next point


		if (directionTracker != markerPoints.GetComponent<DrawMarker> ().waypoints.Count) {
		
			StartCoroutine (TargetDirection ()); // iterate the loop again.


		} else {
		
			 markerPoints.GetComponent<DrawMarker> ().canDraw = true;
			markerPoints.GetComponent<DrawMarker> ().waypoints.Clear();
			directionTracker = 0;
            gameObject.SetActive(false);
            StopCoroutine ("TargetDirection");// if the loop has been exited, stop this coroutine

		}
			
		}

	}



