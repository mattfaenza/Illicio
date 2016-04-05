using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {

    
    public float speed;
    public float moveTime;
    public float fadeAmount;
    public float fadePerSecond;
    public Material newMaterial;
    private float curTime;
    private bool activated;
    private Material material;
    private Color color;
    private Renderer rend;
    // Use this for initialization
    void Start () {
        activated = false;
        moveTime = 1.5f;
        fadeAmount = 40f;
        fadePerSecond = 20f;

    }

    void Activate()
    {
        activated = true;
        curTime = Time.time;
        rend = gameObject.GetComponent<Renderer>();

    }


    // Update is called once per frame
    void Update () {
        if(activated)
        {
            //set target to be 6.1 units above
            transform.Translate(Vector3.forward  * (Time.deltaTime * (6.1F / moveTime)));

            //stop changes, change material -> change this to be a less transparent material that fades into more transparent
            if (Time.time >= curTime + moveTime)
            {
                rend.material = newMaterial;
                activated = false;
            }
        }

    }

}
