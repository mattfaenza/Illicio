using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]

public class DrawMarker : MonoBehaviour {

	public float timeLimit = 2; // How long can they draw the line for?
	public LineRenderer lineRender; // Line renderer for drawing things
	private int numberOfPoints = 0;

	public Color c1 = Color.white; //Start Color of the line
	public Color c2 = new Color(1, 1, 1, 0.5f); //End color of the line

	public List<Vector3> waypoints = new List<Vector3>(); // Save the line points

    public GameObject playerChar;

	public bool canDraw = true; 
	bool allowDraw = false;
	bool isRunning = false;
    public GameObject marker;

    private int frameSave = 4; // Change quality of movement with this number
	private int frameCounter = 0;

	public float speed = 1.5f;
	Vector3 pos;

	public GameObject follower; //The object that will follow the marker

	// Use this for initialization
	void Start () {
        follower.SetActive(false);
        marker  = GameObject.FindGameObjectWithTag("Marker");
        lineRender = gameObject.GetComponent<LineRenderer>();
		lineRender.material = new Material(Shader.Find("Particles/Additive")); // Change material for line renderer
		lineRender.SetColors(c1, c2); //Set the assigned colors


		pos = transform.position;
	}

	// Update is called once per frame
	void Update () {

		// SET THESE TWO UP IN INPUT MANAGER <3
		float hd = Input.GetAxis ("HorizontalDummy") * speed;//how quickly the marker moves on H Axis
		float vd = Input.GetAxis ("VerticalDummy") * speed; //how quickly the marker moves on V Axis

		transform.Translate (new Vector3(hd, 0,vd));//move the marker around

		// *CHANGE THE JOYSTICK BUTTON IF YOU DON'T HAVE JOYSTICK*
		//Initiate Draw
		if (Input.GetButtonDown("Draw") && canDraw) {
			canDraw = false;
			numberOfPoints = 0;
			lineRender.SetVertexCount(0);
            transform.position = playerChar.transform.position;

            allowDraw = true;
		}

        if(canDraw) {
            transform.position = playerChar.transform.position;
        }
        
        if(!allowDraw)
        {
            marker.GetComponent<Renderer>().enabled = false;
        }

		if(allowDraw ) {
            marker.GetComponent<Renderer>().enabled = true;
            numberOfPoints++;
			lineRender.SetVertexCount( numberOfPoints );
			Vector3 worldPos = new Vector3(0,0,0);
			worldPos = transform.position; //Draw where the marker current is
			lineRender.SetPosition(numberOfPoints - 1, worldPos);

			//Save a point every fourth frame
			if (frameCounter >= frameSave) {
				frameCounter = 0;
				waypoints.Add (worldPos); // Add it to the array

			} else {
				frameCounter++;
			}
				
			if(!isRunning){
				StartCoroutine ("Follow");
				isRunning = true;
            }
		} // end of CanDraw


		} // End of Update



	IEnumerator Follow() {

		yield return new WaitForSeconds (timeLimit); // this is adding a delay before booleans are turned off
		allowDraw = false; //Stop the marker from drawing
        //transform.position = playerChar.transform.position;
        follower.SetActive(true);
		follower.GetComponent<ObjectFollow> ().startFollow = true; //Tell the object to start following the marker path
		yield return new WaitForSeconds (timeLimit);
		isRunning = false; // Reset activating following the object

	}



	}


