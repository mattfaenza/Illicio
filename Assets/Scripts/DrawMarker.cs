using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]

public class DrawMarker : MonoBehaviour {

	public float timeLimit = 2; // How long can they draw the line for?
	public LineRenderer lineRender; // Line renderer for drawing things
	public Color c1 = Color.white; //Start Color of the line
	public Color c2 = new Color(1, 1, 1, 0.5f); //End color of the line
    public List<Vector3> waypoints = new List<Vector3>(); // Save the line points
    public GameObject playerChar;
    public GameObject marker;
    public float speed = 9.0f;
	public GameObject follower; //The object that will follow the marker

    public float drawStepTime = 0.1f;
    private enum DrawStates { READY, DRAW, FOLLOW };
    private DrawStates state = DrawStates.READY;
    private int linePoints = 0;
    private float timeMark;
    private int wpNum;
    private float lastT = 0.0f;
    private float deltaT = 0.0f;

    // Use this for initialization
    void Start () {
        follower.SetActive(false);
        marker  = GameObject.FindGameObjectWithTag("Marker");
        lineRender = gameObject.GetComponent<LineRenderer>();
		lineRender.material = new Material(Shader.Find("Particles/Additive")); // Change material for line renderer
		lineRender.SetColors(c1, c2); //Set the assigned colors
        lastT = Time.realtimeSinceStartup;
	}
    void calculateDeltaT() {
        deltaT = Time.realtimeSinceStartup - lastT;
        lastT = Time.realtimeSinceStartup;
    }
    void Move()
    {
        float hd = Input.GetAxis("Horizontal") * speed * deltaT;
        float vd = Input.GetAxis("Vertical")   * speed * deltaT;
        transform.Translate(new Vector3(hd, 0, vd));
    }
    void ScaleTime (float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * scale;
    }
    void DrawStart()
    {
        ScaleTime(0.0f);
        linePoints = 0;
        wpNum = 0;
        timeMark = Time.realtimeSinceStartup;
        state = DrawStates.DRAW;
    }
    void DrawMain()
    {
        Move();
        lineRender.SetVertexCount(++linePoints);
        lineRender.SetPosition(linePoints - 1, transform.position);
        if (timeMark + drawStepTime * wpNum < Time.realtimeSinceStartup)
        {
            waypoints.Add(transform.position);
            wpNum++;
        }
    }
    void DrawEnd()
    {
        ScaleTime(1.0f);
        marker.GetComponent<Renderer>().enabled = false;
        follower.GetComponent<ObjectFollow>().FollowPrime();
        state = DrawStates.FOLLOW;
    }
    public void DrawReset()
    {
        transform.position = playerChar.transform.position;
        marker.GetComponent<Renderer>().enabled = true;
        lineRender.SetVertexCount(0);
        waypoints.Clear();
        state = DrawStates.READY;
    }
    void Update()
    {
        calculateDeltaT();

        if (state == DrawStates.READY) transform.position = playerChar.transform.position;
        if (state == DrawStates.READY && Input.GetButtonDown("Draw")) DrawStart();
        if (state == DrawStates.DRAW  && Input.GetButton    ("Draw")) DrawMain();
        if (state == DrawStates.DRAW  && (
            Input.GetButtonUp("Draw") ||
            timeMark + timeLimit < Time.realtimeSinceStartup))        DrawEnd();
    }
}


