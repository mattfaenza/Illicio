using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour
{


    public float fadeAmount;
    public float fadePerSecond;
    public Material newMaterial;
    private float moveAmount;
    private Vector3 moveToTarget;
    private float moveTime;
    private float curTime;
    private bool activated;
    private Material material;
    private Color color;
    private Renderer rend;
    // Use this for initialization
    void Start()
    {
        activated = false;
        moveTime = 2.5f;
        fadeAmount = 40f;
        fadePerSecond = 20f;
        moveToTarget = gameObject.transform.position;
        moveToTarget.y = moveToTarget.y + 6.1f;
    }

    void Activate()
    {
        activated = true;
        curTime = Time.time;
        rend = gameObject.GetComponent<Renderer>();

    }


    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            //set target to be 6.1 units above

            //dynamically calculate speed by getting the distance between the target

            moveAmount = Mathf.Abs(gameObject.transform.position.y - moveToTarget.y);
            transform.Translate(Vector3.forward * (Time.deltaTime * moveAmount));

            //stop changes, change material -> change this to be a less transparent material that fades into more transparent
            if (Time.time >= curTime + moveTime)
            {
                rend.material = newMaterial;
                activated = false;
            }
        }

    }

}